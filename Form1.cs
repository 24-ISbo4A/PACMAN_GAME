using System.IO;
using System.Windows.Media;
using PACMAN_GAME.Properties;
using PACMAN_GAME.Models;
using Color = System.Drawing.Color;
using Timer = System.Windows.Forms.Timer;
using MessageBox = System.Windows.Forms.MessageBox;
using Size = System.Drawing.Size;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PACMAN_GAME;

public interface ISoundManager
{
    void PlaySound(string soundName);
    void StopSound(string soundName);
    void StopAllSounds();
    void InitializeSounds();
    bool IsPlaying(string soundName);
    Dictionary<string, MediaPlayer> _sounds_pub { get; }
}

public interface IGameEntity
{
    PictureBox View { get; set; }
    bool IsVisible { get; set; }
    int X { get; set; }
    int Y { get; set; }
    int Width { get; }
    int Height { get; }
    Rectangle Bounds { get; }
}

public interface IMovable : IGameEntity
{
    int Speed { get; set; }
    int Direction { get; set; }
    void Move();
    bool CanMove(int newX, int newY, List<IGameEntity> obstacles);
}

public interface ICollisionHandler
{
    bool CheckCollision(IGameEntity entity1, IGameEntity entity2);
    void HandleCollision(IGameEntity entity1, IGameEntity entity2);
}

public interface IGameManager
{
    void StartGame();
    void ResetGame();
    void UpdateGame();
    void GameOver(string message);
    void NextLevel();
}

public interface IInputHandler
{
    void HandleKeyDown(KeyEventArgs e);
    void HandleKeyUp(KeyEventArgs e);
}

public interface IUIManager
{
    void ShowMainMenu();
    void HideMainMenu();
    void ShowGameOverScreen(string message);
    void UpdateScore(int score);
    void UpdateArrowPosition(int selectedOption);
    void HideGameOverScreen();
}

public sealed class SingletonGameManager
{
    private static readonly Lazy<SingletonGameManager> _instance = 
        new Lazy<SingletonGameManager>(() => new SingletonGameManager());
    
    public static SingletonGameManager Instance => _instance.Value;
    
    private bool _isFearMode;
    private bool _isGameOver;
    private bool _isInMenu = true;
    public DateTime FearModeStartTime { get; set; }
    public int Score { get; set; }
    
    public event Action<int> OnScoreChanged = delegate { };
    public event Action<string> OnGameOver = delegate { };
    public event Action OnFearModeChanged = delegate { };
    
    private SingletonGameManager() 
    {
        OnScoreChanged = delegate { };
        OnGameOver = delegate { };
        OnFearModeChanged = delegate { };
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
    
    public bool IsGameOver
    {
        get => _isGameOver;
        set
        {
            if (_isGameOver != value)
            {
                _isGameOver = value;
                if (value) OnGameOver?.Invoke("Game Over");
            }
        }
    }
    
    public bool IsInMenu
    {
        get => _isInMenu;
        set
        {
            _isInMenu = value;
        }
    }
    
    public void AddScore(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
    }
    
    public void ResetState()
    {
        Score = 0;
        IsGameOver = false;
        IsFearMode = false;
        IsInMenu = false;
        OnScoreChanged?.Invoke(Score);
    }
}

public class SoundManager : ISoundManager
{
    private readonly Dictionary<string, MediaPlayer> _sounds = new();
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
                
                _isPlaying[soundName] = soundName == "pacman_move" || 
                                       soundName == "ghost_move" || 
                                       soundName == "fear_mode";
                
                player.Play();
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

public abstract class GameEntity : IGameEntity
{
    public PictureBox View { get; set; }
    public bool IsVisible 
    { 
        get => View.Visible; 
        set => View.Visible = value; 
    }
    
    public int X 
    { 
        get => View.Left; 
        set => View.Left = value; 
    }
    
    public int Y 
    { 
        get => View.Top;
        set => View.Top = value; 
    }
    
    public int Width => View.Width;
    public int Height => View.Height;
    public Rectangle Bounds => View.Bounds;

