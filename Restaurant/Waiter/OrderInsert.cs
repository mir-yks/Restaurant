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
    public partial class OrderInsert : Form
    {
        public OrderInsert()
        {
            InitializeComponent();

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            label3.Font = Fonts.MontserratAlternatesRegular(14f);
            label4.Font = Fonts.MontserratAlternatesRegular(14f);
            label5.Font = Fonts.MontserratAlternatesRegular(14f);
            label6.Font = Fonts.MontserratAlternatesRegular(14f);
            label7.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox2.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox3.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox4.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox5.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            dateTimePicker1.Font = Fonts.MontserratAlternatesRegular(12f);

            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker1.MinDate = DateTime.Today.AddMonths(-3);

            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.BackColor = ColorTranslator.FromHtml("#393C46");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OrderItem OrderItem = new OrderItem(1);
            this.Visible = false;
            OrderItem.ShowDialog();
            this.Visible = true;
        }
    }
}
