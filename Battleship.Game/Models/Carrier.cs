﻿namespace Battleship.Game.Models
{
    public readonly record struct Carrier : IShip
    {
        public int Size => 5;
        public ShipName Name => ShipName.Carrier;
    }
}
