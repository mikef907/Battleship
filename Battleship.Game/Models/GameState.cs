namespace Battleship.Game.Models
{
    public class GameState
    {
        internal Player CurrentTurn { get; set; }

        internal IEnumerable<Move> Moves = Enumerable.Empty<Move>();

        internal bool CheckDuplicateMove(Player player, Coordinate coordinate) => Moves.Any(_ => _.Player == player && _.Coordinate == coordinate);

        internal void AppendMove(Move move) => Moves = Moves.Append(move);
    };
}
