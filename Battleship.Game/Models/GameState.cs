namespace Battleship.Game.Models
{
    public class GameState
    {
        private IEnumerable<ValueTuple<Player, Coordinate, Result, ShipName?>> Moves 
            = Enumerable.Empty<ValueTuple<Player, Coordinate, Result, ShipName?>>();

        public bool CheckDuplicateMove(Player player, Coordinate coordinate)
            => Moves.Any(_ => _.Item1 == player && _.Item2 == coordinate);

        public void AppendMove(ValueTuple<Player, Coordinate, Result, ShipName?> move)
            => Moves = Moves.Append(move);
    };
}