    protected GameEntity(PictureBox view)
    {
        View = view;
    }
}

public class Pacman : GameEntity, IMovable
{
    private const int DirectionUp = 0;
    private const int DirectionRight = 1;
    private const int DirectionDown = 2;
    private const int DirectionLeft = 3;

    public int Speed { get; set; }
    public int Direction { get; set; }
    public int NextDirection { get; set; }
    private readonly Form _parent;
    private readonly ISoundManager _soundManager;

    public Pacman(PictureBox view, Form parent, ISoundManager soundManager) : base(view)
    {
        Direction = DirectionRight;
        NextDirection = DirectionRight;
        Speed = 12;
        _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        _soundManager = soundManager ?? throw new ArgumentNullException(nameof(soundManager));
        UpdateImage();
    }

    public void SetDirection(int direction)
    {
        if (direction < 0 || direction > 3) return;
        
        NextDirection = direction;
        
        List<IGameEntity> walls = _parent.Controls
            .OfType<PictureBox>()
            .Where(p => (string)p.Tag == "wall")
            .Select(p => new Wall(p))
            .Cast<IGameEntity>()
            .ToList();

        int checkX = X;
        int checkY = Y;

        switch (direction)
        {
            case DirectionUp:
                checkY -= Speed;
                break;
            case DirectionRight:
                checkX += Speed;
                break;
            case DirectionDown:
                checkY += Speed;
                break;
            case DirectionLeft:
                checkX -= Speed;
                break;
        }

        if (CanMove(checkX, checkY, walls))
        {
            Direction = direction;
            UpdateImage();
        }
    }

    private void UpdateImage()
    {
        try
        {
            Image newImage = Direction switch
            {
                DirectionUp => Resources.up,
                DirectionDown => Resources.down,
                DirectionLeft => Resources.left,
                DirectionRight => Resources.right,
                _ => View.Image
            };
            
            if (newImage != null)
            {
                View.Image = newImage;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}");
        }
    }

    public void Move()
    {
        int newX = X;
        int newY = Y;

        switch (Direction)
        {
            case DirectionUp:
                newY -= Speed;
                break;
            case DirectionRight:
                newX += Speed;
                break;
            case DirectionDown:
                newY += Speed;
                break;
            case DirectionLeft:
                newX -= Speed;
                break;
        }

        if (newX < -10) newX = _parent.ClientSize.Width - 10;
        if (newX > _parent.ClientSize.Width - 10) newX = -10;
        if (newY < -10) newY = _parent.ClientSize.Height - 10;
        if (newY > _parent.ClientSize.Height - 10) newY = -10;

        List<IGameEntity> walls = _parent.Controls
            .OfType<PictureBox>()
            .Where(p => (string)p.Tag == "wall")
            .Select(p => new Wall(p))
            .Cast<IGameEntity>()
            .ToList();

        if (CanMove(newX, newY, walls))
        {
            X = newX;
            Y = newY;
            if (!_soundManager.IsPlaying("pacman_move") && !_soundManager.IsPlaying("game_start"))
            {
                _soundManager.PlaySound("pacman_move");
            }
        }
        else
        {
            _soundManager.StopSound("pacman_move");
        }
    }

    public bool CanMove(int newX, int newY, List<IGameEntity> obstacles)
    {
        Rectangle newBounds = new Rectangle(
            newX + 5,
            newY + 5,
            Width - 10,
            Height - 10
        );
        return !obstacles.Any(obstacle => newBounds.IntersectsWith(obstacle.Bounds));
    }
}

public abstract class Ghost : GameEntity, IMovable
{
    protected const int DirectionUp = 0;
    protected const int DirectionRight = 1;
    protected const int DirectionDown = 2;
    protected const int DirectionLeft = 3;
    
    protected const int GHOST_SPAWN_X = 710;
    protected const int GHOST_SPAWN_Y = 420;
    
    public int Speed { get; set; }
    public int Direction { get; set; }
    protected readonly Random Random = new();
    protected readonly Form Parent;
    protected bool IsFearMode;
    protected bool IsEaten;
    
    protected Ghost(PictureBox view, Form parent) : base(view)
    {
        Parent = parent;
        Speed = 8;
        Direction = Random.Next(4);
    }
    
