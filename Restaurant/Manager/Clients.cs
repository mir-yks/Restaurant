using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class Clients : Form
    {
        private DataTable clientTable;

        public Clients()
        {
            InitializeComponent();
            InactivityManager.Init();

            labelClient.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxClient.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonNew.Font = Fonts.MontserratAlternatesBold(12f);
            buttonUpdate.Font = Fonts.MontserratAlternatesBold(12f);
            buttonDelete.Font = Fonts.MontserratAlternatesBold(12f);
            buttonBooking.Font = Fonts.MontserratAlternatesBold(12f);
            buttonClearFilters.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);

            KeyboardLayoutManager.AttachRussianLayout(textBoxClient);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            DataGridViewRow row = dataGridView1.CurrentRow;

            ClientsInsert clientsInsert = new ClientsInsert("edit", this)
            {
                ClientFIO = row.Cells["ФИО"].Value.ToString(),
                ClientPhone = row.Cells["Телефон"].Value.ToString(),
                ClientID = Convert.ToInt32(row.Cells["ID"].Value)
            };

            clientsInsert.ShowDialog();
            LoadClients();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            ClientsInsert clientsInsert = new ClientsInsert("add", this);
            clientsInsert.ShowDialog();
            LoadClients();
        }

        private void Clients_Load(object sender, EventArgs e)
        {
            LoadClients();
        }

        private void LoadClients()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                    ClientId AS 'ID',
                                                    ClientFIO AS 'ФИО',
                                                    ClientPhone AS 'Телефон'
                                                FROM client 
                                                WHERE IsActive = 1
                                                ORDER BY ClientFIO;", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    clientTable = new DataTable();
                    da.Fill(clientTable);
                    dataGridView1.DataSource = clientTable;

                    if (dataGridView1.Columns.Contains("ID"))
                        dataGridView1.Columns["ID"].Visible = false;

                    labelTotal.Text = $"Всего: {clientTable.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxClient_TextChanged(object sender, EventArgs e)
        {
            int cursorPos = textBoxClient.SelectionStart;
            string formatted = DataFormatter.ValidateAndFormatName(textBoxClient.Text, ref cursorPos);

            textBoxClient.TextChanged -= textBoxClient_TextChanged;
            textBoxClient.Text = formatted;
            textBoxClient.SelectionStart = cursorPos;
            textBoxClient.TextChanged += textBoxClient_TextChanged;

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (clientTable == null) return;

            string searchText = textBoxClient.Text.Trim().Replace("'", "''");

            DataView view = new DataView(clientTable);
            string filter = "";

            if (!string.IsNullOrEmpty(searchText))
                filter = $"ФИО LIKE '%{searchText}%'";

            view.RowFilter = filter;
            dataGridView1.DataSource = view;
            labelTotal.Text = $"Всего: {view.Count}";
        }

        private void buttonClearFilters_Click(object sender, EventArgs e)
        {
            textBoxClient.Text = "";

            if (clientTable != null)
            {
                DataView view = new DataView(clientTable);
                view.RowFilter = "";
                dataGridView1.DataSource = view;
                labelTotal.Text = $"Всего: {view.Count}";
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null || e.Value == DBNull.Value) return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            string text = e.Value.ToString();

            if (columnName == "ФИО")
            {
                if (!string.IsNullOrEmpty(text))
                    e.Value = DataFormatter.ConvertToInitials(text);
            }
            else if (columnName == "Телефон")
            {
                if (!string.IsNullOrEmpty(text))
                    e.Value = DataFormatter.MaskPhoneNumber(text);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                ClientsInsert form = new ClientsInsert("view", this)
                {
                    ClientFIO = row.Cells["ФИО"].Value.ToString(),
                    ClientPhone = row.Cells["Телефон"].Value.ToString()
                };
                form.ShowDialog();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите клиента для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedClientId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string clientFIO = dataGridView1.CurrentRow.Cells["ФИО"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить клиента \"{clientFIO}\"?\n\n" +
                "Клиент будет помечен как неактивный и скрыт из списка.",
                "Удаление",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE client SET IsActive = 0 WHERE ClientId = @id", con);
                    cmd.Parameters.AddWithValue("@id", selectedClientId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Клиент \"{clientFIO}\" успешно удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadClients();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить клиента.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-\s]$"))
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
                buttonBooking.Enabled = true;
            }
        }

        private void buttonBooking_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите клиента для бронирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dataGridView1.CurrentRow;
            int clientId = Convert.ToInt32(row.Cells["ID"].Value);
            string clientFIO = row.Cells["ФИО"].Value.ToString();
            string clientPhone = row.Cells["Телефон"].Value.ToString();

            ВookingInsert bookingForm = new ВookingInsert("add", this);

            bookingForm.SetClientData(clientId, clientFIO);

            bookingForm.ShowDialog();
        }
    }
}