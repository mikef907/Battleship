using Bship = Battleship.WASM.Shared.BattleShip;

namespace Battleship.Tests
{
    public class BattleshipGameEngineTests
    {
        private readonly BattleshipGameEngine sut;
        private readonly BattleshipMatch game;
        private readonly Player playerOne;
        private readonly Player playerTwo;

        public BattleshipGameEngineTests()
        {
            sut = new BattleshipGameEngine(Substitute.For<ILoggerFactory>());
            playerOne = BattleshipFactory.CreatePlayer("PlayerOne");
            playerTwo = BattleshipFactory.CreatePlayer("PlayerTwo");
            game = BattleshipFactory.CreateMatch(playerOne, playerTwo);
        }

        [Fact]
        public void PlaceShip_Should_ReturnTrue()
        {
            Placement placement = new Placement(new Coordinate(5, 5), Direction.Up, playerOne);
            Carrier? ship = new Carrier();

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            placement = new Placement(new Coordinate(5, 5), Direction.Up, playerTwo);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();
        }

        [Fact]
        public void PlaceShip_Should_ReplaceShipsWhenAlreadyPlaced()
        {
            Placement placement = new Placement(new Coordinate(5, 5), Direction.Up, playerOne);
            Carrier? ship = new Carrier();

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            placement = new Placement(new Coordinate(5, 5), Direction.Right, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            placement = new Placement(new Coordinate(1, 1), Direction.Right, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            placement = new Placement(new Coordinate(9, 1), Direction.Left, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();
        }

        [Fact]
        public void PlaceShip_ShouldNot_AllowShipsToBePlacedOutOfBounds()
        {
            Placement placement = new Placement(new Coordinate(10, 10), Direction.Up, playerOne);
            Carrier? ship = new Carrier();

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(-1, -1), Direction.Up, playerOne);
            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(0, -1), Direction.Up, playerOne);
            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(9, 10), Direction.Up, playerOne);
            sut.PlaceShip(game, placement, ship).Should().BeFalse();
        }

