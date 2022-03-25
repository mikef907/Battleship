namespace Battleship.Game.Models
{
    public interface IShip
    {
        public int Size { get; }
        public ShipName Name { get; }
    }
}