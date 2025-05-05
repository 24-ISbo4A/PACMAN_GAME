using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Managers;

public class SoundManager : ISoundManager
{
    private readonly Dictionary<string, IWavePlayer> _players = new();
    private readonly Dictionary<string, AudioFileReader> _audioFiles = new();
    private readonly Dictionary<string, bool> _isPlaying = new();

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
                    var audioFile = new AudioFileReader(filePath);
                    var player = new WaveOutEvent();
                    player.Init(audioFile);
                    
                    _players[sound.Key] = player;
                    _audioFiles[sound.Key] = audioFile;
                    _isPlaying[sound.Key] = false;
                    
                    player.PlaybackStopped += (s, e) =>
                    {
                        if (_isPlaying[sound.Key])
                        {
                            audioFile.Position = 0;
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
        if (_players.ContainsKey(soundName))
        {
            try
            {
                var player = _players[soundName];
                var audioFile = _audioFiles[soundName];
                audioFile.Position = 0;
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
        if (_players.ContainsKey(soundName) && _isPlaying[soundName])
        {
            _isPlaying[soundName] = false;
            _players[soundName].Stop();
        }
    }

    public void StopAllSounds()
    {
        foreach (var sound in _players.Keys)
        {
            StopSound(sound);
        }
    }

    public bool IsPlaying(string soundName)
    {
        return _isPlaying.ContainsKey(soundName) && _isPlaying[soundName];
    }

    public void Dispose()
    {
        foreach (var player in _players.Values)
        {
            player.Dispose();
        }
        foreach (var audioFile in _audioFiles.Values)
        {
            audioFile.Dispose();
        }
    }
} 