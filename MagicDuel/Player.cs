

namespace MagicDuel
{
    public record Player(Random Random, Element Element)
    {
        private readonly List<Card> _cards = new List<Card>();
        private Modifiers _modifiers = Modifiers.All;

        public PlayerPlay Play(Game game)
        {
            while (_cards.Count < game.HandSize)
                _cards.Add(game.Draw());

            return CalculatePlay(game);
        }

        public void ResetModifiers()
            => _modifiers = Modifiers.All;

        private PlayerPlay CalculatePlay(Game game)
        {
            Card[] knownNonEnemyCards = [.. game.PlayedPile, .. game.DiscardedPile(this), .. _cards];
            var possibleEnemyCards = DeckExtensions.GetDeck().Except(knownNonEnemyCards).ToArray();
            var values = _cards.ToDictionary(card => card, card => possibleEnemyCards.Count(card.Wins));

            var playedCard = CalculatePlayedCard(values);
            _cards.Remove(playedCard);

            var playedModifiers = CalculatePlayedModifiers(values[playedCard], possibleEnemyCards.Length, game.DrawingPileLeft);
            values.Remove(playedCard);

            var discardedCards = CalculateDiscardedCards(values, possibleEnemyCards.Length);

            return (playedCard, playedModifiers, discardedCards);
        }

        private Card CalculatePlayedCard(Dictionary<Card, int> values)
        {
            Card? playedCard = null;
            var playedIndex = Random.Next(values.Values.Sum());
            foreach (var pair in values)
            {
                if (playedIndex < pair.Value)
                {
                    playedCard = pair.Key;
                    break;
                }
                else
                {
                    playedIndex -= pair.Value;
                }
            }

            if (playedCard == null)
                playedCard = _cards.OrderBy(c => c.Value).ThenBy(c => c.Element == Element ? 1 : 0).First();
            return playedCard;
        }

        private Modifiers CalculatePlayedModifiers(int winPossibilities, int totalPossibilities, int drawingPileLeft)
        {
            var result = Modifiers.None;

            if (_modifiers.HasFlag(Modifiers.PlusOneWhenWin) && Random.Next(totalPossibilities) < winPossibilities)
                result |= Modifiers.PlusOneWhenWin;

            if (_modifiers.HasFlag(Modifiers.MinusOneWhenLose) && Random.Next(totalPossibilities) >= winPossibilities)
                result |= Modifiers.MinusOneWhenLose;

            if (Random.Next(20) > drawingPileLeft)
                result = _modifiers;

            _modifiers &= ~result;

            return result;
        }

        private IEnumerable<Card> CalculateDiscardedCards(Dictionary<Card, int> values, int totalPossibilities)
        {
            var discarded = values.Where(pair => Random.Next(totalPossibilities) >= pair.Value).Select(pair => pair.Key).ToArray() ?? [];
            foreach (var card in discarded)
                _cards.Remove(card);
            return discarded;
        }
    }
}