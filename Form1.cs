using System.IO;
using System.Windows.Media;
using PACMAN_GAME.Properties;
using Color = System.Drawing.Color;
using Timer = System.Windows.Forms.Timer;
using MessageBox = System.Windows.Forms.MessageBox;
using Size = System.Drawing.Size;

namespace PACMAN_GAME;

public partial class Form1 : Form
{
    // Размер одной клетки сетки
    private const int GridSize = 40;
    private readonly Random _random = new();

    // Звуковые эффекты
    private readonly Dictionary<string, MediaPlayer> _sounds = new();
    private const int FearModeDuration = 10000; // Duration in milliseconds (10 seconds)

    // Add a new variable to track fear mode
    private bool _isFearMode;
    private bool _isFearSoundPlaying;
    private bool _isGameOver;
    private bool _isGhostSoundPlaying;
    private bool _isInMenu = true;
    private bool _isMoveSoundPlaying;
    private bool _isStartSoundPlaying;

    // Следующее запрошенное направление движения
    private int _nextDirection = 1;

    // Текущее направление движения Пакмана
    private int _pacmanDirection = 1; // 0: вверх, 1: вправо, 2: вниз, 3: влево
    private int _pinkGhostDirection;
    private int _pinkGhostSpeed;
    private int _playerSpeed;

    // Направления движения призраков (0: вверх, 1: вправо, 2: вниз, 3: влево)
    private int _redGhostDirection;
    private int _redGhostSpeed;

    private int _score;
    private int _yellowGhostDirection;
    private int _yellowGhostSpeed;

    // For death and main menus, animations
    private readonly PictureBox _deathAnimation;
    private readonly Timer _deathTimer;
    private DateTime _fearModeStartTime;
    private readonly Timer _fearModeTimer = new();
    private readonly Timer _flickerTimer = new();
    private readonly Label _gameOverLabel;
    private bool _isPinkGhostEaten;

    // Track each ghost's state separately
    private bool _isRedGhostEaten;
    private bool _isYellowGhostEaten;
    private readonly PictureBox _menuArrow;
    private readonly PictureBox _menuBackground;
    private readonly Label _restartLabel;
    private int _selectedOption; // 0 - Start, 1 - Exit

