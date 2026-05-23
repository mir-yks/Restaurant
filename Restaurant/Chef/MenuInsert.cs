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
        private Form parentForm;
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
                 string category = "", string offer = "", string photoHash = "", Form parentForm = null)
        {
            InitializeComponent();
            this.mode = mode;
            this.parentForm = parentForm;

            if (parentForm != null)
            {
                BlurEffect.ShowDimmed(parentForm);
            }

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

            KeyboardLayoutManager.AttachRussianLayout(textBoxName, textBoxDescription, comboBoxCategory);

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
                selectedImageHash = string.IsNullOrWhiteSpace(photoHash) ? null : photoHash;
                oldImageHash = string.IsNullOrWhiteSpace(photoHash) ? null : photoHash;

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
                    selectedImageHash = null;
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
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
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
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
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
            this.Close();
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

            string priceText = textBoxPrice.Text.Trim();

            if (!Regex.IsMatch(priceText, @"^\d*\.?\d*$"))
            {
                MessageBox.Show("Цена может содержать только цифры и не более одной точки!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPrice.Focus();
                return;
            }

            int dotIndex = priceText.IndexOf('.');
            if (dotIndex != -1)
            {
                string beforeDot = priceText.Substring(0, dotIndex);
                if (beforeDot.Length > 8)
                {
                    MessageBox.Show("Целая часть цены не может превышать 8 символов!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxPrice.Focus();
                    return;
                }

                string afterDot = priceText.Substring(dotIndex + 1);
                if (afterDot.Length > 2)
                {
                    MessageBox.Show("Дробная часть цены не может превышать 2 символа!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxPrice.Focus();
                    return;
                }
            }
            else
            {
                if (priceText.Length > 8)
                {
                    MessageBox.Show("Целая часть цены не может превышать 8 символов!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxPrice.Focus();
                    return;
                }
            }

            if (priceText.StartsWith("."))
            {
                priceText = "0" + priceText;
            }

            decimal price;
            try
            {
                if (!decimal.TryParse(priceText.Replace(',', '.'),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out price))
                {
                    MessageBox.Show("Введите корректную цену!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxPrice.Focus();
                    return;
                }
            }
            catch (OverflowException)
            {
                MessageBox.Show("Слишком большое значение цены! Максимально допустимая цена: 99999999.99",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPrice.Focus();
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Введите корректную цену!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPrice.Focus();
                return;
            }

            if (price <= 0)
            {
                MessageBox.Show("Цена должна быть больше 0!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPrice.Focus();
                return;
            }

            if (price > 99999999.99m)
            {
                MessageBox.Show("Слишком большое значение цены! Максимально допустимая цена: 99999999.99",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
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
            INSERT INTO MenuDish (DishName, OriginalDishName, DishDescription, DishPrice, DishCategory, OffersDish, DishPhoto)
            VALUES (
                @name,
                @originalName,
                @desc,
                @price,
                (SELECT CategoryDishId FROM CategoryDish WHERE CategoryDishName = @category),
                (SELECT OffersDishId FROM OffersDish WHERE OffersDishName = @offer),
                @photoHash
            );", con);

                        cmd.Parameters.AddWithValue("@name", DishName.Trim());
                        cmd.Parameters.AddWithValue("@originalName", DishName.Trim());
                        cmd.Parameters.AddWithValue("@desc", DishDescription.Trim());
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@category", DishCategory);
                        cmd.Parameters.AddWithValue("@offer", offerValue);
                        cmd.Parameters.AddWithValue("@photoHash", string.IsNullOrEmpty(selectedImageHash) ? "" : selectedImageHash);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Блюдо \"{DishName}\" успешно добавлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        bool nameChanged = false;
                        string originalName = DishName;

                        MySqlCommand getOriginalCmd = new MySqlCommand(
                            "SELECT DishName, OriginalDishName FROM MenuDish WHERE DishId = @Id", con);
                        getOriginalCmd.Parameters.AddWithValue("@Id", DishID);

                        using (var reader = getOriginalCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string currentName = reader.GetString("DishName");
                                string storedOriginalName = reader.IsDBNull(reader.GetOrdinal("OriginalDishName"))
                                    ? currentName
                                    : reader.GetString("OriginalDishName");

                                if (currentName != DishName)
                                {
                                    nameChanged = true;
                                    originalName = storedOriginalName;
                                }
                                else
                                {
                                    originalName = storedOriginalName;
                                }
                            }
                        }

                        MySqlCommand cmd = new MySqlCommand(@"
            UPDATE MenuDish
            SET 
                DishName = @name,
                OriginalDishName = @originalName,
                DishDescription = @desc,
                DishPrice = @price,
                DishCategory = (SELECT CategoryDishId FROM CategoryDish WHERE CategoryDishName = @category),
                OffersDish = (SELECT OffersDishId FROM OffersDish WHERE OffersDishName = @offer),
                DishPhoto = @photoHash
            WHERE DishId = @id;", con);

                        cmd.Parameters.AddWithValue("@name", DishName.Trim());
                        cmd.Parameters.AddWithValue("@originalName", originalName);
                        cmd.Parameters.AddWithValue("@desc", DishDescription.Trim());
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@category", DishCategory);
                        cmd.Parameters.AddWithValue("@offer", offerValue);
                        cmd.Parameters.AddWithValue("@photoHash", selectedImageHash ?? "");
                        cmd.Parameters.AddWithValue("@id", DishID);

                        try
                        {
                            cmd.ExecuteNonQuery();

                            string message = nameChanged
                                ? $"Блюдо \"{DishName}\" успешно обновлено!\n\nПримечание: в существующих заказах останется предыдущее название блюда."
                                : $"Блюдо \"{DishName}\" успешно обновлено!";

                            MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (MySqlException mysqlEx)
                        {
                            if (mysqlEx.Number == 1264)
                            {
                                MessageBox.Show("Значение цены выходит за допустимые пределы! Максимально допустимая цена: 99,999,999.99",
                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            throw;
                        }
                    }

                    this.Close();
                }
            }
            catch (MySqlException mysqlEx)
            {
                if (mysqlEx.Number == 1264)
                {
                    MessageBox.Show("Значение цены выходит за допустимые пределы! Проверьте корректность введенной цены.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(mysqlEx.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            TextBox textBox = (TextBox)sender;
            string currentText = textBox.Text;

            if (e.KeyChar == '.')
            {
                if (currentText.Contains('.'))
                {
                    e.Handled = true;
                    return;
                }

                if (string.IsNullOrEmpty(currentText))
                {
                    textBox.Text = "0.";
                    textBox.SelectionStart = textBox.Text.Length;
                    e.Handled = true;
                    return;
                }

                if (textBox.SelectionStart == 0)
                {
                    textBox.Text = "0." + currentText;
                    textBox.SelectionStart = 2;
                    e.Handled = true;
                    return;
                }
            }

            if (char.IsDigit(e.KeyChar) || e.KeyChar == '.')
            {
                string newText;
                int selectionStart = textBox.SelectionStart;
                int selectionLength = textBox.SelectionLength;

                if (selectionLength > 0)
                {
                    newText = currentText.Remove(selectionStart, selectionLength)
                                        .Insert(selectionStart, e.KeyChar.ToString());
                }
                else
                {
                    newText = currentText.Insert(selectionStart, e.KeyChar.ToString());
                }

                int dotIndex = newText.IndexOf('.');

                if (dotIndex != -1)
                {
                    string beforeDot = newText.Substring(0, dotIndex);
                    string afterDot = newText.Substring(dotIndex + 1);

                    if (beforeDot.Length > 8)
                    {
                        e.Handled = true;
                        return;
                    }

                    if (afterDot.Length > 2)
                    {
                        e.Handled = true;
                        return;
                    }
                }
                else
                {
                    if (newText.Length > 8)
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
        }

        private void buttonImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                ofd.Title = "Выберите фото для блюда";

                InactivityManager.PauseTimer();

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    InactivityManager.ResumeTimer();

                    if (!ImageManager.Instance.ValidateImageFile(ofd.FileName))
                    {
                        MessageBox.Show(
                            "Невозможно обработать изображение.\n\n" +
                            "Файл имеет слишком большое разрешение или не поддается автоматическому сжатию.\n" +
                            "Разрешены только JPG и PNG изображения.",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }

                    try
                    {
                        byte[] originalData = File.ReadAllBytes(ofd.FileName);

                        byte[] imageData = ImageManager.Instance.CompressImageIfNeeded(originalData);
                        string imageHash = ImageManager.Instance.CalculateImageHash(imageData);

                        if (!string.IsNullOrEmpty(imageHash))
                        {
                            using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
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
                        InactivityManager.ResumeTimer();
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

        private void comboBoxCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я\s]$"))
            {
                e.Handled = true;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (parentForm != null)
            {
                BlurEffect.HideDimmed();
            }
        }
    }
}