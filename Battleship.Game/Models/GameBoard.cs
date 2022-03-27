namespace Battleship.Game.Models
{
    public readonly record struct Placement(Coordinate Coordinate, Direction Direction, Player Owner);

    public readonly record struct Coordinate(int X, int Y);

    public record GameBoard
    {
        private readonly int Size;
        internal readonly Dictionary<Placement, ShipState> Ships;
        private readonly ShipName?[,] PlayerBoard;

        private GameBoard() { }

        internal GameBoard(int size)
        {
            Size = size;
            Ships = new Dictionary<Placement, ShipState>();
            PlayerBoard = new ShipName?[Size, Size];
        }

        public bool WriteShipPlacement(IEnumerable<Coordinate> coordinates, ShipName shipName)
        {
            if (!coordinates.All(_ => CheckShipPlacement(_)))
            {
                return false;
            }

            // Write update once all coordinates are valid
            foreach (var coord in coordinates)
            {
                PlayerBoard[coord.X, coord.Y] = shipName;
            }

            return true;
        }

        public (Result, ShipName?) CheckForHit(Coordinate coordinate)
        {
            if (PlayerBoard[coordinate.X, coordinate.Y] != null)
            {
                var shipState = Ships.Single(_ => _.Value.Ship.Name == PlayerBoard[coordinate.X, coordinate.Y]).Value;

                shipState.IncrementHits();

                if(shipState.IsSunk)
                {
                    return (Result.Sunk, shipState.Ship.Name);
                }

                return (Result.Hit, null);
            }
            return (Result.Miss, null);
        }

        private bool CheckShipPlacement(Coordinate coordinate)
        {
            // Bounds check
            if (coordinate.X > Size || coordinate.Y > Size
                || coordinate.X < 0 || coordinate.Y < 0)
            {
                return false;
            }
            // Ship overlap check
            else if (PlayerBoard[coordinate.X, coordinate.Y] != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
