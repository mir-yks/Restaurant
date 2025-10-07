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
    public partial class OrderInsert : Form
    {
        public OrderInsert()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OrderItem OrderItem = new OrderItem(1);
            this.Visible = false;
            OrderItem.ShowDialog();
            this.Visible = true;
        }
    }
}
