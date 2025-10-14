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

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            label3.Font = Fonts.MontserratAlternatesRegular(14f);
            label4.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox2.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            button3.Font = Fonts.MontserratAlternatesBold(12f);
            button8.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }

        private void ConfigureButtons()
        {
            button2.Visible = false;
            button3.Visible = false;
            button8.Visible = false;
            button1.Visible = true;

            if (roleId == 1)
            {
                button2.Visible = true;
                button3.Visible = true;
                button8.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TablesInsert TablesInsert = new TablesInsert();
            this.Visible = true;
            TablesInsert.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TablesInsert TablesInsert = new TablesInsert();
            this.Visible = true;
            TablesInsert.ShowDialog();
            this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Tables_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(connStr.ConnectionString);
                con.Open();

                MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                        TablesId AS 'Номер столика',
                                                        TablesCountPlace AS 'Количество мест',
                                                        TablesStatus AS 'Статус столика'
                                                    FROM Tables;", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                tablesTable = new DataTable();
                da.Fill(tablesTable);
                dataGridView1.DataSource = tablesTable;

                label2.Text = $"Всего: {tablesTable.Rows.Count}";

                comboBox1.Items.Clear();
                comboBox1.Items.Add("");
                MySqlCommand cmdSeats = new MySqlCommand("SELECT DISTINCT TablesCountPlace FROM Tables ORDER BY TablesCountPlace;", con);
                MySqlDataReader reader = cmdSeats.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetInt32(0).ToString());
                }
                reader.Close();
                comboBox1.SelectedIndex = 0;

                comboBox2.Items.Clear();
                comboBox2.Items.Add(""); 
                MySqlCommand cmdStatus = new MySqlCommand("SELECT DISTINCT TablesStatus FROM Tables;", con);
                MySqlDataReader reader2 = cmdStatus.ExecuteReader();
                while (reader2.Read())
                {
                    comboBox2.Items.Add(reader2.GetString(0));
                }
                reader2.Close();
                comboBox2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (tablesTable == null) return;

            string searchText = textBox1.Text.Trim().Replace("'", "''");
            string selectedSeats = comboBox1.SelectedItem?.ToString() ?? "";
            string selectedStatus = comboBox2.SelectedItem?.ToString() ?? "";

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

            label2.Text = $"Всего: {view.Count}";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

            if (tablesTable != null)
            {
                DataView view = new DataView(tablesTable);
                view.RowFilter = "";
                dataGridView1.DataSource = view;

                label2.Text = $"Всего: {view.Count}";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись?", "Подтверждение удаления записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[0-9]$"))
            {
                e.Handled = true;
            }
        }
    }
}
