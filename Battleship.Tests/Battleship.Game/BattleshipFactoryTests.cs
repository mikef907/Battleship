namespace Battleship.Tests
{
    public class BattleshipFactoryTests
    {
        [Fact]
        public void CreatePlayer_Should_ReturnPlayer()
        {
            BattleshipFactory.CreatePlayer("PlayerOne").Should().BeAssignableTo<Player>().Subject.Guid.Should().NotBeEmpty();

            Guid guid = Guid.NewGuid();

            BattleshipFactory.CreatePlayer("PlayerOne", guid).Guid.Should().Be(guid);

        }

        [Fact]
        public void CreateMatch_Should_ReturnBattleshipGame() => BattleshipFactory.CreateMatch(new Player(Guid.NewGuid(), "PlayerOne"), new Player(Guid.NewGuid(), "PlayerTwo"))
                .Should().NotBeNull().And.BeAssignableTo<BattleshipMatch>();
    }
}