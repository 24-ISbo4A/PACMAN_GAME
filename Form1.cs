using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using System;

namespace PACMAN_GAME
{
    public partial class Form1 : Form
    {
        private bool goUp, goDown, goLeft, goRight, isGameOver, isDeathAnimationPlaying, isInMenu = true;
        int score, playerSpeed, redGhostSpeed, yellowGhostSpeed, pinkGhostX;
        Random random = new Random();

        // Направления движения призраков (0: вверх, 1: вправо, 2: вниз, 3: влево)
        int redGhostDirection = 0;
        int yellowGhostDirection = 0;
        int pinkGhostDirection = 0;

        // Размер одной клетки сетки
        const int GRID_SIZE = 40;
        // Текущее направление движения Пакмана
        int pacmanDirection = 1; // 0: вверх, 1: вправо, 2: вниз, 3: влево
        // Следующее запрошенное направление движения
        int nextDirection = 1;

        // Add a new variable to track fear mode
        bool isFearMode = false;
        int fearModeDuration = 10000; // Duration in milliseconds (10 seconds)
        Timer fearModeTimer = new Timer();
        Timer flickerTimer = new Timer();
        bool isFlickering = false;
        DateTime fearModeStartTime;

        // Track each ghost's state separately
        private bool isRedGhostEaten = false;
        private bool isYellowGhostEaten = false;
        private bool isPinkGhostEaten = false;
        
        // For death and main menus, animations
        private PictureBox deathAnimation;
        private System.Windows.Forms.Timer deathTimer;
        private Label gameOverLabel;
        private Label restartLabel;
        private PictureBox menuBackground;
        private PictureBox menuArrow;
        private int selectedOption = 0; // 0 - Start, 1 - Exit

        public Form1()
        {
            InitializeComponent();
            // Устанавливаем фиксированный размер окна
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            // Запрещаем изменение размера
            this.MaximizeBox = false;
            // Устанавливаем минимальный и максимальный размер окна
            this.MinimumSize = new Size(1445, 1050);
            this.MaximumSize = new Size(1445, 1050);
            // Устанавливаем заголовок окна
            this.Text = "Pac-Man Game";
            
            // Инициализация фона меню
            menuBackground = new PictureBox();
            menuBackground.Size = new Size(1445, 1050); // Размер на весь экран
            menuBackground.Location = new Point(0, 0); // Располагаем в левом верхнем углу
            menuBackground.SizeMode = PictureBoxSizeMode.StretchImage;
            menuBackground.BackColor = Color.Black; // Устанавливаем черный фон по умолчанию

            menuBackground.Image = Properties.Resources.main_menu;
            
            menuBackground.Visible = true;
            this.Controls.Add(menuBackground);

            // Добавляем надписи для опций меню
            Label startLabel = new Label();
            startLabel.Text = "PLAY";
            startLabel.Font = new Font("Arial", 24, FontStyle.Bold);
            startLabel.ForeColor = Color.White;
            startLabel.AutoSize = true;
            startLabel.Location = new Point(
                (menuBackground.Width - startLabel.PreferredWidth) / 2, // По центру
                menuBackground.Bottom - 200 // Снизу
            );
            startLabel.BackColor = Color.Transparent;
            startLabel.Parent = menuBackground;
            this.Controls.Add(startLabel);

            Label exitLabel = new Label();
            exitLabel.Text = "QUIT";
            exitLabel.Font = new Font("Arial", 24, FontStyle.Bold);
            exitLabel.ForeColor = Color.White;
            exitLabel.AutoSize = true;
            exitLabel.Location = new Point(
                (menuBackground.Width - exitLabel.PreferredWidth) / 2, // По центру
                menuBackground.Bottom - 100 // Снизу, под PLAY
            );
            exitLabel.BackColor = Color.Transparent;
            exitLabel.Parent = menuBackground;
            this.Controls.Add(exitLabel);

            // Скрываем все игровые элементы, кроме счёта
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x != menuBackground && x != menuArrow)
                {
                    x.Visible = false;
                }
            }
            txtScore.Visible = false; // Скрываем счёт в меню

            // Инициализация стрелки меню
            menuArrow = new PictureBox();
            menuArrow.Size = new Size(60, 60);
            menuArrow.SizeMode = PictureBoxSizeMode.StretchImage;
            menuArrow.BackColor = ColorTranslator.FromHtml("#0B0102") ; // Устанавливаем красный цвет по умолчанию
            
