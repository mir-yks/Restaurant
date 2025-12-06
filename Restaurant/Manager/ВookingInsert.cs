using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            InitializeDateTimePickers();

            labelClient.Font = Fonts.MontserratAlternatesRegular(14f);
            labelDateBooking.Font = Fonts.MontserratAlternatesRegular(14f);
            labelClientsCount.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTable.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxClientsCount.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxTable.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxClient.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonArrange.Font = Fonts.MontserratAlternatesBold(12f);

            LoadClients();
            ApplyMode();
        }

        private void InitializeDateTimePickers()
        {
            foreach (Control control in this.Controls)
            {
                if (control is DateTimePicker)
                {
                    var picker = control as DateTimePicker;
                    if (picker.Name == "datePicker")
                    {
                        datePicker = picker;
                        datePicker.MinDate = DateTime.Today;
                        datePicker.MaxDate = DateTime.Today.AddDays(14);
                        datePicker.Value = DateTime.Today;
                    }
                    else if (picker.Name == "timePicker")
                    {
                        timePicker = picker;

                        DateTime now = DateTime.Now;
                        int minutes = now.Minute;
                        int roundedMinutes = (minutes / 30) * 30;
                        timePicker.Value = new DateTime(now.Year, now.Month, now.Day, now.Hour, roundedMinutes, 0);
                    }
                }
            }
        }

        private DateTime CombineDateAndTime()
        {
            return new DateTime(
                datePicker.Value.Year,
                datePicker.Value.Month,
                datePicker.Value.Day,
                timePicker.Value.Hour,
                timePicker.Value.Minute,
                0
            );
        }

        private void TextBoxClientsCount_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxClientsCount.Text, out int clientsCount) && clientsCount > 0)
            {
                LoadTablesByCapacity(clientsCount);
            }
            else
            {
                comboBoxTable.Items.Clear();
            }
        }

        private void LoadTablesByCapacity(int minCapacity)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    string query = @"SELECT TablesId, TablesCountPlace 
                                   FROM Tables 
                                   WHERE TablesCountPlace >= @MinCapacity 
                                   AND (TablesStatus = 'Свободен' OR TablesId = @CurrentTableId)
                                   ORDER BY TablesCountPlace ASC, TablesId ASC";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@MinCapacity", minCapacity);

                    if (mode == "edit" && SelectedTableId > 0)
                    {
                        cmd.Parameters.AddWithValue("@CurrentTableId", SelectedTableId);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CurrentTableId", 0);
                    }

                    MySqlDataReader reader = cmd.ExecuteReader();

                    comboBoxTable.Items.Clear();
                    while (reader.Read())
                    {
                        int tableId = reader.GetInt32("TablesId");
                        int capacity = reader.GetInt32("TablesCountPlace");
                        comboBoxTable.Items.Add(new KeyValuePair<int, string>(
                            tableId,
                            $"Стол №{tableId} ({capacity} чел.)"
                        ));
                    }
                    reader.Close();

                    comboBoxTable.DisplayMember = "Value";
                    comboBoxTable.ValueMember = "Key";

                    if (mode == "edit" && SelectedTableId > 0)
                    {
                        SelectedTableId = SelectedTableId;
                    }
                    else if (comboBoxTable.Items.Count > 0)
                    {
                        comboBoxTable.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show($"Нет свободных столиков, вмещающих {minCapacity} и более гостей.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки столиков: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadClients()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    string query = mode == "add"
                        ? "SELECT ClientId, ClientFIO FROM client WHERE IsActive = 1 ORDER BY ClientFIO"
                        : "SELECT ClientId, COALESCE(OriginalClientFIO, ClientFIO) as ClientFIO FROM client WHERE IsActive = 1 ORDER BY ClientFIO";

                    MySqlCommand cmd = new MySqlCommand(query, con);
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
                case "edit":
                    buttonArrange.Text = "Сохранить";
                    comboBoxClient.Enabled = false;
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
            get => CombineDateAndTime();
            set
            {
                if (datePicker != null) datePicker.Value = value.Date;
                if (timePicker != null) timePicker.Value = value;
            }
        }

        public int ClientsCount
        {
            get
            {
                if (int.TryParse(textBoxClientsCount.Text, out int count))
                    return count;
                return 0;
            }
            set
            {
                textBoxClientsCount.Text = value.ToString();
                if (value > 0)
                {
                    LoadTablesByCapacity(value);
                }
            }
        }

        public int SelectedTableId
        {
            get
            {
                if (comboBoxTable.SelectedItem != null)
                    return ((KeyValuePair<int, string>)comboBoxTable.SelectedItem).Key;
                return 0;
            }
            set
            {
                foreach (KeyValuePair<int, string> item in comboBoxTable.Items)
                {
                    if (item.Key == value)
                    {
                        comboBoxTable.SelectedItem = item;
                        break;
                    }
                }
            }
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

            if (!int.TryParse(textBoxClientsCount.Text, out int clientsCount) || clientsCount <= 0)
            {
                MessageBox.Show("Введите корректное количество гостей!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxClientsCount.Focus();
                return;
            }

            if (comboBoxTable.SelectedItem == null)
            {
                MessageBox.Show("Выберите подходящий столик!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxTable.Focus();
                return;
            }

            DateTime selectedDateTime = CombineDateAndTime();
            if (selectedDateTime < DateTime.Now)
            {
                MessageBox.Show("Нельзя выбрать прошедшую дату и время!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                datePicker?.Focus();
                return;
            }

            if (selectedDateTime.Date > DateTime.Today.AddDays(14))
            {
                MessageBox.Show("Бронь возможна только на ближайшие 2 недели!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                datePicker?.Focus();
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    int clientId = ((KeyValuePair<int, string>)comboBoxClient.SelectedItem).Key;
                    int tableId = ((KeyValuePair<int, string>)comboBoxTable.SelectedItem).Key;

                    MySqlCommand checkCmd;
                    if (mode == "add")
                    {
                        checkCmd = new MySqlCommand(
                            "SELECT COUNT(*) FROM booking WHERE ClientId = @ClientId AND BookingDate = @BookingDate",
                            con);
                    }
                    else
                    {
                        checkCmd = new MySqlCommand(
                            "SELECT COUNT(*) FROM booking WHERE ClientId = @ClientId AND BookingDate = @BookingDate AND BookingId != @BookingId",
                            con);
                        checkCmd.Parameters.AddWithValue("@BookingId", BookingID);
                    }

                    checkCmd.Parameters.AddWithValue("@ClientId", clientId);
                    checkCmd.Parameters.AddWithValue("@BookingDate", selectedDateTime);

                    int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (existingCount > 0)
                    {
                        MessageBox.Show("Этот клиент уже забронировал столик на выбранное время!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MySqlCommand checkTimeIntervalCmd;
                    if (mode == "add")
                    {
                        checkTimeIntervalCmd = new MySqlCommand(@"
                    SELECT COUNT(*) FROM booking 
                    WHERE TableId = @TableId 
                    AND ABS(TIMESTAMPDIFF(MINUTE, BookingDate, @BookingDate)) < 120
                    AND BookingId != @BookingId", con);
                        checkTimeIntervalCmd.Parameters.AddWithValue("@BookingId", 0);
                    }
                    else
                    {
                        checkTimeIntervalCmd = new MySqlCommand(@"
                    SELECT COUNT(*) FROM booking 
                    WHERE TableId = @TableId 
                    AND ABS(TIMESTAMPDIFF(MINUTE, BookingDate, @BookingDate)) < 120
                    AND BookingId != @BookingId", con);
                        checkTimeIntervalCmd.Parameters.AddWithValue("@BookingId", BookingID);
                    }

                    checkTimeIntervalCmd.Parameters.AddWithValue("@TableId", tableId);
                    checkTimeIntervalCmd.Parameters.AddWithValue("@BookingDate", selectedDateTime);

                    int conflictingBookings = Convert.ToInt32(checkTimeIntervalCmd.ExecuteScalar());
                    if (conflictingBookings > 0)
                    {
                        MessageBox.Show("Между бронями на один столик должен быть интервал не менее 2 часов.",
                                      "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MySqlCommand checkTableCmd;
                    if (mode == "add")
                    {
                        checkTableCmd = new MySqlCommand(
                            "SELECT COUNT(*) FROM booking WHERE TableId = @TableId AND BookingDate = @BookingDate",
                            con);
                    }
                    else
                    {
                        checkTableCmd = new MySqlCommand(
                            "SELECT COUNT(*) FROM booking WHERE TableId = @TableId AND BookingDate = @BookingDate AND BookingId != @BookingId",
                            con);
                        checkTableCmd.Parameters.AddWithValue("@BookingId", BookingID);
                    }

                    checkTableCmd.Parameters.AddWithValue("@TableId", tableId);
                    checkTableCmd.Parameters.AddWithValue("@BookingDate", selectedDateTime);

                    int tableBookedCount = Convert.ToInt32(checkTableCmd.ExecuteScalar());
                    if (tableBookedCount > 0)
                    {
                        MessageBox.Show("Этот столик уже забронирован на выбранное время!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult result = MessageBox.Show(
                        $"Вы действительно хотите {(mode == "add" ? "забронировать" : "сохранить изменения")}?\n\n" +
                        $"Клиент: {((KeyValuePair<int, string>)comboBoxClient.SelectedItem).Value}\n" +
                        $"Дата: {selectedDateTime:dd.MM.yyyy}\n" +
                        $"Время: {selectedDateTime:HH:mm}\n" +
                        $"Количество гостей: {clientsCount}\n" +
                        $"Столик: {((KeyValuePair<int, string>)comboBoxTable.SelectedItem).Value}",
                        mode == "add" ? "Подтверждение бронирования" : "Подтверждение изменений",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result != DialogResult.Yes) return;

                    MySqlCommand cmd;
                    if (mode == "add")
                    {
                        cmd = new MySqlCommand(
                            "INSERT INTO booking (ClientId, BookingDate, ClientsCount, TableId) VALUES (@ClientId, @BookingDate, @ClientsCount, @TableId)",
                            con);
                    }
                    else
                    {
                        cmd = new MySqlCommand(
                            "UPDATE booking SET ClientId = @ClientId, BookingDate = @BookingDate, ClientsCount = @ClientsCount, TableId = @TableId WHERE BookingId = @BookingId",
                            con);
                        cmd.Parameters.AddWithValue("@BookingId", BookingID);
                    }

                    cmd.Parameters.AddWithValue("@ClientId", clientId);
                    cmd.Parameters.AddWithValue("@BookingDate", selectedDateTime);
                    cmd.Parameters.AddWithValue("@ClientsCount", clientsCount);
                    cmd.Parameters.AddWithValue("@TableId", tableId);

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
        private void comboBoxClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-\s]$"))
            {
                e.Handled = true;
            }
        }

        private void comboBoxTable_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я0-9№\s]$"))
            {
                e.Handled = true;
            }
        }

        private void comboBoxClient_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxClient.Text)) return;

            int cursorPos = comboBoxClient.SelectionStart;
            string input = comboBoxClient.Text;

            int spaceCount = input.Count(c => c == ' ');
            int dashCount = input.Count(c => c == '-');

            if (spaceCount > 2)
            {
                int spaceCounter = 0;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == ' ')
                    {
                        spaceCounter++;
                        if (spaceCounter <= 2)
                        {
                            sb.Append(input[i]);
                        }
                        else
                        {
                            if (i < cursorPos) cursorPos--;
                        }
                    }
                    else
                    {
                        sb.Append(input[i]);
                    }
                }

                input = sb.ToString();
            }

            if (dashCount > 1)
            {
                bool firstDashFound = false;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '-')
                    {
                        if (!firstDashFound)
                        {
                            sb.Append(input[i]);
                            firstDashFound = true;
                        }
                        else
                        {
                            if (i < cursorPos) cursorPos--;
                        }
                    }
                    else
                    {
                        sb.Append(input[i]);
                    }
                }

                input = sb.ToString();
            }

            string[] parts = input
                .Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Length > 0 ? char.ToUpper(p[0]) + p.Substring(1).ToLower() : "")
                .ToArray();

            string result = input;
            int index = 0;
            foreach (string part in parts)
            {
                if (string.IsNullOrEmpty(part)) continue;

                int pos = result.IndexOf(part, index, StringComparison.OrdinalIgnoreCase);
                if (pos >= 0)
                {
                    result = result.Remove(pos, part.Length).Insert(pos, part);
                    index = pos + part.Length;
                }
            }
            if (comboBoxClient.Text != result)
            {
                comboBoxClient.TextChanged -= comboBoxClient_TextChanged;
                comboBoxClient.Text = result;
                comboBoxClient.SelectionStart = Math.Min(cursorPos, result.Length);
                comboBoxClient.TextChanged += comboBoxClient_TextChanged;
            }
        }
    }
}