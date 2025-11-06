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

            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
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
            RoleInsert RoleInsert = new RoleInsert("add");
            RoleInsert.ShowDialog();

            LoadRoles();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["RoleId"].Value);
                string name = dataGridView1.CurrentRow.Cells["Наименование"].Value.ToString();

                if (name.Equals("Администратор", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Нельзя изменить роль 'Администратор'!",
                                  "Запрещено",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                RoleInsert RoleInsert = new RoleInsert("edit");
                RoleInsert.RoleId = id;
                RoleInsert.RoleName = name;
                RoleInsert.ShowDialog();

                LoadRoles();
            }
        }

        private void Role_Load(object sender, EventArgs e)
        {
            LoadRoles();
        }

        private void LoadRoles()
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
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView1.MultiSelect = false;

                    labelTotal.Text = $"Всего: {rolesTable.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            string roleName = dataGridView1.CurrentRow.Cells["Наименование"].Value.ToString();

            if (roleName.Equals("Администратор", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Нельзя удалить роль 'Администратор'!",
                              "Запрещено",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить роль \"{roleName}\"?",
                "Удаление записи",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                    {
                        con.Open();
                        int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["RoleId"].Value);
                        MySqlCommand cmd = new MySqlCommand("DELETE FROM role WHERE RoleId = @id", con);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Роль \"{roleName}\" успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadRoles();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string roleName = dataGridView1.Rows[e.RowIndex].Cells["Наименование"].Value.ToString();

                if (roleName.Equals("Администратор", StringComparison.OrdinalIgnoreCase))
                {
                    buttonUpdate.Enabled = false;
                    buttonDelete.Enabled = false;
                }
                else
                {
                    buttonUpdate.Enabled = true;
                    buttonDelete.Enabled = true;
                }
            }
        }
    }
}