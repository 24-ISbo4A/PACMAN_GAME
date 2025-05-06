using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Managers
{
    public class ScoreManager : IScoreManager
    {
        private int _score;

        public ScoreManager()
        {
            _score = 0;
        }

        public int GetScore()
        {
            return _score;
        }

        public void AddScore(int points)
        {
            _score += points;
        }

        public void ResetScore()
        {
            _score = 0;
        }
    }
} 