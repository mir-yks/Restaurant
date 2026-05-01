using System;
using System.Drawing;
using System.Windows.Forms;

namespace Restaurant
{
    public static class BlurEffect
    {
        private static Form overlayForm;

        public static void ShowDimmed(Form parentForm)
        {
            if (overlayForm != null) return;

            overlayForm = new Form();
            overlayForm.FormBorderStyle = FormBorderStyle.None;
            overlayForm.StartPosition = FormStartPosition.Manual;
            overlayForm.Location = parentForm.Location;
            overlayForm.Size = parentForm.Size;
            overlayForm.BackColor = Color.Black;
            overlayForm.Opacity = 0.5;
            overlayForm.ShowInTaskbar = false;
            overlayForm.Show(parentForm);

            parentForm.BringToFront();
        }

        public static void HideDimmed()
        {
            if (overlayForm != null)
            {
                overlayForm.Close();
                overlayForm.Dispose();
                overlayForm = null;
            }
        }
    }
}