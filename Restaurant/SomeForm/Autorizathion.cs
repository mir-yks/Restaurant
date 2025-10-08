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
        string connectionStr = @"host=localhost;
                                uid=root;
                                pwd=;
                                database=restaurant;";

        public Autorizathion()
        {
            InitializeComponent();

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox2.Font = Fonts.MontserratAlternatesRegular(14f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);

            StyleControl(textBox1);
            StyleControl(textBox2);
            StyleControl(checkBox1);

        }
        private void StyleControl(Control ctrl)
        {
            if (ctrl is TextBox txt)
            {
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.BackColor = ColorTranslator.FromHtml("#393C46");
                txt.ForeColor = Color.White;

            }
            else if (ctrl is CheckBox chk)
            {
                chk.FlatStyle = FlatStyle.Flat;
                chk.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#61636B");
                chk.FlatAppearance.BorderSize = 1;
                chk.FlatAppearance.CheckedBackColor = ColorTranslator.FromHtml("#61636B");
                chk.BackColor = ColorTranslator.FromHtml("#393C46");
            }
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

                using (MySqlConnection con = new MySqlConnection(connectionStr))
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

                    // Всегда открываем одну форму Desktop, передавая данные
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !checkBox1.Checked;
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
    }
}