    public virtual void Move()
    {
        int actualSpeed = IsFearMode ? 4 : Speed;
        
        int newX = X;
        int newY = Y;
        
        switch (Direction)
        {
            case DirectionUp:
                newY -= actualSpeed;
                break;
            case DirectionRight:
                newX += actualSpeed;
                break;
            case DirectionDown:
                newY += actualSpeed;
                break;
            case DirectionLeft:
                newX -= actualSpeed;
                break;
        }
        
        List<IGameEntity> walls = Parent.Controls
            .OfType<PictureBox>()
            .Where(p => (string)p.Tag == "wall")
            .Select(p => new Wall(p))
            .Cast<IGameEntity>()
            .ToList();
        
        if (CanMove(newX, newY, walls))
        {
            X = newX;
            Y = newY;
        }
        else
        {
            Direction = Random.Next(4);
        }
    }
    
    public bool CanMove(int newX, int newY, List<IGameEntity> obstacles)
    {
        Rectangle newBounds = new Rectangle(newX, newY, Width, Height);
        
        foreach (IGameEntity obstacle in obstacles)
        {
            if (newBounds.IntersectsWith(obstacle.Bounds))
            {
                return false;
            }
        }
        
        return true;
    }
    
    public void EnterFearMode()
    {
        IsFearMode = true;
        View.Image = Resources.scared_ghost_anim;
    }
    
    public void ExitFearMode()
    {
        IsFearMode = false;
        IsEaten = false;
        SetNormalImage();
    }
    
    public void SetEaten()
    {
        IsEaten = true;
        ShowEatenState();
    }
    
    public bool GetIsEaten()
    {
        return IsEaten;
    }
    
    public bool GetIsFearMode()
    {
        return IsFearMode;
    }
    
    protected abstract void SetNormalImage();
    
    public void Respawn()
    {
        X = GHOST_SPAWN_X;
        Y = GHOST_SPAWN_Y;
        
        if (IsFearMode && !IsEaten)
        {
            View.Image = Resources.scared_ghost_anim;
        }
        else if (IsEaten)
        {
            ShowEatenState();
        }
        else
        {
            SetNormalImage();
        }
        
        IsVisible = true;
    }

    public void ShowEatenState()
    {
        IsEaten = true;
        View.Image = null;
        IsVisible = true;
    }
}

public class RedGhost : Ghost
{
    public RedGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    protected override void SetNormalImage()
    {
        View.Image = Resources.red_left;
    }
}

public class YellowGhost : Ghost
{
    public YellowGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    protected override void SetNormalImage()
    {
        View.Image = Resources.yellow_right;
    }
}

public class PinkGhost : Ghost
{
    public PinkGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    protected override void SetNormalImage()
    {
        View.Image = Resources.pink_left;
    }
}

public class Coin : GameEntity
{
    public int Value { get; }
    
    public Coin(PictureBox view, int value = 1) : base(view)
    {
        Value = value;
    }
}

public class Wall : GameEntity
{
    public Wall(PictureBox view) : base(view) { }
}

public class CollisionHandler : ICollisionHandler
{
    private readonly ISoundManager _soundManager;
    
    public CollisionHandler(ISoundManager soundManager)
    {
        _soundManager = soundManager;
    }
    
    public bool CheckCollision(IGameEntity entity1, IGameEntity entity2)
    {
        return entity1.Bounds.IntersectsWith(entity2.Bounds);
    }
    
    public void HandleCollision(IGameEntity entity1, IGameEntity entity2)
    {
        // This method is empty because the specific collision handling is done in GameManager
    }
}

public class UIManager : IUIManager
{
    private readonly Form _parent;
    private readonly PictureBox _menuBackground;
    private readonly PictureBox _menuArrow;
    private readonly Label _gameOverLabel;
    private readonly Label _restartLabel;
    private readonly Label _scoreLabel;
    private readonly PictureBox _deathAnimation;
    
    public UIManager(Form parent, PictureBox menuBackground, PictureBox menuArrow, 
                    Label gameOverLabel, Label restartLabel, Label scoreLabel,
                    PictureBox deathAnimation)
    {
        _parent = parent;
        _menuBackground = menuBackground;
        _menuArrow = menuArrow;
        _gameOverLabel = gameOverLabel;
        _restartLabel = restartLabel;
        _scoreLabel = scoreLabel;
        _deathAnimation = deathAnimation;
    }
    
