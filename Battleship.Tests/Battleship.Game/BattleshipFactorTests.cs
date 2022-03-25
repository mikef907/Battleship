namespace Battleship.Tests
{
    public class BattleshipFactorTests
    {
        [Fact]
        public void Create_Should_ReturnBattleshipGame()
        {
            BattleshipFactory.Create().Should().NotBeNull().And.BeAssignableTo<BattleshipGame>();
        }
    }
}