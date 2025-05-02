using LaboratoryEscape.Controller;
using LaboratoryEscape.Core;
using LaboratoryEscape.Interfaces;
using Timer = System.Windows.Forms.Timer;

namespace LaboratoryEscape;

public class GameForm : Form
{
    private int _cellSize;
    private int _characterSize;
    private readonly Size _gameGridSize;

    private readonly Dictionary<string, Bitmap> _bitmaps = new();
    private readonly GameController _controller;
    private Timer? _gameTimer;

    private bool _wPressed, _aPressed, _sPressed, _dPressed, _shiftPressed;
    private bool _debugMode;

    public bool PlayerWon { get; private set; }
    public bool PlayerLost { get; private set; }

    public GameForm(GameController controller)
    {
        _controller = controller;
        _gameGridSize = new Size(
            _controller.Model.Grid.GetLength(0),
            _controller.Model.Grid.GetLength(1));

        CalculateOptimalWindowSize();
        InitializeForm();
        LoadImages();
        InitializeTimer();

        KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.F1) _debugMode = !_debugMode;
        };
    }

    private void CalculateOptimalWindowSize()
    {
        var screen = Screen.PrimaryScreen!.WorkingArea;
        var maxWidth = screen.Width - 100;
        var maxHeight = screen.Height - 100;

        var cellSizeByWidth = maxWidth / _gameGridSize.Width;
        var cellSizeByHeight = maxHeight / _gameGridSize.Height;
        _cellSize = Math.Min(cellSizeByWidth, cellSizeByHeight);
        _cellSize = Math.Clamp(_cellSize, 30, 50);

        _characterSize = (int)(_cellSize * 1.6f);

        ClientSize = new Size(_gameGridSize.Width * _cellSize, _gameGridSize.Height * _cellSize);
    }

    private void InitializeForm()
    {
        Text = $"Laboratory Escape - Уровень {_controller.LevelNumber}";
        DoubleBuffered = true;
        KeyPreview = true;
        BackColor = Color.Black;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
    }

    private void LoadImages()
    {
        try
        {
            var imagesDirectory = new DirectoryInfo("img");
            if (!imagesDirectory.Exists) imagesDirectory.Create();

            LoadTexture("Terrain.png", Color.Gray);
            LoadTexture("Wall.png", Color.DarkSlateGray);

            for (var i = 1; i <= 2; i++)
            {
                LoadCharacterTexture($"CharacterAfk{i}.png", Color.Transparent);
                LoadCharacterTexture($"CharacterAfk{i}Reversed.png", Color.Transparent);
            }

            for (var i = 1; i <= 5; i++)
            {
                LoadCharacterTexture($"CharacterRun{i}.png", Color.Transparent);
                LoadCharacterTexture($"CharacterRun{i}Reversed.png", Color.Transparent);
            }

            LoadCharacterTexture("GuardAfk1.png", Color.Transparent);
            LoadCharacterTexture("GuardAfk1Reversed.png", Color.Transparent);

            for (var i = 1; i <= 3; i++)
            {
                LoadCharacterTexture($"GuardRun{i}.png", Color.Transparent);
                LoadCharacterTexture($"GuardRun{i}Reversed.png", Color.Transparent);
            }

            LoadTexture("Normal.png", Color.White);
            LoadTexture("Speed.png", Color.LightBlue);
            LoadTexture("Damage.png", Color.OrangeRed);
            LoadTexture("Exit.png", Color.Green);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки изображений: {ex.Message}", "Ошибка");
        }
    }

    private void LoadTexture(string filename, Color defaultColor)
    {
        var path = Path.Combine("img", filename);
        if (File.Exists(path))
        {
            using var original = new Bitmap(path);
            _bitmaps[filename] = new Bitmap(original, new Size(_cellSize, _cellSize));
        }
        else
        {
            var bmp = new Bitmap(_cellSize, _cellSize);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(defaultColor);
            }

            _bitmaps[filename] = bmp;
        }
    }

    private void LoadCharacterTexture(string filename, Color defaultColor)
    {
        var path = Path.Combine("img", filename);
        if (File.Exists(path))
        {
            using var original = new Bitmap(path);
            _bitmaps[filename] = new Bitmap(original, new Size(_characterSize, _characterSize));
            _bitmaps[filename].MakeTransparent(_bitmaps[filename].GetPixel(0, 0));
        }
        else
        {
            var bmp = new Bitmap(_characterSize, _characterSize);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(defaultColor);
            }

            _bitmaps[filename] = bmp;
        }
    }

    private void InitializeTimer()
    {
        _gameTimer = new Timer { Interval = 8 };
        _gameTimer.Tick += GameUpdate!;
        _gameTimer.Start();
    }

    private void GameUpdate(object sender, EventArgs e)
    {
        _controller.ProcessInput(_wPressed, _aPressed, _sPressed, _dPressed, _shiftPressed);
        _controller.GameStep();

        if (_controller.Model.Player.HasEscaped)
        {
            PlayerWon = true;
            Close();
        }
        else if (!_controller.Model.Player.IsAlive)
        {
            PlayerLost = true;
            Close();
        }

        Invalidate();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        switch (e.KeyCode)
        {
            case Keys.W:
                _wPressed = true;
                break;
            case Keys.A:
                _aPressed = true;
                break;
            case Keys.S:
                _sPressed = true;
                break;
            case Keys.D:
                _dPressed = true;
                break;
            case Keys.ShiftKey:
                _shiftPressed = true;
                break;
        }
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        switch (e.KeyCode)
        {
            case Keys.W:
                _wPressed = false;
                break;
            case Keys.A:
                _aPressed = false;
                break;
            case Keys.S:
                _sPressed = false;
                break;
            case Keys.D:
                _dPressed = false;
                break;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        RenderGame(e.Graphics);
    }

    private void RenderGame(Graphics g)
    {
        g.Clear(Color.Black);

        var offsetX = (ClientSize.Width - _gameGridSize.Width * _cellSize) / 2;
        var offsetY = (ClientSize.Height - _gameGridSize.Height * _cellSize) / 2;

        var clip = g.Clip;
        g.Clip = new Region(new Rectangle(offsetX, offsetY,
            _gameGridSize.Width * _cellSize,
            _gameGridSize.Height * _cellSize));

        DrawTerrainBackground(g, offsetX, offsetY);
        DrawGrid(g, offsetX, offsetY);
        DrawCreatures(g, offsetX, offsetY);
        DrawUi(g);

        if (_debugMode)
            DrawDebugInfo(g, offsetX, offsetY);

        g.Clip = clip;
    }

    private void DrawTerrainBackground(Graphics g, int offsetX, int offsetY)
    {
        if (_bitmaps.TryGetValue("Terrain.png", out var terrain))
        {
            var grid = _controller.Model.Grid;
            for (var x = 0; x < grid.GetLength(0); x++)
            for (var y = 0; y < grid.GetLength(1); y++)
                g.DrawImage(terrain, offsetX + x * _cellSize, offsetY + y * _cellSize);
        }
    }

    private void DrawGrid(Graphics g, int offsetX, int offsetY)
    {
        var grid = _controller.Model.Grid;
        for (var x = 0; x < grid.GetLength(0); x++)
        for (var y = 0; y < grid.GetLength(1); y++)
        {
            var rect = new Rectangle(
                offsetX + x * _cellSize,
                offsetY + y * _cellSize,
                _cellSize,
                _cellSize);

            if (grid[x, y].Type == CellType.Wall)
            {
                if (_bitmaps.TryGetValue("Wall.png", out var wallTexture))
                    g.DrawImage(wallTexture, rect);
                continue;
            }

            if (grid[x, y].Type != CellType.Normal)
            {
                var imageName = grid[x, y].Type + ".png";
                if (_bitmaps.TryGetValue(imageName, out var bitmap)) g.DrawImage(bitmap, rect);
            }
        }
    }

    private void DrawCreatures(Graphics g, int offsetX, int offsetY)
    {
        DrawCreature(g, _controller.Model.Player, "Character.png", offsetX, offsetY);

        foreach (var guard in _controller.Model.Guards)
        {
            var sprite = guard.IsAlerted ? "GuardRun1Reversed.png" : "GuardAfk1Reversed.png";
            DrawCreature(g, guard, sprite, offsetX, offsetY);
        }
    }

    private void DrawCreature(Graphics g, ICreature creature, string imageName, int offsetX, int offsetY)
    {
        var actualImageName = imageName;

        if (creature is Player player)
        {
            actualImageName = player.GetCurrentAnimationFrame();

            var healthBarWidth = _characterSize;
            var healthBarHeight = 5;
            var healthBarX = offsetX + (int)(player.X * _cellSize) + (_cellSize - _characterSize) / 2;
            var healthBarY = offsetY + (int)(player.Y * _cellSize) - 10;

            g.FillRectangle(Brushes.DarkRed, healthBarX, healthBarY, healthBarWidth, healthBarHeight);
            g.FillRectangle(Brushes.Green, healthBarX, healthBarY,
                (int)(healthBarWidth * (player.Health / 100f)), healthBarHeight);
        }
        else if (creature is Guard guard)
        {
            actualImageName = guard.GetCurrentAnimationFrame();
        }

        if (_bitmaps.TryGetValue(actualImageName, out var bitmap))
        {
            var x = offsetX + (int)(creature.X * _cellSize) + (_cellSize - _characterSize) / 2;
            var y = offsetY + (int)(creature.Y * _cellSize) + (_cellSize - _characterSize) / 2;

            g.DrawImage(bitmap, x, y);
        }
    }

    private void DrawUi(Graphics g)
    {
        var player = _controller.Model.Player;

        if (player.Visibility > 0)
        {
            var alpha = (int)(player.Visibility * 0.4f);
            alpha = Math.Clamp(alpha, 0, 40);
            g.FillRectangle(
                new SolidBrush(Color.FromArgb(alpha, Color.Red)),
                0, 0, ClientSize.Width, ClientSize.Height
            );
        }
    }

    private void DrawDebugInfo(Graphics g, int offsetX, int offsetY)
    {
        var player = _controller.Model.Player;
        var points = new[]
        {
            new PointF(player.X, player.Y),
            new PointF(player.X + 0.9f, player.Y),
            new PointF(player.X, player.Y + 0.9f),
            new PointF(player.X + 0.9f, player.Y + 0.9f)
        };

        foreach (var point in points)
            g.FillEllipse(Brushes.Red,
                offsetX + point.X * _cellSize - 2,
                offsetY + point.Y * _cellSize - 2,
                4, 4);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!PlayerWon && !PlayerLost)
        {
            PlayerWon = false;
            PlayerLost = false;
        }

        base.OnFormClosing(e);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _gameTimer?.Stop();
            foreach (var bitmap in _bitmaps.Values)
                bitmap.Dispose();
        }

        base.Dispose(disposing);
    }
}