using LaboratoryEscape.Controller;
using LaboratoryEscape.Core;
using NUnit.Framework;

namespace LaboratoryEscape.Tests;

[TestFixture]
public class GameControllerTests
{
    [Test]
    public void ProcessInput_UpdatesPlayerCommand()
    {
        var controller = new GameController(1);

        controller.ProcessInput(true, false, false, false, false);

        Assert.Less(controller.Model.Player.CurrentCommand.DeltaY, 0);
    }

    [Test]
    public void GameStep_PlayerDies_WhenGuardTooClose()
    {
        var controller = new GameController(1);
        var guard = controller.Model.Guards[0];
        guard.Alert(controller.Model.Player);

        guard.X = controller.Model.Player.X;
        guard.Y = controller.Model.Player.Y;

        controller.GameStep();

        Assert.IsFalse(controller.Model.Player.IsAlive);
    }

    [Test]
    public void InitializeLevel1_CreatesCorrectMap()
    {
        var controller = new GameController(1);

        Assert.AreEqual(1, controller.Model.Guards.Count);
        Assert.AreEqual(CellType.Exit, controller.Model.Grid[19, 0].Type);
    }
}