using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class WorkerInsert : Form
    {
        private string mode;
        private string originalRole;
        private Form parentForm;
        public int WorkerID { get; set; }
        public WorkerInsert(string mode, Form parentForm = null)
        {
            InitializeComponent();
            this.mode = mode;
            this.parentForm = parentForm;
            InactivityManager.Init();

            if (parentForm != null)
            {
                BlurEffect.ShowDimmed(parentForm);
            }

            labelLogin.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPassword.Font = Fonts.MontserratAlternatesRegular(14f);
            labelRole.Font = Fonts.MontserratAlternatesRegular(14f);
            labelConfPassword.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPhone.Font = Fonts.MontserratAlternatesRegular(14f);
            labelFIO.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPassport.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxFIO.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxLogin.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxPassword.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxConfPassword.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxPassport.Font = Fonts.MontserratAlternatesRegular(14f);
            maskedTextBoxPhone.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxRole.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);

            LoadRoles();
            ApplyMode();

            if (mode == "edit")
            {
                CheckAndLockRoleComboBox();
            }
        }

        private void LoadRoles()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();
                    DataTable rolesTable = new DataTable();
                    MySqlDataAdapter daRoles = new MySqlDataAdapter("SELECT RoleName FROM role;", con);
                    daRoles.Fill(rolesTable);

                    comboBoxRole.Items.Clear();
                    foreach (DataRow row in rolesTable.Rows)
                    {
                        comboBoxRole.Items.Add(row["RoleName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyMode()
        {
            switch (mode)
            {
                case "view":
                    labelPassword.Visible = false;
                    labelConfPassword.Visible = false;
                    textBoxPassword.Visible = false;
                    textBoxConfPassword.Visible = false;
                    buttonWrite.Visible = false;

                    textBoxFIO.ReadOnly = true;
                    textBoxLogin.ReadOnly = true;
                    textBoxPassword.ReadOnly = true;
                    textBoxConfPassword.ReadOnly = true;
                    maskedTextBoxPhone.ReadOnly = true;
                    textBoxPassport.ReadOnly = true;
                    comboBoxRole.Enabled = false;

                    break;

                case "add":
                    textBoxFIO.Text = "";
                    textBoxLogin.Text = "";
                    textBoxPassword.Text = "";
                    textBoxConfPassword.Text = "";
                    maskedTextBoxPhone.Text = "";
                    textBoxPassport.Text = "";
                    comboBoxRole.SelectedIndex = -1;

                    comboBoxRole.Enabled = true;
                    break;

                case "edit":
                    if (string.IsNullOrEmpty(originalRole) && !string.IsNullOrEmpty(comboBoxRole.Text))
                    {
                        originalRole = comboBoxRole.Text;
                    }

                    CheckAndLockRoleComboBox();
                    break;
            }
        }

        private void CheckAndLockRoleComboBox()
        {
            if (mode == "edit" && !string.IsNullOrEmpty(originalRole) &&
                originalRole.Equals("Администратор", StringComparison.OrdinalIgnoreCase))
            {
                comboBoxRole.Enabled = false;
            }
            else if (mode == "edit")
            {
                comboBoxRole.Enabled = true;
            }
        }

        public string WorkerFIO
        {
            get => textBoxFIO.Text;
            set => textBoxFIO.Text = value;
        }

        public string WorkerLogin
        {
            get => textBoxLogin.Text;
            set => textBoxLogin.Text = value;
        }

        public string WorkerPhone
        {
            get => new string(maskedTextBoxPhone.Text.Where(char.IsDigit).ToArray());
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    maskedTextBoxPhone.Text = "";
                    return;
                }

                string digits = new string(value.Where(char.IsDigit).ToArray());

                if (digits.StartsWith("7") && maskedTextBoxPhone.Mask.StartsWith("+7"))
                {
                    digits = digits.Substring(1);
                }

                maskedTextBoxPhone.Text = digits;
            }
        }

        public string WorkerPassport
        {
            get => textBoxPassport.Text;
            set => textBoxPassport.Text = value;
        }

        public string WorkerRole
        {
            get => comboBoxRole.Text;
            set
            {
                comboBoxRole.Text = value;
                if (mode == "edit" && string.IsNullOrEmpty(originalRole))
                {
                    originalRole = value;
                }
                CheckAndLockRoleComboBox();
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxFIO.Text))
            {
                MessageBox.Show("Введите ФИО!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxFIO.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxLogin.Text))
            {
                MessageBox.Show("Введите логин!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxLogin.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBoxPassword.Text) && mode == "add")
            {
                MessageBox.Show("Введите пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPassword.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBoxConfPassword.Text) && mode == "add")
            {
                MessageBox.Show("Введите подтверждение пароля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxConfPassword.Focus();
                return;
            }

            string userDigits = new string(maskedTextBoxPhone.Text.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(maskedTextBoxPhone.Text) || userDigits.Length < 11)
            {
                MessageBox.Show("Введите полный номер телефона!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                maskedTextBoxPhone.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxPassport.Text))
            {
                MessageBox.Show("Введите паспортные данные!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPassport.Focus();
                return;
            }

            if (!IsValidPassport(textBoxPassport.Text))
            {
                MessageBox.Show("Введите корректные паспортные данные!\nФормат: XXXX XXXXXX (4 цифры серии, пробел, 6 цифр номера)",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPassport.Focus();
                return;
            }

            if (comboBoxRole.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите роль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxRole.Focus();
                return;
            }

            if (textBoxPassword.Visible && textBoxConfPassword.Visible)
            {
                string pass = textBoxPassword.Text.Trim();
                string confPass = textBoxConfPassword.Text.Trim();

                if (!string.IsNullOrEmpty(pass) || !string.IsNullOrEmpty(confPass))
                {
                    if (pass != confPass)
                    {
                        MessageBox.Show("Пароль и подтверждение пароля не совпадают!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            var fioParts = WorkerFIO.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (fioParts.Length < 2)
            {
                MessageBox.Show("Введите полное ФИО (минимум фамилия и имя).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxFIO.Focus();
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    string checkLoginQuery = @"SELECT COUNT(*) FROM worker WHERE WorkerLogin = @Login {0}";
                    string checkPhoneQuery = @"SELECT COUNT(*) FROM worker WHERE WorkerPhone = @Phone {0}";
                    string checkPassportQuery = @"SELECT COUNT(*) FROM worker WHERE WorkerPassport = @Passport {0}";

                    string excludeCondition = mode == "edit" ? "AND WorkerId <> @Id" : "";

                    using (MySqlCommand checkLoginCmd = new MySqlCommand(string.Format(checkLoginQuery, excludeCondition), con))
                    {
                        checkLoginCmd.Parameters.AddWithValue("@Login", textBoxLogin.Text);
                        if (mode == "edit")
                            checkLoginCmd.Parameters.AddWithValue("@Id", WorkerID);

                        int loginCount = Convert.ToInt32(checkLoginCmd.ExecuteScalar());
                        if (loginCount > 0)
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBoxLogin.Focus();
                            return;
                        }
                    }

                    using (MySqlCommand checkPhoneCmd = new MySqlCommand(string.Format(checkPhoneQuery, excludeCondition), con))
                    {
                        checkPhoneCmd.Parameters.AddWithValue("@Phone", userDigits);
                        if (mode == "edit")
                            checkPhoneCmd.Parameters.AddWithValue("@Id", WorkerID);

                        int phoneCount = Convert.ToInt32(checkPhoneCmd.ExecuteScalar());
                        if (phoneCount > 0)
                        {
                            MessageBox.Show("Пользователь с таким номером телефона уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            maskedTextBoxPhone.Focus();
                            return;
                        }
                    }

                    using (MySqlCommand checkPassportCmd = new MySqlCommand(string.Format(checkPassportQuery, excludeCondition), con))
                    {
                        checkPassportCmd.Parameters.AddWithValue("@Passport", textBoxPassport.Text);
                        if (mode == "edit")
                            checkPassportCmd.Parameters.AddWithValue("@Id", WorkerID);

                        int passportCount = Convert.ToInt32(checkPassportCmd.ExecuteScalar());
                        if (passportCount > 0)
                        {
                            MessageBox.Show("Пользователь с такими паспортными данными уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBoxPassport.Focus();
                            return;
                        }
                    }

                    DialogResult confirmResult = MessageBox.Show(
                        "Вы действительно хотите сохранить запись?",
                        "Подтверждение записи",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmResult != DialogResult.Yes) return;

                    string hashedPassword = "";
                    if (!string.IsNullOrEmpty(textBoxPassword.Text))
                    {
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(textBoxPassword.Text));
                            hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                        }
                    }

                    if (mode == "add")
                    {
                        string query = @"INSERT INTO worker 
                            (WorkerFIO, OriginalWorkerFIO, WorkerLogin, WorkerPassword, WorkerPhone, WorkerPassport, WorkerRole)
                            VALUES (@FIO, @OriginalFIO, @Login, @Password, @Phone, @Passport, 
                                    (SELECT RoleId FROM role WHERE RoleName = @Role))";

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@FIO", textBoxFIO.Text);
                        cmd.Parameters.AddWithValue("@OriginalFIO", textBoxFIO.Text);
                        cmd.Parameters.AddWithValue("@Login", textBoxLogin.Text);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        cmd.Parameters.AddWithValue("@Phone", userDigits);
                        cmd.Parameters.AddWithValue("@Passport", textBoxPassport.Text);
                        cmd.Parameters.AddWithValue("@Role", comboBoxRole.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Сотрудник \"{textBoxFIO.Text}\" успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        bool fioChanged = false;
                        string originalFIO = textBoxFIO.Text;

                        MySqlCommand getOriginalCmd = new MySqlCommand(
                            "SELECT WorkerFIO, OriginalWorkerFIO FROM Worker WHERE WorkerId = @Id", con);
                        getOriginalCmd.Parameters.AddWithValue("@Id", WorkerID);

                        using (var reader = getOriginalCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string currentFIO = reader.GetString("WorkerFIO");
                                string storedOriginalFIO = reader.IsDBNull(reader.GetOrdinal("OriginalWorkerFIO"))
                                    ? currentFIO
                                    : reader.GetString("OriginalWorkerFIO");

                                if (currentFIO != textBoxFIO.Text)
                                {
                                    fioChanged = true;
                                    originalFIO = storedOriginalFIO;
                                }
                                else
                                {
                                    originalFIO = storedOriginalFIO;
                                }
                            }
                        }

                        string query = @"UPDATE worker 
                            SET WorkerFIO = @FIO,
                                OriginalWorkerFIO = @OriginalFIO,
                                WorkerLogin = @Login,
                                WorkerPhone = @Phone,
                                WorkerPassport = @Passport,
                                WorkerRole = (SELECT RoleId FROM role WHERE RoleName = @Role)
                                {0}
                            WHERE WorkerId = @Id";

                        string passwordPart = !string.IsNullOrEmpty(hashedPassword) ? ", WorkerPassword = @Password" : "";
                        query = string.Format(query, passwordPart);

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@FIO", textBoxFIO.Text);
                        cmd.Parameters.AddWithValue("@OriginalFIO", originalFIO);
                        cmd.Parameters.AddWithValue("@Login", textBoxLogin.Text);
                        cmd.Parameters.AddWithValue("@Phone", userDigits);
                        cmd.Parameters.AddWithValue("@Passport", textBoxPassport.Text);

                        string roleToUse = originalRole.Equals("Администратор", StringComparison.OrdinalIgnoreCase)
                            ? originalRole
                            : comboBoxRole.Text;
                        cmd.Parameters.AddWithValue("@Role", roleToUse);

                        cmd.Parameters.AddWithValue("@Id", WorkerID);
                        if (!string.IsNullOrEmpty(hashedPassword))
                            cmd.Parameters.AddWithValue("@Password", hashedPassword);

                        cmd.ExecuteNonQuery();

                        string message = fioChanged
                            ? $"Данные сотрудника успешно обновлены!\nФИО: \"{textBoxFIO.Text}\"\n\nПримечание: в существующих заказах останется предыдущее ФИО сотрудника."
                            : $"Данные сотрудника успешно обновлены!\nФИО: \"{textBoxFIO.Text}\"";

                        MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxPassport_TextChanged(object sender, EventArgs e)
        {
            int cursorPos = textBoxPassport.SelectionStart;
            string oldText = textBoxPassport.Text;

            string digitsOnly = new string(oldText.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length > 10)
                digitsOnly = digitsOnly.Substring(0, 10);

            string formatted = "";
            if (digitsOnly.Length > 0)
            {
                formatted = digitsOnly;
                if (digitsOnly.Length >= 4 && digitsOnly.Length <= 10)
                {
                    formatted = digitsOnly.Substring(0, 4);
                    if (digitsOnly.Length > 4)
                    {
                        formatted += " " + digitsOnly.Substring(4);
                    }
                }
            }

            if (oldText == formatted)
                return;

            int newCursorPos = cursorPos;

            if (cursorPos == 4 && oldText.Length >= 4 && formatted.Length > 4 && formatted[4] == ' ')
            {
                newCursorPos = 5; 
            }
            else if (cursorPos > 0 && cursorPos < oldText.Length && oldText[cursorPos] == ' ' && formatted.Length > cursorPos)
            {
                newCursorPos = cursorPos;
            }
            else if (oldText.Length > formatted.Length)
            {
                int diff = oldText.Length - formatted.Length;
                newCursorPos = Math.Max(0, cursorPos - diff);
            }
            else if (oldText.Length < formatted.Length)
            {
                if (cursorPos >= 4 && cursorPos <= 5)
                {
                    newCursorPos = 5;
                }
                else
                {
                    newCursorPos = cursorPos + (formatted.Length - oldText.Length);
                }
            }

            textBoxPassport.TextChanged -= textBoxPassport_TextChanged;
            textBoxPassport.Text = formatted;
            textBoxPassport.SelectionStart = Math.Min(newCursorPos, textBoxPassport.Text.Length);
            textBoxPassport.TextChanged += textBoxPassport_TextChanged;
        }

        private bool IsValidPassport(string passport)
        {
            if (string.IsNullOrWhiteSpace(passport))
                return false;

            string digitsOnly = new string(passport.Where(char.IsDigit).ToArray());

            return digitsOnly.Length == 10;
        }

        private void textBoxFIO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-\s]$"))
            {
                e.Handled = true;
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

        private void textBoxPasswd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z0-9!@#$%^&*()\-_=+\[\]{}|;:,.<>?]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxFIO_TextChanged(object sender, EventArgs e)
        {
            int cursorPos = textBoxFIO.SelectionStart;

            string input = textBoxFIO.Text;

            int spaceCount = input.Count(c => c == ' ');
            if (spaceCount > 2)
            {
                int lastSpace = input.LastIndexOf(' ');
                input = input.Remove(lastSpace, 1);
            }

            int dashCount = input.Count(c => c == '-');
            if (dashCount > 1)
            {
                int lastDash = input.LastIndexOf('-');
                input = input.Remove(lastDash, 1);
            }

            string[] parts = input
                .Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => char.ToUpper(p[0]) + p.Substring(1).ToLower())
                .ToArray();

            string formatted = input;
            int index = 0;
            foreach (string part in parts)
            {
                int pos = formatted.IndexOf(part, index, StringComparison.OrdinalIgnoreCase);
                if (pos >= 0)
                {
                    formatted = formatted.Remove(pos, part.Length).Insert(pos, part);
                    index = pos + part.Length;
                }
            }

            textBoxFIO.TextChanged -= textBoxFIO_TextChanged;
            textBoxFIO.Text = formatted;
            textBoxFIO.SelectionStart = Math.Min(cursorPos, textBoxFIO.Text.Length);
            textBoxFIO.TextChanged += textBoxFIO_TextChanged;
        }

        private void textBoxPassport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void maskedTextBoxPhone_Click(object sender, EventArgs e)
        {
            SetCursorToEnd(maskedTextBoxPhone);
        }

        private void maskedTextBoxPhone_Enter(object sender, EventArgs e)
        {
            SetCursorToEnd(maskedTextBoxPhone);
        }

        private void textBoxPassport_Click(object sender, EventArgs e)
        {
            textBoxPassport.SelectionStart = textBoxPassport.Text.Length;
        }

        private void textBoxPassport_Enter(object sender, EventArgs e)
        {
            textBoxPassport.SelectionStart = textBoxPassport.Text.Length;
        }

        private void SetCursorToEnd(MaskedTextBox mtb)
        {
            mtb.SelectionStart = mtb.Text.Length;

            for (int i = mtb.Text.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(mtb.Text[i]))
                {
                    mtb.SelectionStart = i + 1;
                    break;
                }
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