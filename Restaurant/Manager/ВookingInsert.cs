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

            dateTimePickerBooking.MinDate = DateTime.Today;
            dateTimePickerBooking.MaxDate = DateTime.Today.AddDays(+14);

            LoadClients();
            SetupDatePicker();
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
            // Проверка: выбран клиент
            if (comboBoxClient.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxClient.Focus();
                return;
            }

            // Проверка: дата выбрана
            DateTime selectedDate = dateTimePickerBooking.Value.Date;
            if (selectedDate < DateTime.Today || selectedDate > DateTime.Today.AddDays(14))
            {
                MessageBox.Show("Выберите корректную дату бронирования (от сегодня до двух недель вперед)!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    int clientId = ((KeyValuePair<int, string>)comboBoxClient.SelectedItem).Key;

                    // Проверка дублирования: клиент + дата
                    MySqlCommand checkCmd;
                    if (mode == "add")
                    {
                        checkCmd = new MySqlCommand(
                            "SELECT COUNT(*) FROM booking WHERE ClientId = @ClientId AND DATE(BookingDate) = @BookingDate",
                            con);
                    }
                    else // edit
                    {
                        checkCmd = new MySqlCommand(
                            "SELECT COUNT(*) FROM booking WHERE ClientId = @ClientId AND DATE(BookingDate) = @BookingDate AND BookingId != @BookingId",
                            con);
                        checkCmd.Parameters.AddWithValue("@BookingId", BookingID);
                    }

                    checkCmd.Parameters.AddWithValue("@ClientId", clientId);
                    checkCmd.Parameters.AddWithValue("@BookingDate", selectedDate);

                    int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (existingCount > 0)
                    {
                        MessageBox.Show("Этот клиент уже бронировал столик на выбранную дату!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Добавление или обновление брони
                    MySqlCommand cmd;
                    if (mode == "add")
                    {
                        cmd = new MySqlCommand("INSERT INTO booking (ClientId, BookingDate) VALUES (@ClientId, @BookingDate)", con);
                    }
                    else
                    {
                        cmd = new MySqlCommand("UPDATE booking SET ClientId = @ClientId, BookingDate = @BookingDate WHERE BookingId = @BookingId", con);
                        cmd.Parameters.AddWithValue("@BookingId", BookingID);
                    }

                    cmd.Parameters.AddWithValue("@ClientId", clientId);
                    cmd.Parameters.AddWithValue("@BookingDate", selectedDate);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show(mode == "add" ? "Бронь успешно добавлена!" : "Бронь успешно обновлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения брони: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SetupDatePicker()
        {
            dateTimePickerBooking.MinDate = DateTime.Today;
            dateTimePickerBooking.MaxDate = DateTime.Today.AddDays(14); 
            dateTimePickerBooking.Value = DateTime.Today;
        }
    }
}
