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

            buttonUpdate.Enabled = false;
            buttonDelete.Enabled = false;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            WorkerInsert WorkerInsert = new WorkerInsert("add");
            this.Visible = true;
            WorkerInsert.ShowDialog();
            this.Visible = true;

            LoadWorkers();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            DataGridViewRow row = dataGridView1.CurrentRow;

            WorkerInsert form = new WorkerInsert("edit")
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

            this.Visible = true;
            form.ShowDialog();
            this.Visible = true;

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
                            r.RoleName AS 'Роль'
                        FROM worker w
                        JOIN role r ON w.WorkerRole = r.RoleId;", con);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    workersTable = new DataTable();
                    da.Fill(workersTable);
                    dataGridView1.DataSource = workersTable;

                    if (dataGridView1.Columns.Contains("ID"))
                        dataGridView1.Columns["ID"].Visible = false;

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
            bool showSpaceWarning = false;
            bool showDashWarning = false;

            int spaceCount = input.Count(c => c == ' ');
            if (spaceCount > 2)
            {
                int lastSpace = input.LastIndexOf(' ');
                input = input.Remove(lastSpace, 1);
                showSpaceWarning = true;
            }

            int dashCount = input.Count(c => c == '-');
            if (dashCount > 1)
            {
                int lastDash = input.LastIndexOf('-');
                input = input.Remove(lastDash, 1);
                showDashWarning = true;
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

            if (showSpaceWarning)
                InputTooltipHelper.Show(textBoxWorker, "Можно использовать не более двух пробелов.");
            if (showDashWarning)
                InputTooltipHelper.Show(textBoxWorker, "Можно использовать только одно тире.");

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
            if (e.Value == null) return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            string text = e.Value.ToString();

            if (columnName == "Телефон")
            {
                string phone = new string(text.Where(char.IsDigit).ToArray());

                if (phone.Length == 11 && phone.StartsWith("7"))
                {
                    string visiblePart = phone.Substring(0, 4);
                    string hiddenPart = new string('*', phone.Length - 4);
                    string maskedPhone = visiblePart + hiddenPart;

                    e.Value = $"+{maskedPhone[0]}({maskedPhone.Substring(1, 3)}) {maskedPhone.Substring(4, 3)}-{maskedPhone.Substring(7, 2)}-{maskedPhone.Substring(9, 2)}";
                }
            }
            else
            {
                if (text.Length > 4)
                {
                    string visiblePart = text.Substring(0, 4);
                    string hiddenPart = new string('*', text.Length - 4);
                    e.Value = visiblePart + hiddenPart;
                }
            }
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

            if (selectedWorkerId == CurrentUserID)
            {
                MessageBox.Show("Вы не можете удалить самого себя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string workerFIO = dataGridView1.CurrentRow.Cells["ФИО"].Value.ToString();
            DialogResult result = MessageBox.Show($"Вы действительно хотите удалить сотрудника \"{workerFIO}\"?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM worker WHERE WorkerId = @id", con);
                    cmd.Parameters.AddWithValue("@id", selectedWorkerId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Сотрудник \"{workerFIO}\" успешно удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadWorkers();
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
