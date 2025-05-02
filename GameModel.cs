using LaboratoryEscape.Core;

namespace LaboratoryEscape;

public class GameModel
{
    public GameModel(int width, int height, float playerX, float playerY)
    {
        Grid = new Cell[width, height];
        Player = new Player(
            Math.Clamp(playerX, 0, width - 1),
            Math.Clamp(playerY, 0, height - 1),
            100);
        Guards = new List<Guard>();
    }

    public Cell[,] Grid { get; }
    public Player Player { get; }
    public List<Guard> Guards { get; }
    public int Turn { get; set; }
    public bool IsGameOver => !Player.IsAlive;
}