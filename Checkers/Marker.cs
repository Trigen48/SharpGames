using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers
{
   public class Marker: UserControl
    {

       public Marker()
       {
           InitializeComponent();
       }

       private void InitializeComponent()
       {
           this.SuspendLayout();
           // 
           // Marker
           // 
           this.BackColor = System.Drawing.Color.DodgerBlue;
           this.DoubleBuffered = true;
           this.Name = "Marker";
           this.Size = new System.Drawing.Size(64, 64);
           this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Marker_MouseClick);
           this.ResumeLayout(false);

       }

       private void Marker_MouseClick(object sender, MouseEventArgs e)
       {

       }



    }
}
