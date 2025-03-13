namespace PACMAN_GAME
{
    public partial class Form1 : Form

        
        
    {

        bool group, godown, goleft, goright, isGameOver;
        int score, playerSpeed,  redGhostSpeed, yellowGhostSpeed, pinkGhostX, pinkGhostY;
        public Form1()
        {
            InitializeComponent();
            resetGame();
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

        private void keyisdown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Up)
            {
                group = true;
                Console.WriteLine("Тестовая строченька");
            }

            if (e.KeyCode == Keys.Down)
            {
                godown = true;
            }

            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }

        }

        private void keyisup(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Up)
            {
                group = false;
            }

            if (e.KeyCode == Keys.Down)
            {
                godown = false;
            }

            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }

        }

        // sun1zu: ����������� ������ � ���������� �������� �������, ��� �� ��� � ��������
        private void MainGameTimer(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;
            if (goleft == true)
            {
                pacman.Left -= playerSpeed;
                // pacman.Image = Properties.Resources.left;
            }

            if (goright == true)
            {
                pacman.Left += playerSpeed;
                // pacman.Image = Properties.Resources.right;
            }
            if (godown == true)
            {
                pacman.Top += playerSpeed;
                // pacman.Image = Properties.Resources.down;
            }
            if (group == true)
            {
                pacman.Top -= playerSpeed;
                // pacman.Image = Properties.Resources.Up;
            }



            if (pacman.Left < -10)
            {
                pacman.Left = 600;
            }
            if (pacman.Left > 600)
            {
                pacman.Left = -10;
            }

            if (pacman.Top < -10)
            {
                pacman.Top = 550;
            }
            if (pacman.Top > 550)
            {
                pacman.Top = 0;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "coin" && x.Visible == true)
                    {
                        if (pacman.Bounds.IntersectsWith(x.Bounds))
                        {
                            score += 1;
                            x.Visible = false;
                        }
                    }
                    if ((string) x.Tag == "wall")
                    {
                        if (pacman.Bounds.IntersectsWith(x.Bounds))
                            {
                            // ????????? ???? ?????
                        }
                    }
                    if (((string)x.Tag == "ghost"))
                        {
                        if (pacman.Bounds.IntersectsWith(x.Bounds))
                            {
                            // ????????? ???? ????? 
                        }
                    }
                }
            }

            // ???????? ?????????? ???????? sun1zu: ��� �� � �������� ��� ����� �������, ��������� ���� ������� :P

            redGhost.Left += redGhostSpeed;
            if (redGhost.Bounds.IntersectsWith(default))    // sun1zu: refactored: (was: IntersectsWith())
            {

            }    




            if (score == 46)
            {
                // ???-?? ?????
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

            foreach(Control x in this.Controls)
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

        }

    }
}
