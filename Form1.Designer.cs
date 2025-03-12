namespace PACMAN_GAME
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            txtScore = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            pacman = new PictureBox();
            pinkGhost = new PictureBox();
            yellowGhost = new PictureBox();
            redGhost = new PictureBox();
            pictureBox6 = new PictureBox();
            pictureBox7 = new PictureBox();
            pictureBox8 = new PictureBox();
            pictureBox9 = new PictureBox();
            pictureBox10 = new PictureBox();
            pictureBox11 = new PictureBox();
            pictureBox12 = new PictureBox();
            pictureBox13 = new PictureBox();
            pictureBox14 = new PictureBox();
            pictureBox15 = new PictureBox();
            pictureBox16 = new PictureBox();
            pictureBox17 = new PictureBox();
            pictureBox18 = new PictureBox();
            pictureBox19 = new PictureBox();
            pictureBox20 = new PictureBox();
            pictureBox21 = new PictureBox();
            pictureBox22 = new PictureBox();
            pictureBox23 = new PictureBox();
            pictureBox24 = new PictureBox();
            pictureBox25 = new PictureBox();
            pictureBox26 = new PictureBox();
            gameTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pacman).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pinkGhost).BeginInit();
            ((System.ComponentModel.ISupportInitialize)yellowGhost).BeginInit();
            ((System.ComponentModel.ISupportInitialize)redGhost).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox14).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox15).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox16).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox17).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox18).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox19).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox20).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox21).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox22).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox23).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox24).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox25).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox26).BeginInit();
            SuspendLayout();
            // 
            // txtScore
            // 
            txtScore.AutoSize = true;
            txtScore.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            txtScore.ForeColor = Color.White;
            txtScore.Location = new Point(12, 9);
            txtScore.Name = "txtScore";
            txtScore.Size = new Size(80, 25);
            txtScore.TabIndex = 0;
            txtScore.Text = "Score: 0";
            txtScore.Click += label1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Navy;
            pictureBox1.Location = new Point(161, -17);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(39, 169);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Tag = "wall";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Navy;
            pictureBox2.Location = new Point(499, -8);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(39, 169);
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            pictureBox2.Tag = "wall";
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.Navy;
            pictureBox3.Location = new Point(590, 280);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(39, 169);
            pictureBox3.TabIndex = 4;
            pictureBox3.TabStop = false;
            pictureBox3.Tag = "wall";
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.Navy;
            pictureBox4.Location = new Point(251, 280);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(39, 169);
            pictureBox4.TabIndex = 3;
            pictureBox4.TabStop = false;
            pictureBox4.Tag = "wall";
            // 
            // pacman
            // 
            pacman.Image = (Image)resources.GetObject("pacman.Image");
            pacman.Location = new Point(28, 47);
            pacman.Name = "pacman";
            pacman.Size = new Size(45, 60);
            pacman.SizeMode = PictureBoxSizeMode.StretchImage;
            pacman.TabIndex = 5;
            pacman.TabStop = false;
            pacman.Tag = "ghost";
            pacman.Click += pictureBox5_Click;
            // 
            // pinkGhost
            // 
            pinkGhost.Image = (Image)resources.GetObject("pinkGhost.Image");
            pinkGhost.Location = new Point(602, 78);
            pinkGhost.Name = "pinkGhost";
            pinkGhost.Size = new Size(45, 60);
            pinkGhost.SizeMode = PictureBoxSizeMode.StretchImage;
            pinkGhost.TabIndex = 6;
            pinkGhost.TabStop = false;
            // 
            // yellowGhost
            // 
            yellowGhost.Image = (Image)resources.GetObject("yellowGhost.Image");
            yellowGhost.Location = new Point(493, 309);
            yellowGhost.Name = "yellowGhost";
            yellowGhost.Size = new Size(45, 60);
            yellowGhost.SizeMode = PictureBoxSizeMode.StretchImage;
            yellowGhost.TabIndex = 7;
            yellowGhost.TabStop = false;
            // 
            // redGhost
            // 
            redGhost.Image = (Image)resources.GetObject("redGhost.Image");
            redGhost.Location = new Point(366, 92);
            redGhost.Name = "redGhost";
            redGhost.Size = new Size(45, 60);
            redGhost.SizeMode = PictureBoxSizeMode.StretchImage;
            redGhost.TabIndex = 8;
            redGhost.TabStop = false;
            redGhost.Click += pictureBox8_Click;
            // 
            // pictureBox6
            // 
            pictureBox6.Image = (Image)resources.GetObject("pictureBox6.Image");
            pictureBox6.Location = new Point(296, 397);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(43, 41);
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.TabIndex = 9;
            pictureBox6.TabStop = false;
            // 
            // pictureBox7
            // 
            pictureBox7.Image = (Image)resources.GetObject("pictureBox7.Image");
            pictureBox7.Location = new Point(345, 397);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(43, 41);
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.TabIndex = 10;
            pictureBox7.TabStop = false;
            // 
            // pictureBox8
            // 
            pictureBox8.Image = (Image)resources.GetObject("pictureBox8.Image");
            pictureBox8.Location = new Point(394, 397);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(43, 41);
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 11;
            pictureBox8.TabStop = false;
            // 
            // pictureBox9
            // 
            pictureBox9.Image = (Image)resources.GetObject("pictureBox9.Image");
            pictureBox9.Location = new Point(443, 397);
            pictureBox9.Name = "pictureBox9";
            pictureBox9.Size = new Size(43, 41);
            pictureBox9.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox9.TabIndex = 12;
            pictureBox9.TabStop = false;
            // 
            // pictureBox10
            // 
            pictureBox10.Image = (Image)resources.GetObject("pictureBox10.Image");
            pictureBox10.Location = new Point(495, 397);
            pictureBox10.Name = "pictureBox10";
            pictureBox10.Size = new Size(43, 41);
            pictureBox10.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox10.TabIndex = 13;
            pictureBox10.TabStop = false;
            // 
            // pictureBox11
            // 
            pictureBox11.Image = (Image)resources.GetObject("pictureBox11.Image");
            pictureBox11.Location = new Point(296, 341);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new Size(43, 41);
            pictureBox11.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox11.TabIndex = 14;
            pictureBox11.TabStop = false;
            // 
            // pictureBox12
            // 
            pictureBox12.Image = (Image)resources.GetObject("pictureBox12.Image");
            pictureBox12.Location = new Point(415, 12);
            pictureBox12.Name = "pictureBox12";
            pictureBox12.Size = new Size(43, 41);
            pictureBox12.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox12.TabIndex = 19;
            pictureBox12.TabStop = false;
            pictureBox12.Visible = false;
            // 
            // pictureBox13
            // 
            pictureBox13.Image = (Image)resources.GetObject("pictureBox13.Image");
            pictureBox13.Location = new Point(363, 12);
            pictureBox13.Name = "pictureBox13";
            pictureBox13.Size = new Size(43, 41);
            pictureBox13.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox13.TabIndex = 18;
            pictureBox13.TabStop = false;
            pictureBox13.Visible = false;
            // 
            // pictureBox14
            // 
            pictureBox14.Image = (Image)resources.GetObject("pictureBox14.Image");
            pictureBox14.Location = new Point(314, 12);
            pictureBox14.Name = "pictureBox14";
            pictureBox14.Size = new Size(43, 41);
            pictureBox14.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox14.TabIndex = 17;
            pictureBox14.TabStop = false;
            pictureBox14.Visible = false;
            // 
            // pictureBox15
            // 
            pictureBox15.Image = (Image)resources.GetObject("pictureBox15.Image");
            pictureBox15.Location = new Point(265, 12);
            pictureBox15.Name = "pictureBox15";
            pictureBox15.Size = new Size(43, 41);
            pictureBox15.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox15.TabIndex = 16;
            pictureBox15.TabStop = false;
            pictureBox15.Visible = false;
            // 
            // pictureBox16
            // 
            pictureBox16.Image = (Image)resources.GetObject("pictureBox16.Image");
            pictureBox16.Location = new Point(216, 12);
            pictureBox16.Name = "pictureBox16";
            pictureBox16.Size = new Size(43, 41);
            pictureBox16.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox16.TabIndex = 15;
            pictureBox16.TabStop = false;
            // 
            // pictureBox17
            // 
            pictureBox17.Image = (Image)resources.GetObject("pictureBox17.Image");
            pictureBox17.Location = new Point(748, 12);
            pictureBox17.Name = "pictureBox17";
            pictureBox17.Size = new Size(43, 41);
            pictureBox17.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox17.TabIndex = 24;
            pictureBox17.TabStop = false;
            // 
            // pictureBox18
            // 
            pictureBox18.Image = (Image)resources.GetObject("pictureBox18.Image");
            pictureBox18.Location = new Point(696, 12);
            pictureBox18.Name = "pictureBox18";
            pictureBox18.Size = new Size(43, 41);
            pictureBox18.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox18.TabIndex = 23;
            pictureBox18.TabStop = false;
            // 
            // pictureBox19
            // 
            pictureBox19.Image = (Image)resources.GetObject("pictureBox19.Image");
            pictureBox19.Location = new Point(647, 12);
            pictureBox19.Name = "pictureBox19";
            pictureBox19.Size = new Size(43, 41);
            pictureBox19.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox19.TabIndex = 22;
            pictureBox19.TabStop = false;
            // 
            // pictureBox20
            // 
            pictureBox20.Image = (Image)resources.GetObject("pictureBox20.Image");
            pictureBox20.Location = new Point(598, 12);
            pictureBox20.Name = "pictureBox20";
            pictureBox20.Size = new Size(43, 41);
            pictureBox20.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox20.TabIndex = 21;
            pictureBox20.TabStop = false;
            // 
            // pictureBox21
            // 
            pictureBox21.Image = (Image)resources.GetObject("pictureBox21.Image");
            pictureBox21.Location = new Point(549, 12);
            pictureBox21.Name = "pictureBox21";
            pictureBox21.Size = new Size(43, 41);
            pictureBox21.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox21.TabIndex = 20;
            pictureBox21.TabStop = false;
            // 
            // pictureBox22
            // 
            pictureBox22.Image = (Image)resources.GetObject("pictureBox22.Image");
            pictureBox22.Location = new Point(216, 78);
            pictureBox22.Name = "pictureBox22";
            pictureBox22.Size = new Size(43, 41);
            pictureBox22.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox22.TabIndex = 25;
            pictureBox22.TabStop = false;
            // 
            // pictureBox23
            // 
            pictureBox23.Image = (Image)resources.GetObject("pictureBox23.Image");
            pictureBox23.Location = new Point(265, 78);
            pictureBox23.Name = "pictureBox23";
            pictureBox23.Size = new Size(43, 41);
            pictureBox23.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox23.TabIndex = 26;
            pictureBox23.TabStop = false;
            // 
            // pictureBox24
            // 
            pictureBox24.Image = (Image)resources.GetObject("pictureBox24.Image");
            pictureBox24.Location = new Point(748, 78);
            pictureBox24.Name = "pictureBox24";
            pictureBox24.Size = new Size(43, 41);
            pictureBox24.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox24.TabIndex = 27;
            pictureBox24.TabStop = false;
            // 
            // pictureBox25
            // 
            pictureBox25.Image = (Image)resources.GetObject("pictureBox25.Image");
            pictureBox25.Location = new Point(696, 78);
            pictureBox25.Name = "pictureBox25";
            pictureBox25.Size = new Size(43, 41);
            pictureBox25.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox25.TabIndex = 28;
            pictureBox25.TabStop = false;
            // 
            // pictureBox26
            // 
            pictureBox26.Image = (Image)resources.GetObject("pictureBox26.Image");
            pictureBox26.Location = new Point(434, 78);
            pictureBox26.Name = "pictureBox26";
            pictureBox26.Size = new Size(43, 41);
            pictureBox26.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox26.TabIndex = 29;
            pictureBox26.TabStop = false;
            // 
            // gameTimer
            // 
            gameTimer.Tick += MainGameTimer;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBox26);
            Controls.Add(pictureBox25);
            Controls.Add(pictureBox24);
            Controls.Add(pictureBox23);
            Controls.Add(pictureBox22);
            Controls.Add(pictureBox17);
            Controls.Add(pictureBox18);
            Controls.Add(pictureBox19);
            Controls.Add(pictureBox20);
            Controls.Add(pictureBox21);
            Controls.Add(pictureBox12);
            Controls.Add(pictureBox13);
            Controls.Add(pictureBox14);
            Controls.Add(pictureBox15);
            Controls.Add(pictureBox16);
            Controls.Add(pictureBox11);
            Controls.Add(pictureBox10);
            Controls.Add(pictureBox9);
            Controls.Add(pictureBox8);
            Controls.Add(pictureBox7);
            Controls.Add(pictureBox6);
            Controls.Add(redGhost);
            Controls.Add(yellowGhost);
            Controls.Add(pinkGhost);
            Controls.Add(pacman);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(txtScore);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            KeyDown += keyisdown;
            KeyUp += keyisup;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pacman).EndInit();
            ((System.ComponentModel.ISupportInitialize)pinkGhost).EndInit();
            ((System.ComponentModel.ISupportInitialize)yellowGhost).EndInit();
            ((System.ComponentModel.ISupportInitialize)redGhost).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox14).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox15).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox16).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox17).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox18).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox19).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox20).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox21).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox22).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox23).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox24).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox25).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox26).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label txtScore;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private PictureBox pacman;
        private PictureBox pinkGhost;
        private PictureBox yellowGhost;
        private PictureBox redGhost;
        private PictureBox pictureBox6;
        private PictureBox pictureBox7;
        private PictureBox pictureBox8;
        private PictureBox pictureBox9;
        private PictureBox pictureBox10;
        private PictureBox pictureBox11;
        private PictureBox pictureBox12;
        private PictureBox pictureBox13;
        private PictureBox pictureBox14;
        private PictureBox pictureBox15;
        private PictureBox pictureBox16;
        private PictureBox pictureBox17;
        private PictureBox pictureBox18;
        private PictureBox pictureBox19;
        private PictureBox pictureBox20;
        private PictureBox pictureBox21;
        private PictureBox pictureBox22;
        private PictureBox pictureBox23;
        private PictureBox pictureBox24;
        private PictureBox pictureBox25;
        private PictureBox pictureBox26;
        private System.Windows.Forms.Timer gameTimer;
    }
}
