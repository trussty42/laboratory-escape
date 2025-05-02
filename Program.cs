using LaboratoryEscape.Controller;

namespace LaboratoryEscape;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        RunGameLoop();
    }

    private static void RunGameLoop()
    {
        while (true)
            using (var menu = new MainMenuForm())
            {
                Application.Run(menu);

                if (!menu.StartGame)
                    return;

                RunGameSession(menu.SelectedLevel);
            }
    }

    private static void RunGameSession(int startLevel)
    {
        var currentLevel = startLevel;

        while (currentLevel <= 5)
        {
            var result = RunLevel(currentLevel);

            switch (result)
            {
                case LevelResult.Completed:
                    if (currentLevel < 5)
                    {
                        var dialog = new LevelCompleteDialog(currentLevel);
                        var dialogResult = dialog.ShowDialog();

                        if (dialogResult == DialogResult.OK)
                        {
                            if (dialog.ContinueToNextLevel)
                                currentLevel++;
                            else
                                return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Поздравляем! Вы прошли все уровни!", "Игра завершена");
                        return;
                    }

                    break;

                case LevelResult.Failed:
                    var retryDialog = MessageBox.Show(
                        "Вы проиграли! Попробовать еще раз?",
                        "Поражение",
                        MessageBoxButtons.YesNo);

                    if (retryDialog == DialogResult.No)
                        return;
                    break;

                case LevelResult.Quit:
                    return;
            }
        }
    }

    private static LevelResult RunLevel(int levelNumber)
    {
        try
        {
            using (var controller = new GameController(levelNumber))
            using (var gameForm = new GameForm(controller))
            {
                Application.Run(gameForm);

                if (gameForm.PlayerWon)
                    return LevelResult.Completed;
                if (gameForm.PlayerLost)
                    return LevelResult.Failed;
                return LevelResult.Quit;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки уровня: {ex.Message}", "Ошибка");
            return LevelResult.Quit;
        }
    }

    private enum LevelResult
    {
        Completed,
        Failed,
        Quit
    }
}