using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Restaurant
{
    public partial class SettingsForm : Form
    {
        private bool passwordVisible = false;

        public SettingsForm()
        {
            InitializeComponent();

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            label3.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxHost.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxUid.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxPwd.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonEnter.Font = Fonts.MontserratAlternatesBold(12f);
            buttonExit.Font = Fonts.MontserratAlternatesBold(12f);

            KeyboardLayoutManager.AttachEnglishLayout(textBoxHost, textBoxUid, textBoxPwd);

            textBoxPwd.PasswordChar = '*';
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (currentConfig.AppSettings.Settings["host"] != null)
                    textBoxHost.Text = currentConfig.AppSettings.Settings["host"].Value;

                if (currentConfig.AppSettings.Settings["uid"] != null)
                    textBoxUid.Text = currentConfig.AppSettings.Settings["uid"].Value;

                if (currentConfig.AppSettings.Settings["pwd"] != null)
                    textBoxPwd.Text = currentConfig.AppSettings.Settings["pwd"].Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке конфигурации: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            textBoxHost.Focus();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxHost.Text) ||
                    string.IsNullOrWhiteSpace(textBoxUid.Text))
                {
                    MessageBox.Show("Заполните хост и имя пользователя!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string h = textBoxHost.Text.Trim();
                string u = textBoxUid.Text.Trim();
                string p = textBoxPwd.Text; 

                string testConnectionStr;
                if (string.IsNullOrEmpty(p))
                {
                    testConnectionStr = $"host={h};uid={u};";
                }
                else
                {
                    testConnectionStr = $"host={h};uid={u};pwd={p};";
                }

                using (MySqlConnection con = new MySqlConnection(testConnectionStr))
                {
                    con.Open();
                }

                Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                string[] keys = { "host", "uid", "pwd" };
                string[] values = { h, u, p };

                for (int i = 0; i < keys.Length; i++)
                {
                    if (currentConfig.AppSettings.Settings[keys[i]] == null)
                        currentConfig.AppSettings.Settings.Add(keys[i], values[i]);
                    else
                        currentConfig.AppSettings.Settings[keys[i]].Value = values[i];
                }

                currentConfig.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                MessageBox.Show("Настройки подключения успешно сохранены!",
                    "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
                this.Close();
            }
            catch (MySqlException ex)
            {
                string errorMessage = "Ошибка подключения!\n\n";

                switch (ex.Number)
                {
                    case 1042:
                        errorMessage += "Не удается найти указанный хост. Проверьте адрес сервера.";
                        break;
                    case 1045:
                        errorMessage += "Неверное имя пользователя или пароль.";
                        break;
                    case 0:
                        errorMessage += "Сервер MySQL не найден или не запущен.";
                        break;
                    default:
                        errorMessage += ex.Message;
                        break;
                }

                MessageBox.Show(errorMessage, "Ошибка подключения",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            passwordVisible = !passwordVisible;

            if (passwordVisible)
            {
                textBoxPwd.PasswordChar = '\0';
                pictureBox.BackgroundImage = Properties.Resources.eye;
            }
            else
            {
                textBoxPwd.PasswordChar = '*';
                pictureBox.BackgroundImage = Properties.Resources.eye_closed;
            }
        }

        private void textBoxPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z0-9!@#$%^&*()\-_=+\[\]{}|;:,.<>?]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxUid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z0-9@._-]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxHost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z0-9.]$"))
            {
                e.Handled = true;
            }
        }
    }
}