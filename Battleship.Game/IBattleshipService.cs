using Battleship.Game.Models;

namespace Battleship.Game
{
    public interface IBattleshipService
    {
        public Result Fire(GameState gameState, Coordinate coordinate, Player player);
        public Player? CheckState(GameState gameState);
    }
}
