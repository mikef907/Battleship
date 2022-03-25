using Battleship.Game.Models;

namespace Battleship.Game
{
    public static class BattleshipFactory
    {
        public static BattleshipGame Create()
        {
            return new BattleshipGame();
        }
    }
}
