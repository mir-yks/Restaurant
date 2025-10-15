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
    public partial class Menu : Form
    {
        private int roleId;
        private DataTable menuTable;
        public Menu(int role)
        {
            InitializeComponent();
            roleId = role;
            ConfigureButtons();

            labelDish.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            labelCategory.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPrice.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxDish.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxCategory.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxPrice.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonNew.Font = Fonts.MontserratAlternatesBold(12f);
            buttonUpdate.Font = Fonts.MontserratAlternatesBold(12f);
            buttonDelete.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);

            buttonUpdate.Enabled = false;
            buttonDelete.Enabled = false;
        }
        private void ConfigureButtons()
        {
            buttonBack.Visible = true;
            buttonNew.Visible = false;
            buttonUpdate.Visible = false;
            buttonDelete.Visible = false;

            if (roleId == 4)
            {
                buttonNew.Visible = true;
                buttonUpdate.Visible = true;
                buttonDelete.Visible = true;
            }
        }
        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            MenuInsert MenuInsert = new MenuInsert();
            this.Visible = true;
            MenuInsert.ShowDialog();
            this.Visible = true;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            MenuInsert MenuInsert = new MenuInsert();
            this.Visible = true;
            MenuInsert.ShowDialog();
            this.Visible = true;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(connStr.ConnectionString);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                        m.DishName AS 'Блюдо',
                                                        m.DishDescription AS 'Описание',
                                                        m.DishPrice AS 'Стоимость',
                                                        c.CategoryDishName AS 'Категория блюда',
                                                        o.OffersDishName AS 'Предложение блюда'
                                                    FROM MenuDish m
                                                    JOIN CategoryDish c ON m.DishCategory = c.CategoryDishId
                                                    LEFT JOIN OffersDish o ON m.OffersDish = o.OffersDishId;", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                menuTable = new DataTable();
                da.Fill(menuTable);
                dataGridView1.DataSource = menuTable;

                labelTotal.Text = $"Всего: {menuTable.Rows.Count}";

                MySqlCommand cmdCategories = new MySqlCommand("SELECT CategoryDishName FROM CategoryDish;", con);
                MySqlDataReader reader = cmdCategories.ExecuteReader();

                comboBoxCategory.Items.Clear();
                comboBoxCategory.Items.Add("");
                while (reader.Read())
                {
                    comboBoxCategory.Items.Add(reader.GetString(0));
                }
                reader.Close();

                comboBoxCategory.SelectedIndex = 0;

                comboBoxPrice.Items.Clear();
                comboBoxPrice.Items.Add("");
                comboBoxPrice.Items.Add("По возрастанию");
                comboBoxPrice.Items.Add("По убыванию");
                comboBoxPrice.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void textBoxDish_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            if (menuTable == null) return;

            string searchText = textBoxDish.Text.Trim().Replace("'", "''");
            string selectedCategory = comboBoxCategory.SelectedItem?.ToString() ?? "";
            string sortOption = comboBoxPrice.SelectedItem?.ToString() ?? "";

            DataView view = new DataView(menuTable);
            string filter = "";

            if (!string.IsNullOrEmpty(searchText))
            {
                if (searchText.Length > 1)
                {
                    string trimmedSearch = searchText.Substring(1);
                    filter = $"(Блюдо LIKE '%{trimmedSearch}%' OR Описание LIKE '%{trimmedSearch}%')";
                }
            }

            if (!string.IsNullOrEmpty(selectedCategory))
            {
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"[Категория блюда] = '{selectedCategory}'";
            }

            view.RowFilter = filter;

            if (sortOption == "По возрастанию")
                view.Sort = "[Стоимость] ASC";
            else if (sortOption == "По убыванию")
                view.Sort = "[Стоимость] DESC";
            else
                view.Sort = "";

            dataGridView1.DataSource = view;

            labelTotal.Text = $"Всего: {view.Count}";
        }

        private void buttonClearFilters_Click(object sender, EventArgs e)
        {
            textBoxDish.Text = "";

            comboBoxCategory.SelectedIndex = 0;
            comboBoxPrice.SelectedIndex = 0;

            if (menuTable != null)
            {
                DataView view = new DataView(menuTable);
                view.RowFilter = "";
                view.Sort = "";
                dataGridView1.DataSource = view;

                labelTotal.Text = $"Всего: {view.Count}";
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

            }
        }

        private void textBoxDish_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-,.\s]$"))
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