using System.IO;
using System.Windows.Media;
using PACMAN_GAME.Properties;
using Color = System.Drawing.Color;
using Timer = System.Windows.Forms.Timer;
using MessageBox = System.Windows.Forms.MessageBox;
using Size = System.Drawing.Size;

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
        _parent = parent;
        _soundManager = soundManager;
    }

    public void SetDirection(int direction)
    {
        NextDirection = direction;
        UpdateImage();
    }

    private void UpdateImage()
    {
        switch (NextDirection)
        {
            case DirectionUp:
                View.Image = Resources.up;
                break;
            case DirectionDown:
                View.Image = Resources.down;
                break;
            case DirectionLeft:
                View.Image = Resources.left;
                break;
            case DirectionRight:
                View.Image = Resources.right;
                break;
        }
    }

    public void Move()
    {
        CheckDirectionChange();
        
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

        bool moved = false;
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
            moved = true;
        }

        if (moved)
        {
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

    private void CheckDirectionChange()
    {
        if (NextDirection == Direction) return;

        int checkX = X;
        int checkY = Y;

        switch (NextDirection)
        {
            case DirectionUp:
                checkY -= 40;
                break;
            case DirectionRight:
                checkX += 40;
                break;
            case DirectionDown:
                checkY += 40;
                break;
            case DirectionLeft:
                checkX -= 40;
                break;
        }

        List<IGameEntity> walls = _parent.Controls
            .OfType<PictureBox>()
            .Where(p => (string)p.Tag == "wall")
            .Select(p => new Wall(p))
            .Cast<IGameEntity>()
            .ToList();

        if (CanMove(checkX, checkY, walls))
        {
            Direction = NextDirection;
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
}

public abstract class Ghost : GameEntity, IMovable
{
    protected const int DirectionUp = 0;
    protected const int DirectionRight = 1;
    protected const int DirectionDown = 2;
    protected const int DirectionLeft = 3;
    
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
        X = 710;
        Y = 420;
        
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
    private readonly GameEntityFactory _entityFactory;
    
    private int _score;
    private int _selectedOption;
    private bool _isGameOver;
    private bool _isFearMode;
    private DateTime _fearModeStartTime;
    private const int FearModeDuration = 10000;
    
    public bool IsInMenu { get; private set; } = true;
    public bool IsGameOver => _isGameOver;
    
    private Pacman _pacman;
    private List<Ghost> _ghosts;
    private List<Coin> _coins;
    
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
        _entityFactory = new GameEntityFactory(parent, soundManager);
        
        InitializeGameEntities();
        SetupTimers();
    }
    
    public void InitializeGameEntities()
    {
        PictureBox pacmanView = _parent.Controls.OfType<PictureBox>().FirstOrDefault(p => p.Name == "pacman");
        if (pacmanView != null)
        {
            _pacman = (Pacman)_entityFactory.CreateGameEntity("pacman", pacmanView);
        }
        
        _ghosts = new List<Ghost>();
        
        PictureBox redGhostView = _parent.Controls.OfType<PictureBox>().FirstOrDefault(p => p.Name == "redGhost");
        if (redGhostView != null)
        {
            _ghosts.Add((RedGhost)_entityFactory.CreateGameEntity("redGhost", redGhostView));
        }
        
        PictureBox yellowGhostView = _parent.Controls.OfType<PictureBox>().FirstOrDefault(p => p.Name == "yellowGhost");
        if (yellowGhostView != null)
        {
            _ghosts.Add((YellowGhost)_entityFactory.CreateGameEntity("yellowGhost", yellowGhostView));
        }
        
        PictureBox pinkGhostView = _parent.Controls.OfType<PictureBox>().FirstOrDefault(p => p.Name == "pinkGhost");
        if (pinkGhostView != null)
        {
            _ghosts.Add((PinkGhost)_entityFactory.CreateGameEntity("pinkGhost", pinkGhostView));
        }
        
        _coins = _parent.Controls
            .OfType<PictureBox>()
            .Where(p => (string)p.Tag == "coin" || (string)p.Tag == "largeCoin")
            .Select(p => (Coin)_entityFactory.CreateGameEntity("coin", p, (string)p.Tag == "largeCoin" ? 20 : 1))
            .ToList();
    }
    
    private void SetupTimers()
    {
        _fearModeTimer.Interval = 100;
        _fearModeTimer.Tick += (s, e) =>
        {
            if (_isFearMode)
            {
                var elapsed = DateTime.Now - _fearModeStartTime;
                if (elapsed.TotalMilliseconds >= FearModeDuration)
                {
                    EndFearMode();
                }
            }
        };
        
        _flickerTimer.Interval = 200;
        _flickerTimer.Tick += (_, _) =>
        {
            if (_isFearMode)
            {
                var elapsed = DateTime.Now - _fearModeStartTime;
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
        IsInMenu = false;
        _uiManager.HideMainMenu();
        _uiManager.UpdateScore(0);
        ResetGame();
        
        _soundManager.PlaySound("game_start");
        try
        {
            var player = _soundManager._sounds_pub["game_start"];
            player.MediaEnded += StartGameAfterSound;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error setting up sound handler: " + ex.Message);
            Timer startDelay = new Timer();
            startDelay.Interval = 4000;
            startDelay.Tick += (s, e) => 
            {
                _gameTimer.Start();
                startDelay.Stop();
            };
            startDelay.Start();
        }
    }
    
    private void StartGameAfterSound(object? sender, EventArgs e)
    {
        if (sender != null)
        {
            ((MediaPlayer)sender).MediaEnded -= StartGameAfterSound;
        }
        _gameTimer.Start();
    }
    
    public void ResetGame()
    {
        _soundManager.StopAllSounds();
        _score = 0;
        
        _uiManager.HideGameOverScreen();
        _deathAnimation.Visible = false;
        
        _isGameOver = false;
        _isFearMode = false;
        
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
            _pacman.X = 35;
            _pacman.Y = 47;
            _pacman.Direction = 1;
            _pacman.NextDirection = 1;
            _pacman.View.Image = Resources.right;
            _pacman.IsVisible = !IsInMenu;
            _pacman.View.BringToFront();
        }
        
        foreach (Ghost ghost in _ghosts)
        {
            ghost.X = 710;
            ghost.Y = 420;
            ghost.Direction = new Random().Next(4);
            ghost.ExitFearMode();
            ghost.IsVisible = !IsInMenu;
        }
        
        foreach (Coin coin in _coins)
        {
            coin.IsVisible = !IsInMenu;
        }
        
        _fearModeTimer.Stop();
        _flickerTimer.Stop();
        _gameTimer.Stop();
        _deathTimer.Stop();
        
        _uiManager.UpdateScore(_score);
        
        if (!IsInMenu)
        {
            try 
            {
                _soundManager.PlaySound("game_start");
                
                var player = _soundManager._sounds_pub["game_start"];
                player.MediaEnded += StartGameAfterSound;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting game: " + ex.Message);
                Timer startDelay = new Timer();
                startDelay.Interval = 4000;
                startDelay.Tick += (s, e) => 
                {
                    _gameTimer.Start();
                    startDelay.Stop();
                };
                startDelay.Start();
            }
        }
    }
    
    public void UpdateGame()
    {
        _uiManager.UpdateScore(_score);
        
        if (!IsInMenu && !IsGameOver)
        {
            _uiManager.UpdateScore(_score);
        }
        
        foreach (Coin coin in _coins.Where(c => c.IsVisible && c.Value > 1))
        {
            if (_collisionHandler.CheckCollision(_pacman, coin))
            {
                _score += coin.Value;
                coin.IsVisible = false;
                ActivateFearMode();
            }
        }
        
        _pacman.Move();
        
        foreach (Coin coin in _coins.Where(c => c.IsVisible && c.Value == 1))
        {
            if (_collisionHandler.CheckCollision(_pacman, coin))
            {
                _score += coin.Value;
                coin.IsVisible = false;
            }
        }
        
        // Bring all ghosts to front before moving them so they appear on top of coins
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
                // Ghost can kill player if either:
                // 1. Global fear mode is not active OR
                // 2. This specific ghost is in normal mode (has been eaten and respawned)
                if (_isFearMode && !ghost.GetIsEaten() && ghost.GetIsFearMode())
                {
                    // Only eat the ghost if it's in fear mode and hasn't been eaten yet
                    EatGhost(ghost);
                }
                else
                {
                    // Ghost kills player if it's in normal mode (either fear mode is off
                    // or this ghost specifically has been reset to normal mode)
                    GameOver("You Died!");
                    return;
                }
            }
        }
        
        if (anyGhostMoved && !_soundManager.IsPlaying("game_start") && !_isFearMode)
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
        
        // Pacman should be on top of ghosts
        _pacman.View.BringToFront();
        
        if (_score >= 258) GameOver("You Win!");
    }
    
    private void ActivateFearMode()
    {
        _isFearMode = true;
        _fearModeStartTime = DateTime.Now;
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
        _isFearMode = false;
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
        _score += 50;
        _soundManager.PlaySound("ghost_eaten");
        
        // Always respawn ghost in default mode regardless of current fear mode
        ghost.X = 710;
        ghost.Y = 420;
        ghost.IsVisible = true;
        ghost.ExitFearMode(); // Force to normal mode
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
            return;
        }
        
        _isGameOver = true;
        
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
    private readonly GameManager _gameManager;
    private readonly ISoundManager _soundManager;
    private readonly IUIManager _uiManager;
    private readonly IInputHandler _inputHandler;
    
    public Form1()
    {
        InitializeComponent();
        
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimumSize = new Size(1445, 1050);
        MaximumSize = new Size(1445, 1050);
        Text = "Pac-Man Game";
        
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
        
        var fearModeTimer = new Timer();
        var flickerTimer = new Timer();
        var deathTimer = new Timer();
        
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
        
        KeyDown += Form1_KeyDown;
        KeyUp += Form1_KeyUp;
    }
    
    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
        _inputHandler.HandleKeyDown(e);
    }
    
    private void Form1_KeyUp(object sender, KeyEventArgs e)
    {
        _inputHandler.HandleKeyUp(e);
    }
}