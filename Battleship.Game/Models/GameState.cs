namespace Battleship.Game.Models
{

    public record GameState
    { 
        public IEnumerable<ValueTuple<Player, Coordinate, Result>> Moves { get; init; }
    }
}
