using Microsoft.Extensions.Logging;

namespace Battleship.Game
{
    public class BattleshipGameEngine : IBattleshipGameEngine
    {
        private readonly ILogger Logger;

        public BattleshipGameEngine(ILoggerFactory loggerFactory) => Logger = loggerFactory.CreateLogger(nameof(BattleshipFactory));

        public (Result, ShipName?) MarkCoordinate(BattleshipMatch match, Coordinate coordinate, Player attacker)
        {
            try
            {
                if (match.GamePhase != GamePhase.Main)
                {
                    throw new BadGameStateException();
                }

                if (match.State.CheckDuplicateMove(attacker, coordinate))
                {
                    throw new InvalidOperationException("Move would be duplicate");
                }

                Player against = match.Playerboards.Single(_ => _.Key != attacker).Key;

                (Result, ShipName?) results = match.Playerboards[against].CheckForHit(coordinate);

                match.State.AppendMove(new Move(attacker, coordinate, results.Item1, results.Item2));

                match.State.CurrentTurn = against;

                return results;
            }
            catch (Exception ex)
            {
                Logger.LogError(exception: ex, message: "Error Marking Coordinate");
                throw;
            }
        }

        public virtual Player? CheckMatchState(BattleshipMatch match)
        {
            try
            {
                if (match.AllShipsPlaced && match.Playerboards.Any(_ => _.Value.Ships.All(__ => __.Value.IsSunk)))
                {
                    return match.Playerboards.Single(_ => !_.Value.Ships.All(__ => __.Value.IsSunk)).Key;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(exception: ex, message: "Error Checking Game State");
                throw;
            }
        }

        public bool PlaceShip(BattleshipMatch match, Placement placement, IShip ship)
        {
            try
            {
                if (match.GamePhase != GamePhase.Setup)
                {
                    throw new BadGameStateException();
                }

                GameBoard? board = match.Playerboards[placement.Owner];
                Coordinate coordinate = placement.Coordinate;

                // Basic bounds checks
                if (coordinate.X >= match.Size || coordinate.Y >= match.Size ||
                    coordinate.X < 0 || coordinate.Y < 0)
                {
                    return false;
                }
                else
                {
                    // If current exists, attempt to replace, otherwise put current back if unable
                    KeyValuePair<Placement, ShipState> _current = board.Ships.SingleOrDefault(_ => _.Value.Ship.Name == ship.Name);
                    IEnumerable<Coordinate> replacementCoordinates = null;

                    if (_current.Value is not null)
                    {
                        board.Ships.Remove(_current.Key);
                        replacementCoordinates = board.RemoveShipPlacement(ship);
                    }

                    List<Coordinate> writeCoordinates = BattleshipHelpers.BuildWriteCoordinates(placement, ship);

                    if (!board.WriteShipPlacement(writeCoordinates, ship))
                    {
                        // Replace current if it already was present
                        if (_current.Value is not null && replacementCoordinates is not null)
                        {
                            board.Ships.Add(_current.Key, _current.Value);
                            board.WriteShipPlacement(replacementCoordinates, ship);
                        }

                        return false;
                    }

                    board.Ships.Add(placement, new ShipState(ship));

                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(exception: ex, message: "Error Placing Ship");
                throw;
            }
        }
    }
}
