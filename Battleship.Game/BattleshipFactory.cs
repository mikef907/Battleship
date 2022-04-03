namespace Battleship.Game
{
    public static class BattleshipFactory
    {
        public static BattleshipMatch CreateMatch(Player playerOne, Player playerTwo)
        {
            return new BattleshipMatch(playerOne, playerTwo);
        }

        public static Player CreatePlayer(string username, Guid? guid = null)
        {
            return new Player(guid ?? Guid.NewGuid(), username);
        }
    }
}
