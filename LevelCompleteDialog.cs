namespace LaboratoryEscape;

public partial class LevelCompleteDialog : Form
{
    public bool ContinueToNextLevel { get; private set; }

    public LevelCompleteDialog(int completedLevel)
    {
        InitializeComponents(completedLevel);
    }

    private void InitializeComponents(int completedLevel)
    {
        Text = "Уровень пройден!";
        ClientSize = new Size(400, 200);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;
        MinimizeBox = false;

        var label = new Label
        {
            Text = $"Уровень {completedLevel} пройден!",
            Font = new Font("Arial", 16),
            AutoSize = true,
            Location = new Point(50, 40)
        };

        var nextButton = new Button
        {
            Text = "Следующий уровень",
            DialogResult = DialogResult.OK,
            Size = new Size(150, 40),
            Location = new Point(50, 100),
            BackColor = Color.LightGreen
        };
        nextButton.Click += (_, _) =>
        {
            ContinueToNextLevel = true;
            Close();
        };

        var menuButton = new Button
        {
            Text = "В главное меню",
            DialogResult = DialogResult.OK,
            Size = new Size(150, 40),
            Location = new Point(200, 100),
            BackColor = Color.LightGray
        };
        menuButton.Click += (_, _) =>
        {
            ContinueToNextLevel = false;
            Close();
        };

        Controls.Add(label);
        Controls.Add(nextButton);
        Controls.Add(menuButton);
    }
}