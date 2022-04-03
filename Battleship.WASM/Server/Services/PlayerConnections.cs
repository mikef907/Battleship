namespace Battleship.WASM.Server.Services
{
    public class PlayerConnections
    {
        public readonly IDictionary<Player, string> Connections;

        public PlayerConnections()
        {
            Connections = new Dictionary<Player, string>();
        }
    }
}
