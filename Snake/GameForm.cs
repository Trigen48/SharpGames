using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace Snake
{
    public partial class MainForm : Form
    {

        // Colors

        Color SnakeColor = Color.Black,DotColor= Color.Red, CrashColor=Color.Red,CrashPointColor=Color.Blue;
        

        List<Point> head = new List<Point>();
        Dictionary< Direction,Dictionary<Direction,Point>> p;

        int pr = 0;

        Point MovePoint = new Point(-1, 0);
        int Score = 0;

        Point coll = new Point();
        bool gameover = false;

        //
        int hd = 0;
        int speed=10;

        // dot resonate
        int dtt = 0;


        public MainForm()
        {
            InitializeComponent();
            
        }
        Player.Snake l = new Player.Snake();

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (gameover != true)
            {
                
                DrawSnake(e.Graphics);
                if (dtt >= 1)
                {
                    DrawPoint(e.Graphics);
                    dtt = 0;
                }
                else
                {
                    dtt += 1;
                }
                return;
                


            }
            else
            {
                DrawSnake(e.Graphics);
                Point fg = head[1];
                e.Graphics.FillRectangle(new SolidBrush(CrashColor), fg.X * 20, fg.Y * 20, 19, 19);
                e.Graphics.FillRectangle(new SolidBrush(CrashPointColor), coll.X * 20, coll.Y * 20, 19, 19);
            }

        }


        void AddPoint()
        {
            Point gg= head[head.Count-1];
           // Point xx = gg;

            head.Add(gg);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (hd >= speed)
            {
                MoveSnake();
                pr = 0;
                CheckBoanderies();
                if (Collision() == true)
                {
                    
                    lb_Msg.Text = "You Crashed, You Loose! SCORE: "+ Score.ToString();
                    gameover = true;

                    GameBoard.Invalidate();
                    timer1.Stop();

                    return;
                }

                if (HitPoint() == true)
                {
                    Score += 1;
                    lb_Msg.Text = "SCORE: " + Score.ToString();
                    AddPoint();
                    SetupPoint();
                }
                
                GameBoard.Invalidate();
                hd = 0;
            }
            else
            {
                hd += 1;
            }
        }

        bool HitPoint()
        {
            if (head[0] == dot)
            {
                SetupPoint();
                return true;
            }
            return false;
        }


        public Point GetPoint(int x, int y)
        {
            return new Point(x*20,y*20);
        }

        private void DrawPoint( Graphics g)
        {
            g.FillEllipse(new SolidBrush(DotColor), dot.X * 20, dot.Y * 20, 20, 20);
        }

        public void CheckBoanderies()
        {
            float x=l.GetCords.X , y=l.GetCords.Y;
            Point cc = head[0];

            // X Settings
            if (cc.X < 0)
            {
              cc.X=30;
            }
            else if (cc.X > 30)
            {
                cc.X = 0;
            }

            // Y Settings
            if (cc.Y < 0)
            {
                cc.Y = 19;
            }
            else if (cc.Y > 19)
            {
                cc.Y = 0;
            }
            head[0]=cc;
   
        }

        bool Collision()
        {
            if (head.Count > 1)
            {
                for (int x = 1; x < head.Count;x++ )
                {
                    if (head[0] == head[x])
                    {
                        coll = head[x];
                        return true;
                    }
                }
            }
            return false;
        }

        void SetupDirections()
        {
            p = new Dictionary<Direction, Dictionary<Direction, Point>>();

            // Setup Down Directions
            p.Add(Direction.DOWN, new Dictionary<Direction, Point>());
            p[Direction.DOWN].Add(Direction.LEFT, new Point(-1, 0));
            p[Direction.DOWN].Add(Direction.RIGHT, new Point(1, 0));

            // Setup Left Directions
            p.Add(Direction.LEFT, new Dictionary<Direction, Point>());
            p[Direction.LEFT].Add(Direction.DOWN, new Point(0, 1));
            p[Direction.LEFT].Add(Direction.UP, new Point(0, -1));

            // Setup Right Directions
            p.Add(Direction.RIGHT, new Dictionary<Direction, Point>());
            p[Direction.RIGHT].Add(Direction.DOWN, new Point(0, 1));
            p[Direction.RIGHT].Add(Direction.UP, new Point(0, -1));

            // Setup Up Directions
            p.Add(Direction.UP, new Dictionary<Direction, Point>());
            p[Direction.UP].Add(Direction.LEFT, new Point(-1, 0));
            p[Direction.UP].Add(Direction.RIGHT, new Point(1, 0));
        }
        Direction current = Direction.LEFT;

        Point dot;

        void SetupPoint()
        {
            Random r= new Random();
            dot = new Point(r.Next(0, 30), r.Next(0, 19));

            while (head.Contains(dot) == true)
            {
                dot = new Point(r.Next(0, 30), r.Next(0, 19));
            }
        }

        void MoveSnake(Direction d)
        {
            
            MovePoint = p[current][d];
            current = d;
            Point f= head[0];
            


        }

        void MoveSnake()
        {

            Point f = head[0];

            if(head.Count>1)
            {

                Point p_old = head[1];
                Point p_new = head[0];
                for (int x = 1; x < head.Count; x++)
                {
                    p_old = head[x];
                    head[x] = p_new;
                    
                    p_new = p_old;

                }

            }

            head[0] = new Point(f.X + MovePoint.X, f.Y + MovePoint.Y);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetupDirections();
        }

        void SetupNewPlayer()
        {
            head = new List<Point>();
            head.Add(new Point(15, 5));
            SetupPoint();
            gameover = false;
        }

        private void DrawSnake(Graphics g)
        {
            SolidBrush dc= new SolidBrush(SnakeColor);
            for (int x = 0; x < head.Count; x++)
            {
                g.FillRectangle(dc, head[x].X * 20, head[x].Y * 20, 19, 19);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (pr != 0)
            {
                return;
            }

            if (e.KeyData == Keys.Down)
            {
                if (current == Direction.LEFT || current == Direction.RIGHT)
                {
                    MoveSnake(Direction.DOWN);
                    pr = 1;
                        return;
                }
                return;
            }

            if (e.KeyData == Keys.Up)
            {
                if (current == Direction.LEFT || current == Direction.RIGHT)
                {
                    MoveSnake(Direction.UP);
                    pr = 1;
                    return;
                }
                return;
            }

            if (e.KeyData == Keys.Left)
            {
                if (current == Direction.DOWN || current == Direction.UP)
                {
                    MoveSnake(Direction.LEFT);
                    pr = 1;
                    return;
                }
                return;
            }

            if (e.KeyData == Keys.Right)
            {
                if (current == Direction.DOWN || current == Direction.UP)
                {
                    MoveSnake(Direction.RIGHT);
                    pr = 1;
                    return;
                }
                return;
            }

            if (e.KeyData == Keys.Escape)
            {
                timer1.Stop();
                this.Close();
            }

        }

        public void ResetPlayer()
        {
                    Score = 0;
                    lb_Msg.Text = "SCORE: 0";
                    SetupNewPlayer();
                    
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (timer1.Enabled == true)
            {
                timer1.Stop();

                bool t = new ConfirmNew().ShowDialog(this);

                if (t == true)
                {
                    ResetPlayer();
                }

            }
            else
            {
                ResetPlayer();
            }
            timer1.Start();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            bool was = false;

            if (timer1.Enabled == true)
            {
                was = true;
                timer1.Stop();
            }
            
            new About().ShowDialog(this);

            if (was == true)
            {
                timer1.Start();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (hd >= speed)
            {
                MoveSnake();
                pr = 0;
                CheckBoanderies();
                if (Collision() == true)
                {

                    lb_Msg.Text = "You Crashed, You Loose! SCORE: " + Score.ToString();
                    gameover = true;

                    GameBoard.Invalidate();
                    timer2.Stop();

                    return;
                }

                if (HitPoint() == true)
                {
                    Score += 1;
                    lb_Msg.Text = "SCORE: " + Score.ToString();
                    AddPoint();
                    SetupPoint();
                }

                GameBoard.Invalidate();
                hd = 0;
            }
            else
            {
                hd += 1;
            }
        }

    }
}
