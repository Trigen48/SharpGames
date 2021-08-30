using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers
{
    public enum PlayerMode
    {
        AI = 0,
        PLAYER1 = 1,
        PLAYER2 = 2,
    }
   public class Board: UserControl
    {

       Image mg = null;
       PlayerMode b;
       Marker m = new Marker();
       public PlayerMode PlayerTurn
       {
           get
           {
               return b;
           }
           set
           {
               b = value;
           }
           
       }

       public Board()
       {
           InitializeComponent();
           Generate();
           
       }

       public void SetupPlayers(PlayerMode p1, PlayerMode p2)
       {

           /// Setpu P1
           /// 
           Players = new Dictionary<PlayerMode, Dictionary<Point, Piece>>();

           Dictionary<Point, Piece> pp1 = new Dictionary<Point, Piece>();
           for (int x = 0; x < 8; x+=2)
           {
               Piece p = new Piece();
               p.Crowend = false;
               p.Parent = this;
               p.Player = p1;
               p.Location = GetLoc(new Point(x, 7));
               pp1.Add(new Point(x, 7), p);
           }

           for (int x = 1; x < 8; x += 2)
           {
               Piece p = new Piece();
               p.Crowend = false;
               p.Parent = this;
               p.Location = GetLoc(new Point(x, 6));
               p.Player = p1;
               pp1.Add(new Point(x, 6), p);
           }

           for (int x = 0; x < 8; x += 2)
           {
               Piece p = new Piece();
               p.Crowend = false;
               p.Parent = this;
               p.Location = GetLoc(new Point(x, 5));
               p.Player = p1;
               pp1.Add(new Point(x, 5), p);
           }

           Players.Add(p1, pp1);


           //Player 2
           Dictionary<Point, Piece> pp2 = new Dictionary<Point, Piece>();
           for (int x = 1; x < 8; x += 2)
           {
               Piece p = new Piece(Color.FromArgb(125,125,125));
               p.Crowend = false;
               p.Parent = this;
               p.Location = GetLoc(new Point(x, 0));
               p.Player = p2;
               pp2.Add(new Point(x, 0), p);
           }

           for (int x = 0; x < 8; x += 2)
           {
               Piece p = new Piece(Color.FromArgb(125, 125, 125));
               p.Crowend = false;
               p.Parent = this;
               p.Location = GetLoc(new Point(x, 1));
               p.Player = p2;
               pp2.Add(new Point(x, 1), p);
           }

           for (int x = 1; x < 8; x += 2)
           {
               Piece p = new Piece(Color.FromArgb(125, 125, 125));
               p.Crowend = false;
               p.Parent = this;
               p.Location = GetLoc(new Point(x, 2));
               p.Player = p2;
               pp2.Add(new Point(x, 2), p);
           }
           Players.Add(p2, pp2);

       }

       private Point GetCord(Point p)
       {
           return new Point((int)(p.X / 64), (int)(p.Y / 64));
       }
       private Point GetLoc(Point p)
       {
           return new Point(p.X * 64, p.Y * 64);
       }

       public void Generate()
       {

           Generate(Color.Black, Color.White);

       }

       public void Generate(Color s1, Color s2)
       {
           Image g = new Bitmap(64 * 8, 64 * 8);

           Graphics gr = Graphics.FromImage(g);

           int alt = 0;
           int al = 0;

       for (int y = 0; y < 8; y++)         
       {
           for (int x = 0; x < 8; x++)
           {
               switch (alt)
               {
                   case 0:
                       switch (al)
                       {
                           case 0:
                               gr.FillRectangle(new SolidBrush(s1), x * 64, y * 64, 64, 64);
                               break;

                           case 1:
                               gr.FillRectangle(new SolidBrush(s2), x * 64, y * 64, 64, 64);
                               break;
                       }
                       alt = 1;
                           break;

                   case 1:

                           switch (al)
                           {
                               case 0:
                                   gr.FillRectangle(new SolidBrush(s2), x * 64, y * 64, 64, 64);
                                   break;

                               case 1:
                                   gr.FillRectangle(new SolidBrush(s1), x * 64, y * 64, 64, 64);
                                   break;
                           }

                       alt=0;
                       break;
               }
 
           }

           switch (al)
           {
               case 0:

                   al = 1;
                   break;
               case 1:
                   al = 0;
                   break;
           }
       }
       gr.DrawLines(new Pen(new SolidBrush(s1), 1), new Point[] { new Point(0, 0), new Point((64 * 8) - 1, 0), new Point((64 * 8) - 1, (64 * 8) - 1), new Point(0, (64 * 8) - 1), new Point(0, 0) });

           this.Size= new Size(64*8,64*8);

           mg=g;
           gr=null;
           g=null;
           Invalidate();
       }

       private void InitializeComponent()
       {
           this.SuspendLayout();
           // 
           // Board
           // 
           this.BackColor = System.Drawing.Color.White;
           this.DoubleBuffered = true;
           this.Name = "Board";
           this.Paint += new System.Windows.Forms.PaintEventHandler(this.Board_Paint);
           this.ResumeLayout(false);

       }

       private void Board_Paint(object sender, PaintEventArgs e)
       {
           e.Graphics.Clear(Color.White);


           e.Graphics.DrawImage(mg, 0, 0);
       }

       /// Piece Information
       /// 


       private Dictionary<PlayerMode,Dictionary<Point,Piece>> Players;

       private Piece cur;
       public void OnPieceClick(Piece p)
       {
           cur = p;
       }


    }
}
