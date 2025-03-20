namespace PACMAN_GAME
{
    public partial class Form1 : Form
    {
        bool group, godown, goleft, goright, isGameOver;
        int score, playerSpeed, redGhostSpeed, yellowGhostSpeed, pinkGhostX, pinkGhostY;
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

            if (goleft) newLeft -= playerSpeed;
            if (goright) newLeft += playerSpeed;
            if (godown) newTop += playerSpeed;
            if (group) newTop -= playerSpeed;

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
            if (pacman.Left < -10) pacman.Left = 600;
            if (pacman.Left > 600) pacman.Left = -10;
            if (pacman.Top < -10) pacman.Top = 550;
            if (pacman.Top > 550) pacman.Top = 0;

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
            redGhost.Left += redGhostSpeed;

            if (score == 46)
            {
                // Победа
                gameOver("You Win!");
            }
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