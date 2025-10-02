using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class Menu : Form
    {
        private int roleId;
        public Menu(int role)
        {
            InitializeComponent();
            roleId = role;
            ConfigureButtons();
        }
        private void ConfigureButtons()
        {
            button1.Visible = true; 
            button2.Visible = false;
            button3.Visible = false;

            if (roleId == 1) 
            {
                button2.Visible = true;
                button3.Visible = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MenuInsert MenuInsert = new MenuInsert();
            this.Visible = true;
            MenuInsert.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MenuInsert MenuInsert = new MenuInsert();
            this.Visible = true;
            MenuInsert.ShowDialog();
            this.Visible = true;
        }
    }
}
