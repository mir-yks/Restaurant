using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
            ConfigureDataGridView();

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
        }

        private void ConfigureButtons()
        {
            if (roleId == 4)
            {
                buttonNew.Visible = true;
                buttonUpdate.Visible = true;
                buttonDelete.Visible = true;
            }
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["DishId"].Value);
            string name = dataGridView1.CurrentRow.Cells["Блюдо"].Value.ToString();
            string desc = dataGridView1.CurrentRow.Cells["Описание"].Value.ToString();
            decimal price = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Стоимость"].Value);
            string category = dataGridView1.CurrentRow.Cells["Категория блюда"].Value.ToString();
            string offer = dataGridView1.CurrentRow.Cells["Акция"].Value.ToString();

            string photoHash = GetDishPhotoHashFromDatabase(id);

            MenuInsert MenuInsert = new MenuInsert("edit", id, name, desc, price, category, offer, photoHash);
            MenuInsert.ShowDialog();
            LoadMenu();
        }

        private string GetDishPhotoHashFromDatabase(int dishId)
        {
            string photoHash = "";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT DishPhoto FROM MenuDish WHERE DishId = @id", con);
                    cmd.Parameters.AddWithValue("@id", dishId);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        photoHash = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении фото блюда: " + ex.Message);
            }

            return photoHash;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            MenuInsert MenuInsert = new MenuInsert("add");
            MenuInsert.ShowDialog();
            LoadMenu();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            LoadMenu();
            LoadFilters();
        }

        private void LoadMenu()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    string query = @"SELECT 
                                m.DishId,
                                m.DishName AS 'Блюдо',
                                m.DishDescription AS 'Описание',
                                m.DishPrice AS 'Стоимость',
                                c.CategoryDishName AS 'Категория блюда',
                                COALESCE(o.OffersDishName, '') AS 'Акция',
                                m.DishPhoto
                             FROM MenuDish m
                             JOIN CategoryDish c ON m.DishCategory = c.CategoryDishId
                             LEFT JOIN OffersDish o ON m.OffersDish = o.OffersDishId;";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    menuTable = new DataTable();
                    da.Fill(menuTable);

                    dataGridView1.DataSource = menuTable;

                    if (dataGridView1.Columns.Contains("DishPhoto"))
                        dataGridView1.Columns["DishPhoto"].Visible = false;

                    if (dataGridView1.Columns.Contains("DishId"))
                        dataGridView1.Columns["DishId"].Visible = false;

                    LoadImagesToGrid();
                    labelTotal.Text = $"Всего: {menuTable.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadImagesToGrid()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].IsNewRow) continue;

                string photoHash = dataGridView1.Rows[i].Cells["DishPhoto"].Value?.ToString();

                try
                {
                    string imagePath = FindImageByHash(photoHash);
                    string plugPath = GetPlugImagePath();

                    if (imagePath != null && File.Exists(imagePath))
                    {
                        Image image = Image.FromFile(imagePath);
                        dataGridView1.Rows[i].Cells["ColumnImage"].Value = image;
                    }
                    else if (File.Exists(plugPath))
                    {
                        Image plugImage = Image.FromFile(plugPath);
                        dataGridView1.Rows[i].Cells["ColumnImage"].Value = plugImage;
                    }
                }
                catch (Exception)
                {
                   
                }
            }
            dataGridView1.Columns["Акция"].Width = 120;
        }

        private string FindImageByHash(string targetHash)
        {
            if (string.IsNullOrEmpty(targetHash)) return null;

            string[] possibleDirs = {
        Path.Combine(Application.StartupPath, "Resources", "image", "Menu"),
        Path.Combine(Application.StartupPath, "..", "..", "Resources", "image", "Menu")
    };

            foreach (string dir in possibleDirs)
            {
                if (!Directory.Exists(dir)) continue;

                try
                {
                    foreach (string filePath in Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
                        .Where(f => f.ToLower().EndsWith(".jpg") || f.ToLower().EndsWith(".jpeg") || f.ToLower().EndsWith(".png")))
                    {
                        try
                        {
                            byte[] fileData = File.ReadAllBytes(filePath);
                            string fileHash = CalculateImageHash(fileData);

                            if (fileHash == targetHash)
                            {
                                return filePath;
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return null;
        }

        private string CalculateImageHash(byte[] imageData)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(imageData);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private string GetPlugImagePath()
        {
            string[] possiblePaths = {
                Path.Combine(Application.StartupPath, "Resources", "image", "plug.png"),
                Path.Combine(Application.StartupPath, "..", "..", "Resources", "image", "plug.png")
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                    return path;
            }

            return null;
        }

        private void LoadFilters()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand cmdCategories = new MySqlCommand("SELECT CategoryDishName FROM CategoryDish;", con);
                    MySqlDataReader reader = cmdCategories.ExecuteReader();

                    comboBoxCategory.Items.Clear();
                    comboBoxCategory.Items.Add("");
                    while (reader.Read())
                        comboBoxCategory.Items.Add(reader.GetString(0));
                    reader.Close();
                    comboBoxCategory.SelectedIndex = 0;

                    comboBoxPrice.Items.Clear();
                    comboBoxPrice.Items.Add("");
                    comboBoxPrice.Items.Add("По возрастанию");
                    comboBoxPrice.Items.Add("По убыванию");
                    comboBoxPrice.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            LoadImagesToGrid();
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
                LoadImagesToGrid();
                labelTotal.Text = $"Всего: {view.Count}";
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["DishId"].Value);
            string name = dataGridView1.CurrentRow.Cells["Блюдо"].Value.ToString();

            DialogResult result = MessageBox.Show($"Вы действительно хотите удалить блюдо \"{name}\"?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM MenuDish WHERE DishId = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Блюдо \"{name}\" успешно удалено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadMenu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
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