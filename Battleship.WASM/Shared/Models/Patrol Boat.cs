namespace Battleship.WASM.Shared
{
    public readonly record struct Patrol_Boat : IShip
    {
        public int Size => 2;

        public ShipName Name => ShipName.Patrol_Boat;
    }
}