    public void ShowMainMenu()
    {
        _menuBackground.Visible = true;
        _menuArrow.Visible = true;
        
        foreach (Control x in _parent.Controls)
        {
            if (x is PictureBox && x != _menuBackground && x != _menuArrow)
            {
                x.Visible = false;
            }
            
            if (x is Label && (x.Text == "PLAY" || x.Text == "QUIT"))
            {
                x.Visible = true;
            }
        }
        
        _scoreLabel.Visible = false;
    }
    
    public void HideMainMenu()
    {
        _menuBackground.Visible = false;
        _menuArrow.Visible = false;
        
        foreach (Control x in _parent.Controls)
        {
            if (x.Parent == _menuBackground)
            {
                x.Visible = false;
            }
            
            if (x is Label && (x.Text == "PLAY" || x.Text == "QUIT"))
            {
                x.Visible = false;
            }
            
            if (x is PictureBox pictureBox && 
                ((string)pictureBox.Tag == "wall" || 
                 (string)pictureBox.Tag == "coin" || 
                 (string)pictureBox.Tag == "largeCoin"))
            {
                pictureBox.Visible = true;
            }
            
            if (x.Name == "pacman" || x.Name == "redGhost" || 
                x.Name == "yellowGhost" || x.Name == "pinkGhost")
            {
                x.Visible = true;
            }
        }
        
        _scoreLabel.Visible = true;
        _scoreLabel.BringToFront();
    }
    
    public void ShowGameOverScreen(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            _gameOverLabel.Visible = false;
            _restartLabel.Visible = false;
            return;
        }
        
        if (message == "You Win!")
        {
            MessageBox.Show("You Win! Press R to restart or ESC to quit.");
            return;
        }
        
        foreach (Control x in _parent.Controls)
        {
            if (x.Name == "pacman" || x.Name == "redGhost" || 
                x.Name == "yellowGhost" || x.Name == "pinkGhost" ||
                (x is PictureBox && x != _menuBackground && x != _menuArrow && x != _deathAnimation))
            {
                x.Visible = false;
            }
        }
        
        _scoreLabel.Visible = false;
        
        _gameOverLabel.Location = new Point(
            (_parent.ClientSize.Width - _gameOverLabel.Width) / 2,
            (_parent.ClientSize.Height - _gameOverLabel.Height) / 2 - 100
        );
        _gameOverLabel.Visible = true;
        _gameOverLabel.BringToFront();
        
        _restartLabel.Location = new Point(
            (_parent.ClientSize.Width - _restartLabel.Width) / 2,
            _gameOverLabel.Bottom + 50
        );
        _restartLabel.Visible = true;
        _restartLabel.BringToFront();
    }
    
    public void UpdateScore(int score)
    {
        _scoreLabel.Text = "Score: " + score;
        _scoreLabel.Visible = true;
        _scoreLabel.BringToFront();
    }
    
    public void UpdateArrowPosition(int selectedOption)
    {
        int baseX = _menuBackground.Width / 2 + 250;
        int baseY = _menuBackground.Bottom - 290;
        int spacing = 100;
        
        _menuArrow.Location = new Point(baseX, baseY + selectedOption * spacing);
        _menuArrow.BringToFront();
    }

    public void HideGameOverScreen()
    {
        _gameOverLabel.Visible = false;
        _restartLabel.Visible = false;
    }
}

public class InputHandler : IInputHandler
{
    private readonly GameManager _gameManager;
    private readonly Pacman _pacman;
    
    public InputHandler(GameManager gameManager, Pacman pacman)
    {
        _gameManager = gameManager;
        _pacman = pacman;
    }
    
    public void HandleKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            Application.Exit();
            return;
        }
        
