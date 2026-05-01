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
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }
        private void buttonNew_Click(object sender, EventArgs e)
        {
            CategoryInsert categoryInsert = new CategoryInsert("add", 0, "", this);
            categoryInsert.ShowDialog();

            LoadCategories();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите категорию для редактирования!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            int categoryId = Convert.ToInt32(selectedRow.Cells["CategoryDishId"].Value);
            string categoryName = selectedRow.Cells["Категория"].Value.ToString();

            CategoryInsert categoryInsert = new CategoryInsert("edit", categoryId, categoryName, this);
            categoryInsert.ShowDialog();

            LoadCategories();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Category_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT CategoryDishId, CategoryDishName AS 'Категория' FROM CategoryDish ORDER BY CategoryDishName", con);
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
    }
}