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
    public partial class Worker : Form
    {
        private DataTable workersTable;
        public int CurrentUserID { get; set; }

        public Worker()
        {
            InitializeComponent();

            labelWorker.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            labelCategory.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxWorker.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxCategory.Font = Fonts.MontserratAlternatesRegular(14f);
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

        private void buttonNew_Click(object sender, EventArgs e)
        {
            WorkerInsert WorkerInsert = new WorkerInsert("add");
            WorkerInsert.ShowDialog();

            LoadWorkers();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            DataGridViewRow row = dataGridView1.CurrentRow;

            WorkerInsert WorkerInsert = new WorkerInsert("edit")
            {
                WorkerFIO = row.Cells["ФИО"].Value.ToString(),
                WorkerLogin = row.Cells["Логин"].Value.ToString(),
                WorkerPhone = row.Cells["Телефон"].Value.ToString(),
                WorkerEmail = row.Cells["Email"].Value.ToString(),
                WorkerBirthday = Convert.ToDateTime(row.Cells["Дата рождения"].Value),
                WorkerDateEmployment = Convert.ToDateTime(row.Cells["Дата найма"].Value),
                WorkerAddress = row.Cells["Адрес"].Value.ToString(),
                WorkerRole = row.Cells["Роль"].Value.ToString(),
                WorkerID = Convert.ToInt32(row.Cells["ID"].Value)
            };

            WorkerInsert.ShowDialog();
            LoadWorkers();
        }

        private void Worker_Load(object sender, EventArgs e)
        {
            LoadWorkers();
        }

        private void LoadWorkers()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                        SELECT 
                            w.WorkerId AS 'ID',
                            w.WorkerFIO AS 'ФИО',
                            w.WorkerLogin AS 'Логин',
                            w.WorkerPhone AS 'Телефон',
                            w.WorkerEmail AS 'Email',
                            w.WorkerBirthday AS 'Дата рождения',
                            w.WorkerDateEmployment AS 'Дата найма',
                            w.WorkerAddress AS 'Адрес',
                            r.RoleName AS 'Роль',
                            w.IsActive AS 'Активен'
                        FROM worker w
                        JOIN role r ON w.WorkerRole = r.RoleId
                        WHERE w.IsActive = 1;", con); 

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    workersTable = new DataTable();
                    da.Fill(workersTable);
                    dataGridView1.DataSource = workersTable;

                    if (dataGridView1.Columns.Contains("ID"))
                        dataGridView1.Columns["ID"].Visible = false;
                    if (dataGridView1.Columns.Contains("Активен"))
                        dataGridView1.Columns["Активен"].Visible = false;

                    labelTotal.Text = $"Всего: {workersTable.Rows.Count}";

                    MySqlCommand cmdRoles = new MySqlCommand("SELECT RoleName FROM role;", con);
                    MySqlDataReader reader = cmdRoles.ExecuteReader();

                    comboBoxCategory.Items.Clear();
                    comboBoxCategory.Items.Add("");
                    while (reader.Read())
                    {
                        comboBoxCategory.Items.Add(reader.GetString(0));
                    }
                    reader.Close();

                    comboBoxCategory.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void textBoxWorker_TextChanged(object sender, EventArgs e)
        {
            int cursorPos = textBoxWorker.SelectionStart;

            string input = textBoxWorker.Text;

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

            textBoxWorker.TextChanged -= textBoxWorker_TextChanged;
            textBoxWorker.Text = formatted;
            textBoxWorker.SelectionStart = Math.Min(cursorPos, textBoxWorker.Text.Length);
            textBoxWorker.TextChanged += textBoxWorker_TextChanged;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (workersTable == null) return;

            string searchText = textBoxWorker.Text.Trim().Replace("'", "''");
            string selectedRole = comboBoxCategory.SelectedItem?.ToString() ?? "";

            DataView view = new DataView(workersTable);
            string filter = "";

            if (!string.IsNullOrEmpty(searchText))
                filter = $"ФИО LIKE '%{searchText}%'";

            if (!string.IsNullOrEmpty(selectedRole))
            {
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"Роль = '{selectedRole}'";
            }

            view.RowFilter = filter;
            dataGridView1.DataSource = view;

            labelTotal.Text = $"Всего: {view.Count}";
        }

        private void buttonClearFilters_Click(object sender, EventArgs e)
        {
            textBoxWorker.Text = "";
            comboBoxCategory.SelectedIndex = 0;

            if (workersTable != null)
            {
                DataView view = new DataView(workersTable);
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
            else if (columnName == "Email")
            {
                if (!string.IsNullOrEmpty(text))
                {
                    e.Value = MaskEmail(text);
                }
            }
            else if (columnName == "Логин")
            {
                if (!string.IsNullOrEmpty(text))
                {
                    e.Value = MaskLogin(text);
                }
            }
            else if (columnName == "Адрес")
            {
                if (!string.IsNullOrEmpty(text))
                {
                    e.Value = MaskAddress(text);
                }
            }
            else if (columnName == "Дата рождения" || columnName == "Дата найма")
            {
                if (!string.IsNullOrEmpty(text) && DateTime.TryParse(text, out DateTime date))
                {
                    e.Value = MaskDate(date);
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

        private string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return string.Empty;

            int atIndex = email.IndexOf('@');
            if (atIndex > 0)
            {
                string domain = email.Substring(atIndex); 

                return $"***{domain}";
            }
            else
            {
                return "***";
            }
        }

        private string MaskLogin(string login)
        {
            if (string.IsNullOrEmpty(login))
                return string.Empty;

            if (login.Length > 3)
            {
                string visiblePart = login.Substring(0, Math.Min(3, login.Length));
                string hiddenPart = new string('*', 10);
                return $"{visiblePart}{hiddenPart}";
            }
            else
            {
                string hiddenPart = new string('*', 10);
                return $"{login}{hiddenPart}";
            }
        }

        private string MaskAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
                return string.Empty;

            int commaIndex = address.IndexOf(',');
            if (commaIndex > 0)
            {
                string city = address.Substring(0, commaIndex).Trim();
                string hiddenPart = new string('*', 10);
                return $"{city}{hiddenPart}";
            }
            else
            {
                string visiblePart = address.Length > 5
                    ? address.Substring(0, 5)
                    : address;
                string hiddenPart = new string('*', 10);
                return $"{visiblePart}{hiddenPart}";
            }
        }

        private string MaskDate(DateTime date)
        {
            string day = date.Day.ToString("00");
            string month = date.Month.ToString("00");

            return $"{day}.{month}.****";
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                WorkerInsert form = new WorkerInsert("view");

                form.WorkerFIO = row.Cells["ФИО"].Value.ToString();
                form.WorkerLogin = row.Cells["Логин"].Value.ToString();
                form.WorkerPhone = row.Cells["Телефон"].Value.ToString();
                form.WorkerEmail = row.Cells["Email"].Value.ToString();
                form.WorkerBirthday = Convert.ToDateTime(row.Cells["Дата рождения"].Value);
                form.WorkerDateEmployment = Convert.ToDateTime(row.Cells["Дата найма"].Value);
                form.WorkerAddress = row.Cells["Адрес"].Value.ToString();
                form.WorkerRole = row.Cells["Роль"].Value.ToString();

                form.ShowDialog();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите сотрудника для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedWorkerId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string workerRole = dataGridView1.CurrentRow.Cells["Роль"].Value.ToString();
            string workerFIO = dataGridView1.CurrentRow.Cells["ФИО"].Value.ToString();

            if (workerRole.Equals("Администратор", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Нельзя удалить сотрудника с ролью 'Администратор'!",
                              "Запрещено",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            if (selectedWorkerId == CurrentUserID)
            {
                MessageBox.Show("Вы не можете удалить самого себя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить сотрудника \"{workerFIO}\"",
                "Удаление",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE worker SET IsActive = 0 WHERE WorkerId = @id",
                        con);
                    cmd.Parameters.AddWithValue("@id", selectedWorkerId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Сотрудник \"{workerFIO}\" успешно удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadWorkers(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxWorker_KeyPress(object sender, KeyPressEventArgs e)
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