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
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }
        private void buttonNew_Click(object sender, EventArgs e)
        {
            CategoryInsert CategoryInsert = new CategoryInsert();
            CategoryInsert.ShowDialog();

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
    }
}
