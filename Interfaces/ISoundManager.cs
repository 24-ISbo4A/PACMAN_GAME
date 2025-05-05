using System;

namespace PACMAN_GAME.Interfaces;

public interface ISoundManager : IDisposable
{
    void InitializeSounds();
    void PlaySound(string soundName);
    void StopSound(string soundName);
    void StopAllSounds();
    bool IsPlaying(string soundName);
} 