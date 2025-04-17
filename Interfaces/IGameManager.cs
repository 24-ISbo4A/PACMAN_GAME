namespace PACMAN_GAME.Interfaces;

public interface IGameManager
{
    void StartGame();
    void ResetGame();
    void UpdateGame();
    void GameOver(string message);
    void NextLevel();
} 