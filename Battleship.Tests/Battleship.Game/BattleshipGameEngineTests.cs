using Bship = Battleship.Game.Models.Battleship;

namespace Battleship.Tests
{
    public class BattleshipGameEngineTests
    {
        private readonly BattleshipGameEngine sut;
        private readonly BattleshipMatch game;

        public BattleshipGameEngineTests()
        {
            sut = new BattleshipGameEngine(Substitute.For<ILogger>());
            game = BattleshipFactory.Create();
        }

        [Fact]
        public void PlaceShip_Should_ReturnTrue()
        {
            Placement placement = new Placement(new Coordinate(5, 5), Direction.Up, Player.One);
            Carrier? ship = new Carrier();

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            placement = new Placement(new Coordinate(5, 5), Direction.Up, Player.Two);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();
        }

        [Fact]
        public void PlaceShip_ShouldNot_AllowDuplicateShipsPerPlayer()
        {
            Placement placement = new Placement(new Coordinate(5, 5), Direction.Up, Player.One);
            Carrier? ship = new Carrier();

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(5, 5), Direction.Right, Player.One);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(1, 1), Direction.Right, Player.One);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();
        }

        [Fact]
        public void PlaceShip_ShouldNot_AllowShipsToBePlacedOutOfBounds()
        {
            Placement placement = new Placement(new Coordinate(10, 10), Direction.Up, Player.One);
            Carrier? ship = new Carrier();

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(-1, -1), Direction.Up, Player.One);
            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(0, -1), Direction.Up, Player.One);
            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(9, 10), Direction.Up, Player.One);
            sut.PlaceShip(game, placement, ship).Should().BeFalse();
        }

        [Fact]
        public void PlaceShip_ShouldNot_AllowShipsToOverlap()
        {
            Placement placement = new Placement(new Coordinate(5, 5), Direction.Up, Player.One);

            // Size of 5
            IShip ship = new Carrier();

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            // Size of 4
            ship = new Bship();

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(5, 5), Direction.Right, Player.One);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            // We'll check edges by inverting our placement logic
            var offset = ship.Size - 1;

            // Checks up edges
            placement = new Placement(new Coordinate(5, 5 + offset), Direction.Up, Player.One);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            // Checks down edges
            placement = new Placement(new Coordinate(5, 5 - offset), Direction.Down, Player.One);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            // Checks right edges
            placement = new Placement(new Coordinate(5 - offset, 5), Direction.Right, Player.One);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            // Checks left edges
            placement = new Placement(new Coordinate(5 + offset, 5), Direction.Left, Player.One);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();
        }

        [Fact]
        public void PlaceShip_Should_AllowMultipleShipPlacement()
        {
            Placement placement = new Placement(new Coordinate(5, 5), Direction.Up, Player.One);

            // Size of 5
            IShip ship = new Carrier();

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            // Size of 4
            ship = new Bship();

            placement = new Placement(new Coordinate(5 - ship.Size, 5), Direction.Right, Player.One);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            // Size of 3
            ship = new Destroyer();

            placement = new Placement(new Coordinate(5 + ship.Size, 5), Direction.Left, Player.One);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            // Size of 2
            ship = new Patrol_Boat();

            placement = new Placement(new Coordinate(2, 2), Direction.Left, Player.One);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            // Size of 3
            ship = new Submarine();

            placement = new Placement(new Coordinate(9, 9), Direction.Left, Player.One);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();
        }

        [Fact]
        public void MarkCoordinate_Should_ReturnResultTuple()
        {
            sut.PlaceShip(game, new Placement(new Coordinate(5, 5), Direction.Up, Player.Two), new Carrier());

            game.TransitionGamePhase();

            sut.MarkCoordinate(game, new Coordinate(), Player.One, Player.Two).Should().Be((Result.Miss, null));

            sut.MarkCoordinate(game, new Coordinate(5, 5), Player.One, Player.Two).Should().Be((Result.Hit, null));

            sut.MarkCoordinate(game, new Coordinate(5, 4), Player.One, Player.Two).Should().Be((Result.Hit, null));

            sut.MarkCoordinate(game, new Coordinate(5, 3), Player.One, Player.Two).Should().Be((Result.Hit, null));

            sut.MarkCoordinate(game, new Coordinate(5, 2), Player.One, Player.Two).Should().Be((Result.Hit, null));

            sut.MarkCoordinate(game, new Coordinate(5, 1), Player.One, Player.Two).Should().Be((Result.Sunk, ShipName.Carrier));
        }

        [Fact]
        public void MarkCoordinate_ShouldNot_AllowDuplicateMoves()
        {
            game.TransitionGamePhase();

            sut.MarkCoordinate(game, new Coordinate(), Player.One, Player.Two).Should().Be((Result.Miss, null));

            Action act = () => sut.MarkCoordinate(game, new Coordinate(), Player.One, Player.Two);

            act.Should().Throw<InvalidOperationException>().WithMessage("Move would be duplicate");
        }

        [Fact]
        public void CheckGameState_Should_ReturnNull()
        {
            sut.CheckMatchState(game).Should().BeNull();

            sut.PlaceShip(game, new Placement(new Coordinate(5, 5), Direction.Up, Player.Two), new Carrier());

            sut.CheckMatchState(game).Should().BeNull();

            sut.PlaceShip(game, new Placement(new Coordinate(5, 5), Direction.Up, Player.One), new Carrier());

            sut.CheckMatchState(game).Should().BeNull();
        }

        [Fact]
        public void CheckGameState_Should_ReturnPlayer()
        {
            // Player 1 Ships
            sut.PlaceShip(game, new Placement(new Coordinate(0, 0), Direction.Down, Player.One), new Carrier()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(1, 0), Direction.Down, Player.One), new Bship()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(2, 0), Direction.Down, Player.One), new Submarine()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(3, 0), Direction.Down, Player.One), new Patrol_Boat()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(4, 0), Direction.Down, Player.One), new Destroyer()).Should().BeTrue();

            // Player 2 Ships
            sut.PlaceShip(game, new Placement(new Coordinate(0, 0), Direction.Down, Player.Two), new Carrier()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(1, 0), Direction.Down, Player.Two), new Bship()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(2, 0), Direction.Down, Player.Two), new Submarine()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(3, 0), Direction.Down, Player.Two), new Patrol_Boat()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(4, 0), Direction.Down, Player.Two), new Destroyer()).Should().BeTrue();

            game.TransitionGamePhase();

            // Sink Carrier
            sut.MarkCoordinate(game, new Coordinate(0, 0), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(0, 1), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(0, 2), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(0, 3), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(0, 4), Player.Two, Player.One).Should().Be((Result.Sunk, ShipName.Carrier));

            sut.CheckMatchState(game).Should().BeNull();

            // Sink Bhip
            sut.MarkCoordinate(game, new Coordinate(1, 0), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(1, 1), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(1, 2), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(1, 3), Player.Two, Player.One).Should().Be((Result.Sunk, ShipName.Battleship));

            sut.CheckMatchState(game).Should().BeNull();

            // Sink Sub
            sut.MarkCoordinate(game, new Coordinate(2, 0), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(2, 1), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(2, 2), Player.Two, Player.One).Should().Be((Result.Sunk, ShipName.Submarine));

            sut.CheckMatchState(game).Should().BeNull();

            // Sink Patrol Boat
            sut.MarkCoordinate(game, new Coordinate(3, 0), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(3, 1), Player.Two, Player.One).Should().Be((Result.Sunk, ShipName.Patrol_Boat));

            sut.CheckMatchState(game).Should().BeNull();

            // Sink Destroyer
            sut.MarkCoordinate(game, new Coordinate(4, 0), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(4, 1), Player.Two, Player.One).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(4, 2), Player.Two, Player.One).Should().Be((Result.Sunk, ShipName.Destroyer));

            // All ships should now be sunk for player one
            sut.CheckMatchState(game).Should().Be(Player.Two);
        }
    }
}
