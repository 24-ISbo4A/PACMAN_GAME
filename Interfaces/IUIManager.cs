namespace PACMAN_GAME.Interfaces;

public interface IUIManager
{
    void ShowMainMenu();
    void HideMainMenu();
    void ShowGameOverScreen(string message);
    void UpdateScore(int score);
    void UpdateArrowPosition(int selectedOption);
    void HideGameOverScreen();
} 