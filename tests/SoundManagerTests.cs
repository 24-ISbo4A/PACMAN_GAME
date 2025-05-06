using NUnit.Framework;
using PACMAN_GAME.Managers;
using System.IO;

namespace PACMAN_GAME.Tests
{
    [TestFixture]
    public class SoundManagerTests
    {
        private SoundManager _soundManager;

        [SetUp]
        public void Setup()
        {
            _soundManager = new SoundManager();
        }

        [Test]
        public void InitializeSounds_ShouldLoadAllSounds()
        {
            _soundManager.InitializeSounds();
            Assert.That(_soundManager._sounds_pub.Count, Is.EqualTo(6), "Должно быть загружено 6 звуков.");
        }

        [Test]
        public void StopSound_ShouldStopSound()
        {
            _soundManager.InitializeSounds();
            _soundManager.PlaySound("game_start");
            _soundManager.StopSound("game_start");
            Assert.That(_soundManager.IsPlaying("game_start"), Is.False, "Звук должен быть остановлен.");
        }

        [Test]
        public void StopAllSounds_ShouldStopAllSounds()
        {
            _soundManager.InitializeSounds();
            _soundManager.PlaySound("game_start");
            _soundManager.PlaySound("pacman_death");
            _soundManager.StopAllSounds();
            Assert.That(_soundManager.IsPlaying("game_start"), Is.False, "Все звуки должны быть остановлены.");
            Assert.That(_soundManager.IsPlaying("pacman_death"), Is.False, "Все звуки должны быть остановлены.");
        }

        [Test]
        public void IsPlaying_ShouldReturnFalseForNonExistentSound()
        {
            _soundManager.InitializeSounds();
            Assert.That(_soundManager.IsPlaying("non_existent_sound"), Is.False, "Метод IsPlaying должен возвращать false для несуществующего звука.");
        }
    }
} 