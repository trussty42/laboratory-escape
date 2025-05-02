using LaboratoryEscape.Core;
using LaboratoryEscape.Interfaces;

namespace LaboratoryEscape;

public class Player : ICreature
{
    private int _speed = 50;
    private float _x;
    private float _y;
    private Direction _facingDirection = Direction.Right;
    private int _currentAnimationFrame;
    private int _animationCounter;
    private const int AnimationDelay = 8;

    public Player(float x, float y, int health)
    {
        X = x;
        Y = y;
        Health = health;
        CurrentCommand = new PlayerCommand();
    }

    public bool HasEscaped { get; private set; }
    public PlayerCommand CurrentCommand { get; set; }

    public float X
    {
        get => _x;
        set => _x = Math.Clamp(value, 0, float.MaxValue);
    }

    public float Y
    {
        get => _y;
        set => _y = Math.Clamp(value, 0, float.MaxValue);
    }

    public int Health { get; set; }
    public int Visibility { get; set; }
    public bool IsAlive => Health > 0 && !HasEscaped;

    public float Speed => _speed;

    public Direction FacingDirection
    {
        get => _facingDirection;
        set
        {
            if (_facingDirection != value)
            {
                _facingDirection = value;
                _currentAnimationFrame = 0;
            }
        }
    }

    public bool CanSee(ICreature other)
    {
        return DistanceTo(other) <= 7;
    }

    public float DistanceTo(ICreature other)
    {
        return (float)Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2));
    }

    public void Move(Direction direction, Cell[,] grid)
    {
    }

    public void ApplyMovement(Cell[,] grid)
    {
        if (Math.Abs(CurrentCommand.DeltaX) > 0.01f)
            FacingDirection = CurrentCommand.DeltaX > 0 ? Direction.Right : Direction.Left;

        var newX = X + CurrentCommand.DeltaX;
        var newY = Y + CurrentCommand.DeltaY;

        if (CanMoveTo(newX, Y, grid)) X = newX;
        if (CanMoveTo(X, newY, grid)) Y = newY;

        UpdateAnimation(Math.Abs(CurrentCommand.DeltaX) > 0.01f || Math.Abs(CurrentCommand.DeltaY) > 0.01f);
        ApplyCellEffect(grid[(int)X, (int)Y]);
    }

    private void UpdateAnimation(bool isMoving)
    {
        _animationCounter++;
        if (_animationCounter >= AnimationDelay)
        {
            _animationCounter = 0;
            _currentAnimationFrame = isMoving ? (_currentAnimationFrame + 1) % 5 : (_currentAnimationFrame + 1) % 2;
        }
    }

    public string GetCurrentAnimationFrame()
    {
        var isRight = FacingDirection == Direction.Right;
        var isMoving = Math.Abs(CurrentCommand.DeltaX) > 0.01f || Math.Abs(CurrentCommand.DeltaY) > 0.01f;

        return isMoving
            ?
            isRight
                ? $"CharacterRun{_currentAnimationFrame + 1}Reversed.png"
                : $"CharacterRun{_currentAnimationFrame + 1}.png"
            :
            isRight
                ? $"CharacterAfk{_currentAnimationFrame + 1}Reversed.png"
                : $"CharacterAfk{_currentAnimationFrame + 1}.png";
    }

    private bool PositionIsWall(float x, float y, Cell[,] grid)
    {
        var cellX = (int)x;
        var cellY = (int)y;

        return cellX < 0 || cellX >= grid.GetLength(0) ||
               cellY < 0 || cellY >= grid.GetLength(1) ||
               grid[cellX, cellY].Type == CellType.Wall;
    }

    private bool CanMoveTo(float x, float y, Cell[,] grid)
    {
        var offset = 0.1f;
        return !PositionIsWall(x + offset, y + offset, grid) &&
               !PositionIsWall(x + 1 - offset, y + offset, grid) &&
               !PositionIsWall(x + offset, y + 1 - offset, grid) &&
               !PositionIsWall(x + 1 - offset, y + 1 - offset, grid);
    }

    public void ApplyCellEffect(Cell cell)
    {
        switch (cell.Type)
        {
            case CellType.Damage:
                Health -= 10;
                break;
            case CellType.Exit:
                HasEscaped = true;
                break;
            case CellType.Speed:
                _speed = 300;
                break;
            case CellType.Normal:
                _speed = 150;
                break;
        }

        Health = Math.Clamp(Health, 0, 100);
    }
}

public class PlayerCommand
{
    public float DeltaX { get; set; }
    public float DeltaY { get; set; }
}