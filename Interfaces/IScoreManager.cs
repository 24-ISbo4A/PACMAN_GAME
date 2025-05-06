namespace PACMAN_GAME.Interfaces
{
    public interface IScoreManager
    {
        int GetScore();
        void AddScore(int points);
        void ResetScore();
    }
} 