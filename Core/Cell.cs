namespace LaboratoryEscape.Core;

public enum CellType
{
    Normal,
    Speed,
    Damage,
    Exit,
    Wall
}

public class Cell(CellType type)
{
    public CellType Type { get; set; } = type;
}