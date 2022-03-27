using Battleship.Game.Models;

namespace Battleship.Game
{
    public static class BattleshipFactory
    {
        public static BattleshipMatch Create()
        {
            return new BattleshipMatch();
        }
    }
}
