namespace Battleship.WASM.Shared
{
    public readonly record struct BattleShip : IShip
    {
        public int Size => 4;

        public ShipName Name => ShipName.Battleship;
    }
}
