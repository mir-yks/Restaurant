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
            ClientsInsert ClientsInsert = new ClientsInsert("edit");
            this.Visible = true;
            ClientsInsert.ShowDialog();
            this.Visible = true;
        }

        private void buttonNew_Click(object sender, EventArgs e)
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

                labelTotal.Text = $"Всего: {clientTable.Rows.Count}";

                MySqlCommand client = new MySqlCommand("SELECT ClientFIO FROM client;", con);
                MySqlDataReader reader = client.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxClient_TextChanged(object sender, EventArgs e)
        {
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
                ClientsInsert form = new ClientsInsert("view");

                form.ClientFIO = row.Cells["ФИО"].Value.ToString();
                form.ClientPhone = row.Cells["Телефон"].Value.ToString();

                form.ShowDialog();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

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
    }
}
