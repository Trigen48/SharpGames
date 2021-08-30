using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace Snake
{
    public partial class Auto_Load : Form
    {
        public Auto_Load()
        {
            InitializeComponent();
        }

        private void Auto_Load_Load(object sender, EventArgs e)
        {
            string[] t = System.IO.Directory.GetFiles(Application.StartupPath + "\\Save game\\", "*.sav");

            if (t.Length == 0)
            {
                return;
            }

            for (int x = 0; x < t.Length; x++)
            {
                
                listBox1.Items.Add(System.IO.Path.GetFileNameWithoutExtension(t[x]));
            }


        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
            }
            catch
            {
            }
        }
    }
}
