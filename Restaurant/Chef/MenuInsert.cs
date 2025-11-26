using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class MenuInsert : Form
    {
        private string mode;
        private string selectedImageName = null;
        private string selectedImageHash = null;
        private string oldImageHash = null;

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
        public string DishPhotoHash
        {
            get => selectedImageHash;
            set => selectedImageHash = value;
        }

        public MenuInsert(string mode, int dishId = 0, string name = "", string description = "", decimal price = 0,
                 string category = "", string offer = "", string photoHash = "")
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
                selectedImageHash = photoHash;
                oldImageHash = photoHash;

                LoadDishPhotoByHash(photoHash);
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

        private void LoadDishPhotoByHash(string photoHash)
        {
            if (string.IsNullOrWhiteSpace(photoHash))
            {
                LoadDefaultImage();
                return;
            }

            try
            {
                string imagePath = ImageManager.Instance.FindImageByHash(photoHash);

                if (imagePath != null && File.Exists(imagePath))
                {
                    byte[] imageData = File.ReadAllBytes(imagePath);
                    UpdatePictureBox(imageData);
                    selectedImageName = Path.GetFileName(imagePath);
                    selectedImageHash = photoHash;
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
                string plugImagePath = ImageManager.Instance.GetPlugImagePath();
                if (plugImagePath != null && File.Exists(plugImagePath))
                {
                    byte[] imageData = File.ReadAllBytes(plugImagePath);
                    UpdatePictureBox(imageData);
                    selectedImageName = "plug.png";
                    selectedImageHash = ImageManager.Instance.CalculateImageHash(imageData);
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

                    if (!string.IsNullOrEmpty(selectedImageHash) && selectedImageHash != oldImageHash)
                    {
                        MySqlCommand checkHashCmd = new MySqlCommand(
                            "SELECT COUNT(*) FROM MenuDish WHERE DishPhoto = @hash AND DishId <> @id", con);
                        checkHashCmd.Parameters.AddWithValue("@hash", selectedImageHash);
                        checkHashCmd.Parameters.AddWithValue("@id", mode == "edit" ? DishID : 0);

                        int hashCount = Convert.ToInt32(checkHashCmd.ExecuteScalar());
                        if (hashCount > 0)
                        {
                            MessageBox.Show("Данное изображение уже используется для другого блюда!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
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
                                @photoHash
                            );", con);

                        cmd.Parameters.AddWithValue("@name", DishName.Trim());
                        cmd.Parameters.AddWithValue("@desc", DishDescription.Trim());
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@category", DishCategory);
                        cmd.Parameters.AddWithValue("@offer", offerValue);
                        cmd.Parameters.AddWithValue("@photoHash", selectedImageHash ?? "");
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
                                DishPhoto = @photoHash
                            WHERE DishId = @id;", con);

                        cmd.Parameters.AddWithValue("@name", DishName.Trim());
                        cmd.Parameters.AddWithValue("@desc", DishDescription.Trim());
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@category", DishCategory);
                        cmd.Parameters.AddWithValue("@offer", offerValue);
                        cmd.Parameters.AddWithValue("@photoHash", selectedImageHash ?? "");
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
                !Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-,\s]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-,.\s]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !Regex.IsMatch(e.KeyChar.ToString(), @"^[0-9,]$"))
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
                    if (!ImageManager.Instance.ValidateImageFile(ofd.FileName))
                    {
                        MessageBox.Show("Недопустимый тип файла или размер превышает 3 МБ! Разрешены только JPG и PNG изображения.",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        byte[] imageData = File.ReadAllBytes(ofd.FileName);
                        string imageHash = ImageManager.Instance.CalculateImageHash(imageData);

                        using (var con = new MySqlConnection(connStr.ConnectionString))
                        {
                            con.Open();
                            using (var cmd = new MySqlCommand(
                                "SELECT DishId FROM MenuDish WHERE DishPhoto = @hash AND DishId != @id;", con))
                            {
                                cmd.Parameters.AddWithValue("@hash", imageHash);
                                cmd.Parameters.AddWithValue("@id", mode == "edit" ? DishID : 0);
                                object exists = cmd.ExecuteScalar();
                                if (exists != null)
                                {
                                    MessageBox.Show("Данное изображение уже используется для другого блюда!\nВыберите другое изображение.",
                                        "Изображение занято", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }

                        string existingFileName = ImageManager.Instance.FindExistingImageByHash(imageHash);
                        string finalFileName;
                        string originalFileName = Path.GetFileNameWithoutExtension(ofd.FileName);
                        string extension = Path.GetExtension(ofd.FileName);

                        if (existingFileName != null)
                        {
                            finalFileName = existingFileName;
                        }
                        else
                        {
                            finalFileName = ImageManager.Instance.GenerateUniqueFileName(originalFileName, extension);
                            ImageManager.Instance.SaveImageToMenuDirectory(imageData, finalFileName);
                        }

                        UpdatePictureBox(imageData);

                        selectedImageName = finalFileName;
                        selectedImageHash = imageHash;
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