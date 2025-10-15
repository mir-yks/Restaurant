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
        private string mode;
        public int CategoryID { get; set; }
        public string CategoryName
        {
            get => textBoxCategory.Text;
            set => textBoxCategory.Text = value;
        }
        public CategoryInsert(string mode)
        {
            InitializeComponent();
            this.mode = mode;

            labelName.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxCategory.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);

            ApplyMode();
        }

        private void ApplyMode()
        {
            if (mode == "add")
            {
                textBoxCategory.Text = "";
                buttonWrite.Text = "Добавить";
                this.Text = "Добавление категории";
            }
            else if (mode == "edit")
            {
                buttonWrite.Text = "Обновить";
                this.Text = "Редактирование категории";
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxCategory.Text))
            {
                MessageBox.Show("Введите название категории.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Вы действительно хотите сохранить запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    if (mode == "add")
                    {
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO CategoryDish (CategoryDishName) VALUES (@name)", con);
                        cmd.Parameters.AddWithValue("@name", textBoxCategory.Text.Trim());
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Категория \"{textBoxCategory.Text}\" успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        MySqlCommand cmd = new MySqlCommand("UPDATE CategoryDish SET CategoryDishName = @name WHERE CategoryDishId = @id", con);
                        cmd.Parameters.AddWithValue("@name", textBoxCategory.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", CategoryID);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Категория успешно обновлена!\nНазвание: \"{textBoxCategory.Text}\"", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

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
