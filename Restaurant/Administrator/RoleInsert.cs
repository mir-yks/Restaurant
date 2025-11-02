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
        private string mode; 
        public int RoleId { get; set; }
        public string RoleName
        {
            get => textBoxRole.Text;
            set => textBoxRole.Text = value;
        }
        public RoleInsert(string mode)
        {
            InitializeComponent();
            this.mode = mode;

            labelName.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxRole.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);

            ApplyMode();
        }

        private void ApplyMode()
        {
            if (mode == "add")
            {
                textBoxRole.Text = "";
                buttonWrite.Text = "Добавить";
                this.Text = "Добавление роли";
            }
            else if (mode == "edit")
            {
                buttonWrite.Text = "Обновить";
                this.Text = "Редактирование роли";
            }
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

                    string checkQuery = "";
                    MySqlCommand checkCmd;

                    if (mode == "add")
                    {
                        checkQuery = "SELECT COUNT(*) FROM role WHERE LOWER(RoleName) = LOWER(@name)";
                        checkCmd = new MySqlCommand(checkQuery, con);
                        checkCmd.Parameters.AddWithValue("@name", roleName);
                    }
                    else if (mode == "edit")
                    {
                        checkQuery = "SELECT COUNT(*) FROM role WHERE LOWER(RoleName) = LOWER(@name) AND RoleId != @id";
                        checkCmd = new MySqlCommand(checkQuery, con);
                        checkCmd.Parameters.AddWithValue("@name", roleName);
                        checkCmd.Parameters.AddWithValue("@id", RoleId);
                    }
                    else
                    {
                        return;
                    }

                    int duplicateCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (duplicateCount > 0)
                    {
                        MessageBox.Show($"Роль с названием \"{roleName}\" уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult confirm = MessageBox.Show(
                        "Вы действительно хотите сохранить запись?",
                        "Подтверждение",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (confirm != DialogResult.Yes) return;

                    if (mode == "add")
                    {
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO role (RoleName) VALUES (@name)", con);
                        cmd.Parameters.AddWithValue("@name", roleName);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Роль \"{roleName}\" успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        MySqlCommand cmd = new MySqlCommand("UPDATE role SET RoleName = @name WHERE RoleId = @id", con);
                        cmd.Parameters.AddWithValue("@name", roleName);
                        cmd.Parameters.AddWithValue("@id", RoleId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Данные роли успешно обновлены!\nНаименование: \"{roleName}\"", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

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
