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
    public partial class Clients : Form
    {
        public Clients()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClientsInsert ClientsInsert = new ClientsInsert();
            this.Visible = true;
            ClientsInsert.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClientsInsert ClientsInsert = new ClientsInsert();
            this.Visible = true;
            ClientsInsert.ShowDialog();
            this.Visible = true;
        }
    }
}
