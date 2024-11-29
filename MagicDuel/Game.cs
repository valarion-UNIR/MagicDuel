using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicDuel
{
    public record Game(int HandSize, int InitialBoardHalfSize, Player Player1, Player Player2, Random random, int SameElementDamage, int WinElementDamage, int PlayerElementMatchDamage)
    {
        public int BoardPosition { get; private set; } = 0;
        public int TurnsPlayed { get; private set; } = 0;
        public int BoardHalfSize { get; private set; } = InitialBoardHalfSize;

        private Stack<Card> _drawingPile = new(DeckExtensions.GetDeck().Randomize(random));
        private readonly List<Card> _playedPile = [];
        private readonly Dictionary<Element, List<Card>> _discardPile = new() { { Player1.Element, [] }, { Player2.Element, [] } };
        public IReadOnlyList<Card> PlayedPile => [.. _playedPile];

        public int DrawingPileLeft  => _drawingPile.Count;

        public IReadOnlyList<Card> DiscardedPile(Player player) => [.. _discardPile[player.Element]];

        public bool PlayTurn()
        {
            if (Math.Abs(BoardPosition) > BoardHalfSize)
                return true;

            var player1Play = Player1.Play(this);
            var player2Play = Player2.Play(this);

            var result = PlayerPlay.CalculateResult(this, Player1, player1Play, Player2, player2Play);
            BoardPosition += result;

            Console.Write($"Player1:{{{player1Play.PlayedCard.Element}, {player1Play.PlayedCard.Value}, {player1Play.PlayedModifiers}, {player1Play.DiscardedCards?.Count()??0}}}");
            Console.Write($" {(result > 0 ? '>' : '<')} ");
            Console.Write($"Player2:{{{player2Play.PlayedCard.Element}, {player2Play.PlayedCard.Value}, {player2Play.PlayedModifiers}, {player2Play.DiscardedCards?.Count() ?? 0}}}");
            Console.WriteLine($" | BoardChange: {result} | BoardPosition: {BoardPosition}");

            _playedPile.AddRange([player1Play.PlayedCard, player2Play.PlayedCard]);
            _discardPile[Player1.Element].AddRange(player1Play.DiscardedCards ?? []);
            _discardPile[Player2.Element].AddRange(player2Play.DiscardedCards ?? []);

            TurnsPlayed++;

            return Math.Abs(BoardPosition) > BoardHalfSize;
        }

        public Card Draw()
        {
            if(_drawingPile.Count == 0)
            {
                _drawingPile = new([.. _playedPile, .. _discardPile[Player1.Element], .. _discardPile[Player2.Element]]);
                _playedPile.Clear();
                _discardPile[Player1.Element].Clear();
                _discardPile[Player2.Element].Clear();

                Player1.ResetModifiers();
                Player2.ResetModifiers();

                BoardHalfSize--;

                Console.WriteLine("Discard pile shuffled and put in drawing pile");
            }

            return _drawingPile.Pop();
        }
    }
}