        if (_gameManager.IsInMenu)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    _gameManager.SelectMenuOption(-1);
                    break;
                case Keys.Down:
                    _gameManager.SelectMenuOption(1);
                    break;
                case Keys.Enter:
                    _gameManager.ExecuteSelectedMenuOption();
                    break;
            }
            return;
        }
        
        if (e.KeyCode == Keys.R)
        {
            _gameManager.ResetGame();
            return;
        }
        
        if (_gameManager.IsGameOver) return;
        
        switch (e.KeyCode)
        {
            case Keys.Up:
                _pacman.SetDirection(0);
                break;
            case Keys.Down:
                _pacman.SetDirection(2);
                break;
            case Keys.Left:
                _pacman.SetDirection(3);
                break;
            case Keys.Right:
                _pacman.SetDirection(1);
                break;
        }
    }
    
    public void HandleKeyUp(KeyEventArgs e)
    {
        // No specific actions on key up in this game
    }
}

public class GameManager : IGameManager
{
    private readonly ISoundManager _soundManager;
    private readonly IUIManager _uiManager;
    private readonly ICollisionHandler _collisionHandler;
    private readonly Timer _gameTimer;
    private readonly Timer _fearModeTimer;
    private readonly Timer _flickerTimer;
    private readonly Timer _deathTimer;
    private readonly PictureBox _deathAnimation;
    private readonly Form _parent;
    
    private int _selectedOption;
    private const int FearModeDuration = 10000;
    
    public bool IsInMenu => GameState.IsInMenu;
    public bool IsGameOver => GameState.IsGameOver;
    
    private SingletonGameManager GameState => SingletonGameManager.Instance;
    
    private Pacman _pacman = null!;
    private List<Ghost> _ghosts = new();
    private List<Coin> _coins = new();
    
    private const int PACMAN_SPAWN_X = 30;
    private const int PACMAN_SPAWN_Y = 30;
    
    public GameManager(
        Form parent,
        ISoundManager soundManager, 
        IUIManager uiManager, 
        ICollisionHandler collisionHandler,
        Timer gameTimer,
        Timer fearModeTimer,
        Timer flickerTimer,
        Timer deathTimer,
        PictureBox deathAnimation)
    {
        _parent = parent;
        _soundManager = soundManager;
        _uiManager = uiManager;
        _collisionHandler = collisionHandler;
        _gameTimer = gameTimer;
        _fearModeTimer = fearModeTimer;
        _flickerTimer = flickerTimer;
        _deathTimer = deathTimer;
        _deathAnimation = deathAnimation;
        
        GameState.OnScoreChanged += score => _uiManager.UpdateScore(score);
        GameState.OnGameOver += message => GameOver(message);
        GameState.OnFearModeChanged += () => 
        {
            if (GameState.IsFearMode)
            {
                _fearModeTimer.Start();
                _flickerTimer.Start();
            }
            else
            {
                _fearModeTimer.Stop();
                _flickerTimer.Stop();
            }
        };
        
        InitializeGameEntities();
        SetupTimers();
    }
    
    public void InitializeGameEntities()
    {
        PictureBox pacmanView = _parent.Controls.OfType<PictureBox>().FirstOrDefault(p => p.Name == "pacman");
        if (pacmanView != null)
        {
            _pacman = new Pacman(pacmanView, _parent, _soundManager);
        }
        
        _ghosts = new List<Ghost>();
        
        PictureBox redGhostView = _parent.Controls.OfType<PictureBox>().FirstOrDefault(p => p.Name == "redGhost");
        if (redGhostView != null)
        {
            _ghosts.Add(new RedGhost(redGhostView, _parent));
        }
        
        PictureBox yellowGhostView = _parent.Controls.OfType<PictureBox>().FirstOrDefault(p => p.Name == "yellowGhost");
        if (yellowGhostView != null)
        {
            _ghosts.Add(new YellowGhost(yellowGhostView, _parent));
        }
        
        PictureBox pinkGhostView = _parent.Controls.OfType<PictureBox>().FirstOrDefault(p => p.Name == "pinkGhost");
        if (pinkGhostView != null)
        {
            _ghosts.Add(new PinkGhost(pinkGhostView, _parent));
        }
        
        _coins = _parent.Controls
            .OfType<PictureBox>()
            .Where(p => (string)p.Tag == "coin" || (string)p.Tag == "largeCoin")
            .Select(p => new Coin(p, (string)p.Tag == "largeCoin" ? 20 : 1))
            .ToList();
    }
    
