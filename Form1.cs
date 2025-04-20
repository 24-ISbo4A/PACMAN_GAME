using System.IO;
using System.Windows.Media;
using PACMAN_GAME.Properties;
using PACMAN_GAME.Managers;
using PACMAN_GAME.Models;
using PACMAN_GAME.Interfaces;
using Color = System.Drawing.Color;
using Timer = System.Windows.Forms.Timer;
using MessageBox = System.Windows.Forms.MessageBox;
using Size = System.Drawing.Size;

namespace PACMAN_GAME;

/// <summary>
/// Интерфейс для управления звуковыми эффектами в игре
/// </summary>
public interface ISoundManager
{
    /// <summary>
    /// Воспроизводит указанный звук
    /// </summary>
    /// <param name="soundName">Название звука</param>
    void PlaySound(string soundName);

    /// <summary>
    /// Останавливает указанный звук
    /// </summary>
    /// <param name="soundName">Название звука</param>
    void StopSound(string soundName);

    /// <summary>
    /// Останавливает все звуки
    /// </summary>
    void StopAllSounds();

    /// <summary>
    /// Инициализирует звуковые эффекты
    /// </summary>
    void InitializeSounds();

    /// <summary>
    /// Проверяет, воспроизводится ли указанный звук
    /// </summary>
    /// <param name="soundName">Название звука</param>
    /// <returns>True, если звук воспроизводится</returns>
    bool IsPlaying(string soundName);

    /// <summary>
    /// Публичный словарь звуковых плееров
    /// </summary>
    Dictionary<string, MediaPlayer> _sounds_pub { get; }
}

/// <summary>
/// Базовый интерфейс для всех игровых объектов
/// </summary>
public interface IGameEntity
{
    /// <summary>
    /// Визуальное представление объекта
    /// </summary>
    PictureBox View { get; set; }

    /// <summary>
    /// Видимость объекта
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    /// X-координата объекта
    /// </summary>
    int X { get; set; }

    /// <summary>
    /// Y-координата объекта
    /// </summary>
    int Y { get; set; }

    /// <summary>
    /// Ширина объекта
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Высота объекта
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Границы объекта
    /// </summary>
    Rectangle Bounds { get; }
}

/// <summary>
/// Интерфейс для перемещающихся объектов
/// </summary>
public interface IMovable : IGameEntity
{
    /// <summary>
    /// Скорость движения
    /// </summary>
    int Speed { get; set; }

    /// <summary>
    /// Направление движения
    /// </summary>
    int Direction { get; set; }

    /// <summary>
    /// Перемещает объект
    /// </summary>
    void Move();

    /// <summary>
    /// Проверяет возможность перемещения
    /// </summary>
    /// <param name="newX">Новая X-координата</param>
    /// <param name="newY">Новая Y-координата</param>
    /// <param name="obstacles">Список препятствий</param>
    /// <returns>True, если перемещение возможно</returns>
    bool CanMove(int newX, int newY, List<IGameEntity> obstacles);
}

/// <summary>
/// Интерфейс для обработки столкновений
/// </summary>
public interface ICollisionHandler
{
    /// <summary>
    /// Проверяет столкновение между двумя объектами
    /// </summary>
    /// <param name="entity1">Первый объект</param>
    /// <param name="entity2">Второй объект</param>
    /// <returns>True, если объекты сталкиваются</returns>
    bool CheckCollision(IGameEntity entity1, IGameEntity entity2);

    /// <summary>
    /// Обрабатывает столкновение между двумя объектами
    /// </summary>
    /// <param name="entity1">Первый объект</param>
    /// <param name="entity2">Второй объект</param>
    void HandleCollision(IGameEntity entity1, IGameEntity entity2);
}

/// <summary>
/// Интерфейс для управления игровым процессом
/// </summary>
public interface IGameManager
{
    /// <summary>
    /// Запускает игру
    /// </summary>
    void StartGame();

    /// <summary>
    /// Сбрасывает состояние игры
    /// </summary>
    void ResetGame();

    /// <summary>
    /// Обновляет состояние игры
    /// </summary>
    void UpdateGame();

