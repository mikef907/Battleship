namespace Battleship.Game
{
    public interface IShip
    {
        public int Size { get; }
        public ShipName Name { get; }
    }
}