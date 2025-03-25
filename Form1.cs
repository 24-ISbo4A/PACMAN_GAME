using System.Drawing;

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

            // Проверка столкновения Пакмана с призраками
            if (pacman.Bounds.IntersectsWith(redGhost.Bounds) ||
                pacman.Bounds.IntersectsWith(yellowGhost.Bounds) ||
                pacman.Bounds.IntersectsWith(pinkGhost.Bounds))
            {
                gameOver("You Died!");
            }

            if (score == 127)
            {
                gameOver("You Win!");
            }
        }

        private void MoveGhost(PictureBox ghost, ref int direction, int speed)
        {
            // Случайно меняем направление каждые 100 тиков таймера
            if (random.Next(100) == 0)
            {
                direction = random.Next(4); // 0-3 для четырех направлений
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

            redGhostSpeed = 10;
            yellowGhostSpeed = 10;
            pinkGhostX = 10;
            playerSpeed = 12;

            isGameOver = false;

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

            gameTimer.Start();
        }

        private void gameOver(string message)
        {
            isGameOver = true;
            gameTimer.Stop();
            MessageBox.Show(message);
            resetGame();
        }

        private void pictureBox166_Click(object sender, EventArgs e)
        {

        }
    }
}