    /// <summary>
    /// Обрабатывает окончание игры
    /// </summary>
    /// <param name="message">Сообщение об окончании игры</param>
    void GameOver(string message);

    /// <summary>
    /// Переходит на следующий уровень
    /// </summary>
    void NextLevel();
}

/// <summary>
/// Интерфейс для обработки пользовательского ввода
/// </summary>
public interface IInputHandler
{
    /// <summary>
    /// Обрабатывает нажатие клавиши
    /// </summary>
    /// <param name="e">Аргументы события нажатия клавиши</param>
    void HandleKeyDown(KeyEventArgs e);

    /// <summary>
    /// Обрабатывает отпускание клавиши
    /// </summary>
    /// <param name="e">Аргументы события отпускания клавиши</param>
    void HandleKeyUp(KeyEventArgs e);
}

/// <summary>
/// Интерфейс для управления пользовательским интерфейсом
/// </summary>
public interface IUIManager
{
    /// <summary>
    /// Показывает главное меню
    /// </summary>
    void ShowMainMenu();

    /// <summary>
    /// Скрывает главное меню
    /// </summary>
    void HideMainMenu();

    /// <summary>
    /// Показывает экран окончания игры
    /// </summary>
    /// <param name="message">Сообщение для отображения</param>
    void ShowGameOverScreen(string message);

    /// <summary>
    /// Обновляет счет
    /// </summary>
    /// <param name="score">Новое значение счета</param>
    void UpdateScore(int score);

    /// <summary>
    /// Обновляет позицию стрелки в меню
    /// </summary>
    /// <param name="selectedOption">Выбранная опция</param>
    void UpdateArrowPosition(int selectedOption);

    /// <summary>
    /// Скрывает экран окончания игры
    /// </summary>
    void HideGameOverScreen();
}

/// <summary>
/// Класс для управления звуковыми эффектами
/// </summary>
public class SoundManager : ISoundManager
{
    private readonly Dictionary<string, MediaPlayer> _sounds = new();
    private readonly Dictionary<string, bool> _isPlaying = new();
    
    /// <summary>
    /// Публичный словарь звуковых плееров
    /// </summary>
    public Dictionary<string, MediaPlayer> _sounds_pub => _sounds;

    /// <summary>
    /// Инициализирует звуковые файлы
    /// </summary>
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

    /// <summary>
    /// Воспроизводит указанный звук
    /// </summary>
    /// <param name="soundName">Название звука</param>
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

    /// <summary>
    /// Останавливает указанный звук
    /// </summary>
    /// <param name="soundName">Название звука</param>
    public void StopSound(string soundName)
    {
        if (_sounds.ContainsKey(soundName) && _isPlaying[soundName])
        {
            _isPlaying[soundName] = false;
            _sounds[soundName].Stop();
        }
    }

    /// <summary>
    /// Останавливает все звуки
    /// </summary>
    public void StopAllSounds()
    {
        foreach (var sound in _sounds.Keys)
        {
            StopSound(sound);
        }
    }

    /// <summary>
    /// Проверяет, воспроизводится ли указанный звук
    /// </summary>
    /// <param name="soundName">Название звука</param>
    /// <returns>True, если звук воспроизводится</returns>
    public bool IsPlaying(string soundName)
    {
        return _isPlaying.ContainsKey(soundName) && _isPlaying[soundName];
    }
}

/// <summary>
/// Абстрактный базовый класс для игровых объектов
/// </summary>
public abstract class GameEntity : IGameEntity
{
    /// <summary>
    /// Визуальное представление объекта
    /// </summary>
    public PictureBox View { get; set; }

    /// <summary>
    /// Видимость объекта
    /// </summary>
    public bool IsVisible 
    { 
        get => View.Visible; 
        set => View.Visible = value; 
    }
    
    /// <summary>
    /// X-координата объекта
    /// </summary>
    public int X 
    { 
        get => View.Left; 
        set => View.Left = value; 
    }
    
    /// <summary>
    /// Y-координата объекта
    /// </summary>
    public int Y 
    { 
        get => View.Top;
        set => View.Top = value; 
    }
    
    /// <summary>
    /// Ширина объекта
    /// </summary>
    public int Width => View.Width;

