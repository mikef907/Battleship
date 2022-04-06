using Battleship.WASM.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Battleship.WASM.Server.Hubs
{
    public class BattleshipHub : Hub
    {
        private readonly PlayerQueue _playerQueue;
        private readonly PlayerConnections _players;
        private readonly IBattleshipService _battleshipService;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public BattleshipHub(PlayerQueue playerQueue, PlayerConnections playerConnections, IBattleshipService battleshipService)
        {
            _playerQueue = playerQueue;
            _players = playerConnections;
            _battleshipService = battleshipService;
        }

        public async Task JoinQueue(string username)
        {
            Player player;

            if (_playerQueue.Players.Any(_ => _.Username == username))
            {
                return;
            }

            if (_players.Connections.Keys.Any(_ => _.Username == username))
            {
                player = _players.Connections.Keys.Single(_ => _.Username == username);
            }
            else
            {
#if DEBUG
                // Allows me to test this through a single browser instance
                Player player2 = BattleshipFactory.CreatePlayer("TESTPLAYER2");
                _players.Connections.Add(player2, "TESTPLAYER2");
                _playerQueue.Players.Enqueue(player2);
#endif

                player = BattleshipFactory.CreatePlayer(username);
                _players.Connections.Add(player, Context.ConnectionId);
            }

            _playerQueue.Players.Enqueue(player);

            await Clients.Caller.SendAsync("JoinQueueResponse", player, cancellationTokenSource.Token);
        }

        public async Task StartGame(Guid matchId, IDictionary<ShipName, Placement> playerShips)
        {
            foreach (KeyValuePair<ShipName, Placement> ship in playerShips)
            {
                if (_battleshipService.TryPlaceShip(matchId, ship.Value, BattleshipFactory.CreateShip(ship.Key)!, out int placed) && placed == 5)
                {
                    if (_battleshipService.TryStartGame(matchId, out GamePhase gamePhase))
                    {
                        foreach (Player player in _battleshipService.GetPlayers(matchId))
                        {
                            string? connection = _players.Connections[player];

                            Player currentTurn = _battleshipService.GetCurrentTurn(matchId);

                            await Clients.Client(connection).SendAsync("NotifyGameStarted", currentTurn);
                        }
                    }
                }
            }
        }
    }
}
