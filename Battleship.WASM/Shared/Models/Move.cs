namespace Battleship.WASM.Shared
{
    public record Move(Player Player, Coordinate Coordinate, Result Result, ShipName? ShipName);
}