    /// <summary>
    /// Высота объекта
    /// </summary>
    public int Height => View.Height;

    /// <summary>
    /// Границы объекта
    /// </summary>
    public Rectangle Bounds => View.Bounds;

    /// <summary>
    /// Конструктор базового класса
    /// </summary>
    /// <param name="view">Визуальное представление объекта</param>
    protected GameEntity(PictureBox view)
    {
        View = view;
    }
}

/// <summary>
/// Класс, представляющий главного героя игры
/// </summary>
public class Pacman : GameEntity, IMovable
{
    /// <summary>
    /// Константы направлений движения
    /// </summary>
    private const int DirectionUp = 0;
    private const int DirectionRight = 1;
    private const int DirectionDown = 2;
    private const int DirectionLeft = 3;

    /// <summary>
    /// Скорость движения
    /// </summary>
    public int Speed { get; set; }

    /// <summary>
    /// Текущее направление движения
    /// </summary>
    public int Direction { get; set; }

    /// <summary>
    /// Следующее направление движения
    /// </summary>
    public int NextDirection { get; set; }

    private readonly Form _parent;
    private readonly ISoundManager _soundManager;

    /// <summary>
    /// Конструктор класса Pacman
    /// </summary>
    /// <param name="view">Визуальное представление</param>
    /// <param name="parent">Родительская форма</param>
    /// <param name="soundManager">Менеджер звуков</param>
    public Pacman(PictureBox view, Form parent, ISoundManager soundManager) : base(view)
    {
        Direction = DirectionRight;
        NextDirection = DirectionRight;
        Speed = 12;
        _parent = parent;
        _soundManager = soundManager;
    }

    /// <summary>
    /// Устанавливает направление движения
    /// </summary>
    /// <param name="direction">Новое направление</param>
    public void SetDirection(int direction)
    {
        NextDirection = direction;
        UpdateImage();
    }

    /// <summary>
    /// Обновляет изображение в соответствии с направлением
    /// </summary>
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

    /// <summary>
    /// Перемещает объект
    /// </summary>
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

    /// <summary>
    /// Проверяет возможность перемещения
    /// </summary>
    /// <param name="newX">Новая X-координата</param>
    /// <param name="newY">Новая Y-координата</param>
    /// <param name="obstacles">Список препятствий</param>
    /// <returns>True, если перемещение возможно</returns>
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

/// <summary>
/// Абстрактный класс для призраков
/// </summary>
public abstract class Ghost : GameEntity, IMovable
{
    /// <summary>
    /// Константы направлений движения
    /// </summary>
    protected const int DirectionUp = 0;
    protected const int DirectionRight = 1;
    protected const int DirectionDown = 2;
    protected const int DirectionLeft = 3;
    
    /// <summary>
    /// Скорость движения
    /// </summary>
    public int Speed { get; set; }

    /// <summary>
    /// Направление движения
    /// </summary>
    public int Direction { get; set; }

    /// <summary>
    /// Генератор случайных чисел
    /// </summary>
    protected readonly Random Random = new();

    /// <summary>
    /// Родительская форма
    /// </summary>
    protected readonly Form Parent;

    /// <summary>
    /// Флаг режима страха
    /// </summary>
    protected bool IsFearMode;

    /// <summary>
    /// Флаг съеденности
    /// </summary>
    protected bool IsEaten;
    
    /// <summary>
    /// Конструктор базового класса призрака
    /// </summary>
    /// <param name="view">Визуальное представление</param>
    /// <param name="parent">Родительская форма</param>
    protected Ghost(PictureBox view, Form parent) : base(view)
    {
        Parent = parent;
        Speed = 8;
        Direction = Random.Next(4);
    }
    
    /// <summary>
    /// Перемещает призрака
    /// </summary>
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
    
    /// <summary>
    /// Проверяет возможность перемещения
    /// </summary>
    /// <param name="newX">Новая X-координата</param>
    /// <param name="newY">Новая Y-координата</param>
    /// <param name="obstacles">Список препятствий</param>
    /// <returns>True, если перемещение возможно</returns>
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
    
