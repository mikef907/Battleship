using Battleship.Game.Models;

namespace Battleship.Game
{
    public class BattleshipGameEngine : IBattleshipGameEngine
    {
        public Result CheckCoordinate(GameBoard state, Coordinate coordinate)
        {
            throw new NotImplementedException();
        }

        public Player? CheckGameState(GameState board)
        {
            throw new NotImplementedException();
        }

        public bool PlaceShip(GameBoard board, IShip ship)
        {
            throw new NotImplementedException();
        }
    }
}
