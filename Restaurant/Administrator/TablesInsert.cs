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
    public partial class TablesInsert : Form
    {
        private string mode;
        public int TableID { get; set; }
        public TablesInsert(string mode)
        {
            InitializeComponent();
            this.mode = mode;
            InactivityManager.Init();

            labelPlaceCount.Font = Fonts.MontserratAlternatesRegular(14f);
            labelStatus.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxStatus.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxPlaceCount.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);

            LoadStatuses();
            ApplyMode();
        }
        private void LoadStatuses()
        {
            comboBoxStatus.Items.Clear();
            comboBoxStatus.Items.AddRange(new string[] { "Свободен", "Забронирован", "Занят" });
        }

        private void ApplyMode()
        {
            switch (mode)
            {
                case "add":
                    textBoxPlaceCount.Text = "";
                    comboBoxStatus.Text = "Свободен";
                    break;
            }
        }
        public int TablePlaces
        {
            get => int.TryParse(textBoxPlaceCount.Text, out int n) ? n : 0;
            set => textBoxPlaceCount.Text = value.ToString();
        }

        public string TableStatus
        {
            get => comboBoxStatus.Text;
            set => comboBoxStatus.Text = value;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы действительно хотите сохранить запись?",
                "Подтверждение записи",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            if (string.IsNullOrWhiteSpace(textBoxPlaceCount.Text))
            {
                MessageBox.Show("Введите количество мест столика!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPlaceCount.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(comboBoxStatus.Text))
            {
                MessageBox.Show("Введите статус столика!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxStatus.Focus();
                return;
            }

            if (!int.TryParse(textBoxPlaceCount.Text, out int places) || places <= 0)
            {
                MessageBox.Show("Введите корректное количество мест (положительное число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd;

                    if (mode == "add")
                    {
                        MySqlCommand cmdNext = new MySqlCommand("SELECT IFNULL(MAX(TablesId),0)+1 FROM Tables;", con);
                        int nextNumber = Convert.ToInt32(cmdNext.ExecuteScalar());

                        cmd = new MySqlCommand(@"INSERT INTO Tables 
                    (TablesId, TablesCountPlace, TablesStatus)
                    VALUES (@Number, @Places, 'Свободен')", con);

                        cmd.Parameters.AddWithValue("@Number", nextNumber);
                        cmd.Parameters.AddWithValue("@Places", places);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Столик №{nextNumber} успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        cmd = new MySqlCommand(@"UPDATE Tables
                    SET TablesCountPlace = @Places, TablesStatus = @Status
                    WHERE TablesId = @Id", con);

                        cmd.Parameters.AddWithValue("@Places", places);
                        cmd.Parameters.AddWithValue("@Status", comboBoxStatus.Text);
                        cmd.Parameters.AddWithValue("@Id", TableID);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Данные столика успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Не удалось обновить данные. Возможно, столик не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[0-9]$"))
            {
                e.Handled = true;
            }
        }
    }
}
