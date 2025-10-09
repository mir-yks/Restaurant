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
    public partial class Order : Form
    {
        private int roleId;
        public Order(int role)
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
            button4.Font = Fonts.MontserratAlternatesBold(12f);
            button5.Font = Fonts.MontserratAlternatesBold(12f);
            button6.Font = Fonts.MontserratAlternatesBold(12f);
        }

        private void ConfigureButtons()
        {
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button1.Visible = true;

            if (roleId == 1)
            {
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;
            }
            else if (roleId == 2)
            {
                button2.Visible = true;
                button3.Visible = true;
                button3.Location = new System.Drawing.Point(673, 533);
                button2.Location = new System.Drawing.Point(552, 533);
            }
            else if (roleId == 3)
            {
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;
                button6.Location = new System.Drawing.Point(673, 533);
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OrderInsert OrderInsert = new OrderInsert();
            this.Visible = true;
            OrderInsert.ShowDialog();
            this.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OrderInsert OrderInsert = new OrderInsert();
            this.Visible = true;
            OrderInsert.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OrderItem OrderItem = new OrderItem(roleId);
            this.Visible = false;
            OrderItem.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Revenue Revenue = new Revenue();
            this.Visible = true;
            Revenue.ShowDialog();
            this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
