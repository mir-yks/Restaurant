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
    public partial class Worker : Form
    {
        public Worker()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WorkerInsert WorkerInsert = new WorkerInsert();
            this.Visible = true;
            WorkerInsert.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WorkerInsert WorkerInsert = new WorkerInsert();
            this.Visible = true;
            WorkerInsert.ShowDialog();
            this.Visible = true;
        }
    }
}
