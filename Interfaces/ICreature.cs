using LaboratoryEscape.Core;

namespace LaboratoryEscape.Interfaces;

public interface ICreature
{
    float X { get; set; }
    float Y { get; set; }
    int Health { get; }
    bool IsAlive { get; }
    int Visibility { get; set; }
    float Speed { get; }

    void Move(Direction direction, Cell[,] grid);
    bool CanSee(ICreature other);
    float DistanceTo(ICreature other);
}