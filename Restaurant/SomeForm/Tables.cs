using System;
using System.Drawing;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class Tables : Form
    {
        private int roleId;

        public Tables(int role)
        {
            InitializeComponent();
            roleId = role;
            ConfigureButtons();

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            label3.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            button3.Font = Fonts.MontserratAlternatesBold(12f);
        }

        private void ConfigureButtons()
        {
            button2.Visible = false; 
            button3.Visible = false;
            button1.Visible = true; 

            if (roleId == 1) 
            {
                button2.Visible = true;
                button3.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TablesInsert TablesInsert = new TablesInsert();
            this.Visible = true;
            TablesInsert.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TablesInsert TablesInsert = new TablesInsert();
            this.Visible = true;
            TablesInsert.ShowDialog();
            this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
