using System;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class Desktop : Form
    {
        private string currentFIO;
        private int currentRole;
        private string currentRoleName;
        public Desktop(string fio, int role, string roleName)
        {
            InitializeComponent();
            currentFIO = fio;
            currentRole = role;
            currentRoleName = roleName;

            label4.Text = $"Добро пожаловать,\n\n{currentFIO}!";
            label3.Text = $"Ваша роль: {currentRoleName}";

            ConfigureByRole();
        }
        private void ConfigureByRole()
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;
            button10.Visible = false;

            button4.Visible = true;

            if (currentRole == 1) 
            {
                button3.Visible = true;
                button5.Visible = true;
                button9.Visible = true;

                button5.Location = new System.Drawing.Point(12, 319);
                button9.Location = new System.Drawing.Point(12, 258);

                 this.Text = "Администратор";
            }
            if (currentRole == 2)
            {
                button2.Visible = true;
                button6.Visible = true;
                button10.Visible = true;

                button6.Location = new System.Drawing.Point(12, 380);
                button10.Location = new System.Drawing.Point(12, 258);

                this.Text = "Менеджер";
            }
            if (currentRole == 3)
            {
                button3.Visible = true;
                button1.Visible = true;
                button2.Visible = true;

                this.Text = "Официант";
            }
            if (currentRole == 4)
            {
                button1.Visible = true;
                button7.Visible = true;
                button8.Visible = true;

                button7.Location = new System.Drawing.Point(12, 319);
                button8.Location = new System.Drawing.Point(12, 380);

                this.Text = "Шеф-повар";
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            Role Roles = new Role();
            this.Visible = false;
            Roles.ShowDialog();
            this.Visible = true;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            Booking Booking = new Booking();
            this.Visible = false;
            Booking.ShowDialog();
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
            Menu Menu = new Menu(currentRole);
            this.Visible = false;
            Menu.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tables TablesAdministrator = new Tables(currentRole); 
            this.Visible = false;
            TablesAdministrator.ShowDialog();
            this.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Worker Workers = new Worker();
            this.Visible = false;
            Workers.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Order Orders = new Order(currentRole);
            this.Visible = false;
            Orders.ShowDialog();
            this.Visible = true;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Clients Clients = new Clients();
            this.Visible = false;
            Clients.ShowDialog();
            this.Visible = true;
        }
    }
}
