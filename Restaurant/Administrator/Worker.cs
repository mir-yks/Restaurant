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
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(10f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            button3.Font = Fonts.MontserratAlternatesBold(12f);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WorkerInsert WorkerInsert = new WorkerInsert();
            this.Visible = true;
            WorkerInsert.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WorkerInsert WorkerInsert = new WorkerInsert();
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

            // Поиск по ФИО
            if (!string.IsNullOrEmpty(searchText))
                filter = $"ФИО LIKE '%{searchText}%'";

            // Фильтр по роли
            if (!string.IsNullOrEmpty(selectedRole))
            {
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"Роль = '{selectedRole}'";
            }

            view.RowFilter = filter;
            dataGridView1.DataSource = view;
        }

    }

}
