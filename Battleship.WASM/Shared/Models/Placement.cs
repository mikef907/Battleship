namespace Battleship.WASM.Shared
{
    public readonly record struct Placement(Coordinate Coordinate, Direction Direction, Player Owner);
}
