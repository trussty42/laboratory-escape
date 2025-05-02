using LaboratoryEscape.Core;
using NUnit.Framework;

namespace LaboratoryEscape.Tests;

[TestFixture]
public class GuardTests
{
    private Cell[,] _grid;
    private Player _player;

    [SetUp]
    public void Setup()
    {
        _grid = new Cell[20, 20];
        for (var x = 0; x < 20; x++)
        for (var y = 0; y < 20; y++)
            _grid[x, y] = new Cell(CellType.Normal);

        _player = new Player(5, 5, 100);
    }

    [Test]
    public void Constructor_SetsInitialValues()
    {
        var guard = new Guard(10, 10, Direction.Right, _grid);

        Assert.AreEqual(10, guard.X);
        Assert.AreEqual(10, guard.Y);
        Assert.AreEqual(Direction.Right, guard.Facing);
        Assert.IsFalse(guard.IsAlerted);
        Assert.IsNull(guard.Target);
    }

    [Test]
    public void CanSee_ReturnsFalse_WhenPlayerTooFar()
    {
        var guard = new Guard(1, 1, Direction.Right, _grid);
        var farPlayer = new Player(20, 20, 100);

        Assert.IsFalse(guard.CanSee(farPlayer));
    }

    [Test]
    public void CanSee_ReturnsFalse_WhenWallInBetween()
    {
        var guard = new Guard(1, 1, Direction.Right, _grid);
        var player = new Player(5, 1, 100);

        _grid[3, 1].Type = CellType.Wall;

        Assert.IsFalse(guard.CanSee(player));
    }

    [Test]
    public void Alert_SetsTargetAndAlertedStatus()
    {
        var guard = new Guard(1, 1, Direction.Right, _grid);

        guard.Alert(_player);

        Assert.IsTrue(guard.IsAlerted);
        Assert.AreEqual(_player, guard.Target);
    }

    [Test]
    public void MoveTowards_UpdatesPositionCorrectly()
    {
        var guard = new Guard(1, 1, Direction.Right, _grid);
        var target = new Player(3, 3, 100);

        guard.Alert(target);
        guard.MoveTowards(target);

        Assert.IsTrue(guard.X > 1 || guard.Y > 1);
    }
}