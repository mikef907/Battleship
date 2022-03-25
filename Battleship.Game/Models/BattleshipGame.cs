namespace Battleship.Game.Models
{
    public record BattleshipGame
    {
        public Guid Id { get; init; }
        public GameState State { get; init; }
    }
}
