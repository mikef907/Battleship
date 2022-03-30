using Battleship.Game;
using Battleship.Game.Models;
using Battleship.WASM.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Battleship.WASM.Server.Hubs
{

    public class BattleshipHub : Hub
    {
        static List<Player> roster = new List<Player>();
        private readonly PlayerQueue _playerQueue;

        public BattleshipHub(PlayerQueue playerQueue)
        {
            _playerQueue = playerQueue;
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        async public Task JoinQueue(string username)
        {
            Player player;

            if (_playerQueue.Players.Any(_ => _.Username == username))
                return;

            if (roster.Any(_ => _.Username == username))
            {
                player = roster.Single(_ => _.Username == username);
            }
            else
            {
                player = BattleshipFactory.CreatePlayer(username);
                roster.Add(player);
            }

            _playerQueue.Players.Enqueue(player);

            await Clients.Caller.SendAsync("JoinQueueResponse", player, cancellationTokenSource.Token);
        }
    }
}
