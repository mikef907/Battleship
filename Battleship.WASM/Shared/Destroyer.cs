namespace Battleship.WASM.Shared
{
    public readonly record struct Destroyer : IShip
    {
        public int Size => 3;

        public ShipName Name => ShipName.Destroyer;
    }
}
