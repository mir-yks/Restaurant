using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class MenuInsert : Form
    {
        private string mode;
        private string selectedImageName = null;
        private bool imageChanged = false;
        private string oldImageName = null;

        public int DishID { get; set; }
        public string DishName
        {
            get => textBoxName.Text;
            set => textBoxName.Text = value;
        }
        public string DishDescription
        {
            get => textBoxDescription.Text;
            set => textBoxDescription.Text = value;
        }
        public decimal DishPrice
        {
            get => decimal.TryParse(textBoxPrice.Text, out decimal p) ? p : 0;
            set => textBoxPrice.Text = value.ToString("0.##");
        }
        public string DishCategory
        {
            get => comboBoxCategory.Text;
            set => comboBoxCategory.Text = value;
        }
        public string DishOffer
        {
            get => comboBoxOffers.Text;
            set => comboBoxOffers.Text = value;
        }
        public string DishPhoto
        {
            get => selectedImageName;
            set => selectedImageName = value;
        }

        public MenuInsert(string mode, int dishId = 0, string name = "", string description = "", decimal price = 0,
                 string category = "", string offer = "", string photo = "")
        {
            InitializeComponent();
            this.mode = mode;

            labelName.Font = Fonts.MontserratAlternatesRegular(14f);
            labelDescription.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPrice.Font = Fonts.MontserratAlternatesRegular(14f);
            labelCategory.Font = Fonts.MontserratAlternatesRegular(14f);
            labelOffers.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxName.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxDescription.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxPrice.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxCategory.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxOffers.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);
            buttonImage.Font = Fonts.MontserratAlternatesBold(12f);

            LoadCategories();
            LoadOffers();
            ApplyMode();

            if (mode == "edit" && dishId > 0)
            {
                DishID = dishId;
                DishName = name;
                DishDescription = description;
                DishPrice = price;
                DishCategory = category;
                DishOffer = offer;
                selectedImageName = photo;
                oldImageName = photo;

                LoadDishPhoto(photo);
            }
            else
            {
                LoadDefaultImage();
            }
        }

        private void ApplyMode()
        {
            if (mode == "edit")
            {
                buttonWrite.Text = "Обновить";
            }
        }

        private void LoadDishPhoto(string photoName)
        {
            if (string.IsNullOrWhiteSpace(photoName) || photoName == "plug.png")
            {
                LoadDefaultImage();
                return;
            }

            try
            {
                string debugImagePath = Path.Combine(Application.StartupPath, "Resources", "image", "Menu", photoName);
                string sourceImagePath = Path.Combine(Application.StartupPath, "..", "..", "Resources", "image", "Menu", photoName);

                string imagePath = File.Exists(debugImagePath) ? debugImagePath :
                                  File.Exists(sourceImagePath) ? sourceImagePath : null;

                if (imagePath != null && File.Exists(imagePath))
                {
                    byte[] imageData = File.ReadAllBytes(imagePath);
                    UpdatePictureBox(imageData);
                    selectedImageName = photoName;
                }
                else
                {
                    LoadDefaultImage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки изображения блюда: " + ex.Message);
                LoadDefaultImage();
            }
        }

        private void LoadDefaultImage()
        {
            try
            {
                string plugImagePath = Path.Combine(Application.StartupPath, "Resources", "image", "plug.png");
                if (File.Exists(plugImagePath))
                {
                    byte[] imageData = File.ReadAllBytes(plugImagePath);
                    UpdatePictureBox(imageData);
                    selectedImageName = "plug.png";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки изображения-заглушки: " + ex.Message);
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT CategoryDishName FROM CategoryDish;", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    comboBoxCategory.Items.Clear();
                    while (reader.Read())
                        comboBoxCategory.Items.Add(reader.GetString(0));
                    reader.Close();
                    if (comboBoxCategory.Items.Count > 0) comboBoxCategory.SelectedIndex = 0;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void LoadOffers()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT OffersDishName FROM OffersDish;", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    comboBoxOffers.Items.Clear();
                    comboBoxOffers.Items.Add("");
                    while (reader.Read())
                        comboBoxOffers.Items.Add(reader.GetString(0));
                    reader.Close();
                    comboBoxOffers.SelectedIndex = 0;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DishName))
            {
                MessageBox.Show("Введите название блюда!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(DishDescription))
            {
                MessageBox.Show("Введите описание блюда!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxDescription.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxPrice.Text))
            {
                MessageBox.Show("Введите цену блюда!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPrice.Focus();
                return;
            }

            if (!decimal.TryParse(textBoxPrice.Text.Replace(',', '.'), out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (число больше 0)!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPrice.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(DishCategory))
            {
                MessageBox.Show("Выберите категорию блюда!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxCategory.Focus();
                return;
            }

            string offerValue = string.IsNullOrWhiteSpace(DishOffer) ? null : DishOffer;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand checkCmd;
                    if (mode == "add")
                    {
                        checkCmd = new MySqlCommand("SELECT COUNT(*) FROM MenuDish WHERE DishName = @name", con);
                        checkCmd.Parameters.AddWithValue("@name", DishName.Trim());
                    }
                    else
                    {
                        checkCmd = new MySqlCommand("SELECT COUNT(*) FROM MenuDish WHERE DishName = @name AND DishId <> @id", con);
                        checkCmd.Parameters.AddWithValue("@name", DishName.Trim());
                        checkCmd.Parameters.AddWithValue("@id", DishID);
                    }

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Блюдо с таким названием уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxName.Focus();
                        return;
                    }

                    DialogResult confirm = MessageBox.Show("Вы действительно хотите сохранить запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm != DialogResult.Yes) return;

                    if (mode == "add")
                    {
                        MySqlCommand cmd = new MySqlCommand(@"
                            INSERT INTO MenuDish (DishName, DishDescription, DishPrice, DishCategory, OffersDish, DishPhoto)
                            VALUES (
                                @name,
                                @desc,
                                @price,
                                (SELECT CategoryDishId FROM CategoryDish WHERE CategoryDishName = @category),
                                (SELECT OffersDishId FROM OffersDish WHERE OffersDishName = @offer),
                                @photo
                            );", con);

                        cmd.Parameters.AddWithValue("@name", DishName.Trim());
                        cmd.Parameters.AddWithValue("@desc", DishDescription.Trim());
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@category", DishCategory);
                        cmd.Parameters.AddWithValue("@offer", offerValue);
                        cmd.Parameters.AddWithValue("@photo", selectedImageName ?? "plug.png");
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Блюдо \"{DishName}\" успешно добавлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        MySqlCommand cmd = new MySqlCommand(@"
                            UPDATE MenuDish
                            SET 
                                DishName = @name,
                                DishDescription = @desc,
                                DishPrice = @price,
                                DishCategory = (SELECT CategoryDishId FROM CategoryDish WHERE CategoryDishName = @category),
                                OffersDish = (SELECT OffersDishId FROM OffersDish WHERE OffersDishName = @offer),
                                DishPhoto = @photo
                            WHERE DishId = @id;", con);

                        cmd.Parameters.AddWithValue("@name", DishName.Trim());
                        cmd.Parameters.AddWithValue("@desc", DishDescription.Trim());
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@category", DishCategory);
                        cmd.Parameters.AddWithValue("@offer", offerValue);
                        cmd.Parameters.AddWithValue("@photo", selectedImageName ?? "plug.png");
                        cmd.Parameters.AddWithValue("@id", DishID);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Блюдо \"{DishName}\" успешно обновлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-,\s]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-,.\s]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[0-9,]$"))
            {
                e.Handled = true;
            }
        }

        private void buttonImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                ofd.Title = "Выберите фото для блюда";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string fileExtension = Path.GetExtension(ofd.FileName).ToLower();
                    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                    {
                        MessageBox.Show("Недопустимый тип файла! Разрешены только JPG и PNG изображения.",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    FileInfo fileInfo = new FileInfo(ofd.FileName);
                    if (fileInfo.Length > 3 * 1024 * 1024)
                    {
                        MessageBox.Show("Размер изображения превышает 3 МБ! Выберите файл меньшего размера.",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string fileName = Path.GetFileName(ofd.FileName);

                    try
                    {
                        using (var con = new MySqlConnection(connStr.ConnectionString))
                        {
                            con.Open();
                            using (var cmd = new MySqlCommand(
                                "SELECT DishId FROM MenuDish WHERE DishPhoto = @photo AND DishId != @id;", con))
                            {
                                cmd.Parameters.AddWithValue("@photo", fileName);
                                cmd.Parameters.AddWithValue("@id", mode == "edit" ? DishID : 0);
                                object exists = cmd.ExecuteScalar();
                                if (exists != null)
                                {
                                    MessageBox.Show("Данное фото уже используется для другого блюда.\nВыберите другое изображение.",
                                        "Фото занято", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }

                        byte[] imageData = File.ReadAllBytes(ofd.FileName);

                        string sourceDir = Path.Combine(Application.StartupPath, "..", "..", "Resources", "image", "Menu");
                        string debugDir = Path.Combine(Application.StartupPath, "Resources", "image", "Menu");

                        Directory.CreateDirectory(sourceDir);
                        Directory.CreateDirectory(debugDir);

                        string sourcePath = Path.Combine(sourceDir, fileName);
                        string debugPath = Path.Combine(debugDir, fileName);

                        File.WriteAllBytes(sourcePath, imageData);
                        File.WriteAllBytes(debugPath, imageData);

                        UpdatePictureBox(imageData);

                        selectedImageName = fileName;

                        if (mode == "edit" && oldImageName != fileName)
                        {
                            imageChanged = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при выборе изображения: " + ex.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UpdatePictureBox(byte[] imageData)
        {
            if (pictureBoxImage.Image != null)
            {
                pictureBoxImage.Image.Dispose();
                pictureBoxImage.Image = null;
            }

            using (var ms = new MemoryStream(imageData))
            {
                pictureBoxImage.Image = Image.FromStream(ms);
            }
        }

        private void MenuInsert_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pictureBoxImage.Image != null)
            {
                pictureBoxImage.Image.Dispose();
                pictureBoxImage.Image = null;
            }
        }
    }
}