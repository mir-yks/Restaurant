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
                textBoxDiscount.Text = "1";
                buttonWrite.Text = "Добавить";
            }
            else if (mode == "edit")
            {
                buttonWrite.Text = "Обновить";
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            string offerName = textBoxName.Text.Trim();
            string discountText = textBoxDiscount.Text.Trim();

            if (string.IsNullOrWhiteSpace(offerName))
            {
                MessageBox.Show("Введите название акции!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(discountText))
            {
                MessageBox.Show("Введите скидку!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxDiscount.Focus();
                return;
            }

            if (!decimal.TryParse(discountText.Replace(',', '.'), out decimal discount))
            {
                MessageBox.Show("Введите корректное значение скидки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (discount < 1 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть не меньше 1% и не больше 100%.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand checkCmd;
                    if (mode == "add")
                    {
                        checkCmd = new MySqlCommand("SELECT COUNT(*) FROM OffersDish WHERE OffersDishName = @name", con);
                        checkCmd.Parameters.AddWithValue("@name", offerName);
                    }
                    else
                    {
                        checkCmd = new MySqlCommand("SELECT COUNT(*) FROM OffersDish WHERE OffersDishName = @name AND OffersDishId <> @id", con);
                        checkCmd.Parameters.AddWithValue("@name", offerName);
                        checkCmd.Parameters.AddWithValue("@id", OfferID);
                    }

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Акция с таким названием уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult confirm = MessageBox.Show("Вы действительно хотите сохранить запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm != DialogResult.Yes) return;

                    if (mode == "add")
                    {
                        MySqlCommand cmd = new MySqlCommand(
                            "INSERT INTO OffersDish (OffersDishName, OffersDishDicsount) VALUES (@name, @discount)",
                            con);
                        cmd.Parameters.AddWithValue("@name", offerName);
                        cmd.Parameters.AddWithValue("@discount", discount);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Акция \"{offerName}\" успешно добавлена со скидкой {discount}%!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        MySqlCommand cmd = new MySqlCommand(
                            "UPDATE OffersDish SET OffersDishName = @name, OffersDishDicsount = @discount WHERE OffersDishId = @id",
                            con);
                        cmd.Parameters.AddWithValue("@name", offerName);
                        cmd.Parameters.AddWithValue("@discount", discount);
                        cmd.Parameters.AddWithValue("@id", OfferID);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Акция успешно обновлена!\nНазвание: \"{offerName}\", скидка: {discount}%", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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