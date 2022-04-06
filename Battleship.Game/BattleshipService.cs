using Microsoft.Extensions.Logging;

namespace Battleship.Game
{
    public class BattleshipService : IBattleshipService
    {
        protected static IDictionary<Guid, BattleshipMatch> matches
            = new Dictionary<Guid, BattleshipMatch>();

        protected readonly IBattleshipGameEngine _engine;
        private readonly ILogger _logger;

        public BattleshipService(IBattleshipGameEngine engine, ILoggerFactory loggerFactory)
        {
            _engine = engine;
            _logger = loggerFactory.CreateLogger(nameof(BattleshipService));
        }

        public Guid NewGame(Player playerOne, Player playerTwo)
        {
            BattleshipMatch? game = BattleshipFactory.CreateMatch(playerOne, playerTwo);
            matches.Add(game.Id, game);
            return game.Id;
        }

        public GameState GetGameState(Guid guid)
        {
            BattleshipMatch? match = GetMatch(guid);

            return match.State;
        }

        public Player CurrentTurn(Guid guid)
        {
            BattleshipMatch? match = GetMatch(guid);

            return match.State.CurrentTurn;
        }

        public Player? CheckMatchState(Guid guid)
        {
            BattleshipMatch? match = GetMatch(guid);

            if (_engine.CheckMatchState(match) is var player && player != null)
            {
                match.TransitionGamePhase();
                return player;
            }
            else
            {
                return null;
            }
        }

        public GamePhase GetMatchPhase(Guid guid) => GetMatch(guid).GamePhase;

        public int GetMaxShips(Guid guid) => GetMatch(guid).NumShipsPerPlayer * 2;

        public bool TryStartGame(Guid guid, out GamePhase gamePhase)
        {
            BattleshipMatch? match = GetMatch(guid);

            bool result = match.AllShipsPlaced;

            if (result)
            {
                match.TransitionGamePhase();
            }

            gamePhase = match.GamePhase;

            return result;
        }

        public bool TryPlaceShip(Guid guid, Placement placement, IShip ship, out int shipsPlaced)
        {
            BattleshipMatch? match = GetMatch(guid);

            bool result = _engine.PlaceShip(match, placement, ship);

            shipsPlaced = match.Playerboards.Values.Select(_ => _.Ships.Count).Sum();

            return result;
        }

        public (Result, ShipName?) FireShot(Guid guid, Coordinate coordinate, Player attacker, Player against)
        {
            if (CurrentTurn(guid) != attacker)
            {
                throw new InvalidOperationException("Not attackers turn");
            }

            return _engine.MarkCoordinate(GetMatch(guid), coordinate, attacker, against);
        }

        private BattleshipMatch GetMatch(Guid guid)
        {
            if (matches.TryGetValue(guid, out BattleshipMatch? match))
            {
                return match;
            }

            MatchNotFoundException? exception = new MatchNotFoundException();
            _logger.LogError(exception, guid.ToString());
            throw exception;
        }

        public IEnumerable<Player> GetPlayers(Guid guid) => GetMatch(guid).Playerboards.Select(_ => _.Key);

        public Player GetCurrentTurn(Guid guid) => GetMatch(guid).State.CurrentTurn;
    }
}
