using Bship = Battleship.Game.Models.Battleship;

namespace Battleship.Tests
{
    public class BattleshipGameEngineTests
    {
        private readonly BattleshipGameEngine sut;
        private readonly BattleshipGame game;

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
    }
}
