using Battleship.Game.Models;

namespace Battleship.WASM.Server.Services
{
    public class PlayerQueue
    {
        public readonly Queue<Player> Players;

        public PlayerQueue()
        {
            Players = new Queue<Player>();
        }
    }
}
