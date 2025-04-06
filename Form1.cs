using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using System;

namespace PACMAN_GAME
{
    public partial class Form1 : Form
    {
        bool goUp, goDown, goLeft, goRight, isGameOver;
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

        private void label1_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private void pictureBox5_Click(object sender, EventArgs e) { }

        private void pictureBox8_Click(object sender, EventArgs e) { }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            // Проверяем нажатие клавиши R для рестарта
            if (e.KeyCode == Keys.R)
            {
                gameTimer.Stop();
                resetGame();
                return;
            }

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
                if (x is PictureBox)
                {
                    x.Visible = true;
                }
            }

            // Make sure timers are stopped and reset
            fearModeTimer.Stop();
            flickerTimer.Stop();

            gameTimer.Start();
        }

        private void gameOver(string message)
        {
            isGameOver = true;
            gameTimer.Stop();
            MessageBox.Show(message);
            resetGame();
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

