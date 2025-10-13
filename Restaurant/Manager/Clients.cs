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

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            button3.Font = Fonts.MontserratAlternatesBold(12f);
            button8.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(10f);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClientsInsert ClientsInsert = new ClientsInsert("edit");
            this.Visible = true;
            ClientsInsert.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClientsInsert ClientsInsert = new ClientsInsert("add");
            this.Visible = true;
            ClientsInsert.ShowDialog();
            this.Visible = true;
        }

        private void Clients_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(connStr.ConnectionString);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                        ClientFIO AS 'ФИО',
                                                        ClientPhone AS 'Телефон'
                                                    FROM client;", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                clientTable = new DataTable();
                da.Fill(clientTable);
                dataGridView1.DataSource = clientTable;

                label2.Text = $"Всего: {clientTable.Rows.Count}";

                MySqlCommand client = new MySqlCommand("SELECT ClientFIO FROM client;", con);
                MySqlDataReader reader = client.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            if (clientTable == null) return;

            string searchText = textBox1.Text.Trim().Replace("'", "''");

            DataView view = new DataView(clientTable);
            string filter = "";

            if (!string.IsNullOrEmpty(searchText))
                filter = $"ФИО LIKE '%{searchText}%'";


            view.RowFilter = filter;
            dataGridView1.DataSource = view;

            label2.Text = $"Всего: {view.Count}";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";

            if (clientTable != null)
            {
                DataView view = new DataView(clientTable);
                view.RowFilter = "";
                dataGridView1.DataSource = view;

                label2.Text = $"Всего: {view.Count}";
            }
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Телефон" && e.Value != null)
            {
                string phone = e.Value.ToString();

                phone = new string(phone.Where(char.IsDigit).ToArray());

                if (phone.Length == 11 && phone.StartsWith("7"))
                {
                    e.Value = $"+{phone[0]}({phone.Substring(1, 3)}) {phone.Substring(4, 3)}-{phone.Substring(7, 2)}-{phone.Substring(9, 2)}";
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
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
    }
}
