namespace Battleship.Game.Models
{
    public class ShipState
    {
        public IShip Ship { get; private set; }
        private int hits = 0;

        public ShipState(IShip ship)
        {
            Ship = ship;
        }

        public bool IsSunk => Ship.Size == hits;
        public void IncrementHits() => hits++;
    };
}
