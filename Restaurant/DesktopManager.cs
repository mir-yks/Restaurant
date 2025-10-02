using System;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class DesktopManager : Form
    {
        private string currentFIO;
        public DesktopManager(string fio)
        {
            InitializeComponent();
            currentFIO = fio;
            label4.Text = $"Добро пожаловать, {currentFIO}!"; 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Clients ClientsManager = new Clients();
            this.Visible = false;
            ClientsManager.ShowDialog();
            this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menu Menu = new Menu(2);
            this.Visible = false;
            Menu.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tables Tables = new Tables(2);
            this.Visible = false;
            Tables.ShowDialog();
            this.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Orders Orders = new Orders(2);
            this.Visible = false;
            Orders.ShowDialog();
            this.Visible = true;
        }
    }
}
