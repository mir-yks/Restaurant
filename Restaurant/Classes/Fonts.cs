using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Restaurant
{
    public static class Fonts
    {
        private static PrivateFontCollection fontCollection = new PrivateFontCollection();
        private static bool loaded = false;
        private static FontFamily MontserratAlternatesFamily;

        public static void Load()
        {
            if (loaded) return;

            try
            {
                string basePath = Path.Combine(Application.StartupPath, "Resources", "Fonts");

                // Загружаем все MontserratAlternates-файлы
                fontCollection.AddFontFile(Path.Combine(basePath, "MontserratAlternates-Regular.ttf"));
                fontCollection.AddFontFile(Path.Combine(basePath, "MontserratAlternates-Bold.ttf"));
                fontCollection.AddFontFile(Path.Combine(basePath, "MontserratAlternates-Black.ttf"));

                // Получаем первый доступный шрифт MontserratAlternates
                MontserratAlternatesFamily = fontCollection.Families.FirstOrDefault();

                if (MontserratAlternatesFamily == null)
                    throw new Exception("Не удалось инициализировать семейство шрифтов MontserratAlternates.");

                loaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки шрифтов: " + ex.Message);
            }
        }

        public static Font MontserratAlternatesRegular(float size)
        {
            Load();
            return new Font(MontserratAlternatesFamily, size, FontStyle.Regular);
        }

        public static Font MontserratAlternatesBold(float size)
        {
            Load();
            return new Font(MontserratAlternatesFamily, size, FontStyle.Bold);
        }

        public static Font MontserratAlternatesBlack(float size)
        {
            Load();
            return new Font(MontserratAlternatesFamily, size, FontStyle.Bold | FontStyle.Italic);
        }
    }
}
