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
    public partial class Clients : Form
    {
        private DataTable clientTable;
        public Clients()
        {
            InitializeComponent();

            labelClient.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxClient.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonNew.Font = Fonts.MontserratAlternatesBold(12f);
            buttonUpdate.Font = Fonts.MontserratAlternatesBold(12f);
            buttonDelete.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            DataGridViewRow row = dataGridView1.CurrentRow;

            ClientsInsert ClientsInsert = new ClientsInsert("edit")
            {
                ClientFIO = row.Cells["ФИО"].Value.ToString(),
                ClientPhone = row.Cells["Телефон"].Value.ToString(),
                ClientID = Convert.ToInt32(row.Cells["ID"].Value)
            };

            ClientsInsert.ShowDialog();
            LoadClients();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            ClientsInsert ClientsInsert = new ClientsInsert("add");
            ClientsInsert.ShowDialog();
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
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                        ClientId AS 'ID',
                                                        ClientFIO AS 'ФИО',
                                                        ClientPhone AS 'Телефон'
                                                    FROM client;", con);
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

            string input = textBoxClient.Text;

            int spaceCount = input.Count(c => c == ' ');
            if (spaceCount > 2)
            {
                int lastSpace = input.LastIndexOf(' ');
                input = input.Remove(lastSpace, 1);
            }

            int dashCount = input.Count(c => c == '-');
            if (dashCount > 1)
            {
                int lastDash = input.LastIndexOf('-');
                input = input.Remove(lastDash, 1);
            }

            string[] parts = input
                .Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => char.ToUpper(p[0]) + p.Substring(1).ToLower())
                .ToArray();

            string formatted = input;
            int index = 0;
            foreach (string part in parts)
            {
                int pos = formatted.IndexOf(part, index, StringComparison.OrdinalIgnoreCase);
                if (pos >= 0)
                {
                    formatted = formatted.Remove(pos, part.Length).Insert(pos, part);
                    index = pos + part.Length;
                }
            }

            textBoxClient.TextChanged -= textBoxClient_TextChanged;
            textBoxClient.Text = formatted;
            textBoxClient.SelectionStart = Math.Min(cursorPos, textBoxClient.Text.Length);
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
            if (e.Value == null) return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            string text = e.Value.ToString();

            if (columnName == "Телефон")
            {
                string phone = new string(text.Where(char.IsDigit).ToArray());

                if (phone.Length == 11 && phone.StartsWith("7"))
                {
                    string visiblePart = phone.Substring(0, 1);
                    string lastTwoDigits = phone.Length >= 2 ? phone.Substring(phone.Length - 2) : "";
                    e.Value = $"+{visiblePart}(***) ***-**-{lastTwoDigits}";
                }
                else if (text.Length > 5)
                {
                    string visiblePart = text.Substring(0, 3);
                    string lastTwoDigits = text.Substring(text.Length - 2);
                    string middlePart = new string('*', text.Length - 5);
                    e.Value = $"{visiblePart}{middlePart}{lastTwoDigits}";
                }
                else
                {
                    e.Value = text;
                }
            }
            else if (columnName == "ФИО")
            {
                string visiblePart = text.Length > 3 ? text.Substring(0, 3) : text;
                string hiddenPart = new string('*', 70);
                e.Value = visiblePart + hiddenPart;
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                ClientsInsert form = new ClientsInsert("view");

                form.ClientFIO = row.Cells["ФИО"].Value.ToString();
                form.ClientPhone = row.Cells["Телефон"].Value.ToString();

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

            DialogResult result = MessageBox.Show($"Вы действительно хотите удалить клиента \"{clientFIO}\"?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM client WHERE ClientId = @id", con);
                    cmd.Parameters.AddWithValue("@id", selectedClientId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Клиент \"{clientFIO}\" успешно удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadClients();
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
            }
        }
    }
}
