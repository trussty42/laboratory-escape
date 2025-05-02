using LaboratoryEscape.Core;
using NUnit.Framework;

namespace LaboratoryEscape.Tests;

[TestFixture]
public class CellTests
{
    [Test]
    public void Constructor_SetsTypeCorrectly()
    {
        var cell = new Cell(CellType.Wall);

        Assert.AreEqual(CellType.Wall, cell.Type);
    }

    [Test]
    public void Type_CanBeChanged()
    {
        var cell = new Cell(CellType.Normal)
        {
            Type = CellType.Speed
        };

        Assert.AreEqual(CellType.Speed, cell.Type);
    }
}