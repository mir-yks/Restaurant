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

            labelReport.Font = Fonts.MontserratAlternatesRegular(14f);
            labelS.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPo.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPeriod.Font = Fonts.MontserratAlternatesRegular(14f);
            dateTimePickerS.Font = Fonts.MontserratAlternatesRegular(12f);
            dateTimePickerPo.Font = Fonts.MontserratAlternatesRegular(12f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonCreate.Font = Fonts.MontserratAlternatesBold(12f);

            dateTimePickerS.MaxDate = DateTime.Today;
            dateTimePickerS.MinDate = DateTime.Today.AddYears(-100);
            dateTimePickerPo.MaxDate = DateTime.Today;
            dateTimePickerPo.MinDate = DateTime.Today.AddYears(-100);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите создать отчёт?", "Создание отчёта", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

            }
        }
    }
}
