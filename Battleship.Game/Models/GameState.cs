﻿namespace Battleship.Game.Models
{
    public class GameState
    {
        internal Player CurrentTurn { get; set; }

        internal IEnumerable<ValueTuple<Player, Coordinate, Result, ShipName?>> Moves
            = Enumerable.Empty<ValueTuple<Player, Coordinate, Result, ShipName?>>();

        internal bool CheckDuplicateMove(Player player, Coordinate coordinate) => Moves.Any(_ => _.Item1 == player && _.Item2 == coordinate);

        internal void AppendMove(ValueTuple<Player, Coordinate, Result, ShipName?> move) => Moves = Moves.Append(move);
    };
}
