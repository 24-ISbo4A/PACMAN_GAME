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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtScore = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            pictureBox3 = new System.Windows.Forms.PictureBox();
            pictureBox4 = new System.Windows.Forms.PictureBox();
            pacman = new System.Windows.Forms.PictureBox();
            pinkGhost = new System.Windows.Forms.PictureBox();
            yellowGhost = new System.Windows.Forms.PictureBox();
            redGhost = new System.Windows.Forms.PictureBox();
            pictureBox6 = new System.Windows.Forms.PictureBox();
            pictureBox7 = new System.Windows.Forms.PictureBox();
            pictureBox8 = new System.Windows.Forms.PictureBox();
            pictureBox9 = new System.Windows.Forms.PictureBox();
            pictureBox10 = new System.Windows.Forms.PictureBox();
            pictureBox11 = new System.Windows.Forms.PictureBox();
            pictureBox12 = new System.Windows.Forms.PictureBox();
            pictureBox13 = new System.Windows.Forms.PictureBox();
            pictureBox14 = new System.Windows.Forms.PictureBox();
            pictureBox15 = new System.Windows.Forms.PictureBox();
            pictureBox16 = new System.Windows.Forms.PictureBox();
            pictureBox17 = new System.Windows.Forms.PictureBox();
            pictureBox18 = new System.Windows.Forms.PictureBox();
            pictureBox19 = new System.Windows.Forms.PictureBox();
            pictureBox20 = new System.Windows.Forms.PictureBox();
            pictureBox21 = new System.Windows.Forms.PictureBox();
            pictureBox22 = new System.Windows.Forms.PictureBox();
            pictureBox23 = new System.Windows.Forms.PictureBox();
            pictureBox24 = new System.Windows.Forms.PictureBox();
            pictureBox25 = new System.Windows.Forms.PictureBox();
            gameTimer = new System.Windows.Forms.Timer(components);
            pictureBox26 = new System.Windows.Forms.PictureBox();
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
            txtScore.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, ((System.Drawing.FontStyle)(System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)), System.Drawing.GraphicsUnit.Point, ((byte)204));
            txtScore.ForeColor = System.Drawing.Color.White;
            txtScore.Location = new System.Drawing.Point(12, 9);
            txtScore.Name = "txtScore";
            txtScore.Size = new System.Drawing.Size(80, 25);
            txtScore.TabIndex = 0;
            txtScore.Text = "Score: 0";
            txtScore.Click += label1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = System.Drawing.Color.Navy;
            pictureBox1.Location = new System.Drawing.Point(161, -17);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(39, 169);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Tag = "wall";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = System.Drawing.Color.Navy;
            pictureBox2.Location = new System.Drawing.Point(499, -8);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(39, 169);
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            pictureBox2.Tag = "wall";
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = System.Drawing.Color.Navy;
            pictureBox3.Location = new System.Drawing.Point(590, 280);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new System.Drawing.Size(39, 169);
            pictureBox3.TabIndex = 4;
            pictureBox3.TabStop = false;
            pictureBox3.Tag = "wall";
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = System.Drawing.Color.Navy;
            pictureBox4.Location = new System.Drawing.Point(251, 280);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new System.Drawing.Size(39, 169);
            pictureBox4.TabIndex = 3;
            pictureBox4.TabStop = false;
            pictureBox4.Tag = "wall";
            // 
            // pacman
            // 
            pacman.BackColor = System.Drawing.Color.Transparent;
            pacman.Image = global::PACMAN_GAME.Properties.Resources.left;
            pacman.Location = new System.Drawing.Point(28, 47);
            pacman.Name = "pacman";
            pacman.Size = new System.Drawing.Size(45, 60);
            pacman.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pacman.TabIndex = 5;
            pacman.TabStop = false;
            pacman.Tag = "ghost";
            pacman.Click += pictureBox5_Click;
            // 
            // pinkGhost
            // 
            pinkGhost.BackColor = System.Drawing.Color.Transparent;
            pinkGhost.Image = global::PACMAN_GAME.Properties.Resources.red_ghost;
            pinkGhost.Location = new System.Drawing.Point(602, 78);
            pinkGhost.Name = "pinkGhost";
            pinkGhost.Size = new System.Drawing.Size(45, 60);
            pinkGhost.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pinkGhost.TabIndex = 6;
            pinkGhost.TabStop = false;
            // 
            // yellowGhost
            // 
            yellowGhost.BackColor = System.Drawing.Color.Transparent;
            yellowGhost.Image = global::PACMAN_GAME.Properties.Resources.red_ghost;
            yellowGhost.Location = new System.Drawing.Point(493, 309);
            yellowGhost.Name = "yellowGhost";
            yellowGhost.Size = new System.Drawing.Size(45, 60);
            yellowGhost.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            yellowGhost.TabIndex = 7;
            yellowGhost.TabStop = false;
            // 
            // redGhost
            // 
            redGhost.BackColor = System.Drawing.Color.Transparent;
            redGhost.Image = global::PACMAN_GAME.Properties.Resources.red_ghost;
            redGhost.Location = new System.Drawing.Point(366, 92);
            redGhost.Name = "redGhost";
            redGhost.Size = new System.Drawing.Size(45, 60);
            redGhost.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            redGhost.TabIndex = 8;
            redGhost.TabStop = false;
            redGhost.Click += pictureBox8_Click;
            // 
            // pictureBox6
            // 
            pictureBox6.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox6.Location = new System.Drawing.Point(296, 397);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new System.Drawing.Size(43, 41);
            pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox6.TabIndex = 9;
            pictureBox6.TabStop = false;
            pictureBox6.Tag = "coin";
            // 
            // pictureBox7
            // 
            pictureBox7.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox7.Location = new System.Drawing.Point(345, 397);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new System.Drawing.Size(43, 41);
            pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox7.TabIndex = 10;
            pictureBox7.TabStop = false;
            pictureBox7.Tag = "coin";
            // 
            // pictureBox8
            // 
            pictureBox8.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox8.Location = new System.Drawing.Point(394, 397);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new System.Drawing.Size(43, 41);
            pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 11;
            pictureBox8.TabStop = false;
            pictureBox8.Tag = "coin";
            // 
            // pictureBox9
            // 
            pictureBox9.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox9.Location = new System.Drawing.Point(443, 397);
            pictureBox9.Name = "pictureBox9";
            pictureBox9.Size = new System.Drawing.Size(43, 41);
            pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox9.TabIndex = 12;
            pictureBox9.TabStop = false;
            pictureBox9.Tag = "coin";
            // 
            // pictureBox10
            // 
            pictureBox10.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox10.Location = new System.Drawing.Point(495, 397);
            pictureBox10.Name = "pictureBox10";
            pictureBox10.Size = new System.Drawing.Size(43, 41);
            pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox10.TabIndex = 13;
            pictureBox10.TabStop = false;
            pictureBox10.Tag = "coin";
            // 
            // pictureBox11
            // 
            pictureBox11.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox11.Location = new System.Drawing.Point(296, 341);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new System.Drawing.Size(43, 41);
            pictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox11.TabIndex = 14;
            pictureBox11.TabStop = false;
            pictureBox11.Tag = "coin";
            // 
            // pictureBox12
            // 
            pictureBox12.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox12.Location = new System.Drawing.Point(415, 12);
            pictureBox12.Name = "pictureBox12";
            pictureBox12.Size = new System.Drawing.Size(43, 41);
            pictureBox12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox12.TabIndex = 19;
            pictureBox12.TabStop = false;
            pictureBox12.Tag = "coin";
            pictureBox12.Visible = false;
            // 
            // pictureBox13
            // 
            pictureBox13.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox13.Location = new System.Drawing.Point(363, 12);
            pictureBox13.Name = "pictureBox13";
            pictureBox13.Size = new System.Drawing.Size(43, 41);
            pictureBox13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox13.TabIndex = 18;
            pictureBox13.TabStop = false;
            pictureBox13.Tag = "coin";
            pictureBox13.Visible = false;
            // 
            // pictureBox14
            // 
            pictureBox14.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox14.Location = new System.Drawing.Point(314, 12);
            pictureBox14.Name = "pictureBox14";
            pictureBox14.Size = new System.Drawing.Size(43, 41);
            pictureBox14.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox14.TabIndex = 17;
            pictureBox14.TabStop = false;
            pictureBox14.Tag = "coin";
            pictureBox14.Visible = false;
            // 
            // pictureBox15
            // 
            pictureBox15.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox15.Location = new System.Drawing.Point(265, 12);
            pictureBox15.Name = "pictureBox15";
            pictureBox15.Size = new System.Drawing.Size(43, 41);
            pictureBox15.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox15.TabIndex = 16;
            pictureBox15.TabStop = false;
            pictureBox15.Tag = "coin";
            pictureBox15.Visible = false;
            // 
            // pictureBox16
            // 
            pictureBox16.BackColor = System.Drawing.Color.Transparent;
            pictureBox16.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox16.Location = new System.Drawing.Point(216, 12);
            pictureBox16.Name = "pictureBox16";
            pictureBox16.Size = new System.Drawing.Size(43, 41);
            pictureBox16.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox16.TabIndex = 15;
            pictureBox16.TabStop = false;
            pictureBox16.Tag = "coin";
            // 
            // pictureBox17
            // 
            pictureBox17.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox17.Location = new System.Drawing.Point(748, 12);
            pictureBox17.Name = "pictureBox17";
            pictureBox17.Size = new System.Drawing.Size(43, 41);
            pictureBox17.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox17.TabIndex = 24;
            pictureBox17.TabStop = false;
            pictureBox17.Tag = "coin";
            // 
            // pictureBox18
            // 
            pictureBox18.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox18.Location = new System.Drawing.Point(696, 12);
            pictureBox18.Name = "pictureBox18";
            pictureBox18.Size = new System.Drawing.Size(43, 41);
            pictureBox18.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox18.TabIndex = 23;
            pictureBox18.TabStop = false;
            pictureBox18.Tag = "coin";
            // 
            // pictureBox19
            // 
            pictureBox19.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox19.Location = new System.Drawing.Point(647, 12);
            pictureBox19.Name = "pictureBox19";
            pictureBox19.Size = new System.Drawing.Size(43, 41);
            pictureBox19.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox19.TabIndex = 22;
            pictureBox19.TabStop = false;
            pictureBox19.Tag = "coin";
            // 
            // pictureBox20
            // 
            pictureBox20.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox20.Location = new System.Drawing.Point(598, 12);
            pictureBox20.Name = "pictureBox20";
            pictureBox20.Size = new System.Drawing.Size(43, 41);
            pictureBox20.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox20.TabIndex = 21;
            pictureBox20.TabStop = false;
            pictureBox20.Tag = "coin";
            // 
            // pictureBox21
            // 
            pictureBox21.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox21.Location = new System.Drawing.Point(549, 12);
            pictureBox21.Name = "pictureBox21";
            pictureBox21.Size = new System.Drawing.Size(43, 41);
            pictureBox21.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox21.TabIndex = 20;
            pictureBox21.TabStop = false;
            pictureBox21.Tag = "coin";
            // 
            // pictureBox22
            // 
            pictureBox22.BackColor = System.Drawing.Color.Transparent;
            pictureBox22.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox22.Location = new System.Drawing.Point(216, 78);
            pictureBox22.Name = "pictureBox22";
            pictureBox22.Size = new System.Drawing.Size(43, 41);
            pictureBox22.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox22.TabIndex = 25;
            pictureBox22.TabStop = false;
            pictureBox22.Tag = "coin";
            // 
            // pictureBox23
            // 
            pictureBox23.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox23.Location = new System.Drawing.Point(265, 78);
            pictureBox23.Name = "pictureBox23";
            pictureBox23.Size = new System.Drawing.Size(43, 41);
            pictureBox23.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox23.TabIndex = 26;
            pictureBox23.TabStop = false;
            pictureBox23.Tag = "coin";
            // 
            // pictureBox24
            // 
            pictureBox24.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox24.Location = new System.Drawing.Point(748, 78);
            pictureBox24.Name = "pictureBox24";
            pictureBox24.Size = new System.Drawing.Size(43, 41);
            pictureBox24.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox24.TabIndex = 27;
            pictureBox24.TabStop = false;
            pictureBox24.Tag = "coin";
            // 
            // pictureBox25
            // 
            pictureBox25.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox25.Location = new System.Drawing.Point(696, 78);
            pictureBox25.Name = "pictureBox25";
            pictureBox25.Size = new System.Drawing.Size(43, 41);
            pictureBox25.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox25.TabIndex = 28;
            pictureBox25.TabStop = false;
            pictureBox25.Tag = "coin";
            // 
            // gameTimer
            // 
            gameTimer.Tick += MainGameTimer;
            // 
            // pictureBox26
            // 
            pictureBox26.Image = global::PACMAN_GAME.Properties.Resources.moneta;
            pictureBox26.Location = new System.Drawing.Point(434, 78);
            pictureBox26.Name = "pictureBox26";
            pictureBox26.Size = new System.Drawing.Size(43, 41);
            pictureBox26.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox26.TabIndex = 29;
            pictureBox26.TabStop = false;
            pictureBox26.Tag = "coin";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(yellowGhost);
            Controls.Add(pinkGhost);
            Controls.Add(redGhost);
            Controls.Add(pacman);
            Controls.Add(pictureBox26);
            Controls.Add(pictureBox25);
            Controls.Add(pictureBox24);
            Controls.Add(pictureBox23);
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
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(txtScore);
            Controls.Add(pictureBox22);
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
        private System.Windows.Forms.PictureBox pacman;
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
