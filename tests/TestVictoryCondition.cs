using Microsoft.VisualStudio.TestTools.UnitTesting;
using PACMAN_GAME;
using PACMAN_GAME.Managers;
using PACMAN_GAME.Interfaces;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace PACMAN_GAME.Tests
{
    [TestClass]
    public class TestVictoryCondition
    {
        [TestMethod]
        public void NextLevel_WhenCalled_ShouldNotBeGameOver()
        {
            // Arrange
            var form = new Form();
            var soundManager = new FakeSoundManager();
            var uiManager = new FakeUIManager();
            var collisionHandler = new FakeCollisionHandler();
            var gameTimer = new Timer();
            var fearModeTimer = new Timer();
            var flickerTimer = new Timer();
            var deathTimer = new Timer();
            var deathAnimation = new PictureBox();
            var gameManager = new GameManager(form, soundManager, uiManager, collisionHandler, gameTimer, fearModeTimer, flickerTimer, deathTimer, deathAnimation);
            gameManager.StartGame();

            // Act
            gameManager.NextLevel();

            // Assert
            Assert.IsFalse(gameManager.IsGameOver, "После перехода на следующий уровень игра не должна быть окончена");
            Assert.IsFalse(gameManager.IsInMenu, "После перехода на следующий уровень не должно быть меню");
        }
    }
} 