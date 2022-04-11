namespace Battleship.WASM.Server.Services
{
    public class PlayerQueue
    {
        public Queue<Player> Players { get; private set; }

        public PlayerQueue() => Players = new Queue<Player>();

        public void Remove(Player player) => Players = new Queue<Player>(Players.Where(p => p != player));
    }
}