            menuArrow.Image = Properties.Resources.menu_arrow;
            
            menuArrow.Visible = true;
            this.Controls.Add(menuArrow);
            menuArrow.BringToFront();

            // Устанавливаем начальную позицию стрелки
            UpdateArrowPosition();

            // Инициализация анимации смерти
            deathAnimation = new PictureBox();
            deathAnimation.Size = new Size(40, 40);
            deathAnimation.SizeMode = PictureBoxSizeMode.StretchImage;
            deathAnimation.Visible = false;
            this.Controls.Add(deathAnimation);

            // Инициализация таймера смерти
            deathTimer = new System.Windows.Forms.Timer();
            deathTimer.Interval = 1200; // 1 секунда
            deathTimer.Tick += DeathTimer_Tick;

            // Инициализация метки Game Over
            gameOverLabel = new Label();
            gameOverLabel.Text = "GAME OVER";
            gameOverLabel.Font = new Font("Arial", 48, FontStyle.Bold);
            gameOverLabel.ForeColor = Color.Red;
            gameOverLabel.AutoSize = true;
            gameOverLabel.Visible = false;
            this.Controls.Add(gameOverLabel);

            // Инициализация метки Restart
            restartLabel = new Label();
            restartLabel.Text = "Restart - R";
            restartLabel.Font = new Font("Arial", 24, FontStyle.Bold);
            restartLabel.ForeColor = Color.Red;
            restartLabel.AutoSize = true;
            restartLabel.Visible = false;
            this.Controls.Add(restartLabel);
            
            // Timres setup
            fearModeTimer.Interval = 100; // Check every 100ms
            fearModeTimer.Tick += (s, e) =>
            {
                if (isFearMode)
                {
                    TimeSpan elapsed = DateTime.Now - fearModeStartTime;
                    if (elapsed.TotalMilliseconds >= fearModeDuration)
                    {
                        EndFearMode(s, e);
                    }
                }
            };
            
            // Setup flicker timer
            flickerTimer.Interval = 200; // Flicker every 200ms
            flickerTimer.Tick += (s, e) =>
            {
                if (isFearMode)
                {
                    TimeSpan elapsed = DateTime.Now - fearModeStartTime;
                    if (elapsed.TotalMilliseconds >= fearModeDuration - 3000) // Start flickering when 3 seconds remain
                    {
                        redGhost.Visible = !redGhost.Visible;
                        yellowGhost.Visible = !yellowGhost.Visible;
                        pinkGhost.Visible = !pinkGhost.Visible;
                    }
                }
            };
            flickerTimer.Start();
            
