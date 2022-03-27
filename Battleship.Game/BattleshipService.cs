using Battleship.Game.Models;
using Microsoft.Extensions.Logging;

namespace Battleship.Game
{
    public class BattleshipService : IBattleshipService
    {
        protected static IDictionary<Guid, BattleshipMatch> matches
            = new Dictionary<Guid, BattleshipMatch>();

        protected readonly IBattleshipGameEngine engine;
        private readonly ILogger logger;

        public BattleshipService(IBattleshipGameEngine engine, ILogger logger)
        {
            this.engine = engine;
            this.logger = logger;
        }

        public Guid NewGame()
        {
            var game = BattleshipFactory.Create();
            matches.Add(game.Id, game);
            return game.Id;
        }

        public Player? CheckMatchState(Guid guid)
        {
            var match = GetMatch(guid);

            if (engine.CheckMatchState(match) is var player && player != null)
            {
                match.TransitionGamePhase();
                return player;
            }
            else return null;
        }

        public int GetMaxShips(Guid guid) => GetMatch(guid).NumShipsPerPlayer * Enum.GetValues(typeof(Player)).Length;

        public bool TryStartGame(Guid guid, out GamePhase gamePhase)
        {
            var match = GetMatch(guid);

            var result = match.AllShipsPlaced;

            if (result)
            {
                match.TransitionGamePhase();
            }

            gamePhase = match.GamePhase;

            return result;
        }

        public bool TryPlaceShip(Guid guid, Placement placement, IShip ship, out int shipsPlaced)
        {
            var match = GetMatch(guid);

            bool result = engine.PlaceShip(match, placement, ship);

            shipsPlaced = match.Playerboards.Values.Select(_ => _.Ships.Count).Sum();

            return result;
        }

        public (Result, ShipName?) FireShot(Guid guid, Coordinate coordinate, Player attacker, Player against)
            => engine.MarkCoordinate(GetMatch(guid), coordinate, attacker, against);

        private BattleshipMatch GetMatch(Guid guid)
        {
            if (matches.TryGetValue(guid, out var match))
            {
                return match;
            }

            var exception = new MatchNotFoundException();
            logger.LogError(exception, guid.ToString());
            throw exception;
        }
    }
}
