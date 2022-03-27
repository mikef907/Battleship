namespace Battleship.Tests
{
    public class BattleshipFactoryTests
    {
        [Fact]
        public void Create_Should_ReturnBattleshipGame()
        {
            BattleshipFactory.Create().Should().NotBeNull().And.BeAssignableTo<BattleshipMatch>();
        }
    }
}