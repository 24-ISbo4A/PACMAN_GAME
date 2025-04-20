using System;

public class SingletonGameManager
{
    private static SingletonGameManager _instance;
    private static readonly object _lock = new object();

    // Game state properties
    public int Score { get; private set; }
    public bool IsGameOver { get; set; }
    public bool IsPaused { get; private set; }
    public bool IsInMenu { get; set; }
    public DateTime FearModeStartTime { get; set; }
    public bool IsFearMode { get; set; }

    // Events
    public event Action<int> OnScoreChanged;
    public event Action<string> OnGameOver;
    public event Action OnFearModeChanged;

    // Private constructor to prevent direct instantiation
    private SingletonGameManager()
    {
        ResetState();
        OnScoreChanged = delegate { };
        OnGameOver = delegate { };
        OnFearModeChanged = delegate { };
    }

    public static SingletonGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SingletonGameManager();
                    }
                }
            }
            return _instance;
        }
    }

    public void AddScore(int points)
    {
        if (!IsGameOver && !IsPaused)
        {
            Score += points;
            OnScoreChanged?.Invoke(Score);
        }
    }

    public void ResetState()
    {
        Score = 0;
        IsGameOver = false;
        IsPaused = false;
        IsInMenu = true;
        IsFearMode = false;
        OnScoreChanged?.Invoke(Score);
    }

    public void TriggerGameOver(string message)
    {
        IsGameOver = true;
        OnGameOver?.Invoke(message);
    }

    public void SetFearMode(bool value)
    {
        if (IsFearMode != value)
        {
            IsFearMode = value;
            if (value)
            {
                FearModeStartTime = DateTime.Now;
            }
            OnFearModeChanged?.Invoke();
        }
    }
}
