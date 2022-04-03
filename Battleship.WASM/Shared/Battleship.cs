namespace Battleship.WASM.Shared
{
    public readonly record struct Battleship : IShip
    {
        public int Size => 4;

        public ShipName Name => ShipName.Battleship;
    }
}
