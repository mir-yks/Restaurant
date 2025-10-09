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
    public partial class Revenue : Form
    {
        public Revenue()
        {
            InitializeComponent();

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            label3.Font = Fonts.MontserratAlternatesRegular(14f);
            label4.Font = Fonts.MontserratAlternatesRegular(14f);
            dateTimePicker1.Font = Fonts.MontserratAlternatesRegular(12f);
            dateTimePicker2.Font = Fonts.MontserratAlternatesRegular(12f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
