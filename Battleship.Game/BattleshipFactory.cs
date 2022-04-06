namespace Battleship.Game
{
    public static class BattleshipFactory
    {
        public static BattleshipMatch CreateMatch(Player playerOne, Player playerTwo) => new BattleshipMatch(playerOne, playerTwo);

        public static Player CreatePlayer(string username, Guid? guid = null) => new Player(guid ?? Guid.NewGuid(), username);

        public static IShip? CreateShip(ShipName shipName)
        {
            switch (shipName)
            {
                case ShipName.Battleship: return new BattleShip();
                case ShipName.Patrol_Boat: return new Patrol_Boat();
                case ShipName.Submarine: return new Submarine();
                case ShipName.Destroyer: return new Destroyer();
                case ShipName.Carrier: return new Carrier();
                default: return null;
            }
        }
    }
}
