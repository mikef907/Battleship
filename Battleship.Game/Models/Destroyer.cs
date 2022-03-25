namespace Battleship.Game.Models
{
    public record Destroyer : IShip
    {
        public int Size => 3;

        public ShipName Name => ShipName.Destroyer;
    }
}
