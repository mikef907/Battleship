namespace Battleship.Game.Models
{
    public readonly record struct Placement(Coordinate Coordinate, Direction Direction, Player Owner);

    public readonly record struct Coordinate(int X, int Y);

    public record GameBoard
    {
        private readonly int Size;
        internal readonly Dictionary<Placement, IShip> Ships;
        private readonly ShipName?[,] PlayerBoard;

        private GameBoard() { }

        internal GameBoard(int size) 
        {
            Size = size;
            Ships = new Dictionary<Placement, IShip>();
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

        public bool CheckShipPlacement(Coordinate coordinate)
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
