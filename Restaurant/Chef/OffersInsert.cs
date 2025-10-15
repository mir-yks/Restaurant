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
    public partial class OffersInsert : Form
    {
        private string mode;
        public int OfferID { get; set; }
        public string OfferName
        {
            get => textBoxName.Text;
            set => textBoxName.Text = value;
        }
        public decimal OfferDiscount
        {
            get => decimal.TryParse(textBoxDiscount.Text, out decimal d) ? d : 0;
            set => textBoxDiscount.Text = value.ToString("0.##");
        }
        public OffersInsert(string mode)
        {
            InitializeComponent();
            this.mode = mode;

            labelName.Font = Fonts.MontserratAlternatesRegular(14f);
            labelDickount.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxName.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxDiscount.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);

            ApplyMode();
        }

        private void ApplyMode()
        {
            if (mode == "add")
            {
                textBoxName.Text = "";
                textBoxDiscount.Text = "0";
                buttonWrite.Text = "Добавить";
                this.Text = "Добавление предложения";
            }
            else if (mode == "edit")
            {
                buttonWrite.Text = "Обновить";
                this.Text = "Редактирование предложения";
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Введите название предложения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO OffersDish (OffersDishName, OffersDishDicsount) VALUES (@name, @discount)", con);
                        cmd.Parameters.AddWithValue("@name", textBoxName.Text.Trim());
                        cmd.Parameters.AddWithValue("@discount", OfferDiscount);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Предложение \"{textBoxName.Text}\" успешно добавлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        MySqlCommand cmd = new MySqlCommand("UPDATE OffersDish SET OffersDishName = @name, OffersDishDicsount = @discount WHERE OffersDishId = @id", con);
                        cmd.Parameters.AddWithValue("@name", textBoxName.Text.Trim());
                        cmd.Parameters.AddWithValue("@discount", OfferDiscount);
                        cmd.Parameters.AddWithValue("@id", OfferID);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Предложение успешно обновлено!\nНазвание: \"{textBoxName.Text}\"", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-\s]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[0-9,]$"))
            {
                e.Handled = true;
            }
        }
    }
}
