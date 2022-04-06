using Battleship.WASM.Shared;

namespace Battleship.WASM.Client.Models
{
    public class CurrentMatchInfo
    {
        public Player Player { get; set; }
        public Guid? MatchId { get; set; }
        public GamePhase? GamePhase { get; set; }
    }
}
