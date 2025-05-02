namespace LaboratoryEscape;

public class MainMenuForm : Form
{
    public int SelectedLevel { get; private set; } = 1;
    public bool StartGame { get; private set; }

    public MainMenuForm()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        Text = "Laboratory Escape - Главное меню";
        ClientSize = new Size(1000, 1000);
        BackColor = Color.FromArgb(40, 40, 50);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        var menuPanel = new Panel
        {
            Size = new Size(800, 800),
            Location = new Point(100, 100),
            BackColor = Color.FromArgb(60, 60, 70),
            BorderStyle = BorderStyle.FixedSingle
        };
        Controls.Add(menuPanel);

        var title = new Label
        {
            Text = "LABORATORY ESCAPE",
            Font = new Font("Arial", 38, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = false,
            Size = new Size(750, 200),
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(25, 50)
        };
        menuPanel.Controls.Add(title);

        var levelLabel = new Label
        {
            Text = "ВЫБЕРИТЕ УРОВЕНЬ:",
            Font = new Font("Arial", 18),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(200, 300)
        };
        menuPanel.Controls.Add(levelLabel);

        var levelComboBox = new ComboBox
        {
            Font = new Font("Arial", 16),
            Size = new Size(300, 40),
            Location = new Point(250, 350),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        for (var i = 1; i <= 5; i++)
            levelComboBox.Items.Add($"Уровень {i}");

        levelComboBox.SelectedIndex = 0;
        levelComboBox.SelectedIndexChanged += (_, _) =>
            SelectedLevel = levelComboBox.SelectedIndex + 1;
        menuPanel.Controls.Add(levelComboBox);

        var playButton = CreateMenuButton("ИГРАТЬ", 450, Color.FromArgb(70, 130, 180));
        playButton.Click += (_, _) =>
        {
            StartGame = true;
            Close();
        };
        menuPanel.Controls.Add(playButton);

        var infoButton = CreateMenuButton("ИНФОРМАЦИЯ", 550, Color.FromArgb(130, 70, 180));
        infoButton.Click += (_, _) => ShowInfoDialog();
        menuPanel.Controls.Add(infoButton);

        var exitButton = CreateMenuButton("ВЫХОД", 650, Color.FromArgb(180, 70, 70));
        exitButton.Click += (_, _) => Close();
        menuPanel.Controls.Add(exitButton);
    }

    private void ShowInfoDialog()
    {
        var infoDialog = new Form
        {
            Text = "Информация и управление",
            Size = new Size(1000, 1000),
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterScreen,
            MaximizeBox = false,
            BackColor = Color.FromArgb(60, 60, 70)
        };

        var controlsLabel = new Label
        {
            Text = "Управление:\n\n" +
                   "W - Движение вверх\n" +
                   "A - Движение влево\n" +
                   "S - Движение вниз\n" +
                   "D - Движение вправо\n",
            Font = new Font("Arial", 14),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(250, 20)
        };
        infoDialog.Controls.Add(controlsLabel);

        var rulesLabel = new Label
        {
            Text = "\nПравила игры:\n\n" +
                   "1. Доберитесь до выхода\n" +
                   "2. Избегайте столкновений с охранниками\n" +
                   "3. Зеленые клетки увеличивают скорость\n" +
                   "4. Красные клетки наносят урон",
            Font = new Font("Arial", 14),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(250, 200)
        };
        infoDialog.Controls.Add(rulesLabel);

        var okButton = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Size = new Size(100, 40),
            Location = new Point(infoDialog.ClientSize.Width - 120, infoDialog.ClientSize.Height - 60),
            BackColor = Color.FromArgb(70, 130, 180),
            ForeColor = Color.White
        };
        infoDialog.Controls.Add(okButton);

        infoDialog.ShowDialog();
    }

    private Button CreateMenuButton(string text, int top, Color backColor)
    {
        return new Button
        {
            Text = text,
            Font = new Font("Arial", 20, FontStyle.Bold),
            Size = new Size(500, 70),
            Location = new Point(150, top),
            BackColor = backColor,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            TabStop = false
        };
    }

    public void SetSelectedLevel(int level)
    {
        if (level is >= 1 and <= 5)
            if (Controls.OfType<ComboBox>().FirstOrDefault() is { } comboBox)
                comboBox.SelectedIndex = level - 1;
    }
}