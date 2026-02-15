using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Restaurant
{
    public static class DatabaseChecker
    {

        public static bool CheckConnectionWithMessage()
        {
            try
            {
                string connectionString;
                try
                {
                    connectionString = connStr.ConnectionString;
                }
                catch (Exception)
                {
                    DialogResult res = MessageBox.Show(
                        "Не настроено подключение к базе данных.\nПерейти к настройкам?",
                        "Ошибка конфигурации",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error);

                    if (res == DialogResult.Yes)
                    {
                        SettingsForm settingsForm = new SettingsForm();
                        settingsForm.ShowDialog();
                    }
                    return false;
                }

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                }

                return true;
            }
            catch (MySqlException ex)
            {
                string errorMessage = "Ошибка подключения к базе данных!\n\n";

                switch (ex.Number)
                {
                    case 1042:
                        errorMessage += "Не удается найти указанный хост.";
                        break;
                    case 1045:
                        errorMessage += "Неверное имя пользователя или пароль.";
                        break;
                    case 1049:
                        errorMessage += "База данных не найдена.";
                        break;
                    case 0:
                        errorMessage += "Сервер MySQL не найден или не запущен.";
                        break;
                    default:
                        errorMessage += ex.Message;
                        break;
                }

                DialogResult res = MessageBox.Show(
                    errorMessage + "\n\nПерейти к настройкам?",
                    "Ошибка подключения",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error);

                if (res == DialogResult.Yes)
                {
                    SettingsForm settingsForm = new SettingsForm();
                    settingsForm.ShowDialog();
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public static bool QuickCheck()
        {
            try
            {
                string connectionString = connStr.ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}