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
        public int WorkerID { get; set; }
        public WorkerInsert(string mode)
        {
            InitializeComponent();
            this.mode = mode;

            labelLogin.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPassword.Font = Fonts.MontserratAlternatesRegular(14f);
            labelRole.Font = Fonts.MontserratAlternatesRegular(14f);
            labelBirthday.Font = Fonts.MontserratAlternatesRegular(14f);
            labelEmployment.Font = Fonts.MontserratAlternatesRegular(14f);
            labelConfPassword.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPhone.Font = Fonts.MontserratAlternatesRegular(14f);
            labelAddress.Font = Fonts.MontserratAlternatesRegular(14f);
            labelFIO.Font = Fonts.MontserratAlternatesRegular(14f);
            labelEmail.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxFIO.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxLogin.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxPassword.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxConfPassword.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxEmail.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxAddress.Font = Fonts.MontserratAlternatesRegular(14f);
            maskedTextBoxPhone.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxRole.Font = Fonts.MontserratAlternatesRegular(14f);
            dateTimePickerBirthday.Font = Fonts.MontserratAlternatesRegular(14f);
            dateTimePickerEmployment.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);

            dateTimePickerBirthday.MaxDate = DateTime.Today.AddYears(-14);
            dateTimePickerBirthday.MinDate = DateTime.Today.AddYears(-120);
            dateTimePickerEmployment.MaxDate = DateTime.Today;

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
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
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
                    textBoxEmail.ReadOnly = true;
                    textBoxAddress.ReadOnly = true;

                    comboBoxRole.Enabled = false;
                    dateTimePickerBirthday.Enabled = false;

                    comboBoxRole.Location = new System.Drawing.Point(16, 235);
                    labelRole.Location = new System.Drawing.Point(12, 209);
                    textBoxAddress.Location = new System.Drawing.Point(16, 168);
                    labelAddress.Location = new System.Drawing.Point(12, 142);

                    buttonBack.Text = "Закрыть";
                    break;

                case "add":
                    textBoxFIO.Text = "";
                    textBoxLogin.Text = "";
                    textBoxPassword.Text = "";
                    textBoxConfPassword.Text = "";
                    maskedTextBoxPhone.Text = "";
                    textBoxEmail.Text = "";
                    textBoxAddress.Text = "";
                    comboBoxRole.SelectedIndex = 0;
                    dateTimePickerEmployment.Value = DateTime.Today;

                    comboBoxRole.Enabled = true;
                    break;

                case "edit":
                    dateTimePickerBirthday.Enabled = false;

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

        public string WorkerEmail
        {
            get => textBoxEmail.Text;
            set => textBoxEmail.Text = value;
        }

        public DateTime WorkerBirthday
        {
            get => dateTimePickerBirthday.Value;
            set => dateTimePickerBirthday.Value = value;
        }

        public DateTime WorkerDateEmployment
        {
            get => dateTimePickerEmployment.Value;
            set => dateTimePickerEmployment.Value = value;
        }

        public string WorkerAddress
        {
            get => textBoxAddress.Text;
            set => textBoxAddress.Text = value;
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
            this.DialogResult = DialogResult.OK;
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

            if (string.IsNullOrWhiteSpace(textBoxEmail.Text))
            {
                MessageBox.Show("Введите почту!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxEmail.Focus();
                return;
            }
            if (comboBoxRole.SelectedIndex == -1)
            {
                MessageBox.Show("Введите роль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxRole.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBoxAddress.Text))
            {
                MessageBox.Show("Введите адрес!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxAddress.Focus();
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
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    string checkLoginQuery = @"SELECT COUNT(*) FROM worker WHERE WorkerLogin = @Login {0}";
                    string checkPhoneQuery = @"SELECT COUNT(*) FROM worker WHERE WorkerPhone = @Phone {0}";
                    string checkEmailQuery = @"SELECT COUNT(*) FROM worker WHERE WorkerEmail = @Email {0}";

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

                    using (MySqlCommand checkEmailCmd = new MySqlCommand(string.Format(checkEmailQuery, excludeCondition), con))
                    {
                        checkEmailCmd.Parameters.AddWithValue("@Email", textBoxEmail.Text);
                        if (mode == "edit")
                            checkEmailCmd.Parameters.AddWithValue("@Id", WorkerID);

                        int emailCount = Convert.ToInt32(checkEmailCmd.ExecuteScalar());
                        if (emailCount > 0)
                        {
                            MessageBox.Show("Пользователь с такой почтой уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBoxEmail.Focus();
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
                (WorkerFIO, WorkerLogin, WorkerPassword, WorkerPhone, WorkerEmail, 
                 WorkerBirthday, WorkerDateEmployment, WorkerAddress, WorkerRole)
                 VALUES (@FIO, @Login, @Password, @Phone, @Email, 
                         @Birthday, @Employment, @Address, 
                         (SELECT RoleId FROM role WHERE RoleName = @Role))";

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@FIO", textBoxFIO.Text);
                        cmd.Parameters.AddWithValue("@Login", textBoxLogin.Text);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        cmd.Parameters.AddWithValue("@Phone", userDigits);
                        cmd.Parameters.AddWithValue("@Email", textBoxEmail.Text);
                        cmd.Parameters.AddWithValue("@Birthday", dateTimePickerBirthday.Value);
                        cmd.Parameters.AddWithValue("@Employment", dateTimePickerEmployment.Value);
                        cmd.Parameters.AddWithValue("@Address", textBoxAddress.Text);
                        cmd.Parameters.AddWithValue("@Role", comboBoxRole.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Сотрудник \"{textBoxFIO.Text}\" успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        string query = @"UPDATE worker 
                     SET WorkerFIO = @FIO,
                         WorkerLogin = @Login,
                         WorkerPhone = @Phone,
                         WorkerEmail = @Email,
                         WorkerBirthday = @Birthday,
                         WorkerDateEmployment = @Employment,
                         WorkerAddress = @Address,
                         WorkerRole = (SELECT RoleId FROM role WHERE RoleName = @Role)
                         {0}
                     WHERE WorkerId = @Id";

                        string passwordPart = !string.IsNullOrEmpty(hashedPassword) ? ", WorkerPassword = @Password" : "";
                        query = string.Format(query, passwordPart);

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@FIO", textBoxFIO.Text);
                        cmd.Parameters.AddWithValue("@Login", textBoxLogin.Text);
                        cmd.Parameters.AddWithValue("@Phone", userDigits);
                        cmd.Parameters.AddWithValue("@Email", textBoxEmail.Text);
                        cmd.Parameters.AddWithValue("@Birthday", dateTimePickerBirthday.Value);
                        cmd.Parameters.AddWithValue("@Employment", dateTimePickerEmployment.Value);
                        cmd.Parameters.AddWithValue("@Address", textBoxAddress.Text);

                        string roleToUse = originalRole.Equals("Администратор", StringComparison.OrdinalIgnoreCase)
                            ? originalRole
                            : comboBoxRole.Text;
                        cmd.Parameters.AddWithValue("@Role", roleToUse);

                        cmd.Parameters.AddWithValue("@Id", WorkerID);
                        if (!string.IsNullOrEmpty(hashedPassword))
                            cmd.Parameters.AddWithValue("@Password", hashedPassword);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Данные сотрудника успешно обновлены!\nФИО: \"{textBoxFIO.Text}\"", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void textBoxPasswdAndEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z0-9!@#$%^&*()\-_=+\[\]{}|;:,.<>?]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я0-9-,.\s]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxFIO_TextChanged(object sender, EventArgs e)
        {
            int cursorPos = textBoxFIO.SelectionStart;

            string input = textBoxFIO.Text;
            bool showSpaceWarning = false;
            bool showDashWarning = false;

            int spaceCount = input.Count(c => c == ' ');
            if (spaceCount > 2)
            {
                int lastSpace = input.LastIndexOf(' ');
                input = input.Remove(lastSpace, 1);
                showSpaceWarning = true;
            }

            int dashCount = input.Count(c => c == '-');
            if (dashCount > 1)
            {
                int lastDash = input.LastIndexOf('-');
                input = input.Remove(lastDash, 1);
                showDashWarning = true;
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

            if (showSpaceWarning)
                InputTooltipHelper.Show(textBoxFIO, "Можно использовать не более двух пробелов.");
            if (showDashWarning)
                InputTooltipHelper.Show(textBoxFIO, "Можно использовать только одно тире.");
        }
    }
}