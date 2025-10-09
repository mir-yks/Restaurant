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
    public partial class Booking : Form
    {
        public Booking()
        {
            InitializeComponent();

            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            button3.Font = Fonts.MontserratAlternatesBold(12f);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ВookingInsert ВookingInsert = new ВookingInsert();
            this.Visible = true;
            ВookingInsert.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ВookingInsert ВookingInsert = new ВookingInsert();
            this.Visible = true;
            ВookingInsert.ShowDialog();
            this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
