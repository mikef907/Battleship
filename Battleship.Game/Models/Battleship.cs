namespace Battleship.Game.Models
{
    public record Battleship : IShip
    {
        public int Size => 4;

        public ShipName Name => ShipName.Battleship;
    }
}
