using Battleship.Game.Models;

namespace Battleship.Game
{
    public interface IBattleshipGameEngine
    {
        public bool PlaceShip(GameBoard board, Placement placement, IShip ship);
        public Result CheckCoordinate(GameBoard state, Coordinate coordinate);
        public Player? CheckGameState(GameState board);
    }
}
