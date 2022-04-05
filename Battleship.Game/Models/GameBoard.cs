namespace Battleship.Game.Models
{

    public record GameBoard
    {
        private readonly int Size;
        internal readonly Dictionary<Placement, ShipState> Ships;
        private readonly IShip?[,] PlayerBoard;

        internal GameBoard(int size)
        {
            Size = size;
            Ships = new Dictionary<Placement, ShipState>();
            PlayerBoard = new IShip?[Size, Size];
        }

        public bool WriteShipPlacement(IEnumerable<Coordinate> coordinates, IShip ship)
        {
            if (!coordinates.All(_ => BattleshipHelpers.CheckShipPlacement(_, PlayerBoard, ship, Size)))
            {
                return false;
            }

            // Write update once all coordinates are valid
            foreach (Coordinate coord in coordinates)
            {
                PlayerBoard[coord.X, coord.Y] = ship;
            }

            return true;
        }

        public IEnumerable<Coordinate> RemoveShipPlacement(IShip ship)
        {
            List<Coordinate>? coodinates = new List<Coordinate>();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (PlayerBoard[i, j] == ship)
                    {
                        PlayerBoard[i, j] = null;
                        coodinates.Add(new Coordinate(i, j));
                    }
                }
            }

            return coodinates;
        }

        public (Result, ShipName?) CheckForHit(Coordinate coordinate)
        {
            if (PlayerBoard[coordinate.X, coordinate.Y] != null)
            {
                ShipState? shipState = Ships.Single(_ => _.Value.Ship == PlayerBoard[coordinate.X, coordinate.Y]).Value;

                shipState.IncrementHits();

                if (shipState.IsSunk)
                {
                    return (Result.Sunk, shipState.Ship.Name);
                }

                return (Result.Hit, null);
            }
            return (Result.Miss, null);
        }
    }
}
