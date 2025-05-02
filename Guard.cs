using LaboratoryEscape.Core;
using LaboratoryEscape.Interfaces;

namespace LaboratoryEscape;

public class Guard : ICreature
{
    private readonly Cell[,] _grid;
    private float _moveCooldown;
    private int _lostPlayerTimer;
    private const int MaxLostPlayerTime = 60;
    private int _currentAnimationFrame;
    private int _animationCounter;
    private const int AnimationDelay = 8;

    public Guard(float x, float y, Direction facing, Cell[,] grid)
    {
        X = x;
        Y = y;
        Facing = facing;
        _grid = grid;
    }

    public Direction Facing { get; private set; }
    public int ViewDistance => 19;
    public int ViewAngle => 60;
    public bool IsAlerted { get; private set; }
    public ICreature Target { get; private set; } = null!;

    public float X { get; set; }
    public float Y { get; set; }
    public int Health => 100;
    public bool IsAlive => true;
    public int Visibility { get; set; }
    public float Speed => 0.13f;

    public string GetCurrentAnimationFrame()
    {
        if (!IsAlerted) return "GuardAfk1Reversed.png";

        return Facing switch
        {
            Direction.Left => $"GuardRun{_currentAnimationFrame + 1}Reversed.png",
            _ => $"GuardRun{_currentAnimationFrame + 1}.png"
        };
    }

    private void UpdateAnimation()
    {
        _animationCounter++;
        if (_animationCounter >= AnimationDelay)
        {
            _animationCounter = 0;
            if (IsAlerted) _currentAnimationFrame = (_currentAnimationFrame + 1) % 3;
        }
    }

    public bool CanSee(ICreature other)
    {
        if (!other.IsAlive) return false;

        var distance = DistanceTo(other);
        if (distance > ViewDistance) return false;

        var angle = (float)(Math.Atan2(other.Y - Y, other.X - X) * 180 / Math.PI);
        float facingAngle = Facing switch
        {
            Direction.Up => 270,
            Direction.Down => 90,
            Direction.Left => 180,
            Direction.Right => 0,
            _ => 0
        };

        var angleDiff = Math.Abs(angle - facingAngle);
        angleDiff = Math.Min(angleDiff, 360 - angleDiff);
        return !(angleDiff > ViewAngle / 2) && HasClearLineOfSight(other);
    }

    private bool HasClearLineOfSight(ICreature target)
    {
        var x0 = (int)(X + 0.5f);
        var y0 = (int)(Y + 0.5f);
        var x1 = (int)(target.X + 0.5f);
        var y1 = (int)(target.Y + 0.5f);

        var dx = Math.Abs(x1 - x0);
        var dy = Math.Abs(y1 - y0);
        var sx = x0 < x1 ? 1 : -1;
        var sy = y0 < y1 ? 1 : -1;
        var err = dx - dy;

        while (true)
        {
            if (x0 < 0 || x0 >= _grid.GetLength(0) || y0 < 0 || y0 >= _grid.GetLength(1))
                return false;

            if (_grid[x0, y0].Type == CellType.Wall)
                return false;

            if (x0 == x1 && y0 == y1)
                return true;

            var e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    public float DistanceTo(ICreature other)
    {
        return (float)Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2));
    }

    public void Move(Direction direction, Cell[,] grid)
    {
    }

    public void Update(Player player)
    {
        if (!player.IsAlive) return;

        var canSeePlayer = CanSee(player);

        if (IsAlerted)
        {
            if (!canSeePlayer)
            {
                _lostPlayerTimer++;
                if (_lostPlayerTimer >= MaxLostPlayerTime)
                {
                    IsAlerted = false;
                    Target = null!;
                    _lostPlayerTimer = 0;
                    player.Visibility = 0;
                }
            }
            else
            {
                _lostPlayerTimer = 0;
                player.Visibility = 100;
            }

            _moveCooldown += 0.064f;
            if (_moveCooldown >= 0.1f)
            {
                _moveCooldown = 0;
                MoveTowards(player);
            }
        }
        else if (canSeePlayer)
        {
            player.Visibility = 100;
            Alert(player);
        }
        else if (DateTime.Now.Second % 3 == 0)
        {
            Rotate();
        }

        UpdateAnimation();
    }

    public void MoveTowards(ICreature target)
    {
        var dx = target.X - X;
        var dy = target.Y - Y;
        var distance = DistanceTo(target);

        if (distance > 0)
        {
            var newX = X + dx / distance * Speed;
            var newY = Y + dy / distance * Speed;

            if (CanMoveTo(newX, Y))
                X = newX;
            if (CanMoveTo(X, newY))
                Y = newY;

            UpdateFacing(dx, dy);
        }
    }

    public bool CanMoveTo(float x, float y)
    {
        var offset = 0.1f;
        return !PositionIsWall(x + offset, y + offset) &&
               !PositionIsWall(x + 1 - offset, y + offset) &&
               !PositionIsWall(x + offset, y + 1 - offset) &&
               !PositionIsWall(x + 1 - offset, y + 1 - offset);
    }

    private bool PositionIsWall(float x, float y)
    {
        var cellX = (int)x;
        var cellY = (int)y;

        return cellX < 0 || cellX >= _grid.GetLength(0) ||
               cellY < 0 || cellY >= _grid.GetLength(1) ||
               _grid[cellX, cellY].Type == CellType.Wall;
    }

    private void UpdateFacing(float dx, float dy)
    {
        if (Math.Abs(dx) > Math.Abs(dy))
            Facing = dx > 0 ? Direction.Right : Direction.Left;
        else if (Math.Abs(dy) > Math.Abs(dx))
            Facing = dy > 0 ? Direction.Down : Direction.Up;
        else
            Facing = dx > 0 ? Direction.Right : Direction.Left;
    }

    public void Rotate()
    {
        if (!IsAlerted)
            Facing = Facing switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => Facing
            };
    }

    public void Alert(ICreature target)
    {
        IsAlerted = true;
        Target = target;
        _lostPlayerTimer = 0;
    }
}