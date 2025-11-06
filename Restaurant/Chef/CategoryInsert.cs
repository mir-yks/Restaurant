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
    public partial class CategoryInsert : Form
    {
        public CategoryInsert()
        {
            InitializeComponent();

            labelName.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxCategory.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);
        }
        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            string categoryName = textBoxCategory.Text.Trim();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("Введите название категории!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxCategory.Focus();
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM CategoryDish WHERE CategoryDishName = @name", con);
                    checkCmd.Parameters.AddWithValue("@name", categoryName);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Категория с таким названием уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult confirm = MessageBox.Show("Вы действительно хотите сохранить запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm != DialogResult.Yes) return;

                    MySqlCommand cmd = new MySqlCommand("INSERT INTO CategoryDish (CategoryDishName) VALUES (@name)", con);
                    cmd.Parameters.AddWithValue("@name", categoryName);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Категория \"{categoryName}\" успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-\s]$"))
            {
                e.Handled = true;
            }
        }
    }
}