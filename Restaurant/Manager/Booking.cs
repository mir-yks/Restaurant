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

            DateTime bookingDate = Convert.ToDateTime(row.Cells["Дата брони"].Value);

            if (!CanEditBooking(bookingDate))
            {
                MessageBox.Show("Невозможно редактировать бронирование!\nРедактирование запрещено за 1 час до брони и после её начала.",
                              "Редактирование запрещено",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            ВookingInsert bookingInsert = new ВookingInsert("edit")
            {
                BookingID = Convert.ToInt32(row.Cells["ID"].Value),
                ClientName = row.Cells["Клиент"].Value.ToString(),
                BookingDate = bookingDate,
                ClientsCount = Convert.ToInt32(row.Cells["Количество гостей"].Value),
                SelectedTableId = Convert.ToInt32(row.Cells["TableId"].Value)
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

            LoadBookings();
        }

        private void LoadBookings()
        {
            DatabaseCleanup.CleanExpiredBookings();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                    b.BookingId AS 'ID',
                                                    COALESCE(c.OriginalClientFIO, c.ClientFIO) AS 'Клиент',
                                                    b.BookingDate AS 'Дата брони',
                                                    b.ClientsCount AS 'Количество гостей',
                                                    b.TableId AS 'TableId',
                                                    t.TablesCountPlace AS 'Вместимость стола',
                                                    CONCAT('Стол №', b.TableId, ' (', t.TablesCountPlace, ' чел.)') AS 'Столик',
                                                    TIMESTAMPDIFF(MINUTE, NOW(), b.BookingDate) as MinutesUntil
                                                FROM booking b 
                                                JOIN client c ON b.ClientId = c.ClientId
                                                JOIN tables t ON b.TableId = t.TablesId
                                                ORDER BY b.BookingDate DESC;", con);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    bookingTable = new DataTable();
                    da.Fill(bookingTable);
                    dataGridView1.DataSource = bookingTable;

                    if (dataGridView1.Columns.Contains("ID"))
                        dataGridView1.Columns["ID"].Visible = false;
                    if (dataGridView1.Columns.Contains("TableId"))
                        dataGridView1.Columns["TableId"].Visible = false;
                    if (dataGridView1.Columns.Contains("Вместимость стола"))
                        dataGridView1.Columns["Вместимость стола"].Visible = false;
                    if (dataGridView1.Columns.Contains("MinutesUntil"))
                        dataGridView1.Columns["MinutesUntil"].Visible = false;

                    if (dataGridView1.Columns.Contains("Дата брони"))
                    {
                        dataGridView1.Columns["Дата брони"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                    }

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

            DataGridViewRow row = dataGridView1.CurrentRow;

            int selectedBookingId = Convert.ToInt32(row.Cells["ID"].Value);
            string clientName = row.Cells["Клиент"].Value.ToString();
            string bookingDateStr = row.Cells["Дата брони"].Value.ToString();
            string guestsCount = row.Cells["Количество гостей"].Value.ToString();
            string tableInfo = row.Cells["Столик"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить бронирование?\n\nКлиент: {clientName}\nДата: {bookingDateStr}\nГости: {guestsCount}\nСтолик: {tableInfo}",
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
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                buttonUpdate.Enabled = true;
                buttonDelete.Enabled = true;

            }
        }

        private bool CanEditBooking(DateTime bookingDate)
        {
            TimeSpan timeUntilBooking = bookingDate - DateTime.Now;

            if (timeUntilBooking.TotalMinutes <= 60 && timeUntilBooking.TotalMinutes > -30)
            {
                return false;
            }

            return true;
        }
    }
}