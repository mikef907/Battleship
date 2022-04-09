namespace Battleship.WASM.Shared
{
    public class CurrentMatchInfo
    {
        public Player Player { get; set; }
        public Guid? MatchId { get; set; }
        public GamePhase? GamePhase { get; set; }
        public Player? CurrentTurn { get; set; }
    }
}
