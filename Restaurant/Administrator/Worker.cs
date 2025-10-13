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
        public Worker()
        {
            InitializeComponent();

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            label3.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            button3.Font = Fonts.MontserratAlternatesBold(12f);
            button8.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WorkerInsert WorkerInsert = new WorkerInsert("add");
            this.Visible = true;
            WorkerInsert.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WorkerInsert WorkerInsert = new WorkerInsert("edit");
            this.Visible = true;
            WorkerInsert.ShowDialog();
            this.Visible = true;
        }

        private void Worker_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(connStr.ConnectionString);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"SELECT 
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
                MySqlCommand cmdRoles = new MySqlCommand("SELECT RoleName FROM role;", con);
                MySqlDataReader reader = cmdRoles.ExecuteReader();

                label2.Text = $"Всего: {workersTable.Rows.Count}";

                comboBox1.Items.Clear();
                comboBox1.Items.Add("");
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString(0));
                }
                reader.Close();

                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            if (workersTable == null) return;

            string searchText = textBox1.Text.Trim().Replace("'", "''");
            string selectedRole = comboBox1.SelectedItem?.ToString() ?? "";

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

            label2.Text = $"Всего: {view.Count}";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            comboBox1.SelectedIndex = 0;

            if (workersTable != null)
            {
                DataView view = new DataView(workersTable);
                view.RowFilter = "";
                dataGridView1.DataSource = view;

                label2.Text = $"Всего: {view.Count}";
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


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
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
    }
}