    private void SetupTimers()
    {
        _fearModeTimer.Interval = 100;
        _fearModeTimer.Tick += (s, e) =>
        {
            if (GameState.IsFearMode)
            {
                var elapsed = DateTime.Now - GameState.FearModeStartTime;
                if (elapsed.TotalMilliseconds >= FearModeDuration)
                {
                    EndFearMode();
                }
            }
        };
        
        _flickerTimer.Interval = 200;
        _flickerTimer.Tick += (_, _) =>
        {
            if (GameState.IsFearMode)
            {
                var elapsed = DateTime.Now - GameState.FearModeStartTime;
                if (elapsed.TotalMilliseconds >= FearModeDuration - 3000)
                {
                    foreach (Ghost ghost in _ghosts)
                    {
                        ghost.IsVisible = !ghost.IsVisible;
                    }
                }
            }
        };
        
        _deathTimer.Interval = 1200;
        _deathTimer.Tick += (s, e) =>
        {
            _deathTimer.Stop();
            _deathAnimation.Visible = false;
            _uiManager.ShowGameOverScreen("Game Over");
        };
        
        _gameTimer.Tick += (s, e) => UpdateGame();
    }
    
    public void StartGame()
    {
        GameState.IsInMenu = false;
        _uiManager.HideMainMenu();
        _uiManager.UpdateScore(0);
        _gameTimer.Start();
        ResetGame();
    }
    
    public void ResetGame()
    {
        _soundManager.StopAllSounds();
        GameState.ResetState();
        
        // Восстанавливаем видимость стен
        foreach (Control control in _parent.Controls)
        {
            if (control is PictureBox pictureBox && (string)pictureBox.Tag == "wall")
            {
                pictureBox.Visible = true;
                pictureBox.BringToFront();
            }
        }
        
        if (_pacman != null)
        {
            _pacman.X = PACMAN_SPAWN_X;
            _pacman.Y = PACMAN_SPAWN_Y;
            _pacman.Direction = 1;
            _pacman.IsVisible = true;
        }
        
        foreach (Ghost ghost in _ghosts)
        {
            ghost.Respawn();
            ghost.ExitFearMode();
        }
        
        foreach (Coin coin in _coins)
        {
            coin.IsVisible = true;
        }
        
        _uiManager.HideGameOverScreen();
        _gameTimer.Start();
    }
    
    public void UpdateGame()
    {
        _uiManager.UpdateScore(GameState.Score);
        
        if (!GameState.IsInMenu && !GameState.IsGameOver)
        {
            _uiManager.UpdateScore(GameState.Score);
            
            foreach (Coin coin in _coins.Where(c => c.IsVisible))
            {
                if (_collisionHandler.CheckCollision(_pacman, coin))
                {
                    GameState.Score += coin.Value;
                    coin.IsVisible = false;
                    if (coin.Value > 1)
                    {
                        ActivateFearMode();
                    }
                }
            }
            
            _pacman.Move();
            
            foreach (Ghost ghost in _ghosts)
            {
                ghost.View.BringToFront();
            }
            
            bool anyGhostMoved = false;
            foreach (Ghost ghost in _ghosts)
            {
                bool moved = ghost.IsVisible;
                ghost.Move();
                if (moved) anyGhostMoved = true;
                
                if (_collisionHandler.CheckCollision(_pacman, ghost))
                {
                    if (GameState.IsFearMode && !ghost.GetIsEaten() && ghost.GetIsFearMode())
                    {
                        EatGhost(ghost);
                    }
                    else if (!GameState.IsGameOver)
                    {
                        GameOver("You Died!");
                        return;
                    }
                }
            }
            
            if (anyGhostMoved && !_soundManager.IsPlaying("game_start") && !GameState.IsFearMode)
            {
                if (!_soundManager.IsPlaying("ghost_move"))
                {
                    _soundManager.PlaySound("ghost_move");
                }
            }
            else
            {
                _soundManager.StopSound("ghost_move");
            }
            
            _pacman.View.BringToFront();
            
            if (GameState.Score >= 258)
            {
                GameOver("You Win!");
                _gameTimer.Stop();
                return;
            }
        }
    }
    
