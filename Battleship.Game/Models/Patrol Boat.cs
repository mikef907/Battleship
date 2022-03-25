namespace Battleship.Game.Models
{
    public record Patrol_Boat : IShip
    {
        public int Size => 2;

        public ShipName Name => ShipName.Patrol_Boat;
    }
}
