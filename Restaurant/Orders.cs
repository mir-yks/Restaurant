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
    public partial class Orders : Form
    {
        private int roleId;
        public Orders(int role)
        {
            InitializeComponent(); 
            roleId = role;
            ConfigureButtons();
        }

        private void ConfigureButtons()
        {
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button1.Visible = true;

            if (roleId == 1)
            {
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;
            }
            else if (roleId == 2)
            {
                button2.Visible = true;
                button3.Visible = true;
                button3.Location = new System.Drawing.Point(657, 494);
                button2.Location = new System.Drawing.Point(536, 494);
            }
            else if (roleId == 3)
            {
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;
                button6.Location = new System.Drawing.Point(657, 433);
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OrderInsert OrderInsert = new OrderInsert();
            this.Visible = true;
            OrderInsert.ShowDialog();
            this.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OrderInsert OrderInsert = new OrderInsert();
            this.Visible = true;
            OrderInsert.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OrderItem OrderItem = new OrderItem(roleId);
            this.Visible = false;
            OrderItem.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Revenue Revenue = new Revenue();
            this.Visible = true;
            Revenue.ShowDialog();
            this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
