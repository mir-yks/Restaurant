using System;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class DesktopOfficiant : Form
    {
        private string currentFIO;
        public DesktopOfficiant(string fio)
        {
            InitializeComponent();
            currentFIO = fio;
            label4.Text = $"Добро пожаловать, {currentFIO}!"; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menu Menu = new Menu(3);
            this.Visible = false;
            Menu.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Orders Orders = new Orders(3);
            this.Visible = false;
            Orders.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tables Tables = new Tables(3);
            this.Visible = false;
            Tables.ShowDialog();
            this.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
