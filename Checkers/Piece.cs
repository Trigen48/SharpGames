using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers
{
   public class Piece: UserControl
    {
       Image i=null;
       bool IsCrowned;
       PlayerMode b;

       public PlayerMode Player
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

       public Piece()
       {
           InitializeComponent();
           Generate(Color.Black);
       }

       public Piece(Color c)
       {
           InitializeComponent();
           Generate(c);
       }

       public Piece(PlayerMode p, Color c)
       {
           InitializeComponent();
           Generate(c);
           b = p;
       }

       public Boolean Crowend
       {
           get
           {
               return IsCrowned;
           }

           set
           {
               IsCrowned = value;
           }
       }

       public void Generate(Color c)
       {
           this.Size = new Size((63), (63));

           Image gd = new Bitmap((int)(128) + 1, (int)(128) + 1);

           Graphics g = Graphics.FromImage(gd);

           SolidBrush sd = new SolidBrush(Color.FromArgb(150, c));
           Pen p = new Pen(new SolidBrush(c), 5);
           SolidBrush ss = new SolidBrush(Color.FromArgb(255, c));


           // Draw Checker
           g.FillEllipse(sd, 0, 0, 128 , 128);

           // Draw Radial Border
           g.DrawEllipse(p, 3, 3, 123 , 123 );

           float f = 0.50f * (128);



           // draw inner ring 1;
           g.DrawEllipse(p, ((128 * 00.25f)) , ((128 * 00.25f)), f, f);

           f = 0.25f * (128 * 00.90f);
           // draw inner ring 2;
           //    g.FillEllipse(new SolidBrush(Color.White), 32 - f / 2, 32 - f / 2, f, f);
           //    g.FillEllipse(sd, 32 - f / 2, 32 - f / 2, f, f);


           Image gg = new Bitmap(60, 60);
           g.Flush();
           g = null;
         g = Graphics.FromImage(gg);

         g.DrawImage(gd, 0, 0, 60, 60);

         this.BackgroundImage = gg;
       }

       private void InitializeComponent()
       {
           this.SuspendLayout();
           // 
           // Piece
           // 
           this.BackColor = System.Drawing.Color.White;
           this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
           this.DoubleBuffered = true;
           this.Name = "Piece";
           this.Size = new System.Drawing.Size(64, 64);
           this.Paint += new System.Windows.Forms.PaintEventHandler(this.Piece_Paint);
           this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Piece_MouseClick);
           this.ResumeLayout(false);

       }

       private void Piece_Paint(object sender, PaintEventArgs e)
       {
         //  e.Graphics.DrawImage(i, 0, 0, 64 , 64 );
       }

       private void Piece_MouseClick(object sender, MouseEventArgs e)
       {
           if (((Board)Parent).PlayerTurn == b)
           {
               ((Board)Parent).OnPieceClick(this);
           }
       }
       

       

    }
}
