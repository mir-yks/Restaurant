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
    public partial class ВookingInsert : Form
    {
        private string mode;
        public int BookingID { get; set; }
        public ВookingInsert(string mode)
        {
            InitializeComponent();
            this.mode = mode;

            labelClient.Font = Fonts.MontserratAlternatesRegular(14f);
            labelDateBooking.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxClient.Font = Fonts.MontserratAlternatesRegular(14f);
            dateTimePickerBooking.Font = Fonts.MontserratAlternatesRegular(12f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonArrange.Font = Fonts.MontserratAlternatesBold(12f);

            LoadClients();
            ApplyMode();
        }

        private void LoadClients()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT ClientId, ClientFIO FROM client ORDER BY ClientFIO", con);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    comboBoxClient.Items.Clear();
                    while (reader.Read())
                    {
                        comboBoxClient.Items.Add(new KeyValuePair<int, string>(
                            reader.GetInt32("ClientId"),
                            reader.GetString("ClientFIO")
                        ));
                    }
                    reader.Close();

                    comboBoxClient.DisplayMember = "Value";
                    comboBoxClient.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyMode()
        {
            switch (mode)
            {
                case "add":
                    comboBoxClient.Enabled = true;
                    dateTimePickerBooking.Enabled = true;
                    buttonArrange.Visible = true;
                    buttonBack.Text = "Отмена";
                    dateTimePickerBooking.Value = DateTime.Now;
                    break;

                case "edit":
                    comboBoxClient.Enabled = true;
                    dateTimePickerBooking.Enabled = true;
                    buttonArrange.Visible = true;
                    buttonBack.Text = "Отмена";
                    break;
            }
        }

        public string ClientName
        {
            set
            {
                foreach (KeyValuePair<int, string> item in comboBoxClient.Items)
                {
                    if (item.Value == value)
                    {
                        comboBoxClient.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public DateTime BookingDate
        {
            get => dateTimePickerBooking.Value;
            set => dateTimePickerBooking.Value = value;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonArrange_Click(object sender, EventArgs e)
        {
            if (comboBoxClient.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxClient.Focus();
                return;
            }

            if (dateTimePickerBooking.Value < DateTime.Now.Date)
            {
                MessageBox.Show("Дата бронирования не может быть в прошлом!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePickerBooking.Focus();
                return;
            }

            DialogResult result = MessageBox.Show(
                "Вы действительно хотите записать бронь?",
                "Подтверждение записи",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    // Получаем ID выбранного клиента
                    int clientId = ((KeyValuePair<int, string>)comboBoxClient.SelectedItem).Key;

                    if (mode == "add")
                    {
                        // Проверяем, нет ли уже брони на эту дату для этого клиента
                        MySqlCommand checkCmd = new MySqlCommand(
                            "SELECT COUNT(*) FROM booking WHERE ClientId = @ClientId AND DATE(BookingDate) = DATE(@BookingDate)",
                            con);
                        checkCmd.Parameters.AddWithValue("@ClientId", clientId);
                        checkCmd.Parameters.AddWithValue("@BookingDate", dateTimePickerBooking.Value);
                        int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (existingCount > 0)
                        {
                            MessageBox.Show("У этого клиента уже есть бронь на выбранную дату!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string query = @"INSERT INTO booking (ClientId, BookingDate) VALUES (@ClientId, @BookingDate)";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@ClientId", clientId);
                        cmd.Parameters.AddWithValue("@BookingDate", dateTimePickerBooking.Value);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Бронь успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        MySqlCommand checkCmd = new MySqlCommand(
                            "SELECT COUNT(*) FROM booking WHERE ClientId = @ClientId AND DATE(BookingDate) = DATE(@BookingDate) AND BookingId != @BookingId",
                            con);
                        checkCmd.Parameters.AddWithValue("@ClientId", clientId);
                        checkCmd.Parameters.AddWithValue("@BookingDate", dateTimePickerBooking.Value);
                        checkCmd.Parameters.AddWithValue("@BookingId", BookingID);
                        int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (existingCount > 0)
                        {
                            MessageBox.Show("У этого клиента уже есть бронь на выбранную дату!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string query = @"UPDATE booking SET ClientId = @ClientId, BookingDate = @BookingDate WHERE BookingId = @BookingId";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@ClientId", clientId);
                        cmd.Parameters.AddWithValue("@BookingDate", dateTimePickerBooking.Value);
                        cmd.Parameters.AddWithValue("@BookingId", BookingID);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Бронь успешно обновлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения брони: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
