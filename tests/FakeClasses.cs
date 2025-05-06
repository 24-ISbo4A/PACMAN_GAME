using PACMAN_GAME.Interfaces;
using System.Windows.Media;

namespace PACMAN_GAME.Tests
{
    public class FakeSoundManager : ISoundManager
    {
        public Dictionary<string, MediaPlayer> _sounds_pub => new();
        public bool IsPlaying(string soundName) => false;
        public void InitializeSounds() { }
        public void PlaySound(string soundName) { }
        public void StopAllSounds() { }
        public void StopSound(string soundName) { }
    }

    public class FakeUIManager : IUIManager
    {
        public void HideGameOverScreen() { }
        public void HideMainMenu() { }
        public void ShowGameOverScreen(string message) { }
        public void ShowMainMenu() { }
        public void UpdateArrowPosition(int selectedOption) { }
        public void UpdateScore(int score) { }
    }

    public class FakeCollisionHandler : ICollisionHandler
    {
        public bool CheckCollision(IGameEntity entity1, IGameEntity entity2) => false;
        public void HandleCollision(IGameEntity entity1, IGameEntity entity2) { }
    }
} 