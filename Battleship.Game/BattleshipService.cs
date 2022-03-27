using Battleship.Game.Models;
using Microsoft.Extensions.Logging;

namespace Battleship.Game
{
    public class BattleshipService
    {
        private static IDictionary<Guid, BattleshipMatch> games
            = new Dictionary<Guid, BattleshipMatch>();

        private readonly IBattleshipGameEngine engine;
        private readonly ILogger logger;

        public BattleshipService(IBattleshipGameEngine engine, ILogger logger)
        {
            this.engine = engine;
            this.logger = logger;
        }

        public Guid StartGame()
        {
            var game = BattleshipFactory.Create();
            games.Add(game.Id, game);
            return game.Id;
        }

        public Player? CheckMatchState(Guid guid)
        {
            if (games.TryGetValue(guid, out var match))
            { 
                return engine.CheckMatchState(match);
            }

            var exception = new MatchNotFoundException();
            logger.LogError(exception, guid.ToString());
            throw exception;
        }

        public (Result, ShipName?) FireShot(Guid guid, Coordinate coordinate, Player attacker, Player against)
        {
            if (games.TryGetValue(guid, out var match))
            {
                return engine.MarkCoordinate(match, coordinate, attacker, against);
            }

            var exception = new MatchNotFoundException();
            logger.LogError(exception, guid.ToString());
            throw exception;
        }
    }
}
