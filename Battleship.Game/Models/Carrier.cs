namespace Battleship.Game.Models
{
    public record Carrier : IShip
    {
        public int Size => 5;
        public ShipName Name => ShipName.Carrier;
    }
}
