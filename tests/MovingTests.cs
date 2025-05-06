using NUnit.Framework;
using PACMAN_GAME.Managers;
using PACMAN_GAME.Models;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace PACMAN_GAME.Tests
{
    [TestFixture]
    public class PacmanMovementTests
    {
        private Pacman _pacman;
        private Form _form;
        private SoundManager _soundManager;
        private List<IGameEntity> _walls;

        [SetUp]
        public void Setup()
        {
            // Инициализация формы
            _form = new Form();
            _form.ClientSize = new Size(800, 600);
            
            // Инициализация менеджера звуков
            _soundManager = new SoundManager();
            
            // Создание PictureBox для Пакмана
            var pacmanView = new PictureBox
            {
                Name = "pacman",
                Size = new Size(30, 30),
                Location = new Point(100, 100),
                Image = Properties.Resources.right,
                Tag = "pacman"
            };
            _form.Controls.Add(pacmanView);
            
            // Создание тестовых стен
            _walls = new List<IGameEntity>();
            var wall1 = new PictureBox
            {
                Size = new Size(50, 200),
                Location = new Point(150, 0),
                BackColor = Color.Blue,
                Tag = "wall"
            };
            _form.Controls.Add(wall1);
            _walls.Add(new Wall(wall1));
            
            // Инициализация Пакмана
            _pacman = new Pacman(pacmanView, _form, _soundManager);
        }

        [Test]
        public void Move_Right_ShouldChangePosition()
        {
            // Arrange
            _pacman.SetDirection(1); // Направление вправо
            int initialX = _pacman.X;
            
            // Act
            _pacman.Move();
            
            // Assert
            Assert.That(_pacman.X, Is.EqualTo(initialX + _pacman.Speed), "Пакман должен переместиться вправо");
        }

        [Test]
        public void Move_Left_ShouldChangePosition()
        {
            // Arrange
            _pacman.SetDirection(3); // Направление влево
            int initialX = _pacman.X;
            
            // Act
            _pacman.Move();
            
            // Assert
            Assert.That(_pacman.X, Is.EqualTo(initialX - _pacman.Speed), "Пакман должен переместиться влево");
        }

        [Test]
        public void Move_Up_ShouldChangePosition()
        {
            // Arrange
            _pacman.SetDirection(0); // Направление вверх
            int initialY = _pacman.Y;
            
            // Act
            _pacman.Move();
            
            // Assert
            Assert.That(_pacman.Y, Is.EqualTo(initialY - _pacman.Speed), "Пакман должен переместиться вверх");
        }

        [Test]
        public void Move_Down_ShouldChangePosition()
        {
            // Arrange
            _pacman.SetDirection(2); // Направление вниз
            int initialY = _pacman.Y;
            
            // Act
            _pacman.Move();
            
            // Assert
            Assert.That(_pacman.Y, Is.EqualTo(initialY + _pacman.Speed), "Пакман должен переместиться вниз");
        }

        [Test]
        public void Move_IntoWall_ShouldNotChangePosition()
        {
            // Arrange
            _pacman.SetDirection(1); // Направление вправо
            _pacman.X = 100; // Позиция перед стеной
            int initialX = _pacman.X;
            
            // Act
            _pacman.Move();
            
            // Assert
            Assert.That(_pacman.X, Is.EqualTo(initialX), "Пакман не должен проходить сквозь стену");
        }

        [Test]
        public void Move_LeftBorder_ShouldWrapToRight()
        {
            // Arrange
            _pacman.SetDirection(3); // Направление влево
            _pacman.X = -5; // Почти за левой границей
            
            // Act
            _pacman.Move();
            
            // Assert
            Assert.That(_pacman.X, Is.EqualTo(_form.ClientSize.Width - 10), "Пакман должен появиться с правой стороны");
        }

        [Test]
        public void Move_RightBorder_ShouldWrapToLeft()
        {
            // Arrange
            _pacman.SetDirection(1); // Направление вправо
            _pacman.X = _form.ClientSize.Width - 5; // Почти за правой границей
            
            // Act
            _pacman.Move();
            
            // Assert
            Assert.That(_pacman.X, Is.EqualTo(-10), "Пакман должен появиться с левой стороны");
        }

        [Test]
        public void ChangeDirection_WhenPathClear_ShouldChangeDirection()
        {
            // Arrange
            _pacman.SetDirection(1); // Движение вправо
            _pacman.Move(); // Начальное движение
            
            // Act
            _pacman.SetDirection(0); // Попытка движения вверх
            _pacman.Move();
            
            // Assert
            Assert.That(_pacman.Direction, Is.EqualTo(0), "Направление должно измениться на вверх");
        }

        [Test]
        public void ChangeDirection_WhenPathBlocked_ShouldNotChangeDirection()
        {
            // Arrange
            _pacman.X = 100;
            _pacman.Y = 150;
            _pacman.SetDirection(1); // Движение вправо
            
            // Добавляем стену сверху
            var wall = new PictureBox
            {
                Size = new Size(30, 30),
                Location = new Point(100, 120),
                BackColor = Color.Blue,
                Tag = "wall"
            };
            _form.Controls.Add(wall);
            _walls.Add(new Wall(wall));
            
            // Act
            _pacman.SetDirection(0); // Попытка движения вверх
            _pacman.Move();
            
            // Assert
            Assert.That(_pacman.Direction, Is.Not.EqualTo(0), "Направление не должно измениться, если путь заблокирован");
        }
    }
}