namespace Battleship.Game.Models
{
    public class BattleshipMatch
    {
        // Default size of grid 10x10 but may expose this as an option
        public readonly int Size = 10;
        // Default number of ships will be 5 but may expose this as an option
        public readonly int NumShipsPerPlayer = 5;
        public GamePhase GamePhase { get; private set; }
        public virtual bool AllShipsPlaced =>
            Playerboards.Values.All(_ => _.Ships.Count == NumShipsPerPlayer);

        public BattleshipMatch()
        {
            Id = Guid.NewGuid();
            GamePhase = GamePhase.Setup;
            State = new GameState();
            Playerboards = new Dictionary<Player, GameBoard>();

            Playerboards.Add(Player.One, new GameBoard(Size));
            Playerboards.Add(Player.Two, new GameBoard(Size));
        }

        public Guid Id { get; init; }
        public GameState State { internal get; init; }
        public IDictionary<Player, GameBoard> Playerboards { internal get; init; }
        public void TransitionGamePhase()
        {
            switch (GamePhase)
            {
                case GamePhase.Setup:
                    GamePhase = GamePhase.Main;
                    break;
                case GamePhase.Main:
                    GamePhase = GamePhase.Completed;
                    break;
            }
        }
    }
}
