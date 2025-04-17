using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media;
using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Managers;

public class SoundManager : ISoundManager
{
    public readonly Dictionary<string, MediaPlayer> _sounds = new();
    private readonly Dictionary<string, bool> _isPlaying = new();

    public Dictionary<string, MediaPlayer> _sounds_pub => _sounds;

    public void InitializeSounds()
    {
        try
        {
            var resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Sounds");
            if (!Directory.Exists(resourcePath))
            {
                MessageBox.Show($"Папка со звуками не найдена: {resourcePath}");
                return;
            }

            var soundFiles = new Dictionary<string, string>
            {
                { "game_start", "game_start.mp3" },
                { "pacman_death", "pacman_death.mp3" },
                { "ghost_eaten", "ghost_eaten.mp3" },
                { "pacman_move", "pacman_move.mp3" },
                { "ghost_move", "phonepacman.mp3" },
                { "fear_mode", "pacmanghostik.mp3" }
            };

            foreach (var sound in soundFiles)
            {
                var filePath = Path.Combine(resourcePath, sound.Value);
                if (File.Exists(filePath))
                {
                    var player = new MediaPlayer();
                    player.Open(new Uri(Path.GetFullPath(filePath)));
                    _sounds[sound.Key] = player;
                    _isPlaying[sound.Key] = false;
                    
                    player.MediaEnded += (s, e) =>
                    {
                        if (_isPlaying[sound.Key])
                        {
                            player.Position = TimeSpan.Zero;
                            player.Play();
                        }
                    };
                }
                else
                {
                    MessageBox.Show($"Ошибка: файл {filePath} не найден");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке звуков: {ex.Message}");
        }
    }

    public void PlaySound(string soundName)
    {
        if (_sounds.ContainsKey(soundName))
        {
            try
            {
                var player = _sounds[soundName];
                player.Position = TimeSpan.Zero;
                player.Play();
                _isPlaying[soundName] = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при воспроизведении звука {soundName}: {ex.Message}");
            }
        }
    }

    public void StopSound(string soundName)
    {
        if (_sounds.ContainsKey(soundName) && _isPlaying[soundName])
        {
            _isPlaying[soundName] = false;
            _sounds[soundName].Stop();
        }
    }

    public void StopAllSounds()
    {
        foreach (var sound in _sounds.Keys)
        {
            StopSound(sound);
        }
    }

    public bool IsPlaying(string soundName)
    {
        return _isPlaying.ContainsKey(soundName) && _isPlaying[soundName];
    }
} 