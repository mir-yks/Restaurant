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
    public partial class OrderItem : Form
    {
        private int roleId;
        public OrderItem(int role)
        {
            InitializeComponent();
            roleId = role;
            ConfigureButtons();
        }
        private void ConfigureButtons()
        {
            button2.Visible = false;
            button1.Visible = true;

            if (roleId == 3)
            {
                button2.Visible = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
