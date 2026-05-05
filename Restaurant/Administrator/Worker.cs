using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
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
            InactivityManager.Init();

            labelWorker.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            labelCategory.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxWorker.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxCategory.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonNew.Font = Fonts.MontserratAlternatesBold(12f);
            buttonUpdate.Font = Fonts.MontserratAlternatesBold(12f);
            buttonDelete.Font = Fonts.MontserratAlternatesBold(12f);
            buttonClearFilters.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);

            KeyboardLayoutManager.AttachRussianLayout(textBoxWorker);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            WorkerInsert workerInsert = new WorkerInsert("add", this);
            workerInsert.ShowDialog();
            LoadWorkers();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            DataGridViewRow row = dataGridView1.CurrentRow;

            WorkerInsert workerInsert = new WorkerInsert("edit", this)
            {
                WorkerFIO = row.Cells["ФИО"].Value.ToString(),
                WorkerLogin = row.Cells["Логин"].Value.ToString(),
                WorkerPhone = row.Cells["Телефон"].Value.ToString(),
                WorkerPassport = row.Cells["Паспорт"].Value.ToString(),
                WorkerRole = row.Cells["Роль"].Value.ToString(),
                WorkerID = Convert.ToInt32(row.Cells["ID"].Value)
            };

            workerInsert.ShowDialog();
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
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                        SELECT 
                            w.WorkerId AS 'ID',
                            w.WorkerFIO AS 'ФИО',
                            w.WorkerLogin AS 'Логин',
                            w.WorkerPhone AS 'Телефон',
                            w.WorkerPassport AS 'Паспорт',
                            r.RoleName AS 'Роль',
                            w.IsActive AS 'Активен'
                        FROM worker w
                        JOIN role r ON w.WorkerRole = r.RoleId
                        WHERE w.IsActive = 1
                        ORDER BY w.WorkerFIO;", con);

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
            catch (MySqlException ex)
            {
                if (ex.Number == 1049)
                {
                    MessageBox.Show($"База данных 'db57' не найдена. Проверьте настройки подключения.",
                        "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string formatted = DataFormatter.ValidateAndFormatName(textBoxWorker.Text, ref cursorPos);

            textBoxWorker.TextChanged -= textBoxWorker_TextChanged;
            textBoxWorker.Text = formatted;
            textBoxWorker.SelectionStart = cursorPos;
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
                    e.Value = DataFormatter.ConvertToInitials(text);
            }
            else if (columnName == "Телефон")
            {
                if (!string.IsNullOrEmpty(text))
                    e.Value = DataFormatter.MaskPhoneNumber(text);
            }
            else if (columnName == "Паспорт")
            {
                if (!string.IsNullOrEmpty(text))
                    e.Value = DataFormatter.MaskPassport(text);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                WorkerInsert form = new WorkerInsert("view", this)
                {
                    WorkerFIO = row.Cells["ФИО"].Value.ToString(),
                    WorkerLogin = row.Cells["Логин"].Value.ToString(),
                    WorkerPhone = row.Cells["Телефон"].Value.ToString(),
                    WorkerPassport = row.Cells["Паспорт"].Value.ToString(),
                    WorkerRole = row.Cells["Роль"].Value.ToString()
                };

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
            string workerFIO = dataGridView1.CurrentRow.Cells["ФИО"].Value.ToString();

            if (selectedWorkerId == CurrentUserID)
            {
                MessageBox.Show("Вы не можете удалить самого себя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить сотрудника \"{workerFIO}\"?",
                "Удаление",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE worker SET IsActive = 0 WHERE WorkerId = @id", con);
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