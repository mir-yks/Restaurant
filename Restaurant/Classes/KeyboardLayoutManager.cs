using System;
using System.Linq;
using System.Windows.Forms;

namespace Restaurant
{
    public static class KeyboardLayoutManager
    {
        public static void SetRussianLayout(Control control)
        {
            if (control == null) return;

            try
            {
                foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
                {
                    if (lang.Culture.Name == "ru-RU")
                    {
                        InputLanguage.CurrentInputLanguage = lang;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка переключения раскладки: {ex.Message}");
            }
        }

        public static void SetEnglishLayout(Control control)
        {
            if (control == null) return;

            try
            {
                foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
                {
                    if (lang.Culture.Name == "en-US")
                    {
                        InputLanguage.CurrentInputLanguage = lang;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка переключения раскладки: {ex.Message}");
            }
        }

        public static void OnEnterSetRussian(object sender, EventArgs e)
        {
            if (sender is Control control)
            {
                SetRussianLayout(control);
            }
        }

        public static void OnEnterSetEnglish(object sender, EventArgs e)
        {
            if (sender is Control control)
            {
                SetEnglishLayout(control);
            }
        }

        public static void AttachRussianLayout(params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Enter += OnEnterSetRussian;
            }
        }

        public static void AttachEnglishLayout(params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Enter += OnEnterSetEnglish;
            }
        }
    }
}