    public Form1()
    {
        InitializeComponent();
        // Устанавливаем фиксированный размер окна
        FormBorderStyle = FormBorderStyle.FixedSingle;
        // Запрещаем изменение размера
        MaximizeBox = false;
        // Устанавливаем минимальный и максимальный размер окна
        MinimumSize = new Size(1445, 1050);
        MaximumSize = new Size(1445, 1050);
        // Устанавливаем заголовок окна
        Text = "Pac-Man Game";

        // Инициализация звуков
        InitializeSounds();

        // Инициализация фона меню
        _menuBackground = new PictureBox();
        _menuBackground.Size = new Size(1445, 1050); // Размер на весь экран
        _menuBackground.Location = new Point(0, 0); // Располагаем в левом верхнем углу
        _menuBackground.SizeMode = PictureBoxSizeMode.StretchImage;
        _menuBackground.BackColor = Color.Black; // Устанавливаем черный фон по умолчанию

        _menuBackground.Image = Resources.main_menu;

        _menuBackground.Visible = true;
        Controls.Add(_menuBackground);

        // Добавляем надписи для опций меню
        var startLabel = new Label();
        startLabel.Text = "PLAY";
        startLabel.Font = new Font("Arial", 24, FontStyle.Bold);
        startLabel.ForeColor = Color.White;
        startLabel.AutoSize = true;
        startLabel.Location = new Point(
            (_menuBackground.Width - startLabel.PreferredWidth) / 2, // По центру
            _menuBackground.Bottom - 200 // Снизу
        );
        startLabel.BackColor = Color.Transparent;
        startLabel.Parent = _menuBackground;
        Controls.Add(startLabel);

        var exitLabel = new Label();
        exitLabel.Text = "QUIT";
        exitLabel.Font = new Font("Arial", 24, FontStyle.Bold);
        exitLabel.ForeColor = Color.White;
        exitLabel.AutoSize = true;
        exitLabel.Location = new Point(
            (_menuBackground.Width - exitLabel.PreferredWidth) / 2, // По центру
            _menuBackground.Bottom - 100 // Снизу, под PLAY
        );
        exitLabel.BackColor = Color.Transparent;
        exitLabel.Parent = _menuBackground;
        Controls.Add(exitLabel);

        // Скрываем все игровые элементы, кроме счёта
        foreach (Control x in Controls)
            if (x is PictureBox && x != _menuBackground && x != _menuArrow)
                x.Visible = false;

        txtScore.Visible = false; // Скрываем счёт в меню

        // Инициализация стрелки меню
        _menuArrow = new PictureBox();
        _menuArrow.Size = new Size(60, 60);
        _menuArrow.SizeMode = PictureBoxSizeMode.StretchImage;
        _menuArrow.BackColor = ColorTranslator.FromHtml("#0B0102"); // Устанавливаем красный цвет по умолчанию

        _menuArrow.Image = Resources.menu_arrow;

        _menuArrow.Visible = true;
        Controls.Add(_menuArrow);
        _menuArrow.BringToFront();

        // Устанавливаем начальную позицию стрелки
        UpdateArrowPosition();

        // Инициализация анимации смерти
        _deathAnimation = new PictureBox();
        _deathAnimation.Size = new Size(40, 40);
        _deathAnimation.SizeMode = PictureBoxSizeMode.StretchImage;
        _deathAnimation.Visible = false;
        Controls.Add(_deathAnimation);

        // Инициализация таймера смерти
        _deathTimer = new Timer();
        _deathTimer.Interval = 1200; // 1 секунда
        _deathTimer.Tick += DeathTimer_Tick;

        // Инициализация метки Game Over
        _gameOverLabel = new Label();
        _gameOverLabel.Text = "GAME OVER";
        _gameOverLabel.Font = new Font("Arial", 48, FontStyle.Bold);
        _gameOverLabel.ForeColor = Color.Red;
        _gameOverLabel.AutoSize = true;
        _gameOverLabel.Visible = false;
        Controls.Add(_gameOverLabel);

        // Инициализация метки Restart
        _restartLabel = new Label();
        _restartLabel.Text = "Restart - R";
        _restartLabel.Font = new Font("Arial", 24, FontStyle.Bold);
        _restartLabel.ForeColor = Color.Red;
        _restartLabel.AutoSize = true;
        _restartLabel.Visible = false;
        Controls.Add(_restartLabel);

        // Timers setup
        _fearModeTimer.Interval = 100; // Check every 100ms
        _fearModeTimer.Tick += (s, e) =>
        {
            if (_isFearMode)
            {
                var elapsed = DateTime.Now - _fearModeStartTime;
                if (elapsed.TotalMilliseconds >= FearModeDuration) EndFearMode();
            }
        };

        // Setup flicker timer
        _flickerTimer.Interval = 200; // Flicker every 200ms
        _flickerTimer.Tick += (_, _) =>
        {
            if (_isFearMode)
            {
                var elapsed = DateTime.Now - _fearModeStartTime;
                if (elapsed.TotalMilliseconds >= FearModeDuration - 3000) // Start flickering when 3 seconds remain
                {
                    redGhost.Visible = !redGhost.Visible;
                    yellowGhost.Visible = !yellowGhost.Visible;
                    pinkGhost.Visible = !pinkGhost.Visible;
                }
            }
        };
        _flickerTimer.Start();

        ResetGame();
    }

    private void UpdateArrowPosition()
    {
        // Позиции для стрелки
        var baseX = _menuBackground.Width / 2 + 250; // Стрелка справа
        var baseY = _menuBackground.Bottom - 290; // Поднимаем стрелку выше
        var spacing = 100; // Расстояние между опциями

        _menuArrow.Location = new Point(baseX, baseY + _selectedOption * spacing);
        _menuArrow.BringToFront();
    }