        [Fact]
        public void PlaceShip_ShouldNot_AllowShipsToOverlap()
        {
            Placement placement = new Placement(new Coordinate(5, 5), Direction.Up, playerOne);

            // Size of 5
            IShip ship = new Carrier();

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            // Size of 4
            ship = new Bship();

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(5, 5), Direction.Right, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            // We'll check edges by inverting our placement logic
            int offset = ship.Size - 1;

            // Checks up edges
            placement = new Placement(new Coordinate(5, 5 + offset), Direction.Right, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(5, 5 - offset), Direction.Left, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(5 - offset, 5), Direction.Down, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();

            placement = new Placement(new Coordinate(5 + offset, 5), Direction.Up, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeFalse();
        }

        [Fact]
        public void PlaceShip_Should_AllowMultipleShipPlacement()
        {
            Placement placement = new Placement(new Coordinate(5, 5), Direction.Up, playerOne);

            // Size of 5
            IShip ship = new Carrier();

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            // Size of 4
            ship = new Bship();

            placement = new Placement(new Coordinate(8, 5), Direction.Right, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            // Size of 3
            ship = new Destroyer();

            placement = new Placement(new Coordinate(6, 5), Direction.Right, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            // Size of 2
            ship = new Patrol_Boat();

            placement = new Placement(new Coordinate(2, 2), Direction.Left, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();

            // Size of 3
            ship = new Submarine();

            placement = new Placement(new Coordinate(9, 9), Direction.Left, playerOne);

            sut.PlaceShip(game, placement, ship).Should().BeTrue();
        }

        [Fact]
        public void MarkCoordinate_Should_ReturnResultTuple()
        {
            sut.PlaceShip(game, new Placement(new Coordinate(5, 5), Direction.Left, playerTwo), new Carrier());

            game.TransitionGamePhase();

            sut.MarkCoordinate(game, new Coordinate(), playerOne, playerTwo).Should().Be((Result.Miss, null));

            sut.MarkCoordinate(game, new Coordinate(5, 5), playerOne, playerTwo).Should().Be((Result.Hit, null));

            sut.MarkCoordinate(game, new Coordinate(5, 4), playerOne, playerTwo).Should().Be((Result.Hit, null));

            sut.MarkCoordinate(game, new Coordinate(5, 3), playerOne, playerTwo).Should().Be((Result.Hit, null));

            sut.MarkCoordinate(game, new Coordinate(5, 2), playerOne, playerTwo).Should().Be((Result.Hit, null));

            sut.MarkCoordinate(game, new Coordinate(5, 1), playerOne, playerTwo).Should().Be((Result.Sunk, ShipName.Carrier));
        }

        [Fact]
        public void MarkCoordinate_ShouldNot_AllowDuplicateMoves()
        {
            game.TransitionGamePhase();

            sut.MarkCoordinate(game, new Coordinate(), playerOne, playerTwo).Should().Be((Result.Miss, null));

            Action act = () => sut.MarkCoordinate(game, new Coordinate(), playerOne, playerTwo);

            act.Should().Throw<InvalidOperationException>().WithMessage("Move would be duplicate");
        }

        [Fact]
        public void CheckGameState_Should_ReturnNull()
        {
            sut.CheckMatchState(game).Should().BeNull();

            sut.PlaceShip(game, new Placement(new Coordinate(5, 5), Direction.Up, playerTwo), new Carrier());

            sut.CheckMatchState(game).Should().BeNull();

            sut.PlaceShip(game, new Placement(new Coordinate(5, 5), Direction.Up, playerOne), new Carrier());

            sut.CheckMatchState(game).Should().BeNull();
        }

        [Fact]
        public void CheckGameState_Should_ReturnPlayer()
        {
            // Player 1 Ships
            sut.PlaceShip(game, new Placement(new Coordinate(0, 0), Direction.Right, playerOne), new Carrier()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(1, 0), Direction.Right, playerOne), new Bship()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(2, 0), Direction.Right, playerOne), new Submarine()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(3, 0), Direction.Right, playerOne), new Patrol_Boat()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(4, 0), Direction.Right, playerOne), new Destroyer()).Should().BeTrue();

            // Player 2 Ships
            sut.PlaceShip(game, new Placement(new Coordinate(0, 0), Direction.Right, playerTwo), new Carrier()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(1, 0), Direction.Right, playerTwo), new Bship()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(2, 0), Direction.Right, playerTwo), new Submarine()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(3, 0), Direction.Right, playerTwo), new Patrol_Boat()).Should().BeTrue();
            sut.PlaceShip(game, new Placement(new Coordinate(4, 0), Direction.Right, playerTwo), new Destroyer()).Should().BeTrue();

            game.TransitionGamePhase();

            // Sink Carrier
            sut.MarkCoordinate(game, new Coordinate(0, 0), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(0, 1), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(0, 2), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(0, 3), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(0, 4), playerTwo, playerOne).Should().Be((Result.Sunk, ShipName.Carrier));

            sut.CheckMatchState(game).Should().BeNull();

            // Sink Bhip
            sut.MarkCoordinate(game, new Coordinate(1, 0), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(1, 1), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(1, 2), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(1, 3), playerTwo, playerOne).Should().Be((Result.Sunk, ShipName.Battleship));

            sut.CheckMatchState(game).Should().BeNull();

            // Sink Sub
            sut.MarkCoordinate(game, new Coordinate(2, 0), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(2, 1), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(2, 2), playerTwo, playerOne).Should().Be((Result.Sunk, ShipName.Submarine));

            sut.CheckMatchState(game).Should().BeNull();

            // Sink Patrol Boat
            sut.MarkCoordinate(game, new Coordinate(3, 0), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(3, 1), playerTwo, playerOne).Should().Be((Result.Sunk, ShipName.Patrol_Boat));

            sut.CheckMatchState(game).Should().BeNull();

            // Sink Destroyer
            sut.MarkCoordinate(game, new Coordinate(4, 0), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(4, 1), playerTwo, playerOne).Should().Be((Result.Hit, null));
            sut.MarkCoordinate(game, new Coordinate(4, 2), playerTwo, playerOne).Should().Be((Result.Sunk, ShipName.Destroyer));

            // All ships should now be sunk for player one
            sut.CheckMatchState(game).Should().Be(playerTwo);
        }
    }
}
