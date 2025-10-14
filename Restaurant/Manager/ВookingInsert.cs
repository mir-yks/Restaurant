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
    public partial class ВookingInsert : Form
    {
        public ВookingInsert()
        {
            InitializeComponent();

            labelClient.Font = Fonts.MontserratAlternatesRegular(14f);
            labelDateBooking.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxClient.Font = Fonts.MontserratAlternatesRegular(14f);
            dateTimePickerBooking.Font = Fonts.MontserratAlternatesRegular(12f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonArrange.Font = Fonts.MontserratAlternatesBold(12f);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonArrange_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите записать бронь?", "Подтверждение записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

            }
        }
    }
}
