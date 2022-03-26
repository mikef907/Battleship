namespace Battleship.Game.Models
{
    public readonly record struct BattleshipGame
    {
        public readonly int SIZE = 10;

        public BattleshipGame()
        {
            Id = Guid.NewGuid();
            State = new GameState();
            Playerboards = new Dictionary<Player, GameBoard>();

            Playerboards.Add(Player.One, new GameBoard(SIZE));
            Playerboards.Add(Player.Two, new GameBoard(SIZE));
        }

        public Guid Id { get; init; }
        public GameState State { internal get; init; }
        public IDictionary<Player, GameBoard> Playerboards { internal get; init; }
    }
}
