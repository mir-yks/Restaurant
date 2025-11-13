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
    public partial class Offers : Form
    {
        private DataTable offersTable;
        public Offers()
        {
            InitializeComponent();

            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonNew.Font = Fonts.MontserratAlternatesBold(12f);
            buttonUpdate.Font = Fonts.MontserratAlternatesBold(12f);
            buttonDelete.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["OffersDishId"].Value);
            string name = dataGridView1.CurrentRow.Cells["Акция"].Value.ToString();
            decimal discount = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Скидка"].Value);

            OffersInsert OfferInsert = new OffersInsert("edit")
            {
                OfferID = id,
                OfferName = name,
                OfferDiscount = discount
            };

            OfferInsert.ShowDialog();
            LoadOffers();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            OffersInsert OfferInsert = new OffersInsert("add");
            OfferInsert.ShowDialog();

            LoadOffers();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Offers_Load(object sender, EventArgs e)
        {
            LoadOffers();
        }

        private void LoadOffers()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT OffersDishId, OffersDishName AS 'Акция', OffersDishDicsount AS 'Скидка' FROM OffersDish",
                        con);
                    offersTable = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(offersTable);
                    dataGridView1.DataSource = offersTable;

                    if (dataGridView1.Columns.Contains("OffersDishId"))
                        dataGridView1.Columns["OffersDishId"].Visible = false;

                    labelTotal.Text = $"Всего: {offersTable.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["OffersDishId"].Value);
            string name = dataGridView1.CurrentRow.Cells["Акция"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить акцию \"{name}\"?\n\n" +
                $"У связанных блюд будет автоматически удалена привязка к этой акции.",
                "Удаление акции",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand clearCmd = new MySqlCommand(
                        "UPDATE MenuDish SET OffersDish = NULL WHERE OffersDish = @id",
                        con);
                    clearCmd.Parameters.AddWithValue("@id", id);
                    clearCmd.ExecuteNonQuery();

                    MySqlCommand deleteCmd = new MySqlCommand(
                        "DELETE FROM OffersDish WHERE OffersDishId = @id",
                        con);
                    deleteCmd.Parameters.AddWithValue("@id", id);
                    deleteCmd.ExecuteNonQuery();

                    MessageBox.Show($"Акция \"{name}\" успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOffers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                buttonUpdate.Enabled = true;
                buttonDelete.Enabled = true;
            }
        }
    }
}