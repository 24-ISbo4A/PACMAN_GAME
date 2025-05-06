using NUnit.Framework;
using System.Windows.Forms;
using PACMAN_GAME;
using Timer = System.Windows.Forms.Timer;

namespace PACMAN_GAME.Tests
{
    [TestFixture]
    public class ScoreTests
    {
        private GameManager _gameManager = null!;
        private ISoundManager _soundManager = null!;
        private IUIManager _uiManager = null!;
        private Form _testForm = null!;

        [SetUp]
        public void Setup()
        {
            _testForm = new Form();
            _soundManager = new SoundManager();
            _uiManager = new UIManager(
                _testForm,
                new PictureBox(), // menuBackground
                new PictureBox(), // menuArrow
                new Label(), // gameOverLabel
                new Label(), // restartLabel
                new Label(), // scoreLabel
                new PictureBox() // deathAnimation
            );

            var collisionHandler = new CollisionHandler(_soundManager);
            _gameManager = new GameManager(
                _testForm,
                _soundManager,
                _uiManager,
                collisionHandler,
                new Timer(),
                new Timer(),
                new Timer(),
                new Timer(),
                new PictureBox()
            );
        }

        [Test]
        public void AddScore_ShouldIncreaseScore_WhenRegularCoinEaten()
        {
            // Arrange
            int initialScore = _gameManager.GetScore();
            int coinValue = 10;

            // Act
            _gameManager.AddScore(coinValue);

            // Assert
            Assert.That(_gameManager.GetScore(), Is.EqualTo(initialScore + coinValue),
                "Очки должны увеличиться на 10 при поедании обычной монеты");
        }

        [Test]
        public void AddScore_ShouldIncreaseScore_WhenEnergyEaten()
        {
            // Arrange
            int initialScore = _gameManager.GetScore();
            int energyValue = 50;

            // Act
            _gameManager.AddScore(energyValue);

            // Assert
            Assert.That(_gameManager.GetScore(), Is.EqualTo(initialScore + energyValue),
                "Очки должны увеличиться на 50 при поедании энергетика");
        }

        [Test]
        public void AddScore_ShouldIncreaseScore_WhenGhostEaten()
        {
            // Arrange
            int initialScore = _gameManager.GetScore();
            int ghostValue = 200;

            // Act
            _gameManager.AddScore(ghostValue);

            // Assert
            Assert.That(_gameManager.GetScore(), Is.EqualTo(initialScore + ghostValue),
                "Очки должны увеличиться на 200 при поедании призрака");
        }

        [TearDown]
        public void Cleanup()
        {
            _testForm.Dispose();
        }
    }
} 