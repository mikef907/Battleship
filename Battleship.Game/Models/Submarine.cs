namespace Battleship.Game.Models
{
    public record Submarine : IShip
    {
        public int Size => 3;
        public ShipName Name => ShipName.Submarine;
    }
}
