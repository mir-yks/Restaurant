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
    public partial class Category : Form
    {
        private DataTable categoriesTable;
        public Category()
        {
            InitializeComponent();

            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonNew.Font = Fonts.MontserratAlternatesBold(12f);
            buttonUpdate.Font = Fonts.MontserratAlternatesBold(12f);
            buttonDelete.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);

            buttonUpdate.Enabled = false;
            buttonDelete.Enabled = false;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["CategoryDishId"].Value);
            string name = dataGridView1.CurrentRow.Cells["Категория"].Value.ToString();

            CategoryInsert form = new CategoryInsert("edit")
            {
                CategoryID = id,
                CategoryName = name
            };

            this.Visible = true;
            if (form.ShowDialog() == DialogResult.OK)
                LoadCategories();
            this.Visible = true;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            CategoryInsert CategoryInsert = new CategoryInsert("add");
            this.Visible = true;
            CategoryInsert.ShowDialog();
            this.Visible = true;

            LoadCategories();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Category_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT CategoryDishId, CategoryDishName AS 'Категория' FROM CategoryDish", con);
                    categoriesTable = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(categoriesTable);
                    dataGridView1.DataSource = categoriesTable;

                    if (dataGridView1.Columns.Contains("CategoryDishId"))
                        dataGridView1.Columns["CategoryDishId"].Visible = false;

                    labelTotal.Text = $"Всего: {categoriesTable.Rows.Count}";
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

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["CategoryDishId"].Value);
            string name = dataGridView1.CurrentRow.Cells["Категория"].Value.ToString();

            DialogResult result = MessageBox.Show($"Вы действительно хотите удалить категорию \"{name}\"?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM CategoryDish WHERE CategoryDishId = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Категория \"{name}\" успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategories();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
