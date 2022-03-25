namespace Battleship.Game.Models
{

    public record Placement
    {
        public Coordinate Coordinate { get; init; }
        public Direction Direction { get; init; }
    }

    public record Coordinate
    {
        public int X { get; init; }
        public int Y { get; init; }
    }

    public record GameBoard
    {
        public int[][] Board { get; init; }
        public Dictionary<Placement, IShip> P1Ships { get; init; }
        public Dictionary<Placement, IShip> P2Ships { get; init; }
    }
}
