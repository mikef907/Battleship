using Battleship.Game.Models;
using Microsoft.Extensions.Logging;

namespace Battleship.Game
{
    public class BattleshipGameEngine
    {
        private readonly ILogger Logger;

        public BattleshipGameEngine(ILogger logger)
        {
            Logger = logger;
        }

        public Result CheckCoordinate(BattleshipGame game, Coordinate coordinate, Player player)
        {
            throw new NotImplementedException();
        }

        public Player? CheckGameState(BattleshipGame game)
        {
            throw new NotImplementedException();
        }

        public bool PlaceShip(BattleshipGame game, Placement placement, IShip ship)
        {
            try
            {
                var board = game.Playerboards[placement.Owner];
                var coordinate = placement.Coordinate;

                // Don't allow duplicate ships
                if (board.Ships.Any(_ => _.Value == ship))
                {
                    return false;
                }
                // Basic bounds checks
                else if (coordinate.X > game.SIZE || coordinate.Y > game.SIZE ||
                    coordinate.X < 0 || coordinate.Y < 0)
                {
                    return false;
                }
                else
                {
                    List<Coordinate> writeCoordinates = BuildWriteCoordinates(placement, ship, coordinate);

                    if (!board.WriteShipPlacement(writeCoordinates, ship.Name))
                    {
                        return false;
                    }

                    board.Ships.Add(placement, ship);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(exception: ex, message: "Error Placing Ship");
                return false;
            }
        }

        private static List<Coordinate> BuildWriteCoordinates(Placement placement, IShip ship, Coordinate coordinate)
        {
            var writeCoordinates = new List<Coordinate>();

            switch (placement.Direction)
            {
                case Direction.Up:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        writeCoordinates.Add(new Coordinate(coordinate.X, coordinate.Y - i));
                    }
                    break;
                case Direction.Down:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        writeCoordinates.Add(new Coordinate(coordinate.X, coordinate.Y + i));
                    }
                    break;
                case Direction.Right:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        writeCoordinates.Add(new Coordinate(coordinate.X + i, coordinate.Y));
                    }
                    break;
                case Direction.Left:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        writeCoordinates.Add(new Coordinate(coordinate.X - i, coordinate.Y));
                    }
                    break;
            }

            return writeCoordinates;
        }
    }
}
