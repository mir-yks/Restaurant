using System;
using System.Linq;
using System.Windows.Forms;

namespace Restaurant
{
    public static class InactivityManager
    {
        private static Timer inactivityTimer;
        private static int inactivityLimit = 30000; 
        private static DateTime lastActivityTime = DateTime.Now;
        private static bool isTimerRunning = false;

        public static void Init()
        {
            if (inactivityTimer != null) return;

            inactivityTimer = new Timer();
            inactivityTimer.Interval = 1000; 
            inactivityTimer.Tick += InactivityTimer_Tick;
            inactivityTimer.Start();

            isTimerRunning = true;
            
            Application.AddMessageFilter(new UserActivityFilter());
        }

        private static void InactivityTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan idleTime = DateTime.Now - lastActivityTime;
            
            if (idleTime.TotalMilliseconds >= inactivityLimit && isTimerRunning)
            {
                LockSystem();
            }
        }

        public static void ResetTimer()
        {
            lastActivityTime = DateTime.Now;
        }

        public static void PauseTimer()
        {
            isTimerRunning = false;
            if (inactivityTimer != null)
            {
                inactivityTimer.Stop();
            }
        }

        public static void ResumeTimer()
        {
            isTimerRunning = true;
            ResetTimer();
            if (inactivityTimer != null)
            {
                inactivityTimer.Start();
            }
        }

        private static void LockSystem()
        {
            PauseTimer();

            Form activeForm = Form.ActiveForm;

            MessageBox.Show("Система заблокирована из-за отсутствия активности более 30 секунд.",
                "Блокировка", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (!form.IsDisposed && form.Visible)
                {
                    form.Hide();
                }
            }

            Autorizathion authForm = new Autorizathion();
            authForm.ShowDialog();

            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (!form.IsDisposed)
                {
                    form.Show();
                }
            }

            if (activeForm != null && !activeForm.IsDisposed)
            {
                activeForm.Focus();
            }

            ResumeTimer();
        }

        private class UserActivityFilter : IMessageFilter
        {
            private const int WM_MOUSEMOVE = 0x0200;
            private const int WM_KEYDOWN = 0x0100;
            private const int WM_LBUTTONDOWN = 0x0201;
            private const int WM_RBUTTONDOWN = 0x0204;
            private const int WM_MBUTTONDOWN = 0x0207;

            public bool PreFilterMessage(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_MOUSEMOVE:
                    case WM_KEYDOWN:
                    case WM_LBUTTONDOWN:
                    case WM_RBUTTONDOWN:
                    case WM_MBUTTONDOWN:
                        ResetTimer();
                        break;
                }
                return false;
            }
        }
    }
}