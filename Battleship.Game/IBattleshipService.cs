namespace Battleship.Game
{
    public interface IBattleshipService
    {
        Player? CheckMatchState(Guid guid);
        (Result, ShipName?) FireShot(Guid guid, Coordinate coordinate, Player attacker, Player against);
        int GetMaxShips(Guid guid);
        Guid NewGame(Player playerOne, Player playerTwo);
        bool TryPlaceShip(Guid guid, Placement placement, IShip ship, out int shipsPlaced);
        bool TryStartGame(Guid guid, out GamePhase gamePhase);
    }
}