using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace Snake
{
    public partial class ConfirmNew : Form
    {
        public ConfirmNew()
        {
            InitializeComponent();
        }

        bool t = false;

        public Boolean ShowDialog(Form Window)
        {

            base.ShowDialog(Window);
            return t;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            t = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            t = false;
            this.Close();
        }
    }
}
