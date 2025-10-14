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
    public partial class ClientsInsert : Form
    {
        private string mode;
        public ClientsInsert(string mode)
        {
            InitializeComponent();
            this.mode = mode;

            labelFIO.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPhone.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxFIO.Font = Fonts.MontserratAlternatesRegular(14f);
            maskedTextBoxPhone.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);

            ApplyMode();
        }

        private void ApplyMode()
        {
            switch (mode)
            {
                case "view":
                    textBoxFIO.ReadOnly = true;
                    maskedTextBoxPhone.ReadOnly = true;

                    buttonWrite.Visible = false;

                    buttonBack.Text = "Закрыть";
                    break;

                case "add":
                    textBoxFIO.Text = "";
                    maskedTextBoxPhone.Text = "";

                    textBoxFIO.ReadOnly = false;
                    maskedTextBoxPhone.ReadOnly = false;

                    buttonWrite.Visible = true;
                    buttonBack.Text = "Отмена";
                    break;

                case "edit":
                    textBoxFIO.ReadOnly = false;
                    maskedTextBoxPhone.ReadOnly = false;

                    buttonWrite.Visible = true;
                    buttonBack.Text = "Отмена";
                    break;
            }
        }

        public string ClientFIO
        {
            get => textBoxFIO.Text;
            set => textBoxFIO.Text = value;
        }

        public string ClientPhone
        {
            get => maskedTextBoxPhone.Text;
            set => maskedTextBoxPhone.Text = value;
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
    }
}