            resetGame();
        }
        
        private void UpdateArrowPosition()
        {
            // Позиции для стрелки
            int baseX = (menuBackground.Width / 2) + 250; // Стрелка справа
            int baseY = menuBackground.Bottom - 290; // Поднимаем стрелку выше
            int spacing = 100; // Расстояние между опциями

            menuArrow.Location = new Point(baseX, baseY + (selectedOption * spacing));
            menuArrow.BringToFront();
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private void pictureBox5_Click(object sender, EventArgs e) { }

        private void pictureBox8_Click(object sender, EventArgs e) { }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            // Проверяем нажатие ESC в любой момент
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
                return;
            }

            if (isInMenu)
            {
                if (e.KeyCode == Keys.Up)
                {
                    selectedOption = (selectedOption - 1 + 2) % 2;
                    UpdateArrowPosition();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    selectedOption = (selectedOption + 1) % 2;
                    UpdateArrowPosition();
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    if (selectedOption == 0)
                    {
                        // Start Game
                        isInMenu = false;
                        menuBackground.Visible = false;
                        menuArrow.Visible = false;
                        // Скрываем все элементы меню
                        foreach (Control x in this.Controls)
                        {
                            if (x is Label && x.Parent == menuBackground)
                            {
                                x.Visible = false;
                            }
                        }
                        // Скрываем кнопки PLAY и QUIT
                        foreach (Control x in this.Controls)
                        {
                            if (x is Label && (x.Text == "PLAY" || x.Text == "QUIT"))
                            {
                                x.Visible = false;
                            }
                        }
                        txtScore.Visible = true; // Показываем счёт при старте игры
                        resetGame();
                    }
                    else if (selectedOption == 1)
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
                gameOverLabel.Visible = false;
                restartLabel.Visible = false;
                resetGame();
                return;
            }
            
            if(isGameOver) return;

            // Устанавливаем следующее направление движения
            if (e.KeyCode == Keys.Up)
            {
                nextDirection = 0;
                pacman.Image = Properties.Resources.up;
            }
            else if (e.KeyCode == Keys.Down)
            {
                nextDirection = 2;
                pacman.Image = Properties.Resources.down;
            }
            else if (e.KeyCode == Keys.Left)
            {
                nextDirection = 3;
                pacman.Image = Properties.Resources.left;
            }
            else if (e.KeyCode == Keys.Right)
            {
                nextDirection = 1;
                pacman.Image = Properties.Resources.right;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) goUp = false;
            if (e.KeyCode == Keys.Down) goDown = false;
            if (e.KeyCode == Keys.Left) goLeft = false;
            if (e.KeyCode == Keys.Right) goRight = false;
        }

        private void MainGameTimer(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;

            // Check for large coin collection
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "largeCoin" && x.Visible)
                {
                    if (pacman.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 20; // Large coin score
                        x.Visible = false;
                        ActivateFearMode();
                    }
                }
            }

            // Проверяем возможность поворота в запрошенном направлении
            if (nextDirection != pacmanDirection)
            {
                int checkLeft = pacman.Left;
                int checkTop = pacman.Top;

                // Вычисляем новую позицию для проверки
                switch (nextDirection)
                {
                    case 0: // вверх
                        checkTop -= GRID_SIZE;
                        break;
                    case 1: // вправо
                        checkLeft += GRID_SIZE;
                        break;
                    case 2: // вниз
                        checkTop += GRID_SIZE;
                        break;
                    case 3: // влево
                        checkLeft -= GRID_SIZE;
                        break;
                }

                // Проверяем, не столкнется ли Пакман со стеной при повороте
                bool canTurn = true;
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && (string)x.Tag == "wall")
                    {
                        Rectangle newPacmanBounds = new Rectangle(checkLeft, checkTop, pacman.Width, pacman.Height);
                        if (newPacmanBounds.IntersectsWith(x.Bounds))
                        {
                            canTurn = false;
                            break;
                        }
                    }
                }

                // Если поворот возможен, меняем направление
                if (canTurn)
                {
                    pacmanDirection = nextDirection;
                }
            }

            // Движение Пакмана в текущем направлении
            int newLeft = pacman.Left;
            int newTop = pacman.Top;

            switch (pacmanDirection)
            {
                case 0: // вверх
                    newTop -= playerSpeed;
                    break;
                case 1: // вправо
                    newLeft += playerSpeed;
                    break;
                case 2: // вниз
                    newTop += playerSpeed;
                    break;
                case 3: // влево
                    newLeft -= playerSpeed;
                    break;
            }

            // Проверяем столкновение со стенами
            bool canMove = true;
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "wall")
                {
                    Rectangle newPacmanBounds = new Rectangle(newLeft, newTop, pacman.Width, pacman.Height);
                    if (newPacmanBounds.IntersectsWith(x.Bounds))
                    {
                        canMove = false;
                        break;
                    }
                }
            }

            // Если движение возможно, обновляем позицию Пакмана
            if (canMove)
            {
                pacman.Left = newLeft;
                pacman.Top = newTop;
            }

            // Проверка выхода за границы экрана
            if (pacman.Left < -10) pacman.Left = this.ClientSize.Width - 10; // правая граница
            if (pacman.Left > this.ClientSize.Width - 10) pacman.Left = -10; // левая граница
            if (pacman.Top < -10) pacman.Top = this.ClientSize.Height - 10; // верхняя граница
            if (pacman.Top > this.ClientSize.Height - 10) pacman.Top = -10; // нижняя границы
            
            // Проверка сбора монет
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "coin" && x.Visible)
                {
                    if (pacman.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 1;
                        x.Visible = false;
                    }
                }
            }

            // Движение призраков
            MoveGhost(redGhost, ref redGhostDirection, redGhostSpeed);
            MoveGhost(yellowGhost, ref yellowGhostDirection, yellowGhostSpeed);
            MoveGhost(pinkGhost, ref pinkGhostDirection, pinkGhostX);

            // Обеспечиваем, что призраки отображаются поверх монет
            redGhost.BringToFront();
            yellowGhost.BringToFront();
            pinkGhost.BringToFront();

            // Проверка столкновения Пакмана с призраками
            CheckGhostCollision(redGhost);
            CheckGhostCollision(yellowGhost);
            CheckGhostCollision(pinkGhost);

            if (score >= 258)
            {
                gameOver("You Win!");
            }
        }

        private void MoveGhost(PictureBox ghost, ref int direction, int speed)
        {
            if (isFearMode)
            {
                // В режиме страха призраки движутся медленнее
                speed = 4; // Медленная скорость во время страха
                
                // Randomly change direction more frequently during fear mode
                if (random.Next(50) == 0)
                {
                    direction = random.Next(4);
                }
            }
            else if (score >= 258) // На втором уровне
            {
                // На втором уровне призраки движутся со скоростью 80% от базовой
                speed = 9; // 80% от базовой скорости (8)
            }
            else
            {
                // На первом уровне призраки движутся со скоростью 75% от базовой
                speed = 8; // 75% от базовой скорости (8)
            }

            int newLeft = ghost.Left;
            int newTop = ghost.Top;

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
            bool canMove = true;
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "wall")
                {
                    Rectangle newGhostBounds = new Rectangle(newLeft, newTop, ghost.Width, ghost.Height);
                    if (newGhostBounds.IntersectsWith(x.Bounds))
                    {
                        canMove = false;
                        direction = random.Next(4); // Меняем направление при столкновении
                        break;
                    }
                }
            }

            // Если движение возможно, обновляем позицию призрака
            if (canMove)
            {
                ghost.Left = newLeft;
                ghost.Top = newTop;
            }

            // Проверка выхода за границы экрана
            if (ghost.Left < 0) ghost.Left = 0;
            if (ghost.Left > this.ClientSize.Width - ghost.Width) ghost.Left = this.ClientSize.Width - ghost.Width;
            if (ghost.Top < 0) ghost.Top = 0;
            if (ghost.Top > this.ClientSize.Height - ghost.Height) ghost.Top = this.ClientSize.Height - ghost.Height;
        }

        private void resetGame()
        {
            txtScore.Text = "Score: 0";
            score = 0;

            // Изменяем базовые скорости
            redGhostSpeed = 8; // 75% от базовой скорости (8)
            yellowGhostSpeed = 8;
            pinkGhostX = 8;
            playerSpeed = 12; // 150% от базовой скорости (8)

            isGameOver = false;
            isFearMode = false;
            isRedGhostEaten = false;
            isYellowGhostEaten = false;
            isPinkGhostEaten = false;

            // Reset ghost images to normal state
            redGhost.Image = Properties.Resources.red_left;
            yellowGhost.Image = Properties.Resources.yellow_right;
            pinkGhost.Image = Properties.Resources.pink_left;
            
            // Показываем все элементы игры
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x != deathAnimation && x != menuBackground && x != menuArrow && !isInMenu)
                {
                    x.Visible = true;
                }
            }
            txtScore.Visible = true;
            
            // Показываем Пакмана
            if(!isInMenu) pacman.Visible = true;
            
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
            redGhostDirection = random.Next(4);
            yellowGhostDirection = random.Next(4);
            pinkGhostDirection = random.Next(4);

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && ((string)x.Tag == "coin"  || (string)x.Tag == "big-coin" ) && !isInMenu)
                {
                    x.Visible = true;
                }
            }

            // Make sure timers are stopped and reset
            fearModeTimer.Stop();
            flickerTimer.Stop();
            
            // Скрываем все элементы смерти
            deathAnimation.Visible = false;
            gameOverLabel.Visible = false;
            restartLabel.Visible = false;

            // Устанавливаем начальное направление Пакмана
            pacmanDirection = 1;
            nextDirection = 1;
            pacman.Image = Properties.Resources.right;

            gameTimer.Start();
        }
        
        private void DeathTimer_Tick(object sender, EventArgs e)
        {
            deathTimer.Stop();
            deathAnimation.Visible = false;
            ShowGameOverScreen();
        }

        private void ShowGameOverScreen()
        {
            // Скрываем все элементы игры
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x != deathAnimation && x != menuBackground && x != menuArrow)
                {
                    x.Visible = false;
                }
            }
            txtScore.Visible = false;

            // Центрируем надпись Game Over
            gameOverLabel.Location = new Point(
                (this.ClientSize.Width - gameOverLabel.Width) / 2,
                (this.ClientSize.Height - gameOverLabel.Height) / 2 - 100
            );
            gameOverLabel.Visible = true;
            gameOverLabel.BringToFront();

            // Размещаем надпись Restart под Game Over
            restartLabel.Location = new Point(
                (this.ClientSize.Width - restartLabel.Width) / 2,
                gameOverLabel.Bottom + 50
            );
            restartLabel.Visible = true;
            restartLabel.BringToFront();
        }

        private void gameOver(string message)
        {
            gameTimer.Stop();
            isGameOver = true;

            // Скрываем все элементы игры
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x != deathAnimation && x != menuBackground && x != menuArrow)
                {
                    x.Visible = false;
                }
            }
            txtScore.Visible = false;

            // Показываем анимацию смерти
            deathAnimation.Image = Properties.Resources.pacman_death_anim;
            
            deathAnimation.Location = new Point(pacman.Left, pacman.Top);
            deathAnimation.Size = new Size(45, 60);
            deathAnimation.SizeMode = PictureBoxSizeMode.StretchImage;
            deathAnimation.BackColor = Color.Transparent;
            deathAnimation.BringToFront();
            deathAnimation.Visible = true;

            // Запускаем таймер для показа Game Over через 1.2 секунды
            deathTimer.Start();
        }

        /// <summary>
        /// checks if there is a wall in front of the packman on X and Y so that he does not pass through the walls
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsWallAt(int x, int y)
        {
            Rectangle testRect = new Rectangle(x, y, pacman.Width, pacman.Height);

            foreach (Control control in this.Controls)
            {
                if (control is PictureBox && (string)control.Tag == "wall")
                {
                    if (testRect.IntersectsWith(control.Bounds))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void pictureBox166_Click(object sender, EventArgs e)
        {

        }

        private void ActivateFearMode()
        {
            isFearMode = true;
            fearModeStartTime = DateTime.Now; // Record when fear mode started
            fearModeTimer.Start();
            // Change ghost images to fear mode (placeholder)
            redGhost.Image = Properties.Resources.scared_ghost_anim;
            yellowGhost.Image = Properties.Resources.scared_ghost_anim;
            pinkGhost.Image = Properties.Resources.scared_ghost_anim;
        }

        private void EndFearMode(object sender, EventArgs e)
        {
            isFearMode = false;
            fearModeTimer.Stop();
            flickerTimer.Stop();
            // Reset ghost images to normal (placeholder)
            redGhost.Image = Properties.Resources.red_left;
            yellowGhost.Image = Properties.Resources.yellow_right;
            pinkGhost.Image = Properties.Resources.pink_left;
            // Make sure all ghosts are visible
            redGhost.Visible = true;
            yellowGhost.Visible = true;
            pinkGhost.Visible = true;
            // Reset ghost states
            isRedGhostEaten = false;
            isYellowGhostEaten = false;
            isPinkGhostEaten = false;
        }

        private void CheckGhostCollision(PictureBox ghost)
        {
            if (pacman.Bounds.IntersectsWith(ghost.Bounds))
            {
                bool isGhostEaten = false;
                if (ghost == redGhost) isGhostEaten = isRedGhostEaten;
                else if (ghost == yellowGhost) isGhostEaten = isYellowGhostEaten;
                else if (ghost == pinkGhost) isGhostEaten = isPinkGhostEaten;

                if (isFearMode && !isGhostEaten)
                {
                    // Only eat ghost if it's in fear mode and hasn't been eaten yet
                    if (ghost == redGhost)
                    {
                        EatGhost(ghost);
                        isRedGhostEaten = true;
                    }
                    else if (ghost == yellowGhost)
                    {
                        EatGhost(ghost);
                        isYellowGhostEaten = true;
                    }
                    else if (ghost == pinkGhost)
                    {
                        EatGhost(ghost);
                        isPinkGhostEaten = true;
                    }
                }
                else if (isGhostEaten || !isFearMode)
                {
                    // Ghost kills player if it's either respawned or not in fear mode
                    gameOver("You Died!");
                }
            }
        }

        private void EatGhost(PictureBox ghost)
        {
            score += 50; // Points for eating a ghost
            ghost.Visible = false;
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
                ghost.Image = Properties.Resources.red_left;
                redGhostSpeed = score >= 300 ? 6 : 6; // 80% или 75% от базовой скорости
            }
            else if (ghost == yellowGhost)
            {
                ghost.Image = Properties.Resources.yellow_right;
                yellowGhostSpeed = score >= 300 ? 6 : 6; // 80% или 75% от базовой скорости
            }
            else if (ghost == pinkGhost)
            {
                ghost.Image = Properties.Resources.pink_left;
                pinkGhostX = score >= 300 ? 6 : 6; // 80% или 75% от базовой скорости
            }
        }
    }
}

