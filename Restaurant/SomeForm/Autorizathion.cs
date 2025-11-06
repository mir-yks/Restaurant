using System;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Restaurant
{
    public partial class Autorizathion : Form
    {
        private bool passwordVisible = false;
        public Autorizathion()
        {
            InitializeComponent();

            labelLogin.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxLogin.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxPassword.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonEnter.Font = Fonts.MontserratAlternatesBold(12f);

            textBoxPassword.UseSystemPasswordChar = false; 
            textBoxPassword.PasswordChar = '*';    
        }
        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите выйти?", "Подтверждение выхода", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxLogin.Text) || string.IsNullOrEmpty(textBoxPassword.Text))
                {
                    MessageBox.Show("Введите логин и пароль для входа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string login = textBoxLogin.Text;
                string passwd = textBoxPassword.Text;

                string hash_pass;
                using (var sha256 = SHA256.Create())
                {
                    var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwd));
                    hash_pass = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }

                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT WorkerPassword, WorkerRole, WorkerFIO, WorkerId FROM Worker WHERE WorkerLogin = @login;", con);
                    cmd.Parameters.AddWithValue("@login", login);

                    DataTable dt = new DataTable();
                    new MySqlDataAdapter(cmd).Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Пользователя с таким логином не существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBoxLogin.Clear();
                        textBoxPassword.Clear();
                        return;
                    }

                    string passwordHashInDB = dt.Rows[0]["WorkerPassword"].ToString();
                    int userRole = Convert.ToInt32(dt.Rows[0]["WorkerRole"]);
                    string workerFIO = dt.Rows[0]["WorkerFIO"].ToString();


                    if (hash_pass != passwordHashInDB)
                    {
                        MessageBox.Show("Введен неверный пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBoxPassword.Clear();
                        return;
                    }

                    DatabaseCleanup.CleanExpiredBookings();
                    TableStatusUpdater.UpdateTablesStatus();

                    string roleName = "";
                    switch (userRole)
                    {
                        case 1: roleName = "Администратор"; break;
                        case 2: roleName = "Менеджер"; break;
                        case 3: roleName = "Официант"; break;
                        case 4: roleName = "Шеф-повар"; break;
                    }

                    int userID = Convert.ToInt32(dt.Rows[0]["WorkerId"]);
                    Form nextForm = new Desktop(workerFIO, userRole, roleName, userID);

                    this.Visible = false;
                    nextForm.ShowDialog();
                    textBoxLogin.Clear();
                    textBoxPassword.Clear();
                    this.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z0-9@._-]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z0-9!@#$%^&*()\-_=+\[\]{}|;:,.<>?]$"))
            {
                e.Handled = true;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            passwordVisible = !passwordVisible;

            if (passwordVisible)
            {
                textBoxPassword.PasswordChar = '\0'; 
                pictureBox.BackgroundImage = Properties.Resources.eye;
            }
            else
            {
                textBoxPassword.PasswordChar = '*';
                pictureBox.BackgroundImage = Properties.Resources.eye_closed;
            }
        }
    }
}
