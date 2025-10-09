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
    public partial class Menu : Form
    {
        private int roleId;
        public Menu(int role)
        {
            InitializeComponent();
            roleId = role;
            ConfigureButtons();

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            label3.Font = Fonts.MontserratAlternatesRegular(14f);
            label4.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox2.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            button3.Font = Fonts.MontserratAlternatesBold(12f);
        }
        private void ConfigureButtons()
        {
            button1.Visible = true; 
            button2.Visible = false;
            button3.Visible = false;

            if (roleId == 4) 
            {
                button2.Visible = true;
                button3.Visible = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MenuInsert MenuInsert = new MenuInsert();
            this.Visible = true;
            MenuInsert.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MenuInsert MenuInsert = new MenuInsert();
            this.Visible = true;
            MenuInsert.ShowDialog();
            this.Visible = true;
        }
    }
}
