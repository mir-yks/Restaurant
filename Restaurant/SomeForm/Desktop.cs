using System;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class Desktop : Form
    {
        private string currentFIO;
        private int currentRole;
        private string currentRoleName;
        private int currentUserID;
        public Desktop(string fio, int role, string roleName, int userID)
        {
            InitializeComponent();
            currentFIO = fio;
            currentRole = role;
            currentRoleName = roleName;
            currentUserID = userID;
            InactivityManager.Init();

            labelWelcome.Text = $"Добро пожаловать,\n\n{currentFIO}!";
            labelRole.Text = $"Ваша роль: {currentRoleName}";

            labelRole.Font = Fonts.MontserratAlternatesBold(14f);
            labelWelcome.Font = Fonts.MontserratAlternatesBold(14f);
            buttonMenu.Font = Fonts.MontserratAlternatesBold(12f);
            buttonOrders.Font = Fonts.MontserratAlternatesBold(12f);
            buttonTables.Font = Fonts.MontserratAlternatesBold(12f);
            buttonExit.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWorkers.Font = Fonts.MontserratAlternatesBold(12f);
            buttonClients.Font = Fonts.MontserratAlternatesBold(12f);
            buttonCategory.Font = Fonts.MontserratAlternatesBold(12f);
            buttonOffers.Font = Fonts.MontserratAlternatesBold(12f);
            buttonRoles.Font = Fonts.MontserratAlternatesBold(12f);
            buttonBooking.Font = Fonts.MontserratAlternatesBold(12f);

            ConfigureByRole();
        }
        private void ConfigureByRole()
        {
            if (currentRole == 1) 
            {
                buttonTables.Visible = true;
                buttonWorkers.Visible = true;
                buttonRoles.Visible = true;

                buttonWorkers.Location = new System.Drawing.Point(12, 359);
                buttonRoles.Location = new System.Drawing.Point(12, 298);
            }
            if (currentRole == 2)
            {
                buttonOrders.Visible = true;
                buttonClients.Visible = true;
                buttonBooking.Visible = true;

                buttonClients.Location = new System.Drawing.Point(12, 420);
                buttonBooking.Location = new System.Drawing.Point(12, 298);
            }
            if (currentRole == 3)
            {
                buttonTables.Visible = true;
                buttonMenu.Visible = true;
                buttonOrders.Visible = true;
            }
            if (currentRole == 4)
            {
                buttonMenu.Visible = true;
                buttonCategory.Visible = true;
                buttonOffers.Visible = true;

                buttonCategory.Location = new System.Drawing.Point(12, 359);
                buttonOffers.Location = new System.Drawing.Point(12, 420);
            }
        }
        private void buttonRoles_Click(object sender, EventArgs e)
        {
            Role Roles = new Role();
            this.Visible = false;
            Roles.ShowDialog();
            this.Visible = true;
        }
        private void buttonBooking_Click(object sender, EventArgs e)
        {
            Booking Booking = new Booking();
            this.Visible = false;
            Booking.ShowDialog();
            this.Visible = true;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите выйти?", "Подтверждение выхода", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void buttonOffers_Click(object sender, EventArgs e)
        {
            Offers Offers = new Offers();
            this.Visible = false;
            Offers.ShowDialog();
            this.Visible = true;
        }

        private void buttonCategory_Click(object sender, EventArgs e)
        {
            Category Category = new Category();
            this.Visible = false;
            Category.ShowDialog();
            this.Visible = true;
        }

        private void buttonMenu_Click(object sender, EventArgs e)
        {
            Menu Menu = new Menu(currentRole);
            this.Visible = false;
            Menu.ShowDialog();
            this.Visible = true;
        }

        private void buttonTables_Click(object sender, EventArgs e)
        {
            Tables Tables = new Tables(currentRole); 
            this.Visible = false;
            Tables.ShowDialog();
            this.Visible = true;
        }

        private void buttonWorkers_Click(object sender, EventArgs e)
        {
            Worker Workers = new Worker();
            Workers.CurrentUserID = currentUserID;
            this.Visible = false;
            Workers.ShowDialog();
            this.Visible = true;
        }

        private void buttonOrders_Click(object sender, EventArgs e)
        {
            Order Orders = new Order(currentRole, currentUserID);
            this.Visible = false;
            Orders.ShowDialog();
            this.Visible = true;
        }
        private void buttonClients_Click(object sender, EventArgs e)
        {
            Clients Clients = new Clients();
            this.Visible = false;
            Clients.ShowDialog();
            this.Visible = true;
        }
    }
}
