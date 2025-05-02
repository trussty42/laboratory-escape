using LaboratoryEscape.Core;

namespace LaboratoryEscape.Controller;

public class GameController : IDisposable
{
    public GameModel Model { get; }
    public int LevelNumber { get; }

    public GameController(int level)
    {
        LevelNumber = level;
        Model = new GameModel(20, 20, 0, 0);
        InitializeLevel(level);
    }

    private void InitializeLevel(int level)
    {
        Model.Guards.Clear();

        for (var x = 0; x < Model.Grid.GetLength(0); x++)
        for (var y = 0; y < Model.Grid.GetLength(1); y++)
            Model.Grid[x, y] = new Cell(CellType.Normal);

        switch (level)
        {
            case 1:
                InitializeLevel1();
                break;
            case 2:
                InitializeLevel2();
                break;
            case 3:
                InitializeLevel3();
                break;
            case 4:
                InitializeLevel4();
                break;
            case 5:
                InitializeLevel5();
                break;
        }
    }

    private void InitializeLevel1()
    {
        SetWall(5, 0, 5, 14);
        SetWall(10, 4, 10, 19);
        SetWall(15, 0, 15, 14);

        Model.Grid[19, 0].Type = CellType.Exit;

        Model.Grid[6, 14].Type = CellType.Speed;
        Model.Grid[7, 14].Type = CellType.Speed;
        Model.Grid[8, 14].Type = CellType.Speed;
        Model.Grid[9, 14].Type = CellType.Speed;

        Model.Grid[6, 12].Type = CellType.Damage;
        Model.Grid[9, 12].Type = CellType.Damage;
        Model.Grid[6, 13].Type = CellType.Damage;
        Model.Grid[9, 13].Type = CellType.Damage;

        Model.Grid[10, 2].Type = CellType.Damage;
        Model.Grid[10, 3].Type = CellType.Damage;

        Model.Grid[11, 4].Type = CellType.Speed;
        Model.Grid[12, 4].Type = CellType.Speed;
        Model.Grid[13, 4].Type = CellType.Speed;
        Model.Grid[14, 4].Type = CellType.Speed;

        Model.Guards.Add(new Guard(8, 10, Direction.Right, Model.Grid));
    }

    private void InitializeLevel2()
    {
        SetWall(3, 0, 3, 16);
        SetWall(3, 16, 16, 16);
        SetWall(16, 16, 16, 4);
        SetWall(16, 4, 6, 4);
        SetWall(6, 4, 6, 12);
        SetWall(6, 12, 19, 12);

        Model.Grid[19, 0].Type = CellType.Exit;

        Model.Guards.Add(new Guard(10, 5, Direction.Down, Model.Grid));
        Model.Guards.Add(new Guard(17, 14, Direction.Left, Model.Grid));

        Model.Grid[15, 13].Type = CellType.Speed;
        Model.Grid[15, 14].Type = CellType.Speed;
        Model.Grid[15, 15].Type = CellType.Speed;
        Model.Grid[16, 13].Type = CellType.Speed;
        Model.Grid[16, 14].Type = CellType.Speed;
        Model.Grid[16, 15].Type = CellType.Speed;

        Model.Grid[6, 0].Type = CellType.Damage;
        Model.Grid[6, 1].Type = CellType.Damage;
    }

    private void InitializeLevel3()
    {
        SetWall(3, 0, 19, 0);
        SetWall(0, 19, 19, 19);
        SetWall(0, 3, 0, 19);
        SetWall(19, 0, 19, 19);

        SetWall(5, 5, 15, 5);
        SetWall(5, 15, 15, 15);
        SetWall(5, 5, 5, 15);
        SetWall(15, 5, 15, 8);
        SetWall(15, 11, 15, 15);

        Model.Grid[10, 10].Type = CellType.Exit;

        Model.Guards.Add(new Guard(4, 4, Direction.Right, Model.Grid));
        Model.Guards.Add(new Guard(17, 2, Direction.Left, Model.Grid));
        Model.Guards.Add(new Guard(10, 17, Direction.Up, Model.Grid));
    }

    private void InitializeLevel4()
    {
        var flag = false;
        for (var y = 2; y < 20; y += 4)
        for (var x = 2; x < 20; x += 4)
            if (x > 0 && y > 0)
            {
                if (x == 18 && y == 18)
                    flag = true;
                if (!flag)
                    SetWall(x, y, x + 1, y + 1);
            }

        Model.Grid[19, 19].Type = CellType.Exit;

        Model.Guards.Add(new Guard(5, 4, Direction.Right, Model.Grid));
        Model.Guards.Add(new Guard(13, 4, Direction.Left, Model.Grid));
        Model.Guards.Add(new Guard(9, 15, Direction.Right, Model.Grid));
        Model.Guards.Add(new Guard(17, 15, Direction.Left, Model.Grid));
    }

    private void InitializeLevel5()
    {
        int[,] mazeWalls =
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        for (var y = 0; y < 20; y++)
        for (var x = 0; x < 20; x++)
        {
            if (mazeWalls[y, x] == 1)
                Model.Grid[x, y].Type = CellType.Wall;
            if (mazeWalls[y, x] == 2)
                Model.Grid[x, y].Type = CellType.Speed;
        }

        Model.Grid[19, 18].Type = CellType.Exit;

        Model.Guards.Add(new Guard(19, 2, Direction.Left, Model.Grid));
        Model.Guards.Add(new Guard(19, 6, Direction.Left, Model.Grid));
        Model.Guards.Add(new Guard(19, 10, Direction.Left, Model.Grid));
        Model.Guards.Add(new Guard(19, 14, Direction.Left, Model.Grid));
        Model.Guards.Add(new Guard(12, 0, Direction.Left, Model.Grid));
        Model.Guards.Add(new Guard(12, 4, Direction.Left, Model.Grid));
        Model.Guards.Add(new Guard(12, 8, Direction.Left, Model.Grid));
        Model.Guards.Add(new Guard(12, 12, Direction.Left, Model.Grid));
        Model.Guards.Add(new Guard(12, 16, Direction.Left, Model.Grid));
    }

    private void SetWall(int startX, int startY, int endX, int endY)
    {
        for (var x = startX; x <= endX; x++)
        for (var y = startY; y <= endY; y++)
            if (x >= 0 && x < Model.Grid.GetLength(0) &&
                y >= 0 && y < Model.Grid.GetLength(1))
                Model.Grid[x, y].Type = CellType.Wall;
    }

    public void ProcessInput(bool w, bool a, bool s, bool d, bool shift)
    {
        var command = new PlayerCommand();
        var speed = 0.05f * (Model.Player.Speed / 100f);

        if (w) command.DeltaY = -speed;
        if (s) command.DeltaY = speed;
        if (a) command.DeltaX = -speed;
        if (d) command.DeltaX = speed;

        Model.Player.CurrentCommand = command;
    }

    public void GameStep()
    {
        if (Model.IsGameOver) return;

        Model.Player.ApplyMovement(Model.Grid);

        foreach (var guard in Model.Guards)
        {
            guard.Update(Model.Player);
            if (guard.IsAlerted && guard.DistanceTo(Model.Player) < 0.5f)
                Model.Player.Health = 0;
        }

        Model.Turn++;
    }

    public void Dispose()
    {
    }
}