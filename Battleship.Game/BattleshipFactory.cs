using Battleship.Game.Models;

namespace Battleship.Game
{
    public static class BattleshipFactory
    {
        public static BattleshipMatch Create(Player playerOne, Player playerTwo)
        {
            return new BattleshipMatch(playerOne, playerTwo);
        }
    }
}