    private void ActivateFearMode()
    {
        GameState.IsFearMode = true;
        GameState.FearModeStartTime = DateTime.Now;
        _fearModeTimer.Start();
        _flickerTimer.Start();
        
        foreach (Ghost ghost in _ghosts)
        {
            ghost.EnterFearMode();
        }
        
        _soundManager.StopSound("ghost_move");
        
        if (!_soundManager.IsPlaying("fear_mode"))
        {
            _soundManager.PlaySound("fear_mode");
        }
    }
    
    private void EndFearMode()
    {
        GameState.IsFearMode = false;
        _fearModeTimer.Stop();
        _flickerTimer.Stop();
        
        _soundManager.StopSound("fear_mode");
        
        foreach (Ghost ghost in _ghosts)
        {
            ghost.ExitFearMode();
            ghost.IsVisible = true;
        }
    }
    
    private void EatGhost(Ghost ghost)
    {
        GameState.Score += 50;
        _soundManager.PlaySound("ghost_eaten");
        
        // Always respawn ghost in default mode regardless of current fear mode
        ghost.Respawn();
    }
    
    public void GameOver(string message)
    {
        // Stop all timers to prevent any ghost flickering or movement
        _gameTimer.Stop();
        _fearModeTimer.Stop();
        _flickerTimer.Stop();
        _deathTimer.Stop();
        
        _soundManager.StopAllSounds();
        
        if (message == "You Win!")
        {
            _uiManager.ShowGameOverScreen(message);
            foreach (Ghost ghost in _ghosts)
            {
                ghost.IsVisible = false;  // Скрываем призраков при победе
            }
            return;  // Не устанавливаем IsGameOver при победе
        }
        
        GameState.IsGameOver = true;
        
        // Explicitly hide all ghosts before showing death animation
        foreach (Ghost ghost in _ghosts)
        {
            ghost.IsVisible = false;
        }
        
        // Play death sound
        _soundManager.PlaySound("pacman_death");
        
        // Show death animation
        _deathAnimation.Image = Resources.pacman_death_anim;
        _deathAnimation.Location = new Point(_pacman.X, _pacman.Y);
        _deathAnimation.Size = new Size(45, 60);
        _deathAnimation.SizeMode = PictureBoxSizeMode.StretchImage;
        _deathAnimation.BackColor = Color.Transparent;
        _deathAnimation.BringToFront();
        _deathAnimation.Visible = true;
        
        _deathTimer.Start();
    }
    
    public void NextLevel()
    {
        // Implementation for multiple levels
    }
    
    public void SelectMenuOption(int direction)
    {
        _selectedOption = (_selectedOption + direction + 2) % 2;
        _uiManager.UpdateArrowPosition(_selectedOption);
    }
    
    public void ExecuteSelectedMenuOption()
    {
        if (_selectedOption == 0)
        {
            StartGame();
        }
        else if (_selectedOption == 1)
        {
            Application.Exit();
        }
    }

    public Pacman GetPacman()
    {
        return _pacman;
    }
}

public partial class Form1 : Form
{
    private GameManager _gameManager = null!;
    private ISoundManager _soundManager = null!;
    private IUIManager _uiManager = null!;
    private IInputHandler _inputHandler = null!;
    
