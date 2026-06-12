using System;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;

namespace Restaurant
{
    public partial class Autorizathion : Form
    {
        private bool passwordVisible = false;
        private int failedAttempts = 0;
        private string currentCaptcha = "";
        private Random random = new Random();   
        private Timer blockTimer;
        public Autorizathion()
        {
            InitializeComponent();

            labelLogin.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            labelCaptcha.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxLogin.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxPassword.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxCaptcha.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonEnter.Font = Fonts.MontserratAlternatesBold(12f);

            KeyboardLayoutManager.AttachEnglishLayout(textBoxLogin, textBoxPassword, textBoxCaptcha);

            blockTimer = new Timer();
            blockTimer.Interval = 10000;
            blockTimer.Tick += BlockTimer_Tick;

            textBoxPassword.PasswordChar = '*';
        }

        private void Autorizathion_Load(object sender, EventArgs e)
        {
            CheckConnectionBeforeShow();
        }

        private void CheckConnectionBeforeShow()
        {
            if (!DatabaseChecker.QuickCheck())
            {
                DialogResult res = MessageBox.Show(
                    "Отсутствует подключение к базе данных.\nПерейти к настройкам?",
                    "Ошибка подключения",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error);

                if (res == DialogResult.Yes)
                {
                    SettingsForm settingsForm = new SettingsForm();
                    settingsForm.ShowDialog();

                    if (!DatabaseChecker.QuickCheck())
                    {
                        DialogResult exitRes = MessageBox.Show(
                            "Подключение не установлено. Завершить работу приложения?",
                            "Ошибка подключения",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Error);

                        if (exitRes == DialogResult.Yes)
                        {
                            Application.Exit();
                        }
                    }
                }
                else
                {
                    Application.Exit();
                }
            }

            this.BeginInvoke(new Action(() => {
                DatabaseChecker.CheckConnectionWithMessage();
            }));
        }
        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите выйти?", "Подтверждение выхода", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxLogin.Text) || string.IsNullOrEmpty(textBoxPassword.Text))
                {
                    MessageBox.Show("Введите логин и пароль для входа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!DatabaseChecker.QuickCheck())
                {
                    DialogResult res = MessageBox.Show(
                        "Отсутствует подключение к базе данных.\nПерейти к настройкам?",
                        "Ошибка подключения",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error);

                    if (res == DialogResult.Yes)
                    {
                        SettingsForm settingsForm = new SettingsForm();
                        settingsForm.ShowDialog();
                        if (!DatabaseChecker.QuickCheck())
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                if (failedAttempts >= 2)
                {
                    if (string.IsNullOrWhiteSpace(textBoxCaptcha.Text))
                    {
                        MessageBox.Show("Введите captcha!", "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        return;
                    }

                    if (textBoxCaptcha.Text.Trim().ToUpper() != currentCaptcha)
                    {
                        MessageBox.Show(
                            "Неудачная авторизация. Вход заблокирован на 10 секунд.",
                            "Ошибка авторизации",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        BlockAuthorization();

                        LoadRandomCaptcha();
                        textBoxCaptcha.Clear();

                        return;
                    }
                }

                string login = textBoxLogin.Text;
                string passwd = textBoxPassword.Text;

                if (login == "admin" && passwd == "admin")
                {
                    failedAttempts = 0;

                    textBoxLogin.Clear();
                    textBoxPassword.Clear();
                    textBoxCaptcha.Clear();

                    this.Hide();

                    ManagementBD form = new ManagementBD();
                    form.ShowDialog();

                    this.Show();

                    return;
                }

                string hash_pass;
                using (var sha256 = SHA256.Create())
                {
                    var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwd));
                    hash_pass = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }

                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT WorkerPassword, WorkerRole, WorkerFIO, WorkerId FROM Worker WHERE WorkerLogin = @login;", con);
                    cmd.Parameters.AddWithValue("@login", login);

                    DataTable dt = new DataTable();
                    new MySqlDataAdapter(cmd).Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        failedAttempts++;

                        if (failedAttempts == 1)
                        {
                            MessageBox.Show(
                                "Пользователя с таким логином не существует!",
                                "Ошибка авторизации",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                        else if (failedAttempts == 2)
                        {
                            MessageBox.Show(
                                "Повторная ошибка авторизации. Теперь требуется ввод captcha.",
                                "Ошибка авторизации",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            ShowCaptcha();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Неудачная авторизация. Вход заблокирован на 10 секунд.",
                                "Ошибка авторизации",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            BlockAuthorization();
                        }

                        textBoxLogin.Clear();
                        textBoxPassword.Clear();
                        return;
                    }

                    string passwordHashInDB = dt.Rows[0]["WorkerPassword"].ToString();
                    int userRole = Convert.ToInt32(dt.Rows[0]["WorkerRole"]);
                    string workerFIO = dt.Rows[0]["WorkerFIO"].ToString();

                    if (hash_pass != passwordHashInDB)
                    {
                        failedAttempts++;

                        if (failedAttempts == 1)
                        {
                            MessageBox.Show(
                                "Введен неверный пароль!",
                                "Ошибка авторизации",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                        else if (failedAttempts == 2)
                        {
                            MessageBox.Show(
                                "Повторная ошибка авторизации. Теперь требуется ввод captcha.",
                                "Ошибка авторизации",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            ShowCaptcha();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Неудачная авторизация. Вход заблокирован на 10 секунд.",
                                "Ошибка авторизации",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            BlockAuthorization();
                        }

                        textBoxPassword.Clear();
                        return;
                    }

                    DatabaseCleanup.CleanExpiredBookings();
                    TableStatusUpdater.UpdateTablesStatus();

                    string roleName = "";
                    switch (userRole)
                    {
                        case 1: roleName = "Администратор"; break;
                        case 2: roleName = "Менеджер"; break;
                        case 3: roleName = "Официант"; break;
                        case 4: roleName = "Шеф-повар"; break;
                    }

                    int userID = Convert.ToInt32(dt.Rows[0]["WorkerId"]);

                    failedAttempts = 0;
                    HideCaptcha();

                    Form nextForm = new Desktop(workerFIO, userRole, roleName, userID);

                    this.Visible = false;
                    nextForm.ShowDialog();
                    textBoxLogin.Clear();
                    textBoxPassword.Clear();
                    this.Visible = true;
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1049)
                {
                    MessageBox.Show($"База данных 'db57' не найдена. Проверьте наличие базы данных.",
                        "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z0-9@._-]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z0-9!@#$%^&*()\-_=+\[\]{}|;:,.<>?]$"))
            {
                e.Handled = true;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            passwordVisible = !passwordVisible;

            if (passwordVisible)
            {
                textBoxPassword.PasswordChar = '\0';
                pictureBox.BackgroundImage = Properties.Resources.eye;
            }
            else
            {
                textBoxPassword.PasswordChar = '*';
                pictureBox.BackgroundImage = Properties.Resources.eye_closed;
            }
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            SettingsForm SettingForm = new SettingsForm();
            SettingForm.ShowDialog();
            DatabaseChecker.CheckConnectionWithMessage();
        }

        private void ShowCaptcha()
        {
            if (labelCaptcha.Visible)
                return;

            labelCaptcha.Visible = true;
            textBoxCaptcha.Visible = true;
            pictureBoxCaptcha.Visible = true;

            this.Height += 100;
            this.CenterToScreen();

            buttonEnter.Location = new Point(95, 430);

            LoadRandomCaptcha();
        }

        private void LoadRandomCaptcha()
        {
            string captchaFolder =
                Path.Combine(
                    Application.StartupPath,
                    "Resources",
                    "image",
                    "captcha");

            string[] files = Directory.GetFiles(captchaFolder, "*.png");

            if (files.Length == 0)
                return;

            string selectedFile = files[random.Next(files.Length)];

            pictureBoxCaptcha.Image = Image.FromFile(selectedFile);

            currentCaptcha =
                Path.GetFileNameWithoutExtension(selectedFile)
                .ToUpper();
        }

        private void BlockTimer_Tick(object sender, EventArgs e)
        {
            blockTimer.Stop();

            textBoxLogin.Enabled = true;
            textBoxPassword.Enabled = true;
            textBoxCaptcha.Enabled = true;

            buttonEnter.Enabled = true;
            buttonSettings.Enabled = true;
            buttonExit.Enabled = true;

            pictureBox.Enabled = true;

            MessageBox.Show(
                "Вход снова доступен.",
                "Разблокировка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BlockAuthorization()
        {
            textBoxLogin.Enabled = false;
            textBoxPassword.Enabled = false;
            textBoxCaptcha.Enabled = false;

            buttonEnter.Enabled = false;
            buttonSettings.Enabled = false;
            buttonExit.Enabled = false;

            pictureBox.Enabled = false;

            textBoxLogin.Clear();
            textBoxPassword.Clear();
            textBoxCaptcha.Clear();

            blockTimer.Start();
        }

        private void HideCaptcha()
        {
            labelCaptcha.Visible = false;
            textBoxCaptcha.Visible = false;
            pictureBoxCaptcha.Visible = false;

            textBoxCaptcha.Clear();

            this.Height -= 100;
            this.CenterToScreen();

            buttonEnter.Location = new Point(95, 285);
        }

        private void textBoxCaptcha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z0-9]$"))
            {
                e.Handled = true;
            }
        }
    }
}