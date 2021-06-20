using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agar.io
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private int _xPos;
        private int _yPos;
        private bool _dragging;
        private void ovalPictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var c = sender as PictureBox;
            if (!_dragging || null == c) return;
            c.Top = e.Y + c.Top - _yPos;
            c.Left = e.X + c.Left - _xPos;
            Refresh();
        }

        private void ovalPictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _dragging = true;
            _xPos = e.X;
            _yPos = e.Y;
        }

        private void ovalPictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            var c = sender as PictureBox;
            if (null == c) return;
            _dragging = false;
        }

        List<PictureBox> food = new List<PictureBox>();
        private void hrana()
        {
            Graphics g = CreateGraphics();
            Random r = new Random();
            PictureBox picture = new PictureBox
            {
                Name = "pictureBox".ToString(),
                Size = new Size(8, 8),
                Location = new Point(r.Next(0, ClientRectangle.Width), r.Next(0, ClientRectangle.Height)),
                BackColor = Color.Red,
            };
            food.Add(picture);        
        }

        private void prikazivanje()
        {
            if (food.Count < 200)
            {
                for (int i = 0; i < food.Count; i++)
                {
                    Controls.Add(food[i]);
                }
            }
        }
        List<PictureBox> bombs = new List<PictureBox>();
        int k = 0;
        private void jedenje()
        {
            Random r = new Random();
            if (ovalPictureBox1.Width >= 300)
            {
                for(int i = 0; i < food.Count; i++)
                {
                    food[i].Width /= 2;
                    food[i].Height /= 2;
                }
                ovalPictureBox1.Width /= 2;
                ovalPictureBox1.Height /= 2;
                if (bombs.Count <=5)
                {
                    PictureBox bomb = new PictureBox
                    {
                        Name = "bomb".ToString() + k.ToString(),
                        Size = new Size(20, 20),
                        Location = new Point(r.Next(0, ClientRectangle.Width), r.Next(0, ClientRectangle.Height)),
                        BackColor = Color.Black,
                    };
                    bombs.Add(bomb);
                    Controls.Add(bomb);
                }
            }
            if (bombs != null)
            {
                for (int i = 0; i < bombs.Count; i++)
                {
                    if (ovalPictureBox1.Bounds.IntersectsWith(bombs[i].Bounds))
                    {
                        timer1.Stop();
                        timer2.Stop();
                        MessageBox.Show("GAME OVER! \n Your score is " + score.ToString() + ".");                      
                    }
                }
            }
            for (int i = 0; i < food.Count; i++) 
            {
                if (ovalPictureBox1.Bounds.IntersectsWith(food[i].Bounds))
                {
                    food[i].Location = new Point(r.Next(0, ClientRectangle.Width), r.Next(0, ClientRectangle.Height));
                    ovalPictureBox1.Height++;
                    ovalPictureBox1.Width++;
                    Refresh();
                    foodEaten++;
                    score++;
                }
            }
            randomBlobsSpawn();
            randomBlobsCollision();
        }
        List<OvalPictureBox> randomBlobs = new List<OvalPictureBox>();
        private void randomBlobsSpawn()
        {
            Random r = new Random();
            if(foodEaten >= 30)
            {
                int size = r.Next(50, ovalPictureBox1.Width + 100);
                OvalPictureBox randomBlob = new OvalPictureBox
                {
                    Name = "randomBlob".ToString() + k.ToString(),
                    Size = new Size(size, size),
                    Location = new Point(r.Next(size, ClientRectangle.Width - size), r.Next(size, ClientRectangle.Height - size)),
                    BackColor = Color.Blue,
                };
                randomBlobs.Add(randomBlob);
                Controls.Add(randomBlob);
                foodEaten -= 30;
            }
        }
        private void randomBlobsCollision()
        {
            for (int i = 0; i < randomBlobs.Count; i++) 
            {
                if (randomBlobs[i].Visible == true && ovalPictureBox1.Bounds.IntersectsWith(randomBlobs[i].Bounds))
                {
                    if(randomBlobs[i].Width > ovalPictureBox1.Width)
                    {
                        timer1.Stop();
                        timer2.Stop();
                        MessageBox.Show("GAME OVER! \n Your score is " + score.ToString() + ".");
                    }
                    else
                    {
                        randomBlobs[i].Visible = false;
                        randomBlobs.RemoveAt(i);
                        ovalPictureBox1.Width += 20;
                        ovalPictureBox1.Height += 20;
                        score += 20;
                    }
                }
            }
        }

        int foodEaten;
        int score;
        private void timer1_Tick(object sender, EventArgs e)
        {
            hrana();
            prikazivanje();
            int directionX = 1;
            int directionY = 1;
            for(int i = 0; i < randomBlobs.Count; i++)
            {
                Point a = new Point(randomBlobs[i].Location.X + directionX * 5, randomBlobs[i].Location.Y - directionY * 5);
                randomBlobs[i].Location = a;
                if (randomBlobs[i].Top <= 0) directionY *= 1;
                if (randomBlobs[i].Left <= 0) directionX *= -1;
                if (randomBlobs[i].Bottom >= ClientRectangle.Height) directionY *= -1;
                if (randomBlobs[i].Right >= ClientRectangle.Width) directionX *= -1;
            }
            Refresh();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            jedenje();
            Refresh();
        }
    }
}
