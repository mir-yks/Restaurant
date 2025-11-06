using MySql.Data.MySqlClient;
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
    public partial class RoleInsert : Form
    {
        public RoleInsert()
        {
            InitializeComponent();

            labelName.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxRole.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);

        }
        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxRole.Text))
            {
                MessageBox.Show("Введите название роли!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string roleName = textBoxRole.Text.Trim();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM role WHERE RoleName = @name", con);
                    checkCmd.Parameters.AddWithValue("@name", roleName);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Роль с таким названием уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult confirm = MessageBox.Show(
                        "Вы действительно хотите сохранить запись?",
                        "Подтверждение",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (confirm != DialogResult.Yes) return;

                    MySqlCommand cmd = new MySqlCommand("INSERT INTO role (RoleName) VALUES (@name)", con);
                    cmd.Parameters.AddWithValue("@name", roleName);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Роль \"{roleName}\" успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxRole_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-\s]$"))
            {
                e.Handled = true;
            }
        }
    }
}