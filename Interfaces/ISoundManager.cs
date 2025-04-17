namespace PACMAN_GAME.Interfaces;

public interface ISoundManager
{
    void PlaySound(string soundName);
    void StopSound(string soundName);
    void StopAllSounds();
    void InitializeSounds();
    bool IsPlaying(string soundName);
    Dictionary<string, System.Windows.Media.MediaPlayer> _sounds_pub { get; }
} 