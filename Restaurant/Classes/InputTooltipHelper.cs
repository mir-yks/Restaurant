using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant
{
    public static class InputTooltipHelper
    {
        private static readonly ToolTip tip = new ToolTip();

        public static void Show(Control control, string message, string title = "Ограничение ввода")
        {
            if (control == null) return;

            tip.RemoveAll();
            tip.IsBalloon = true;
            tip.ToolTipIcon = ToolTipIcon.Warning;
            tip.ToolTipTitle = title;

            tip.Show(message, control, control.Width / 3, -35, 2000);
        }
    }
}
