using Battleship.Game;
using Battleship.Game.Models;
using Microsoft.AspNetCore.SignalR;

namespace Battleship.WASM.Server.Hubs
{

    public class BattleshipHub : Hub
    {
        static List<Player> roster = new List<Player>();
        static Queue<Player> playerQueue = new Queue<Player>();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        async public Task JoinQueue(string username)
        {
            Player player;

            if (roster.Any(_ => _.Username == username))
            {
                player = roster.Single(_ => _.Username == username);
            }
            else
            {
                player = BattleshipFactory.CreatePlayer(username);
                roster.Add(player);
            }

            playerQueue.Enqueue(player);

            await Clients.Caller.SendAsync("JoinQueueResponse", player, cancellationTokenSource.Token);
        }
    }
}
