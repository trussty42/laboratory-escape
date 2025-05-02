using LaboratoryEscape.Core;
using NUnit.Framework;

namespace LaboratoryEscape.Tests;

[TestFixture]
public class PlayerTests
{
    private Cell[,] _grid;

    [SetUp]
    public void Setup()
    {
        _grid = new Cell[20, 20];
        for (var x = 0; x < 20; x++)
        for (var y = 0; y < 20; y++)
            _grid[x, y] = new Cell(CellType.Normal);
    }

    [Test]
    public void Constructor_SetsInitialValues()
    {
        var player = new Player(5, 5, 100);

        Assert.AreEqual(5, player.X);
        Assert.AreEqual(5, player.Y);
        Assert.AreEqual(100, player.Health);
        Assert.IsFalse(player.HasEscaped);
    }

    [Test]
    public void ApplyMovement_UpdatesPosition()
    {
        var player = new Player(5, 5, 100)
        {
            CurrentCommand = new PlayerCommand { DeltaX = 0.5f, DeltaY = 0.5f }
        };

        player.ApplyMovement(_grid);

        Assert.AreEqual(5.5f, player.X);
        Assert.AreEqual(5.5f, player.Y);
    }

    [Test]
    public void ApplyMovement_DoesNotMoveThroughWalls()
    {
        _grid[6, 5].Type = CellType.Wall;
        var player = new Player(5, 5, 100)
        {
            CurrentCommand = new PlayerCommand { DeltaX = 1.5f }
        };

        player.ApplyMovement(_grid);

        Assert.Less(player.X, 6);
    }

    [Test]
    public void ApplyCellEffect_Damage_ReducesHealth()
    {
        var player = new Player(0, 0, 100);
        var damageCell = new Cell(CellType.Damage);

        player.ApplyCellEffect(damageCell);

        Assert.AreEqual(90, player.Health);
    }

    [Test]
    public void ApplyCellEffect_Exit_SetsHasEscaped()
    {
        var player = new Player(0, 0, 100);
        var exitCell = new Cell(CellType.Exit);

        player.ApplyCellEffect(exitCell);

        Assert.IsTrue(player.HasEscaped);
    }
}