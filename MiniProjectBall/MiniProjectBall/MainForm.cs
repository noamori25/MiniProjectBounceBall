using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniProjectBall
{
    public delegate void MoveBallHandler(Ball ball);

    public partial class MainForm : Form
    {
        Random r = new Random();
        List<Ball> allBalls = new List<Ball>();
        const int HEIGHT_PIC = 40;
        const int WIDTH_PIC = 40;
        public static int windowWidth;
        public static int windowHeight;
        public static int buttonHeight;
        private static int ballNumber;
        private bool isClick = false;
        private int counter;
        
        public MainForm()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            windowHeight = ClientSize.Height - HEIGHT_PIC;
            windowWidth = ClientSize.Width - WIDTH_PIC;
            buttonHeight = btnAddBall.Height;
        }

        private void btnAddBall_Click(object sender, EventArgs e)
        {
            ballNumber++;
            Ball newBall = new Ball(ballNumber);
            newBall.X = newBall.PictureBox.Location.X;
            newBall.Y = newBall.PictureBox.Location.Y;
            newBall.PictureBox.Visible = true;
            this.Controls.Add(newBall.PictureBox);
            allBalls.Add(newBall);
            newBall.PictureBox.MouseClick += PictureBox_MouseClick;
            newBall.PictureBox.MouseLeave += PictureBox_MouseLeave;
            MoveBall(newBall);
            CheckCollision();
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            if (isClick)
            {
                Action a = () =>
                {
                    Ball b = allBalls.FirstOrDefault(ball => ball.PictureBox.Tag == p.Tag);
                    allBalls.Remove(b);
                    this.Controls.Remove(p);
                    counter++;
                    isClick = false;
                    if (allBalls.Count == 0)
                    {
                        MessageBox.Show($"You Won :) ({counter})");
                        counter = 0;
                    }
                };
                p.BeginInvoke(a);
            }
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            Action a = () =>
            {
                p.Image = Image.FromFile(@"C:\Users\Noam\Desktop\DotNET\boom.jpg");
                isClick = true;
            };
            p.BeginInvoke(a);
        }

        private void CheckBounds (Ball ball)
        {
            int NewBallX = ball.X + ball.Dx;
            int NewBallY = ball.Y + ball.Dy;
            {
                if (NewBallX < 0)
                {
                    ball.Dx = 1;
                }
                if (NewBallX + WIDTH_PIC > ClientSize.Width)
                {
                    ball.Dx = -1;
                }
                if (NewBallY < 0)
                {
                    ball.Dy = -1;
                }
                if (NewBallY > ClientSize.Height - HEIGHT_PIC)
                {
                    ball.Dy = -1;
                }
                if (NewBallY < btnAddBall.Height)
                {
                    ball.Dy = 1;
                }

                ball.X += ball.Dx;
                ball.Y += ball.Dy;
            }

        }

        private Task MoveBall(Ball ball)
        {
            Action a = () => MoveBallMethod(ball);
            Task.Run(() =>
            {
            while (true)
            {
                PictureBox p = ball.PictureBox;
                p.BeginInvoke(a);

                    CheckBounds(ball);
                    Thread.Sleep(5);
                }
            });

            return Task.CompletedTask;
        }

        private Task CheckCollision()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    CalcCollision();
                    Thread.Sleep(1);
                }
            });
            return Task.CompletedTask;
        }

        public void MoveBallMethod(Ball ball)
        {
            ball.PictureBox.Location = new Point(ball.X, ball.Y);
        }

        public void CalcCollision()
        {
            for (int i = 0; i < allBalls.Count; i++)
            {
                for (int j = 0; j < allBalls.Count; j ++)
                {
                    if ((allBalls[i].X > allBalls[j].X) &&
                        (allBalls[i].X < allBalls[j].X + allBalls[j].PictureBox.Width) &&
                        (allBalls[i].Y > allBalls[j].Y) &&
                        (allBalls[i].Y < allBalls[j].Y + allBalls[j].PictureBox.Height))
                    {
                        allBalls[j].Dx = -1;
                        allBalls[j].Dy = -1;
                    }
                }
            }
        }
    }
}
