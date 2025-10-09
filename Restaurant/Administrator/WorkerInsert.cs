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
        public WorkerInsert()
        {
            InitializeComponent();

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            label3.Font = Fonts.MontserratAlternatesRegular(14f);
            label4.Font = Fonts.MontserratAlternatesRegular(14f);
            label11.Font = Fonts.MontserratAlternatesRegular(14f);
            label6.Font = Fonts.MontserratAlternatesRegular(14f);
            label7.Font = Fonts.MontserratAlternatesRegular(14f);
            label12.Font = Fonts.MontserratAlternatesRegular(14f);
            label9.Font = Fonts.MontserratAlternatesRegular(14f);
            label10.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox4.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox5.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox6.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox7.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox9.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox3.Font = Fonts.MontserratAlternatesRegular(14f);
            dateTimePicker1.Font = Fonts.MontserratAlternatesRegular(14f);
            dateTimePicker2.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}