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
            InactivityManager.Init();

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
                                                FROM client 
                                                WHERE IsActive = 1;", con);
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
            if (e.Value == null || e.Value == DBNull.Value) return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            string text = e.Value.ToString();

            if (columnName == "ФИО")
            {
                if (!string.IsNullOrEmpty(text))
                {
                    e.Value = ConvertToInitials(text);
                }
            }
            else if (columnName == "Телефон")
            {
                if (!string.IsNullOrEmpty(text))
                {
                    e.Value = MaskPhoneNumber(text);
                }
            }
        }

        private string ConvertToInitials(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return string.Empty;

            string[] parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 3)
            {
                return $"{parts[0]} {parts[1][0]}.{parts[2][0]}.";
            }
            else if (parts.Length == 2)
            {
                return $"{parts[0]} {parts[1][0]}.";
            }
            else
            {
                return fullName;
            }
        }

        private string MaskPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return string.Empty;

            string digitsOnly = new string(phone.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length == 11 && digitsOnly.StartsWith("7"))
            {
                string visiblePrefix = "+7";
                string firstHidden = "***";  
                string secondHidden = "***"; 
                string lastFourDigits = digitsOnly.Substring(digitsOnly.Length - 4);

                string formattedLastDigits = $"{lastFourDigits.Substring(0, 2)}-{lastFourDigits.Substring(2)}";

                return $"{visiblePrefix}({firstHidden}) {secondHidden}-{formattedLastDigits}";
            }
            else if (digitsOnly.Length == 11 && digitsOnly.StartsWith("8"))
            {
                string visiblePrefix = "8";
                string firstHidden = "***";
                string secondHidden = "***";
                string lastFourDigits = digitsOnly.Substring(digitsOnly.Length - 4);
                string formattedLastDigits = $"{lastFourDigits.Substring(0, 2)}-{lastFourDigits.Substring(2)}";

                return $"{visiblePrefix}({firstHidden}) {secondHidden}-{formattedLastDigits}";
            }
            else if (digitsOnly.Length >= 6)
            {
                int visibleStartCount = Math.Min(2, digitsOnly.Length - 4);
                string visibleStart = digitsOnly.Substring(0, visibleStartCount);
                string lastFourDigits = digitsOnly.Length >= 4
                    ? digitsOnly.Substring(digitsOnly.Length - 4)
                    : digitsOnly;

                string formattedLastDigits = lastFourDigits.Length == 4
                    ? $"{lastFourDigits.Substring(0, 2)}-{lastFourDigits.Substring(2)}"
                    : lastFourDigits;

                int hiddenCount = digitsOnly.Length - visibleStartCount - 4;
                if (hiddenCount > 0)
                {
                    string hiddenPart = new string('*', hiddenCount);
                    return $"{visibleStart}{hiddenPart}-{formattedLastDigits}";
                }
                else
                {
                    return $"{visibleStart}-{formattedLastDigits}";
                }
            }
            else
            {
                return phone;
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

            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить клиента \"{clientFIO}\"?\n\n" +
                "Клиент будет помечен как неактивный и скрыт из списка.",
                "Удаление",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
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
            }
        }
    }
}