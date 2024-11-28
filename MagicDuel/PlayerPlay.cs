

namespace MagicDuel
{
    public record struct PlayerPlay(Card PlayedCard, Modifiers PlayedModifiers, IEnumerable<Card>? DiscardedCards)
    {
        public static implicit operator (Card PlayedCard, Modifiers PlayedModifiers, IEnumerable<Card>? DiscardedCards)(PlayerPlay value)
        {
            return (value.PlayedCard, value.PlayedModifiers, value.DiscardedCards);
        }

        public static implicit operator PlayerPlay((Card PlayedCard, Modifiers PlayedModifiers, IEnumerable<Card>? DiscardedCards) value)
        {
            return new PlayerPlay(value.PlayedCard, value.PlayedModifiers, value.DiscardedCards);
        }

        public readonly int CalculateWin(Game game, Player player, PlayerPlay play)
            => (play.PlayedCard.Element == PlayedCard.Element ? game.SameElementDamage : game.WinElementDamage)
             + (PlayedCard.Element == player.Element ? game.PlayerElementMatchDamage : 0)
             + (PlayedModifiers.HasFlag(Modifiers.PlusOneWhenWin) ? 1 : 0)
             + (play.PlayedModifiers.HasFlag(Modifiers.MinusOneWhenLose) ? -1 : 0);

        public static int CalculateResult(Game game, Player player1, PlayerPlay player1Play, Player player2, PlayerPlay player2Play)
        {
            if (player1Play.PlayedCard.Wins(player2Play.PlayedCard))
                return player1Play.CalculateWin(game, player1, player2Play);
            else
                return -player2Play.CalculateWin(game, player2, player1Play);
        }
    }
}