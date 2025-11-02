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
    public partial class Booking : Form
    {
        private DataTable bookingTable;
        public Booking()
        {
            InitializeComponent();

            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonNew.Font = Fonts.MontserratAlternatesBold(12f);
            buttonUpdate.Font = Fonts.MontserratAlternatesBold(12f);
            buttonDelete.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            ВookingInsert bookingInsert = new ВookingInsert("add");
            bookingInsert.ShowDialog();
            LoadBookings();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            DataGridViewRow row = dataGridView1.CurrentRow;

            ВookingInsert bookingInsert = new ВookingInsert("edit")
            {
                BookingID = Convert.ToInt32(row.Cells["ID"].Value),
                ClientName = row.Cells["Клиент"].Value.ToString(),
                BookingDate = Convert.ToDateTime(row.Cells["Дата брони"].Value)
            };

            bookingInsert.ShowDialog();
            LoadBookings();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Booking_Load(object sender, EventArgs e)
        {
            DatabaseCleanup.CleanExpiredBookings();
            LoadBookings();
        }

        private void LoadBookings()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                        b.BookingId AS 'ID',
                                                        c.ClientFIO AS 'Клиент',
                                                        b.BookingDate AS 'Дата брони'
                                                    FROM booking b 
                                                    JOIN client c ON b.ClientId = c.ClientId
                                                    ORDER BY b.BookingDate DESC;", con);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    bookingTable = new DataTable();
                    da.Fill(bookingTable);
                    dataGridView1.DataSource = bookingTable;

                    if (dataGridView1.Columns.Contains("ID"))
                        dataGridView1.Columns["ID"].Visible = false;

                    labelTotal.Text = $"Всего: {bookingTable.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите бронирование для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedBookingId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string clientName = dataGridView1.CurrentRow.Cells["Клиент"].Value.ToString();
            string bookingDate = dataGridView1.CurrentRow.Cells["Дата брони"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить бронирование?\nКлиент: {clientName}\nДата: {bookingDate}",
                "Удаление бронирования",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM booking WHERE BookingId = @id", con);
                    cmd.Parameters.AddWithValue("@id", selectedBookingId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Бронирование успешно удалено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBookings();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
