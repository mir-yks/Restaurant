using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class Tables : Form
    {
        private int roleId;
        private DataTable tablesTable;

        public Tables(int role)
        {
            InitializeComponent();
            roleId = role;
            ConfigureButtons();

            labelTable.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPlaceCount.Font = Fonts.MontserratAlternatesRegular(14f);
            labelStatus.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxTable.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxPlaceCount.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxStatus.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonNew.Font = Fonts.MontserratAlternatesBold(12f);
            buttonUpdate.Font = Fonts.MontserratAlternatesBold(12f);
            buttonDelete.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }

        private void ConfigureButtons()
        {
            if (roleId == 1)
            {
                buttonNew.Visible = true;
                buttonUpdate.Visible = true;
                buttonDelete.Visible = true;
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            TablesInsert TablesInsert = new TablesInsert("add");
            TablesInsert.ShowDialog();

            LoadTables();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            DataGridViewRow row = dataGridView1.CurrentRow;
            TablesInsert TablesInsert = new TablesInsert("edit")
            {
                TableID = Convert.ToInt32(row.Cells["Номер столика"].Value),
                TablePlaces = Convert.ToInt32(row.Cells["Количество мест"].Value),
                TableStatus = row.Cells["Статус столика"].Value.ToString()
            };

            TablesInsert.ShowDialog();
            LoadTables();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Tables_Load(object sender, EventArgs e)
        {
            LoadTables();
        }

        private void LoadTables()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                        SELECT 
                            TablesId AS 'Номер столика',
                            TablesCountPlace AS 'Количество мест',
                            TablesStatus AS 'Статус столика'
                        FROM Tables;", con);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    tablesTable = new DataTable();
                    da.Fill(tablesTable);
                    dataGridView1.DataSource = tablesTable;

                    labelTotal.Text = $"Всего: {tablesTable.Rows.Count}";

                    comboBoxPlaceCount.Items.Clear();
                    comboBoxPlaceCount.Items.Add("");
                    MySqlCommand cmdSeats = new MySqlCommand("SELECT DISTINCT TablesCountPlace FROM Tables ORDER BY TablesCountPlace;", con);
                    MySqlDataReader reader = cmdSeats.ExecuteReader();
                    while (reader.Read())
                        comboBoxPlaceCount.Items.Add(reader.GetInt32(0).ToString());
                    reader.Close();

                    comboBoxStatus.Items.Clear();
                    comboBoxStatus.Items.Add("");
                    MySqlCommand cmdStatus = new MySqlCommand("SELECT DISTINCT TablesStatus FROM Tables;", con);
                    MySqlDataReader reader2 = cmdStatus.ExecuteReader();
                    while (reader2.Read())
                        comboBoxStatus.Items.Add(reader2.GetString(0));
                    reader2.Close();

                    comboBoxPlaceCount.SelectedIndex = 0;
                    comboBoxStatus.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void textBoxTable_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (tablesTable == null) return;

            string searchText = textBoxTable.Text.Trim().Replace("'", "''");
            string selectedSeats = comboBoxPlaceCount.SelectedItem?.ToString() ?? "";
            string selectedStatus = comboBoxStatus.SelectedItem?.ToString() ?? "";

            DataView view = new DataView(tablesTable);
            string filter = "";

            if (!string.IsNullOrEmpty(searchText))
            {
                filter = $"Convert([Номер столика], 'System.String') LIKE '%{searchText}%'";
            }

            if (!string.IsNullOrEmpty(selectedSeats))
            {
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"[Количество мест] = {selectedSeats}";
            }

            if (!string.IsNullOrEmpty(selectedStatus))
            {
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"[Статус столика] = '{selectedStatus}'";
            }

            view.RowFilter = filter;
            dataGridView1.DataSource = view;

            labelTotal.Text = $"Всего: {view.Count}";
        }

        private void buttonClearFilters_Click(object sender, EventArgs e)
        {
            textBoxTable.Text = "";
            comboBoxPlaceCount.SelectedIndex = 0;
            comboBoxStatus.SelectedIndex = 0;

            if (tablesTable != null)
            {
                DataView view = new DataView(tablesTable);
                view.RowFilter = "";
                dataGridView1.DataSource = view;

                labelTotal.Text = $"Всего: {view.Count}";
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int selectedTable = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Номер столика"].Value);
            DialogResult result = MessageBox.Show($"Удалить столик №{selectedTable}?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM Tables WHERE TablesId = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", selectedTable);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show($"Столик №{selectedTable} успешно удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTables();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить запись — возможно, столик уже удалён.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void textBoxTable_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[0-9]$"))
            {
                e.Handled = true;
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
