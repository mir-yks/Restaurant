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

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            maskedTextBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);

            ApplyMode();
        }

        private void ApplyMode()
        {
            switch (mode)
            {
                case "view":
                    textBox1.ReadOnly = true;
                    maskedTextBox1.ReadOnly = true;

                    button2.Visible = false;

                    button1.Text = "Закрыть";
                    break;

                case "add":
                    textBox1.Text = "";
                    maskedTextBox1.Text = "";

                    textBox1.ReadOnly = false;
                    maskedTextBox1.ReadOnly = false;

                    button2.Visible = true;
                    button1.Text = "Отмена";
                    break;

                case "edit":
                    textBox1.ReadOnly = false;
                    maskedTextBox1.ReadOnly = false;

                    button2.Visible = true;
                    button1.Text = "Отмена";
                    break;
            }
        }

        public string ClientFIO
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }

        public string ClientPhone
        {
            get => maskedTextBox1.Text;
            set => maskedTextBox1.Text = value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
