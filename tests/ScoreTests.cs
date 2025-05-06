using NUnit.Framework;
using System.Windows.Forms;
using PACMAN_GAME;

namespace PACMAN_GAME.Tests
{
    [TestFixture]
    public class ScoreTests
    {
        private Form1 _gameForm;
        private Label _scoreLabel;

        [SetUp]
        public void Setup()
        {
            _scoreLabel = new Label();
            _gameForm = new Form1();
        }

        [Test]
        public void AddScore_ShouldIncreaseScore_WhenRegularCoinEaten()
        {
            // Arrange
            int initialScore = 0;
            int coinValue = 10;

            // Act
            _gameForm.AddScore(coinValue);

            // Assert
            Assert.That(_gameForm.GetScore(), Is.EqualTo(initialScore + coinValue), 
                "Очки должны увеличиться на 10 при поедании обычной монеты");
        }

        [Test]
        public void AddScore_ShouldIncreaseScore_WhenEnergyEaten()
        {
            // Arrange
            int initialScore = 0;
            int energyValue = 50;

            // Act
            _gameForm.AddScore(energyValue);

            // Assert
            Assert.That(_gameForm.GetScore(), Is.EqualTo(initialScore + energyValue), 
                "Очки должны увеличиться на 50 при поедании энергетика");
        }

        [Test]
        public void AddScore_ShouldIncreaseScore_WhenGhostEaten()
        {
            // Arrange
            int initialScore = 0;
            int ghostValue = 200;

            // Act
            _gameForm.AddScore(ghostValue);

            // Assert
            Assert.That(_gameForm.GetScore(), Is.EqualTo(initialScore + ghostValue), 
                "Очки должны увеличиться на 200 при поедании призрака");
        }
    }
} 