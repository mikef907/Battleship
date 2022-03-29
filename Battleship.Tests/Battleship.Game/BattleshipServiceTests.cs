namespace Battleship.Tests
{
    public class BattleshipServiceTests: BattleshipService
    {
        private readonly Player playerOne = new Player(Guid.NewGuid(), "PlayerOne");
        private readonly Player playerTwo = new Player(Guid.NewGuid(), "PlayerTwo");
        private readonly Guid matchId;

        public BattleshipServiceTests()
            : base(Substitute.ForPartsOf<BattleshipGameEngine>(Substitute.For<ILogger>()), Substitute.For<ILogger>())
        {
            matches.Clear();
            matchId = NewGame(playerOne, playerTwo);
        }

        [Fact]
        public void NewGame_Should_ReturnNonEmptyGuid()
        {
            matchId.Should().NotBeEmpty();
            matches.Should().HaveCount(1).And.ContainKey(matchId);
        }

        [Fact]
        public void CheckMatchState_ShouldReturnNull()
        {
            CheckMatchState(matchId).Should().BeNull();
        }

        [Fact]
        public void CheckMatchState_ShouldReturnPlayer()
        {
            var match = matches[matchId];

            engine.CheckMatchState(match).Returns(playerOne);

            CheckMatchState(matchId).Should().Be(playerOne);
        }

        [Fact]
        public void GetMaxShips_Should_ReturnIntGreaterThanZero()
        {
            GetMaxShips(matchId).Should().BeGreaterThan(0);
        }

        [Fact]
        public void TryStartGame_Should_ReturnBool()
        {
            TryStartGame(matchId, out var gamePhase).Should().BeFalse();

            gamePhase.Should().Be(GamePhase.Setup);

            var match = Substitute.For<BattleshipMatch>(playerOne, playerTwo);

            match.AllShipsPlaced.Returns(true);

            matches.Add(match.Id, match);

            TryStartGame(match.Id, out gamePhase).Should().BeTrue();

            gamePhase.Should().Be(GamePhase.Main);
        }

        [Fact]
        public void TryPlaceShip_Should_ReturnBool()
        {
            var placement = new Placement { Coordinate = new Coordinate(0, 0), Direction = Direction.Down, Owner = playerOne };
            
            TryPlaceShip(matchId, placement, new Carrier(), out int shipsPlaced).Should().BeTrue();

            shipsPlaced.Should().Be(1);
        }

        [Fact]
        public void FireShot_Should_ReturnResult()
        {
            var match = new BattleshipMatch(playerOne, playerTwo);

            match.TransitionGamePhase();

            matches.Add(match.Id, match);

            FireShot(match.Id, new Coordinate(0, 0), playerOne, playerTwo).Should().Be((Result.Miss, null));
        }
    }
}