    /// <summary>
    /// Активирует режим страха
    /// </summary>
    public void EnterFearMode()
    {
        IsFearMode = true;
        View.Image = Resources.scared_ghost_anim;
    }
    
    /// <summary>
    /// Деактивирует режим страха
    /// </summary>
    public void ExitFearMode()
    {
        IsFearMode = false;
        IsEaten = false;
        SetNormalImage();
    }
    
    /// <summary>
    /// Устанавливает состояние съеденности
    /// </summary>
    public void SetEaten()
    {
        IsEaten = true;
        ShowEatenState();
    }
    
    /// <summary>
    /// Возвращает состояние съеденности
    /// </summary>
    /// <returns>True, если призрак съеден</returns>
    public bool GetIsEaten()
    {
        return IsEaten;
    }
    
    /// <summary>
    /// Возвращает состояние режима страха
    /// </summary>
    /// <returns>True, если призрак в режиме страха</returns>
    public bool GetIsFearMode()
    {
        return IsFearMode;
    }
    
    /// <summary>
    /// Устанавливает нормальное изображение призрака
    /// </summary>
    protected abstract void SetNormalImage();
    
    /// <summary>
    /// Возрождает призрака
    /// </summary>
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

    /// <summary>
    /// Показывает состояние съеденности
    /// </summary>
    public void ShowEatenState()
    {
        IsEaten = true;
        View.Image = null;
        IsVisible = true;
    }
}

/// <summary>
/// Класс красного призрака
/// </summary>
public class RedGhost : Ghost
{
    /// <summary>
    /// Конструктор красного призрака
    /// </summary>
    /// <param name="view">Визуальное представление</param>
    /// <param name="parent">Родительская форма</param>
    public RedGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    /// <summary>
    /// Устанавливает нормальное изображение красного призрака
    /// </summary>
    protected override void SetNormalImage()
    {
        View.Image = Resources.red_left;
    }
}

/// <summary>
/// Класс желтого призрака
/// </summary>
public class YellowGhost : Ghost
{
    /// <summary>
    /// Конструктор желтого призрака
    /// </summary>
    /// <param name="view">Визуальное представление</param>
    /// <param name="parent">Родительская форма</param>
    public YellowGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    /// <summary>
    /// Устанавливает нормальное изображение желтого призрака
    /// </summary>
    protected override void SetNormalImage()
    {
        View.Image = Resources.yellow_right;
    }
}

/// <summary>
/// Класс розового призрака
/// </summary>
public class PinkGhost : Ghost
{
    /// <summary>
    /// Конструктор розового призрака
    /// </summary>
    /// <param name="view">Визуальное представление</param>
    /// <param name="parent">Родительская форма</param>
    public PinkGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    /// <summary>
    /// Устанавливает нормальное изображение розового призрака
    /// </summary>
    protected override void SetNormalImage()
    {
        View.Image = Resources.pink_left;
    }
}

/// <summary>
/// Класс для монеток
/// </summary>
public class Coin : GameEntity
{
    /// <summary>
    /// Значение монетки
    /// </summary>
    public int Value { get; }
    
    /// <summary>
    /// Конструктор монетки
    /// </summary>
    /// <param name="view">Визуальное представление</param>
    /// <param name="value">Значение монетки</param>
    public Coin(PictureBox view, int value = 1) : base(view)
    {
        Value = value;
    }
}

/// <summary>
/// Класс для стен
/// </summary>
public class Wall : GameEntity
{
    /// <summary>
    /// Конструктор стены
    /// </summary>
    /// <param name="view">Визуальное представление</param>
    public Wall(PictureBox view) : base(view) { }
}

/// <summary>
/// Класс для обработки столкновений
/// </summary>
public class CollisionHandler : ICollisionHandler
{
    private readonly ISoundManager _soundManager;
    
    /// <summary>
    /// Конструктор обработчика столкновений
    /// </summary>
    /// <param name="soundManager">Менеджер звуков</param>
    public CollisionHandler(ISoundManager soundManager)
    {
        _soundManager = soundManager;
    }
    
