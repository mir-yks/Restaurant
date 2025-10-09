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

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            label3.Font = Fonts.MontserratAlternatesRegular(14f);
            label4.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox2.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            button3.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(10f);
        }
        private void ConfigureButtons()
        {
            button1.Visible = true; 
            button2.Visible = false;
            button3.Visible = false;

            if (roleId == 4) 
            {
                button2.Visible = true;
                button3.Visible = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MenuInsert MenuInsert = new MenuInsert();
            this.Visible = true;
            MenuInsert.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
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
                MySqlCommand cmdCategories = new MySqlCommand("SELECT CategoryDishName FROM CategoryDish;", con);
                MySqlDataReader reader = cmdCategories.ExecuteReader();

                comboBox1.Items.Clear();
                comboBox1.Items.Add(""); 
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString(0));
                }
                reader.Close();

                comboBox1.SelectedIndex = 0;

                comboBox2.Items.Clear();
                comboBox2.Items.Add("");
                comboBox2.Items.Add("По возрастанию");
                comboBox2.Items.Add("По убыванию");
                comboBox2.SelectedIndex = 0;
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            if (menuTable == null) return;

            string searchText = textBox1.Text.Trim().Replace("'", "''");
            string selectedCategory = comboBox1.SelectedItem?.ToString() ?? "";
            string sortOption = comboBox2.SelectedItem?.ToString() ?? "";

            DataView view = new DataView(menuTable);
            string filter = "";

            if (!string.IsNullOrEmpty(searchText))
                filter = $"Блюдо LIKE '%{searchText}%'";

            if (!string.IsNullOrEmpty(selectedCategory))
            {
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";
                filter += $"`Категория блюда` = '{selectedCategory}'";
            }

            view.RowFilter = filter;

            if (sortOption == "По возрастанию")
                view.Sort = "[Стоимость] ASC";
            else if (sortOption == "По убыванию")
                view.Sort = "[Стоимость] DESC";
            else
                view.Sort = ""; 

            dataGridView1.DataSource = view;
        }

    }

}