    public Form1()
    {
        InitializeComponent();
        
        // Проверка наличия необходимых ресурсов
        if (!ValidateResources())
        {
            MessageBox.Show("Не удалось загрузить необходимые ресурсы. Игра будет закрыта.");
            Application.Exit();
            return;
        }
        
        this.KeyPreview = true;
        this.Focus();
        
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimumSize = new Size(1445, 1050);
        MaximumSize = new Size(1445, 1050);
        Text = "Pac-Man Game";
        
        gameTimer.Interval = 20;
        gameTimer.Enabled = false;
        
        var fearModeTimer = new Timer { Interval = 100, Enabled = false };
        var flickerTimer = new Timer { Interval = 200, Enabled = false };
        var deathTimer = new Timer { Interval = 1200, Enabled = false };
        
        foreach (Control control in Controls)
        {
            if (control is PictureBox pictureBox && (string)pictureBox.Tag == "wall")
            {
                pictureBox.Visible = true;
                pictureBox.BringToFront();
            }
        }
        
        var menuBackground = new PictureBox
        {
            Size = new Size(1445, 1050),
            Location = new Point(0, 0),
            SizeMode = PictureBoxSizeMode.StretchImage,
            BackColor = Color.Black,
            Image = Resources.main_menu,
            Visible = true
        };
        Controls.Add(menuBackground);
        
        var startLabel = new Label
        {
            Text = "PLAY",
            Font = new Font("Arial", 24, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            BackColor = Color.Transparent,
            Parent = menuBackground
        };
        startLabel.Location = new Point(
            (menuBackground.Width - startLabel.PreferredWidth) / 2,
            menuBackground.Bottom - 200
        );
        Controls.Add(startLabel);
        
        var exitLabel = new Label
        {
            Text = "QUIT",
            Font = new Font("Arial", 24, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            BackColor = Color.Transparent,
            Parent = menuBackground
        };
        exitLabel.Location = new Point(
            (menuBackground.Width - exitLabel.PreferredWidth) / 2,
            menuBackground.Bottom - 100
        );
        Controls.Add(exitLabel);
        
        foreach (Control x in Controls)
        {
            if (x is PictureBox && x != menuBackground)
            {
                x.Visible = false;
            }
        }
        
        txtScore.Visible = false;
        
        var menuArrow = new PictureBox
        {
            Size = new Size(60, 60),
            SizeMode = PictureBoxSizeMode.StretchImage,
            BackColor = ColorTranslator.FromHtml("#0B0102"),
            Image = Resources.menu_arrow,
            Visible = true
        };
        Controls.Add(menuArrow);
        menuArrow.BringToFront();
        
        var deathAnimation = new PictureBox
        {
            Size = new Size(40, 40),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Visible = false
        };
        Controls.Add(deathAnimation);
        
        var gameOverLabel = new Label
        {
            Text = "GAME OVER",
            Font = new Font("Arial", 48, FontStyle.Bold),
            ForeColor = Color.Red,
            AutoSize = true,
            Visible = false
        };
        Controls.Add(gameOverLabel);
        
        var restartLabel = new Label
        {
            Text = "Restart - R",
            Font = new Font("Arial", 24, FontStyle.Bold),
            ForeColor = Color.Red,
            AutoSize = true,
            Visible = false
        };
        Controls.Add(restartLabel);
        
        _soundManager = new SoundManager();
        _soundManager.InitializeSounds();
        
        _uiManager = new UIManager(this, menuBackground, menuArrow, gameOverLabel, restartLabel, txtScore, deathAnimation);
        
        var collisionHandler = new CollisionHandler(_soundManager);
        
        _gameManager = new GameManager(
            this,
            _soundManager,
            _uiManager,
            collisionHandler,
            gameTimer,
            fearModeTimer,
            flickerTimer,
            deathTimer,
            deathAnimation
        );
        
        _inputHandler = new InputHandler(_gameManager, _gameManager.GetPacman());
        
        _uiManager.UpdateArrowPosition(0);
        
        this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        this.KeyUp += new KeyEventHandler(Form1_KeyUp);
    }
    
    private bool ValidateResources()
    {
        try
        {
            var requiredImages = new[]
            {
                Resources.up,
                Resources.down,
                Resources.left,
                Resources.right,
                Resources.red_left,
                Resources.yellow_right,
                Resources.pink_left,
                Resources.scared_ghost_anim,
                Resources.pacman_death_anim,
                Resources.main_menu,
                Resources.menu_arrow
            };

            return requiredImages.All(img => img != null);
        }
        catch
        {
            return false;
        }
    }
    
    private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        _inputHandler.HandleKeyDown(e);
    }
    
    private void Form1_KeyUp(object? sender, KeyEventArgs e)
    {
        _inputHandler.HandleKeyUp(e);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        
        gameTimer?.Stop();
        gameTimer?.Dispose();
        
        _soundManager?.StopAllSounds();
        
        foreach (Control control in Controls)
        {
            if (control is PictureBox pictureBox)
            {
                pictureBox.Image?.Dispose();
            }
            control.Dispose();
        }
    }
}