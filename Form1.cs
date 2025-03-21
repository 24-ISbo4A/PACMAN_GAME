namespace PACMAN_GAME
{
    public partial class Form1 : Form
    {
        bool group, godown, goleft, goright, isGameOver;
        int score, playerSpeed, redGhostSpeed, yellowGhostSpeed, pinkGhostX, pinkGhostY;

        // Направления движения призраков
        bool redGhostGoRight = true;
        bool yellowGhostGoDown = true;
        bool pinkGhostGoLeft = true;

        public Form1()
        {
            InitializeComponent();
            resetGame();
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private void pictureBox5_Click(object sender, EventArgs e) { }

        private void pictureBox8_Click(object sender, EventArgs e) { }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) group = true;
            if (e.KeyCode == Keys.Down) godown = true;
            if (e.KeyCode == Keys.Left) goleft = true;
            if (e.KeyCode == Keys.Right) goright = true;
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) group = false;
            if (e.KeyCode == Keys.Down) godown = false;
            if (e.KeyCode == Keys.Left) goleft = false;
            if (e.KeyCode == Keys.Right) goright = false;
        }

        private void MainGameTimer(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;

            // Сохраняем текущие координаты Пакмана
            int newLeft = pacman.Left;
            int newTop = pacman.Top;

            if (goleft)
            {
                newLeft -= playerSpeed;
                pacman.Image = Properties.Resources.left;
            }

            if (goright)
            {
                newLeft += playerSpeed;
                pacman.Image = Properties.Resources.right;
            }

            if (godown)
            {
                newTop += playerSpeed;
                pacman.Image = Properties.Resources.down;
            }

            if (group)
            {
                newTop -= playerSpeed;
                pacman.Image = Properties.Resources.up;
            }

            // Проверяем, не столкнется ли Пакман с какой-либо стеной после перемещения
            bool canMove = true;
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "wall")
                {
                    // Создаем временный прямоугольник для проверки столкновений
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
            if (pacman.Left < -10) pacman.Left = 750; // правая граница
            if (pacman.Left > 750) pacman.Left = -10; // левая граница
            if (pacman.Top < -10) pacman.Top = 450; // верхняя граница
            if (pacman.Top > 450) pacman.Top = 0; // нижняя границы

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

            // Движение красного призрака
            if (redGhostGoRight)
            {
                redGhost.Left += redGhostSpeed;
                if (redGhost.Right >= this.ClientSize.Width || CheckWallCollision(redGhost))
                {
                    redGhostGoRight = false;
                }
            }
            else
            {
                redGhost.Left -= redGhostSpeed;
                if (redGhost.Left <= 0 || CheckWallCollision(redGhost))
                {
                    redGhostGoRight = true;
                }
            }

            // Движение желтого призрака
            if (yellowGhostGoDown)
            {
                yellowGhost.Top += yellowGhostSpeed;
                if (yellowGhost.Bottom >= this.ClientSize.Height || CheckWallCollision(yellowGhost))
                {
                    yellowGhostGoDown = false;
                }
            }
            else
            {
                yellowGhost.Top -= yellowGhostSpeed;
                if (yellowGhost.Top <= 0 || CheckWallCollision(yellowGhost))
                {
                    yellowGhostGoDown = true;
                }
            }

            // Движение розового призрака
            if (pinkGhostGoLeft)
            {
                pinkGhost.Left -= pinkGhostX;
                if (pinkGhost.Left <= 0 || CheckWallCollision(pinkGhost))
                {
                    pinkGhostGoLeft = false;
                }
            }
            else
            {
                pinkGhost.Left += pinkGhostX;
                if (pinkGhost.Right >= this.ClientSize.Width || CheckWallCollision(pinkGhost))
                {
                    pinkGhostGoLeft = true;
                }
            }

            // Проверка столкновения Пакмана с призраками
            if (pacman.Bounds.IntersectsWith(redGhost.Bounds) ||
                pacman.Bounds.IntersectsWith(yellowGhost.Bounds) ||
                pacman.Bounds.IntersectsWith(pinkGhost.Bounds))
            {
                // Пакман умер
                gameOver("You Died!");
            }

            if (score == 21)
            {
                // Победа
                gameOver("You Win!");
            }
        }

        private bool CheckWallCollision(PictureBox ghost)
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "wall")
                {
                    if (ghost.Bounds.IntersectsWith(x.Bounds))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void resetGame()
        {
            txtScore.Text = "Score: 0";
            score = 0;

            redGhostSpeed = 5;
            yellowGhostSpeed = 5;
            pinkGhostX = 5;
            pinkGhostY = 5;
            playerSpeed = 8;

            isGameOver = false;

            pacman.Left = 28;
            pacman.Top = 47;

            redGhost.Left = 366;
            redGhost.Top = 92;

            yellowGhost.Left = 493;
            yellowGhost.Top = 309;

            pinkGhost.Left = 602;
            pinkGhost.Top = 78;

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
    }
}