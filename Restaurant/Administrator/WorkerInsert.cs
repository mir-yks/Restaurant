using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class WorkerInsert : Form
    {
        private string mode;
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

            LoadRoles();
            ApplyMode();
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
                    dateTimePickerEmployment.Enabled = false;
                    
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
                    dateTimePickerBirthday.Value = DateTime.Today;
                    dateTimePickerEmployment.Value = DateTime.Today;

                    buttonWrite.Visible = true; 
                    buttonBack.Text = "Отмена";
                    break;

                case "edit":
                    textBoxFIO.ReadOnly = false;
                    textBoxLogin.ReadOnly = false;
                    textBoxPassword.ReadOnly = false;
                    textBoxConfPassword.ReadOnly = false;
                    maskedTextBoxPhone.ReadOnly = false;
                    textBoxEmail.ReadOnly = false;
                    textBoxAddress.ReadOnly = false;

                    comboBoxRole.Enabled = true;
                    dateTimePickerBirthday.Enabled = true;
                    dateTimePickerEmployment.Enabled = true;

                    buttonWrite.Visible = true; 
                    buttonBack.Text = "Отмена";
                    break;
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
            get => maskedTextBoxPhone.Text;
            set => maskedTextBoxPhone.Text = value;
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
            set => comboBoxRole.Text = value;
        }


        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите сохранить запись?", "Подтверждение записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

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
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-,.\s]$"))
            {
                e.Handled = true;
            }
        }
    }
}