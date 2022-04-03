namespace Battleship.WASM.Shared
{
    public interface IShip
    {
        public int Size { get; }
        public ShipName Name { get; }
    }
}