using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class Role : Form
    {
        private DataTable rolesTable;
        public Role()
        {
            InitializeComponent();
            InactivityManager.Init();

            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Role_Load(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT RoleId, RoleName AS 'Наименование' FROM role", con);
                    rolesTable = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(rolesTable);
                    dataGridView1.DataSource = rolesTable;

                    dataGridView1.Columns["RoleId"].Visible = false;

                    labelTotal.Text = $"Всего: {rolesTable.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}