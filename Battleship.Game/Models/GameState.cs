namespace Battleship.Game.Models
{

    public record GameState
    {
        public GameBoard Board { get; init; }
        public IEnumerable<ValueTuple<Player, Coordinate, Result>> Moves { get; init; }
    }
}
