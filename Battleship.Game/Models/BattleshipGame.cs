namespace Battleship.Game.Models
{
    public enum GamePhase
    { 
        Setup,
        Main,
        Completed
    }

    public class BadGameStateException : Exception { }

    public class BattleshipGame
    {
        public readonly int Size = 10;
        public GamePhase GamePhase { get; private set; }

        public BattleshipGame()
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
