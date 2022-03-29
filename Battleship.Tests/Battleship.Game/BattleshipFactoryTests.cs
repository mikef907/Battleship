namespace Battleship.Tests
{
    public class BattleshipFactoryTests
    {
        [Fact]
        public void Create_Should_ReturnBattleshipGame()
        {
            BattleshipFactory.Create(new Player(Guid.NewGuid(), "PlayerOne"), new Player(Guid.NewGuid(), "PlayerTwo"))
                .Should().NotBeNull().And.BeAssignableTo<BattleshipMatch>();
        }
    }
}