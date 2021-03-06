namespace Battleship.WASM.Shared
{
    public readonly record struct Submarine : IShip
    {
        public int Size => 3;
        public ShipName Name => ShipName.Submarine;
    }
}
