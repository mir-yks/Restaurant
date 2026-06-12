using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class Menu : Form
    {
        private int roleId;
        private DataTable menuTable;
        private Dictionary<string, Image> imageCache = new Dictionary<string, Image>();
        private Image plugImage;
        private CancellationTokenSource imageLoadingCts;
        private bool _resettingFilters;
        public Menu(int role)
        {
            InitializeComponent();

            InitPerformanceTweaks();

            roleId = role;
            ConfigureButtons();
            LoadPlugImage();

            ColumnImage.DefaultCellStyle.NullValue = plugImage;

            labelLegend.Font = Fonts.MontserratAlternatesRegular(14f);
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
            buttonClearFilters.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);

            KeyboardLayoutManager.AttachRussianLayout(textBoxDish);
        }

        private void InitPerformanceTweaks()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
            this.UpdateStyles();

            typeof(DataGridView)
                .GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic)
                .SetValue(dataGridView1, true, null);
        }

        private void LoadPlugImage()
        {
            string plugPath = ImageManager.Instance.GetPlugImagePath();
            if (File.Exists(plugPath))
            {
                try
                {
                    plugImage = ImageManager.Instance.LoadImageFromFile(plugPath);
                }
                catch (Exception)
                {
                    plugImage = null;
                }
            }
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

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["DishId"].Value);
            string name = dataGridView1.CurrentRow.Cells["Блюдо"].Value.ToString();
            string desc = dataGridView1.CurrentRow.Cells["Описание"].Value.ToString();
            decimal price = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Стоимость"].Value);
            string category = dataGridView1.CurrentRow.Cells["Категория блюда"].Value.ToString();
            object offerObj = dataGridView1.CurrentRow.Cells["OffersDishId"].Value;
            int offerId = (offerObj == DBNull.Value || offerObj == null) ? -1 : Convert.ToInt32(offerObj);

            string photoHash = GetDishPhotoHashFromDatabase(id);

            MenuInsert MenuInsert = new MenuInsert("edit", id, name, desc, price, category, offerId.ToString(), photoHash, this);
            MenuInsert.ShowDialog();
            LoadMenuAsync();
        }

        private string GetDishPhotoHashFromDatabase(int dishId)
        {
            string photoHash = "";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
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
            MenuInsert MenuInsert = new MenuInsert("add", 0, "", "", 0, "", "", "", this);
            MenuInsert.ShowDialog();
            LoadMenuAsync();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            LoadMenuAsync();
            LoadFilters();
        }

        private async void LoadMenuAsync()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    await con.OpenAsync();

                    string query = @"SELECT 
                m.DishId,
                m.DishName AS 'Блюдо',
                m.DishDescription AS 'Описание',
                CASE 
                    WHEN o.OffersDishDicsount IS NULL THEN m.DishPrice
                    ELSE m.DishPrice - (m.DishPrice * o.OffersDishDicsount / 100)
                END AS 'FinalPrice',
                m.DishPrice AS 'Стоимость',
                c.CategoryDishName AS 'Категория блюда',
                CASE 
                    WHEN o.OffersDishName IS NULL THEN ''
                    ELSE CONCAT(o.OffersDishName, ' (-', o.OffersDishDicsount, '%)')
                END AS 'Акция',
                o.OffersDishId,
                m.DishPhoto
             FROM MenuDish m
             JOIN CategoryDish c ON m.DishCategory = c.CategoryDishId
             LEFT JOIN OffersDish o ON m.OffersDish = o.OffersDishId
             WHERE m.IsActive = 1;";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    menuTable = new DataTable();
                    da.Fill(menuTable);

                    dataGridView1.DataSource = menuTable;
                    EnsureImageColumn();
                    HideColumns();

                    SetPlugs();

                    labelTotal.Text = $"Всего: {menuTable.Rows.Count}";

                    imageLoadingCts?.Cancel();
                    imageLoadingCts?.Dispose();

                    imageLoadingCts = new CancellationTokenSource();

                    try
                    {
                        await FillImagesOnce(imageLoadingCts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetPlugs()
        {
            if (!dataGridView1.Columns.Contains("ColumnImage"))
                return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                    row.Cells["ColumnImage"].Value = plugImage;
            }
        }

        private void HideColumns()
        {
            if (dataGridView1.Columns.Contains("DishPhoto"))
                dataGridView1.Columns["DishPhoto"].Visible = false;

            if (dataGridView1.Columns.Contains("DishId"))
                dataGridView1.Columns["DishId"].Visible = false;

            if (dataGridView1.Columns.Contains("FinalPrice"))
                dataGridView1.Columns["FinalPrice"].Visible = false;

            if (dataGridView1.Columns.Contains("OffersDishId"))
                dataGridView1.Columns["OffersDishId"].Visible = false;
        }

        private async Task FillImagesOnce(CancellationToken token)
        {
            if (!dataGridView1.Columns.Contains("ColumnImage"))
                return;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                token.ThrowIfCancellationRequested();

                var row = dataGridView1.Rows[i];

                if (row.IsNewRow)
                    continue;

                string hash = row.Cells["DishPhoto"].Value?.ToString();

                Image img = null;

                if (!string.IsNullOrEmpty(hash))
                {
                    img = await ImageManager.Instance.LoadImageByHashAsync(hash);

                    token.ThrowIfCancellationRequested();
                }

                if (!dataGridView1.Columns.Contains("ColumnImage"))
                    return;

                row.Cells["ColumnImage"].Value = img ?? plugImage;
            }
        }

        private void LoadFilters()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
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

        private async void ApplyFilters()
        {
            if (_resettingFilters)
                return;

            if (menuTable == null) return;

            string searchText = textBoxDish.Text.Trim().Replace("'", "''");
            string selectedCategory = comboBoxCategory.SelectedItem?.ToString() ?? "";
            string sortOption = comboBoxPrice.SelectedItem?.ToString() ?? "";

            DataView view = new DataView(menuTable);
            string filter = "";

            if (!string.IsNullOrEmpty(searchText))
            {
                string trimmed = searchText.Length > 1 ? searchText.Substring(1) : searchText;
                filter = $"(Блюдо LIKE '%{trimmed}%' OR Описание LIKE '%{trimmed}%')";
            }

            if (!string.IsNullOrEmpty(selectedCategory))
            {
                if (!string.IsNullOrEmpty(filter))
                    filter += " AND ";

                filter += $"[Категория блюда] = '{selectedCategory}'";
            }

            view.RowFilter = filter;

            if (sortOption == "По возрастанию")
            {
                view.Sort = "[Стоимость] ASC";
            }
            else if (sortOption == "По убыванию")
            {
                view.Sort = "[Стоимость] DESC";
            }
            else
            {
                view.Sort = "";
            }

            dataGridView1.DataSource = view;
            EnsureImageColumn();
            SetPlugs();

            labelTotal.Text = $"Всего: {view.Count}";

            imageLoadingCts?.Cancel();
            imageLoadingCts?.Dispose();

            imageLoadingCts = new CancellationTokenSource();

            try
            {
                await FillImagesOnce(imageLoadingCts.Token);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void buttonClearFilters_Click(object sender, EventArgs e)
        {
            _resettingFilters = true;

            textBoxDish.Text = "";
            comboBoxCategory.SelectedIndex = 0;
            comboBoxPrice.SelectedIndex = 0;

            _resettingFilters = false;

            ApplyFilters();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["DishId"].Value);
            string name = dataGridView1.CurrentRow.Cells["Блюдо"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить блюдо \"{name}\"?",
                "Удаление",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE MenuDish SET IsActive = 0 WHERE DishId = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Блюдо \"{name}\" успешно удалено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMenuAsync();
                    }
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
                !Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-,.\s]$"))
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

        private void comboBoxCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я\s]$"))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dataGridView1.Rows[e.RowIndex];

            if (row.IsNewRow) return;

            var offerValue = row.Cells["Акция"].Value?.ToString();

            if (!string.IsNullOrEmpty(offerValue))
            {
                row.DefaultCellStyle.BackColor = Color.LightGreen;
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.ColumnIndex >= dataGridView1.Columns.Count)
                return;

            if (dataGridView1.Columns[e.ColumnIndex].Name != "Стоимость")
                return;

            var row = dataGridView1.Rows[e.RowIndex];

            string offer = row.Cells["Акция"].Value?.ToString();
            if (string.IsNullOrEmpty(offer))
                return;

            if (!decimal.TryParse(row.Cells["Стоимость"].Value?.ToString(), out decimal oldPrice))
                return;

            if (!decimal.TryParse(row.Cells["FinalPrice"].Value?.ToString(), out decimal newPrice))
                return;

            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);

            string oldText = oldPrice.ToString("0.00");  
            string newText = newPrice.ToString("0.00");   

            using (Font font = Fonts.MontserratAlternatesRegular(12f))
            {
                int x = e.CellBounds.Left + 6;
                int y = e.CellBounds.Top + (e.CellBounds.Height - font.Height) / 2;

                TextRenderer.DrawText(
                    e.Graphics,
                    oldText,
                    new Font(font, FontStyle.Strikeout),
                    new Point(x, y),
                    Color.Gray,
                    TextFormatFlags.NoPadding
                );

                int oldWidth = TextRenderer.MeasureText(oldText, font).Width;

                TextRenderer.DrawText(
                    e.Graphics,
                    newText,
                    font,
                    new Point(x + oldWidth, y),
                    Color.Black,
                    TextFormatFlags.NoPadding
                );
            }

            e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            imageLoadingCts?.Cancel();
            imageLoadingCts?.Dispose();

            base.OnFormClosing(e);
        }

        private void EnsureImageColumn()
        {
            if (!dataGridView1.Columns.Contains("ColumnImage"))
            {
                DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
                imgCol.Name = "ColumnImage";
                imgCol.HeaderText = "Фото";
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;

                dataGridView1.Columns.Insert(0, imgCol);
            }
        }

        private async void dataGridView1_Sorted(object sender, EventArgs e)
        {
            if (!dataGridView1.Columns.Contains("ColumnImage"))
                return;

            imageLoadingCts?.Cancel();
            imageLoadingCts?.Dispose();

            imageLoadingCts = new CancellationTokenSource();

            SetPlugs();

            try
            {
                await FillImagesOnce(imageLoadingCts.Token);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}