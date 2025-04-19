using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Models;

/// <summary>
/// Singleton class responsible for managing the game state
/// </summary>
public sealed class SingletonGameManager
{
    private static readonly Lazy<SingletonGameManager> _instance = 
        new Lazy<SingletonGameManager>(() => new SingletonGameManager());

    public static SingletonGameManager Instance => _instance.Value;

    private int _score;
    private bool _isGameOver;
    private bool _isFearMode;
    private bool _isInMenu;
    private DateTime _fearModeStartTime;

    public event Action<int> OnScoreChanged;
    public event Action<string> OnGameOver;
    public event Action OnFearModeChanged;

    private SingletonGameManager()
    {
        ResetState();
    }

    public int Score
    {
        get => _score;
        set
        {
            if (_score != value)
            {
                _score = value;
                OnScoreChanged?.Invoke(_score);
            }
        }
    }

    public bool IsGameOver
    {
        get => _isGameOver;
        set
        {
            if (_isGameOver != value)
            {
                _isGameOver = value;
                if (_isGameOver)
                {
                    OnGameOver?.Invoke("Game Over");
                }
            }
        }
    }

    public bool IsFearMode
    {
        get => _isFearMode;
        set
        {
            if (_isFearMode != value)
            {
                _isFearMode = value;
                OnFearModeChanged?.Invoke();
            }
        }
    }

    public bool IsInMenu
    {
        get => _isInMenu;
        set => _isInMenu = value;
    }

    public DateTime FearModeStartTime
    {
        get => _fearModeStartTime;
        set => _fearModeStartTime = value;
    }

    public void ResetState()
    {
        Score = 0;
        IsGameOver = false;
        IsFearMode = false;
        IsInMenu = true;
        FearModeStartTime = DateTime.MinValue;
    }
}
