namespace Battleship.WASM.Shared
{
    public static class BattleshipHelpers
    {
        public static List<Coordinate> BuildWriteCoordinates(Placement placement, IShip ship)
        {
            List<Coordinate>? writeCoordinates = new List<Coordinate>();

            switch (placement.Direction)
            {
                case Direction.Up:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        writeCoordinates.Add(new Coordinate(placement.Coordinate.X - i, placement.Coordinate.Y));
                    }
                    break;
                case Direction.Down:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        writeCoordinates.Add(new Coordinate(placement.Coordinate.X + i, placement.Coordinate.Y));
                    }
                    break;
                case Direction.Right:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        writeCoordinates.Add(new Coordinate(placement.Coordinate.X, placement.Coordinate.Y + i));
                    }
                    break;
                case Direction.Left:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        writeCoordinates.Add(new Coordinate(placement.Coordinate.X, placement.Coordinate.Y - i));
                    }
                    break;
            }

            return writeCoordinates;
        }

        public static bool CheckShipPlacement(Coordinate coordinate, IShip?[,] gameboard, IShip target, int Size = 10)
        {
            // Bounds check
            if (coordinate.X >= Size || coordinate.Y >= Size
                || coordinate.X < 0 || coordinate.Y < 0)
            {
                return false;
            }
            // Ship overlap check
            else if (gameboard[coordinate.X, coordinate.Y] != null && gameboard[coordinate.X, coordinate.Y]?.Name != target.Name)
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