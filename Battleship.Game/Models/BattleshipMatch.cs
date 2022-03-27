namespace Battleship.Game.Models
{
    public class BattleshipMatch
    {
        public readonly int Size = 10;
        public GamePhase GamePhase { get; private set; }

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
