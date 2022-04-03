namespace Battleship.Game
{
    public static class BattleshipFactory
    {
        public static BattleshipMatch CreateMatch(Player playerOne, Player playerTwo) => new BattleshipMatch(playerOne, playerTwo);

        public static Player CreatePlayer(string username, Guid? guid = null) => new Player(guid ?? Guid.NewGuid(), username);
    }
}
