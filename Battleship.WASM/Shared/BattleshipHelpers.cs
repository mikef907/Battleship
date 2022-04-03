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
                        writeCoordinates.Add(new Coordinate(placement.Coordinate.X, placement.Coordinate.Y - i));
                    }
                    break;
                case Direction.Down:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        writeCoordinates.Add(new Coordinate(placement.Coordinate.X, placement.Coordinate.Y + i));
                    }
                    break;
                case Direction.Right:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        writeCoordinates.Add(new Coordinate(placement.Coordinate.X + i, placement.Coordinate.Y));
                    }
                    break;
                case Direction.Left:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        writeCoordinates.Add(new Coordinate(placement.Coordinate.X - i, placement.Coordinate.Y));
                    }
                    break;
            }

            return writeCoordinates;
        }
    }
}