    private void InitializeSounds()
    {
        try
        {
            var resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Sounds");
            if (!Directory.Exists(resourcePath))
            {
                MessageBox.Show($"Папка со звуками не найдена: {resourcePath}");
                return;
            }

            // Загружаем звуки
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


    private void Keyisdown(object sender, KeyEventArgs e)
    {
        // Проверяем нажатие ESC в любой момент
        if (e.KeyCode == Keys.Escape)
        {
            Application.Exit();
            return;
        }

        if (_isInMenu)
        {
            if (e.KeyCode == Keys.Up)
            {
                _selectedOption = (_selectedOption - 1 + 2) % 2;
                UpdateArrowPosition();
            }
            else if (e.KeyCode == Keys.Down)
            {
                _selectedOption = (_selectedOption + 1) % 2;
                UpdateArrowPosition();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (_selectedOption == 0)
                {
                    // Start Game
                    _isInMenu = false;
                    _menuBackground.Visible = false;
                    _menuArrow.Visible = false;
                    // Скрываем все элементы меню
                    foreach (Control x in Controls)
                        if (x is Label && x.Parent == _menuBackground)
                            x.Visible = false;

                    // Скрываем кнопки PLAY и QUIT
                    foreach (Control x in Controls)
                        if (x is Label && (x.Text == "PLAY" || x.Text == "QUIT"))
                            x.Visible = false;

                    txtScore.Visible = true; // Показываем счёт при старте игры
                    ResetGame();
                }
                else if (_selectedOption == 1)
                {
                    // Exit
                    Application.Exit();
                }
            }

            return;
        }

        // Проверяем нажатие клавиши R для рестарта
        if (e.KeyCode == Keys.R)
        {
            _gameOverLabel.Visible = false;
            _restartLabel.Visible = false;
            ResetGame();
            return;
        }

        if (_isGameOver) return;

        // Устанавливаем следующее направление движения
        if (e.KeyCode == Keys.Up)
        {
            _nextDirection = 0;
            pacman.Image = Resources.up;
        }
        else if (e.KeyCode == Keys.Down)
        {
            _nextDirection = 2;
            pacman.Image = Resources.down;
        }
        else if (e.KeyCode == Keys.Left)
        {
            _nextDirection = 3;
            pacman.Image = Resources.left;
        }
        else if (e.KeyCode == Keys.Right)
        {
            _nextDirection = 1;
            pacman.Image = Resources.right;
        }
    }

    private void Keyisup(object sender, KeyEventArgs e)
    {
        // Этот метод больше не нужен, но оставляем его пустым для обратной совместимости
    }

    private void MainGameTimer(object sender, EventArgs e)
    {
        txtScore.Text = "Score: " + _score;

        // Check for large coin collection
        foreach (Control x in Controls)
            if (x is PictureBox && (string)x.Tag == "largeCoin" && x.Visible)
                if (pacman.Bounds.IntersectsWith(x.Bounds))
                {
                    _score += 20; // Large coin score
                    x.Visible = false;
                    ActivateFearMode();
                }

        // Проверяем возможность поворота в запрошенном направлении
        if (_nextDirection != _pacmanDirection)
        {
            var checkLeft = pacman.Left;
            var checkTop = pacman.Top;

            // Вычисляем новую позицию для проверки
            switch (_nextDirection)
            {
                case 0: // вверх
                    checkTop -= GridSize;
                    break;
                case 1: // вправо
                    checkLeft += GridSize;
                    break;
                case 2: // вниз
                    checkTop += GridSize;
                    break;
                case 3: // влево
                    checkLeft -= GridSize;
                    break;
            }

            // Проверяем, не столкнется ли Пакман со стеной при повороте
            var canTurn = true;
            foreach (Control x in Controls)
                if (x is PictureBox && (string)x.Tag == "wall")
                {
                    var newPacmanBounds = new Rectangle(checkLeft, checkTop, pacman.Width, pacman.Height);
                    if (newPacmanBounds.IntersectsWith(x.Bounds))
                    {
                        canTurn = false;
                        break;
                    }
                }

            // Если поворот возможен, меняем направление
            if (canTurn) _pacmanDirection = _nextDirection;
        }

        // Движение Пакмана в текущем направлении
        var newLeft = pacman.Left;
        var newTop = pacman.Top;

        switch (_pacmanDirection)
        {
            case 0: // вверх
                newTop -= _playerSpeed;
                break;
            case 1: // вправо
                newLeft += _playerSpeed;
                break;
            case 2: // вниз
                newTop += _playerSpeed;
                break;
            case 3: // влево
                newLeft -= _playerSpeed;
                break;
        }

        // Проверяем столкновение со стенами
        var canMove = true;
        foreach (Control x in Controls)
            if (x is PictureBox && (string)x.Tag == "wall")
            {
                var newPacmanBounds = new Rectangle(newLeft, newTop, pacman.Width, pacman.Height);
                if (newPacmanBounds.IntersectsWith(x.Bounds))
                {
                    canMove = false;
                    break;
                }
            }

        // Если движение возможно, обновляем позицию Пакмана
        if (canMove)
        {
            pacman.Left = newLeft;
            pacman.Top = newTop;
            StartMoveSound();
        }
        else
        {
            StopMoveSound();
        }

        // Проверка выхода за границы экрана
        if (pacman.Left < -10) pacman.Left = ClientSize.Width - 10; // правая граница
        if (pacman.Left > ClientSize.Width - 10) pacman.Left = -10; // левая граница
        if (pacman.Top < -10) pacman.Top = ClientSize.Height - 10; // верхняя граница
        if (pacman.Top > ClientSize.Height - 10) pacman.Top = -10; // нижняя границы

        // Проверка сбора монет
        foreach (Control x in Controls)
            if (x is PictureBox && (string)x.Tag == "coin" && x.Visible)
                if (pacman.Bounds.IntersectsWith(x.Bounds))
                {
                    _score += 1;
                    x.Visible = false;
                }

        // Движение призраков
        bool anyGhostMoved = MoveGhost(redGhost, ref _redGhostDirection, _redGhostSpeed);
        if (MoveGhost(yellowGhost, ref _yellowGhostDirection, _yellowGhostSpeed)) anyGhostMoved = true;
        if (MoveGhost(pinkGhost, ref _pinkGhostDirection, _pinkGhostSpeed)) anyGhostMoved = true;

        if (anyGhostMoved)
            StartGhostSound();
        else
            StopGhostSound();

        // Обеспечиваем, что призраки отображаются поверх монет
        redGhost.BringToFront();
        yellowGhost.BringToFront();
        pinkGhost.BringToFront();

        // Проверка столкновения Пакмана с призраками
        CheckGhostCollision(redGhost);
        CheckGhostCollision(yellowGhost);
        CheckGhostCollision(pinkGhost);

        if (_score >= 258) GameOver("You Win!");
    }

    private bool MoveGhost(PictureBox ghost, ref int direction, int speed)
    {
        if (_isFearMode)
        {
            // В режиме страха призраки движутся медленнее
            speed = 4; // Медленная скорость во время страха

            // Randomly change direction more frequently during fear mode
            if (_random.Next(50) == 0) direction = _random.Next(4);
        }
        else if (_score >= 258) // На втором уровне
        {
            // На втором уровне призраки движутся со скоростью 80% от базовой
            speed = 9; // 80% от базовой скорости (8)
        }
        else
        {
            // На первом уровне призраки движутся со скоростью 75% от базовой
            speed = 8; // 75% от базовой скорости (8)
        }

        var newLeft = ghost.Left;
        var newTop = ghost.Top;

        switch (direction)
        {
            case 0: // вверх
                newTop -= speed;
                break;
            case 1: // вправо
                newLeft += speed;
                break;
            case 2: // вниз
                newTop += speed;
                break;
            case 3: // влево
                newLeft -= speed;
                break;
        }

        // Проверяем столкновение со стенами
        var canMove = true;
        foreach (Control x in Controls)
            if (x is PictureBox && (string)x.Tag == "wall")
            {
                var newGhostBounds = new Rectangle(newLeft, newTop, ghost.Width, ghost.Height);
                if (newGhostBounds.IntersectsWith(x.Bounds))
                {
                    canMove = false;
                    direction = _random.Next(4); // Меняем направление при столкновении
                    break;
                }
            }

        // Если движение возможно, обновляем позицию призрака
        if (canMove)
        {
            ghost.Left = newLeft;
            ghost.Top = newTop;
            return true;
        }

        return false;
    }

    private void PlaySound(string soundName)
    {
        // Не воспроизводим другие звуки, если играет звук начала игры
        if (_isStartSoundPlaying && soundName != "game_start") return;

        if (_sounds.ContainsKey(soundName))
            try
            {
                var player = _sounds[soundName];
                player.Position = TimeSpan.Zero;
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при воспроизведении звука {soundName}: {ex.Message}");
            }
    }

    private void ResetGame()
    {
        StopAllSounds();
        txtScore.Text = "Score: 0";
        _score = 0;

        // Изменяем базовые скорости
        _redGhostSpeed = 8;
        _yellowGhostSpeed = 8;
        _pinkGhostSpeed = 8;
        _playerSpeed = 12;

        _isGameOver = false;
        _isFearMode = false;
        _isRedGhostEaten = false;
        _isYellowGhostEaten = false;
        _isPinkGhostEaten = false;
        _isMoveSoundPlaying = false;
        _isGhostSoundPlaying = false;
        _isFearSoundPlaying = false;

        // Reset ghost images to normal state
        redGhost.Image = Resources.red_left;
        yellowGhost.Image = Resources.yellow_right;
        pinkGhost.Image = Resources.pink_left;

        // Показываем все элементы игры
        foreach (Control x in Controls)
            if (x is PictureBox && x != _deathAnimation && x != _menuBackground && x != _menuArrow && !_isInMenu)
                x.Visible = true;

        txtScore.Visible = true;

        // Показываем Пакмана
        if (!_isInMenu) pacman.Visible = true;

        // Фиксированная позиция спавна Пакмана
        pacman.Left = 35;
        pacman.Top = 47;

        // Фиксированные позиции спавна призраков
        redGhost.Left = 710;
        redGhost.Top = 420;

        yellowGhost.Left = 710;
        yellowGhost.Top = 420;

        pinkGhost.Left = 710;
        pinkGhost.Top = 420;

        // Случайные начальные направления для призраков
        _pinkGhostDirection = _random.Next(4);
        _pinkGhostDirection = _random.Next(4);
        _pinkGhostDirection = _random.Next(4);

        foreach (Control x in Controls)
            if (x is PictureBox && ((string)x.Tag == "coin" || (string)x.Tag == "big-coin") && !_isInMenu)
                x.Visible = true;

        // Make sure timers are stopped and reset
        _fearModeTimer.Stop();
        _flickerTimer.Stop();
        gameTimer.Stop(); // Останавливаем таймер игры


        // Скрываем все элементы смерти
        _deathAnimation.Visible = false;
        _gameOverLabel.Visible = false;
        _restartLabel.Visible = false;

        // Устанавливаем начальное направление Пакмана
        _pacmanDirection = 1;
        _nextDirection = 1;
        pacman.Image = Resources.right;


        // Воспроизведение звука начала игры и ожидание его окончания
        if (_sounds.ContainsKey("game_start") && !_isInMenu)
        {
            _isStartSoundPlaying = true;
            var player = _sounds["game_start"];
            player.Position = TimeSpan.Zero;
            player.MediaEnded += (_, _) =>
            {
                player.MediaEnded -= (_, _) => { }; // Удаляем обработчик
                _isStartSoundPlaying = false; // Отмечаем, что звук начала игры закончился
                gameTimer.Start(); // Запускаем игру только после окончания звука
            };
            player.Play();
        }
    }

    private void DeathTimer_Tick(object sender, EventArgs e)
    {
        _deathTimer.Stop();
        _deathAnimation.Visible = false;
        ShowGameOverScreen();
    }

    private void ShowGameOverScreen()
    {
        // Скрываем все элементы игры
        foreach (Control x in Controls)
            if (x is PictureBox && x != _deathAnimation && x != _menuBackground && x != _menuArrow)
                x.Visible = false;

        txtScore.Visible = false;

        // Центрируем надпись Game Over
        _gameOverLabel.Location = new Point(
            (ClientSize.Width - _gameOverLabel.Width) / 2,
            (ClientSize.Height - _gameOverLabel.Height) / 2 - 100
        );
        _gameOverLabel.Visible = true;
        _gameOverLabel.BringToFront();

        // Размещаем надпись Restart под Game Over
        _restartLabel.Location = new Point(
            (ClientSize.Width - _restartLabel.Width) / 2,
            _gameOverLabel.Bottom + 50
        );
        _restartLabel.Visible = true;
        _restartLabel.BringToFront();
    }


    private void StopAllSounds()
    {
        StopMoveSound();
        StopGhostSound();
        if (_sounds.ContainsKey("fear_mode") && _isFearSoundPlaying)
        {
            _isFearSoundPlaying = false;
            _sounds["fear_mode"].Stop();
        }
    }

    private void GameOver(string message)
    {
        gameTimer.Stop();

        // Останавливаем все звуки
        StopAllSounds();

        if (message.Equals("You Win!"))
        {
            MessageBox.Show("You Win! Press R to restart or ESC to quit.");
            return;
        }

        // Воспроизведение только звука смерти Пакмана
        PlaySound("pacman_death");

        _isGameOver = true;

        // Скрываем все элементы игры
        foreach (Control x in Controls)
            if (x is PictureBox && x != _deathAnimation && x != _menuBackground && x != _menuArrow)
                x.Visible = false;

        txtScore.Visible = false;

        // Показываем анимацию смерти
        _deathAnimation.Image = Resources.pacman_death_anim;

        _deathAnimation.Location = new Point(pacman.Left, pacman.Top);
        _deathAnimation.Size = new Size(45, 60);
        _deathAnimation.SizeMode = PictureBoxSizeMode.StretchImage;
        _deathAnimation.BackColor = Color.Transparent;
        _deathAnimation.BringToFront();
        _deathAnimation.Visible = true;

        // Запускаем таймер для показа Game Over через 1.2 секунды
        _deathTimer.Start();
    }

    private void pictureBox166_Click(object sender, EventArgs e)
    {
    }

    private void ActivateFearMode()
    {
        _isFearMode = true;
        _fearModeStartTime = DateTime.Now;
        _fearModeTimer.Start();

        // Change ghost images to fear mode
        redGhost.Image = Resources.scared_ghost_anim;
        yellowGhost.Image = Resources.scared_ghost_anim;
        pinkGhost.Image = Resources.scared_ghost_anim;

        // Останавливаем звук движения призраков
        StopGhostSound();

        // Запускаем звук режима страха
        if (_sounds.ContainsKey("fear_mode"))
        {
            _isFearSoundPlaying = true;
            var player = _sounds["fear_mode"];
            player.Position = TimeSpan.Zero;
            player.Play();
            player.MediaEnded += (_, _) =>
            {
                if (_isFearSoundPlaying && _isFearMode)
                {
                    player.Position = TimeSpan.Zero;
                    player.Play();
                }
            };
        }
    }

    private void EndFearMode()
    {
        _isFearMode = false;
        _fearModeTimer.Stop();
        _flickerTimer.Stop();

        // Останавливаем звук режима страха
        if (_sounds.ContainsKey("fear_mode"))
        {
            _isFearSoundPlaying = false;
            _sounds["fear_mode"].Stop();
        }

        // Reset ghost images to normal
        redGhost.Image = Resources.red_left;
        yellowGhost.Image = Resources.yellow_right;
        pinkGhost.Image = Resources.pink_left;
        // Make sure all ghosts are visible
        redGhost.Visible = true;
        yellowGhost.Visible = true;
        pinkGhost.Visible = true;
        // Reset ghost states
        _isRedGhostEaten = false;
        _isYellowGhostEaten = false;
        _isPinkGhostEaten = false;
    }

    private void CheckGhostCollision(PictureBox ghost)
    {
        if (pacman.Bounds.IntersectsWith(ghost.Bounds))
        {
            var isGhostEaten = false;
            if (ghost == redGhost) isGhostEaten = _isRedGhostEaten;
            else if (ghost == yellowGhost) isGhostEaten = _isYellowGhostEaten;
            else if (ghost == pinkGhost) isGhostEaten = _isPinkGhostEaten;

            if (_isFearMode && !isGhostEaten)
            {
                // Only eat ghost if it's in fear mode and hasn't been eaten yet
                if (ghost == redGhost)
                {
                    EatGhost(ghost);
                    _isRedGhostEaten = true;
                }
                else if (ghost == yellowGhost)
                {
                    EatGhost(ghost);
                    _isYellowGhostEaten = true;
                }
                else if (ghost == pinkGhost)
                {
                    EatGhost(ghost);
                    _isPinkGhostEaten = true;
                }
            }
            else if (isGhostEaten || !_isFearMode)
            {
                // Ghost kills player if it's either respawned or not in fear mode
                GameOver("You Died!");
            }
        }
    }

    private void EatGhost(PictureBox ghost)
    {
        _score += 50; // Points for eating a ghost
        ghost.Visible = false;
        // Воспроизведение звука поедания призрака
        PlaySound("ghost_eaten");
        RespawnGhost(ghost);
    }

    private void RespawnGhost(PictureBox ghost)
    {
        ghost.Visible = true;
        ghost.Left = 710;
        ghost.Top = 420;

        // Reset ghost image and speed based on current level
        if (ghost == redGhost)
        {
            ghost.Image = Resources.red_left;
            _redGhostSpeed = _score >= 300 ? 6 : 6; // 80% или 75% от базовой скорости
        }
        else if (ghost == yellowGhost)
        {
            ghost.Image = Resources.yellow_right;
            _yellowGhostSpeed = _score >= 300 ? 6 : 6; // 80% или 75% от базовой скорости
        }
        else if (ghost == pinkGhost)
        {
            ghost.Image = Resources.pink_left;
            _pinkGhostSpeed = _score >= 300 ? 6 : 6; // 80% или 75% от базовой скорости
        }
    }

    private void StartMoveSound()
    {
        // Не воспроизводим звук движения, если играет звук начала игры
        if (_isStartSoundPlaying) return;

        if (_sounds.ContainsKey("pacman_move") && _isMoveSoundPlaying == false && _isGameOver == false)
        {
            _isMoveSoundPlaying = true;
            var player = _sounds["pacman_move"];
            player.Position = TimeSpan.Zero;
            player.Play();
            player.MediaEnded += (_, _) =>
            {
                if (_isMoveSoundPlaying && _isGameOver == false)
                {
                    player.Position = TimeSpan.Zero;
                    player.Play();
                }
            };
        }
    }

    private void StopMoveSound()
    {
        if (_sounds.ContainsKey("pacman_move") && _isMoveSoundPlaying)
        {
            _isMoveSoundPlaying = false;
            _sounds["pacman_move"].Stop();
        }
    }

    private void StartGhostSound()
    {
        // Не воспроизводим звук призраков, если играет звук начала игры
        if (_isStartSoundPlaying) return;

        if (_sounds.ContainsKey("ghost_move") && _isMoveSoundPlaying == false && _isGameOver == false)
        {
            _isGhostSoundPlaying = true;
            var player = _sounds["ghost_move"];
            player.Position = TimeSpan.Zero;
            player.Play();
            player.MediaEnded += (_, _) =>
            {
                if (_isMoveSoundPlaying && _isGameOver == false)
                {
                    player.Position = TimeSpan.Zero;
                    player.Play();
                }
            };
        }
    }

    private void StopGhostSound()
    {
        if (_sounds.ContainsKey("ghost_move") && _isGhostSoundPlaying)
        {
            _isGhostSoundPlaying = false;
            _sounds["ghost_move"].Stop();
        }
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void Form1_Load(object sender, EventArgs e)
    {
    }

    private void pictureBox5_Click(object sender, EventArgs e)
    {
    }

    private void pictureBox8_Click(object sender, EventArgs e)
    {
    }
}