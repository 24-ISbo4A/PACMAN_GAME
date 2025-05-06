using System;
using System.Drawing;
using System.Windows.Forms;
using PACMAN_GAME;

namespace PACMAN_GAME.tests
{
    // Простая заглушка для ISoundManager
    public class DummySoundManager : ISoundManager
    {
        public Dictionary<string, System.Windows.Media.MediaPlayer> _sounds_pub => new();
        public void InitializeSounds() { }
        public bool IsPlaying(string soundName) => false;
        public void PlaySound(string soundName) { }
        public void StopAllSounds() { }
        public void StopSound(string soundName) { }
    }

    public class ManualCollisionTests
    {
        public void TestPacmanCollidesWithGhost()
        {
            var soundManager = new DummySoundManager();
            var collisionHandler = new CollisionHandler(soundManager);
            var pacmanView = new PictureBox { Bounds = new Rectangle(10, 10, 30, 30) };
            var ghostView = new PictureBox { Bounds = new Rectangle(20, 20, 30, 30) };
            var pacman = new Pacman(pacmanView, new Form(), soundManager);
            var ghost = new RedGhost(ghostView, new Form());
            bool collision = collisionHandler.CheckCollision(pacman, ghost);
            if (collision)
                Console.WriteLine("[OK] Pacman vs Ghost: Столкновение обнаружено");
            else
                Console.WriteLine("[FAIL] Pacman vs Ghost: Столкновение не обнаружено");
        }

        public void TestPacmanCollidesWithWall()
        {
            var soundManager = new DummySoundManager();
            var collisionHandler = new CollisionHandler(soundManager);
            var pacmanView = new PictureBox { Bounds = new Rectangle(50, 50, 30, 30) };
            var wallView = new PictureBox { Bounds = new Rectangle(60, 60, 30, 30) };
            var pacman = new Pacman(pacmanView, new Form(), soundManager);
            var wall = new Wall(wallView);
            bool collision = collisionHandler.CheckCollision(pacman, wall);
            if (collision)
                Console.WriteLine("[OK] Pacman vs Wall: Столкновение обнаружено");
            else
                Console.WriteLine("[FAIL] Pacman vs Wall: Столкновение не обнаружено");
        }

        public void TestPacmanCollidesWithCoin()
        {
            var soundManager = new DummySoundManager();
            var collisionHandler = new CollisionHandler(soundManager);
            var pacmanView = new PictureBox { Bounds = new Rectangle(100, 100, 30, 30) };
            var coinView = new PictureBox { Bounds = new Rectangle(110, 110, 10, 10) };
            var pacman = new Pacman(pacmanView, new Form(), soundManager);
            var coin = new Coin(coinView);
            bool collision = collisionHandler.CheckCollision(pacman, coin);
            if (collision)
                Console.WriteLine("[OK] Pacman vs Coin: Столкновение обнаружено");
            else
                Console.WriteLine("[FAIL] Pacman vs Coin: Столкновение не обнаружено");
        }
    }
}
