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

            labelWaiter.Font = Fonts.MontserratAlternatesRegular(14f);
            labelClient.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTable.Font = Fonts.MontserratAlternatesRegular(14f);
            labelDateOrder.Font = Fonts.MontserratAlternatesRegular(14f);
            labelSum.Font = Fonts.MontserratAlternatesRegular(14f);
            labelStatusOrder.Font = Fonts.MontserratAlternatesRegular(14f);
            labelStatusPayment.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxSum.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxWaiter.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxClient.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxWorker.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxStatusOrder.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxStatusPayment.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonOrderItem.Font = Fonts.MontserratAlternatesBold(12f);
            dateTimePickerOder.Font = Fonts.MontserratAlternatesRegular(12f);

            dateTimePickerOder.MaxDate = DateTime.Today;
            dateTimePickerOder.MinDate = DateTime.Today.AddMonths(-3);
        }


        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonOrderItem_Click(object sender, EventArgs e)
        {
        }
    }
}
