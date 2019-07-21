using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniProjectBall
{
   public class Ball
    {
        private Random r = new Random();
        public const int PIC_HEIGHT = 60;
        public const int PIC_WIDTH = 60;
        public Point Point { get; set; }
        public int Dx { get; set; }
        public int Dy { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public PictureBox PictureBox { get; set; }

        public Ball (int number)
        {
            Dx = 1;
            Dy = 1;
            PictureBox = new PictureBox();
            PictureBox.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory+ "\\Images\\ball.jpg");
            PictureBox.Height = PIC_HEIGHT;
            PictureBox.Width = PIC_WIDTH;
            PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            Point = new Point(r.Next(0,MainForm.windowWidth), r.Next(MainForm.buttonHeight, MainForm.windowHeight));
            PictureBox.Location = Point;
            PictureBox.Tag = "Pic_" + number;
        }
        
    }
}
