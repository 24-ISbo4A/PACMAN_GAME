using NUnit.Framework;
using PACMAN_GAME.Managers;

namespace PACMAN_GAME.Tests
{
    [TestFixture]
    public class ScoreManagerTests
    {
        private ScoreManager _scoreManager;

        [SetUp]
        public void Setup()
        {
            _scoreManager = new ScoreManager();
        }

        [Test]
        public void AddScore_ShouldIncreaseScore_WhenRegularPointEaten()
        {
            // Тест начисления очков за обычные монеты
            int initialScore = _scoreManager.GetScore();
            _scoreManager.AddScore(10);
            Assert.That(_scoreManager.GetScore(), Is.EqualTo(initialScore + 10), "Очки должны увеличиться на 10.");
        }

        [Test]
        public void AddScore_ShouldIncreaseScore_WhenEnergyEaten()
        {
            // Тест начисления очков за энергетики
            int initialScore = _scoreManager.GetScore();
            _scoreManager.AddScore(50);
            Assert.That(_scoreManager.GetScore(), Is.EqualTo(initialScore + 50), "Очки должны увеличиться на 50.");
        }

        [Test]
        public void AddScore_ShouldIncreaseScore_WhenGhostEaten()
        {
            // Тест начисления очков за съедение призраков
            int initialScore = _scoreManager.GetScore();
            _scoreManager.AddScore(200);
            Assert.That(_scoreManager.GetScore(), Is.EqualTo(initialScore + 200), "Очки должны увеличиться на 200.");
        }

        [Test]
        public void ResetScore_ShouldSetScoreToZero()
        {
            // Тест сброса очков
            _scoreManager.AddScore(100);
            _scoreManager.ResetScore();
            Assert.That(_scoreManager.GetScore(), Is.EqualTo(0), "Очки должны быть сброшены до нуля.");
        }
    }
} 