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

            labelPlaceCount.Font = Fonts.MontserratAlternatesRegular(14f);
            labelStatus.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxStatus.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxPlaceCount.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);

            ApplyMode();
        }

        private void ApplyMode()
        {
            switch (mode)
            {
                case "add":
                    textBoxPlaceCount.Text = "";
                    comboBoxStatus.Text = "Свободен";
                    comboBoxStatus.Enabled = false; // нельзя менять
                    buttonWrite.Visible = true;
                    buttonBack.Text = "Отмена";
                    break;

                case "edit":
                    textBoxPlaceCount.ReadOnly = false;
                    comboBoxStatus.Enabled = true; // можно менять статус
                    buttonWrite.Visible = true;
                    buttonBack.Text = "Отмена";
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
                        cmd.Parameters.AddWithValue("@Places", TablePlaces);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Столик №{nextNumber} успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        cmd = new MySqlCommand(@"UPDATE Tables
                            SET TablesCountPlace = @Places, TablesStatus = @Status
                            WHERE TablesId = @Id", con);

                        cmd.Parameters.AddWithValue("@Places", TablePlaces);
                        cmd.Parameters.AddWithValue("@Status", TableStatus);
                        cmd.Parameters.AddWithValue("@Id", TableID);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Данные столика успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
