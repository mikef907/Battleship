namespace Battleship.Game
{
    public interface IBattleshipService
    {
        Player? CheckMatchState(Guid guid);
        (Result, ShipName?) FireShot(Guid guid, Coordinate coordinate, Player attacker);
        int GetMaxShips(Guid guid);
        Guid NewGame(Player playerOne, Player playerTwo);
        bool TryPlaceShip(Guid guid, Placement placement, IShip ship, out int shipsPlaced);
        bool TryStartGame(Guid guid, out GamePhase gamePhase);
        GamePhase GetMatchPhase(Guid guid);
        IEnumerable<Player> GetPlayers(Guid guid);
        Player GetCurrentTurn(Guid guid);
        IEnumerable<Move> Moves(Guid matchId);
        Player? PlayerDiconnectedFromMatch(Player player);
    }
}