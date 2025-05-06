using NUnit.Framework;
using PACMAN_GAME.Managers;

namespace PACMAN_GAME.Tests
{
    [TestFixture]
    public class ScoreManagerTests
    {
        private GameManager _gameManager;

        [SetUp]
        public void Setup()
        {
            _gameManager = new GameManager();
        }

        [Test]
        public void AddScore_ShouldIncreaseScore_WhenRegularPointEaten()
        {
            // Тест начисления очков за обычные действия
            int initialScore = _gameManager.GetScore();
            _gameManager.AddScore(10);
            Assert.That(_gameManager.GetScore(), Is.EqualTo(initialScore + 10), "Очки должны увеличиться на 10.");
        }

        [Test]
        public void AddScore_ShouldIncreaseScore_WhenBonusPointEaten()
        {
            // Тест начисления очков за бонусные действия
            int initialScore = _gameManager.GetScore();
            _gameManager.AddScore(50);
            Assert.That(_gameManager.GetScore(), Is.EqualTo(initialScore + 50), "Очки должны увеличиться на 50.");
        }

        [Test]
        public void ResetScore_ShouldSetScoreToZero()
        {
            // Тест сброса очков
            _gameManager.AddScore(100);
            _gameManager.ResetScore();
            Assert.That(_gameManager.GetScore(), Is.EqualTo(0), "Очки должны быть сброшены до нуля.");
        }
    }
} 