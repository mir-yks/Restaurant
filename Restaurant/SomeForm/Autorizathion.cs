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

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox2.Font = Fonts.MontserratAlternatesRegular(14f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);

            textBox2.UseSystemPasswordChar = false; 
            textBox2.PasswordChar = '*';    
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Введите логин и пароль для входа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string login = textBox1.Text;
                string passwd = textBox2.Text;

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
                        "SELECT WorkerPassword, WorkerRole, WorkerFIO FROM Worker WHERE WorkerLogin = @login;", con);
                    cmd.Parameters.AddWithValue("@login", login);

                    DataTable dt = new DataTable();
                    new MySqlDataAdapter(cmd).Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Пользователя с таким логином не существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox1.Clear();
                        textBox2.Clear();
                        return;
                    }

                    string passwordHashInDB = dt.Rows[0]["WorkerPassword"].ToString();
                    int userRole = Convert.ToInt32(dt.Rows[0]["WorkerRole"]);
                    string workerFIO = dt.Rows[0]["WorkerFIO"].ToString();


                    if (hash_pass != passwordHashInDB)
                    {
                        MessageBox.Show("Введен неверный пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox2.Clear();
                        return;
                    }

                    string roleName = "";
                    switch (userRole)
                    {
                        case 1: roleName = "Администратор"; break;
                        case 2: roleName = "Менеджер"; break;
                        case 3: roleName = "Официант"; break;
                        case 4: roleName = "Шеф-повар"; break;
                    }

                    Form nextForm = new Desktop(workerFIO, userRole, roleName);

                    this.Visible = false;
                    nextForm.ShowDialog();
                    textBox1.Clear();
                    textBox2.Clear();
                    this.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            string allowedSymbols = "!@#$%^&*()-_=+[]{}|;:'\",.<>?/\\`~"; 
            if (!char.IsControl(e.KeyChar) &&
                !char.IsWhiteSpace(e.KeyChar) &&
                (e.KeyChar < 'A' || e.KeyChar > 'z') &&
                (e.KeyChar < '0' || e.KeyChar > '9') &&
                !allowedSymbols.Contains(e.KeyChar.ToString())) 
            {
                e.Handled = true;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            passwordVisible = !passwordVisible;

            if (passwordVisible)
            {
                textBox2.PasswordChar = '\0'; 
                pictureBox2.BackgroundImage = Properties.Resources.eye;
            }
            else
            {
                textBox2.PasswordChar = '*';
                pictureBox2.BackgroundImage = Properties.Resources.eye_closed;
            }
        }


    }
}
