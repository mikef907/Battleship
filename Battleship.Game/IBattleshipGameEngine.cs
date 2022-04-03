namespace Battleship.Game
{
    public interface IBattleshipGameEngine
    {
        Player? CheckMatchState(BattleshipMatch game);
        (Result, ShipName?) MarkCoordinate(BattleshipMatch game, Coordinate coordinate, Player attacker, Player against);
        bool PlaceShip(BattleshipMatch game, Placement placement, IShip ship);
    }
}