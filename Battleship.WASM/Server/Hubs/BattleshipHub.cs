using Battleship.Game;
using Battleship.Game.Models;
using Battleship.WASM.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Battleship.WASM.Server.Hubs
{
    public class BattleshipHub : Hub
    {
        private readonly PlayerQueue _playerQueue;
        private readonly PlayerConnections _players;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public BattleshipHub(PlayerQueue playerQueue, PlayerConnections playerConnections)
        {
            _playerQueue = playerQueue;
            _players = playerConnections;
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
                player = BattleshipFactory.CreatePlayer(username);
                _players.Connections.Add(player, Context.ConnectionId);
            }

            _playerQueue.Players.Enqueue(player);

            await Clients.Caller.SendAsync("JoinQueueResponse", player, cancellationTokenSource.Token);
        }
    }
}
