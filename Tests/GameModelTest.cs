using NUnit.Framework;

namespace LaboratoryEscape.Tests;

[TestFixture]
public class GameModelTests
{
    [Test]
    public void Constructor_InitializesCorrectly()
    {
        var model = new GameModel(20, 20, 5, 5);

        Assert.AreEqual(20, model.Grid.GetLength(0));
        Assert.AreEqual(20, model.Grid.GetLength(1));
        Assert.AreEqual(5, model.Player.X);
        Assert.AreEqual(5, model.Player.Y);
        Assert.IsEmpty(model.Guards);
    }

    [Test]
    public void IsGameOver_ReturnsTrue_WhenPlayerDead()
    {
        var model = new GameModel(20, 20, 5, 5);
        model.Player.Health = 0;

        Assert.IsTrue(model.IsGameOver);
    }
}