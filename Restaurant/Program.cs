using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuestPDF.Infrastructure;

namespace Restaurant
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Fonts.Load();
            Autorizathion form =
                new Autorizathion();

            form.FormClosing += Form_FormClosing;

            Application.Run(form);
        }

        private static void Form_FormClosing(
            object sender,
            FormClosingEventArgs e)
        {
            try
            {
                string path =
                    BackupManager.CreateBackup();

                MessageBox.Show(
                    $"Резервная копия создана:\n{path}",
                    "Автобэкап",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка автобэкапа",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