    /// <summary>
    /// Проверяет столкновение между двумя объектами
    /// </summary>
    /// <param name="entity1">Первый объект</param>
    /// <param name="entity2">Второй объект</param>
    /// <returns>True, если объекты сталкиваются</returns>
    public bool CheckCollision(IGameEntity entity1, IGameEntity entity2)
    {
        return entity1.Bounds.IntersectsWith(entity2.Bounds);
    }
    
    /// <summary>
    /// Обрабатывает столкновение между двумя объектами
    /// </summary>
    /// <param name="entity1">Первый объект</param>
    /// <param name="entity2">Второй объект</param>
    public void HandleCollision(IGameEntity entity1, IGameEntity entity2)
    {
        // This method is empty because the specific collision handling is done in GameManager
    }
}

/// <summary>
/// Класс для управления пользовательским интерфейсом
/// </summary>
public class UIManager : IUIManager
{
    private readonly Form _parent;
    private readonly PictureBox _menuBackground;
    private readonly PictureBox _menuArrow;
    private readonly Label _gameOverLabel;
    private readonly Label _restartLabel;
    private readonly Label _scoreLabel;
    private readonly PictureBox _deathAnimation;
    
    /// <summary>
    /// Конструктор менеджера пользовательского интерфейса
    /// </summary>
    /// <param name="parent">Родительская форма</param>
    /// <param name="menuBackground">Фон меню</param>
    /// <param name="menuArrow">Стрелка меню</param>
    /// <param name="gameOverLabel">Метка окончания игры</param>
    /// <param name="restartLabel">Метка перезапуска</param>
    /// <param name="scoreLabel">Метка счета</param>
    /// <param name="deathAnimation">Анимация смерти</param>
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
    
    /// <summary>
    /// Показывает главное меню
    /// </summary>
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
    
    /// <summary>
    /// Скрывает главное меню
    /// </summary>
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
    
    /// <summary>
    /// Показывает экран окончания игры
    /// </summary>
    /// <param name="message">Сообщение для отображения</param>
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
    
    /// <summary>
    /// Обновляет счет
    /// </summary>
    /// <param name="score">Новое значение счета</param>
    public void UpdateScore(int score)
    {
        _scoreLabel.Text = "Score: " + score;
        _scoreLabel.Visible = true;
        _scoreLabel.BringToFront();
    }
    
    /// <summary>
    /// Обновляет позицию стрелки в меню
    /// </summary>
    /// <param name="selectedOption">Выбранная опция</param>
    public void UpdateArrowPosition(int selectedOption)
    {
        int baseX = _menuBackground.Width / 2 + 250;
        int baseY = _menuBackground.Bottom - 290;
        int spacing = 100;
        
        _menuArrow.Location = new Point(baseX, baseY + selectedOption * spacing);
        _menuArrow.BringToFront();
    }

    /// <summary>
    /// Скрывает экран окончания игры
    /// </summary>
    public void HideGameOverScreen()
    {
        _gameOverLabel.Visible = false;
        _restartLabel.Visible = false;
    }
}

/// <summary>
/// Класс для обработки пользовательского ввода
/// </summary>
public class InputHandler : IInputHandler
{
    private readonly GameManager _gameManager;
    private readonly Pacman _pacman;
    
    /// <summary>
    /// Конструктор обработчика ввода
    /// </summary>
    /// <param name="gameManager">Менеджер игры</param>
    /// <param name="pacman">Главный герой</param>
    public InputHandler(GameManager gameManager, Pacman pacman)
    {
        _gameManager = gameManager;
        _pacman = pacman;
    }
    
    /// <summary>
    /// Обрабатывает нажатие клавиши
    /// </summary>
    /// <param name="e">Аргументы события нажатия клавиши</param>
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
    
    /// <summary>
    /// Обрабатывает отпускание клавиши
    /// </summary>
    /// <param name="e">Аргументы события отпускания клавиши</param>
    public void HandleKeyUp(KeyEventArgs e)
    {
        // No specific actions on key up in this game
    }
}

/// <summary>
/// Класс для управления игровым процессом
/// </summary>
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
    
    /// <summary>
    /// Состояние меню
    /// </summary>
    public bool IsInMenu { get; private set; } = true;

    /// <summary>
    /// Состояние окончания игры
    /// </summary>
    public bool IsGameOver => _isGameOver;
    
