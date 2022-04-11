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

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string? connectionId = Context.ConnectionId;


            if (_players.Connections.Any(_ => _.Value == connectionId))
            {
                KeyValuePair<Player, string> connection = _players.Connections.Single(_ => _.Value == connectionId);

                Player? opponent = _battleshipService.PlayerDiconnectedFromMatch(connection.Key);

                if (opponent is not null)
                {
                    KeyValuePair<Player, string> opponentConnection = _players.Connections.Single(_ => _.Key == opponent);

                    await Clients.Client(opponentConnection.Value).SendAsync("NotifyOpponentDisconnected");
                }

                if (_playerQueue.Players.Contains(connection.Key))
                {
                    _playerQueue.Remove(connection.Key);
                }

                _players.Connections.Remove(connection.Key);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinQueue(string username)
        {
            Player player;

            if (_playerQueue.Players.Any(_ => _.Username == username))
            {
                await Clients.Caller.SendAsync("JoinQueueResponse", null, cancellationTokenSource.Token);
                return;
            }

            if (_players.Connections.Keys.Any(_ => _.Username == username))
            {
                player = _players.Connections.Keys.Single(_ => _.Username == username);
            }
            else
            {
                player = BattleshipFactory.CreatePlayer(username);
                _players.Connections.Add(player, Context.ConnectionId);
            }

            _playerQueue.Players.Enqueue(player);
#if DEBUG
            // Allows me to test this through a single browser instance
            //Player player2 = BattleshipFactory.CreatePlayer("TESTPLAYER2");
            //_players.Connections.Add(player2, "TESTPLAYER2");
            //_playerQueue.Players.Enqueue(player2);
#endif

            await Clients.Caller.SendAsync("JoinQueueResponse", player, cancellationTokenSource.Token);
        }

        public async Task StartGame(Guid matchId, IDictionary<ShipName, Placement> playerShips)
        {
#if DEBUG
            // Easy setup for both sides, the will mirror eachout
            //Player opponent = _players.Connections.First(_ => _.Key.Username == "TESTPLAYER2").Key;
            //foreach (KeyValuePair<ShipName, Placement> ship in playerShips)
            //{
            //    _battleshipService.TryPlaceShip(matchId, new Placement(ship.Value.Coordinate, ship.Value.Direction, opponent), BattleshipFactory.CreateShip(ship.Key)!, out int placed);
            //}
#endif

            foreach (KeyValuePair<ShipName, Placement> ship in playerShips)
            {
                if (_battleshipService.TryPlaceShip(matchId, ship.Value, BattleshipFactory.CreateShip(ship.Key)!, out int placed) && placed == 10)
                {
                    if (_battleshipService.TryStartGame(matchId, out GamePhase gamePhase))
                    {
                        foreach (Player player in _battleshipService.GetPlayers(matchId))
                        {
                            string? connection = _players.Connections[player];

                            Player currentTurn = _battleshipService.GetCurrentTurn(matchId);

                            await Clients.Client(connection).SendAsync("NotifyGameStarted", currentTurn);

                        }
                        return;
                    }
                }

            }

            foreach (Player player in _battleshipService.GetPlayers(matchId))
            {
                string? connection = _players.Connections[player];

                await Clients.Client(connection).SendAsync("NotifyGameStarted", null);
            }
        }

        public async Task Shoot(CurrentMatchInfo currentMatchInfo, Coordinate coordinate)
        {
            Guid matchId = currentMatchInfo.MatchId!.Value;

            (Result, ShipName?) results = _battleshipService.FireShot(matchId, coordinate, currentMatchInfo.Player);

            Player currentTurn = _battleshipService.GetCurrentTurn(matchId);

            foreach (Player player in _battleshipService.GetPlayers(matchId))
            {
                string? connection = _players.Connections[player];

                await Clients.Client(connection).SendAsync("NotifyShotResult", currentTurn, _battleshipService.Moves(matchId));

                Player? winner = _battleshipService.CheckMatchState(matchId);

                if (winner is not null)
                {
                    await Clients.Client(connection).SendAsync("NotifyMatchFinished", winner);
                }
            }
        }
    }
}
