using System;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class DesktopAdministrator : Form
    {
        private string currentFIO;
        public DesktopAdministrator(string fio)
        {
            InitializeComponent();
            currentFIO = fio;
            label4.Text = $"Добро пожаловать, {currentFIO}!"; 
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Roles Roles = new Roles();
            this.Visible = false;
            Roles.ShowDialog();
            this.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Offers Offers = new Offers();
            this.Visible = false;
            Offers.ShowDialog();
            this.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Category Category = new Category();
            this.Visible = false;
            Category.ShowDialog();
            this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menu Menu = new Menu(1);
            this.Visible = false;
            Menu.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tables TablesAdministrator = new Tables(1); 
            this.Visible = false;
            TablesAdministrator.ShowDialog();
            this.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Workers Workers = new Workers();
            this.Visible = false;
            Workers.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Orders Orders = new Orders(1);
            this.Visible = false;
            Orders.ShowDialog();
            this.Visible = true;
        }
    }
}