    private Pacman _pacman;
    private List<Ghost> _ghosts;
    private List<Coin> _coins;
    
    /// <summary>
    /// Конструктор менеджера игры
    /// </summary>
    /// <param name="parent">Родительская форма</param>
    /// <param name="soundManager">Менеджер звуков</param>
    /// <param name="uiManager">Менеджер интерфейса</param>
    /// <param name="collisionHandler">Обработчик столкновений</param>
    /// <param name="gameTimer">Игровой таймер</param>
    /// <param name="fearModeTimer">Таймер режима страха</param>
    /// <param name="flickerTimer">Таймер мерцания</param>
    /// <param name="deathTimer">Таймер смерти</param>
    /// <param name="deathAnimation">Анимация смерти</param>
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
        _entityFactory = new GameEntityFactory(_parent, _soundManager);
        
        InitializeGameEntities();
        SetupTimers();
    }
    
    /// <summary>
    /// Инициализирует игровые объекты
    /// </summary>
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
    
    /// <summary>
    /// Настраивает таймеры
    /// </summary>
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
    
    /// <summary>
    /// Запускает игру
    /// </summary>
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
    
    /// <summary>
    /// Сбрасывает состояние игры
    /// </summary>
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
    
    /// <summary>
    /// Обновляет состояние игры
    /// </summary>
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
    
    /// <summary>
    /// Активирует режим страха
    /// </summary>
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
    
    /// <summary>
    /// Деактивирует режим страха
    /// </summary>
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
    
    /// <summary>
    /// Обрабатывает съедание призрака
    /// </summary>
    /// <param name="ghost">Призрак, которого съели</param>
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
    
    /// <summary>
    /// Обрабатывает окончание игры
    /// </summary>
    /// <param name="message">Сообщение об окончании игры</param>
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
    
    /// <summary>
    /// Переходит на следующий уровень
    /// </summary>
    public void NextLevel()
    {
        // Implementation for multiple levels
    }
    
    /// <summary>
    /// Выбирает опцию в меню
    /// </summary>
    /// <param name="direction">Направление выбора</param>
    public void SelectMenuOption(int direction)
    {
        _selectedOption = (_selectedOption + direction + 2) % 2;
        _uiManager.UpdateArrowPosition(_selectedOption);
    }
    
    /// <summary>
    /// Выполняет выбранную опцию меню
    /// </summary>
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

    /// <summary>
    /// Возвращает объект Пакмана
    /// </summary>
    /// <returns>Объект Пакмана</returns>
    public Pacman GetPacman()
    {
        return _pacman;
    }
}

/// <summary>
/// Главная форма приложения
/// </summary>
public partial class Form1 : Form
{
    private readonly GameManager _gameManager;
    private readonly ISoundManager _soundManager;
    private readonly IUIManager _uiManager;
    private IInputHandler _inputHandler;
    
    /// <summary>
    /// Конструктор главной формы
    /// </summary>
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
        
        // Delay the InputHandler initialization by using a small delay
        // This ensures GameManager has fully initialized its entities
        Timer initInputHandlerTimer = new Timer();
        initInputHandlerTimer.Interval = 100;
        initInputHandlerTimer.Tick += (s, e) => {
            _inputHandler = new InputHandler(_gameManager, _gameManager.GetPacman());
            initInputHandlerTimer.Stop();
        };
        initInputHandlerTimer.Start();
        
        _uiManager.UpdateArrowPosition(0);
        
        KeyDown += Form1_KeyDown;
        KeyUp += Form1_KeyUp;
    }
    
    /// <summary>
    /// Обработчик нажатия клавиши
    /// </summary>
    /// <param name="sender">Источник события</param>
    /// <param name="e">Аргументы события</param>
    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
        _inputHandler?.HandleKeyDown(e);
    }
    
    /// <summary>
    /// Обработчик отпускания клавиши
    /// </summary>
    /// <param name="sender">Источник события</param>
    /// <param name="e">Аргументы события</param>
    private void Form1_KeyUp(object sender, KeyEventArgs e)
    {
        _inputHandler?.HandleKeyUp(e);
    }
}