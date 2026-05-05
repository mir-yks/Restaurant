using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class ManagementBD : Form
    {
        public ManagementBD()
        {
            InitializeComponent();

            buttonImportFile.Font = Fonts.MontserratAlternatesBold(12f);
            buttonExportFile.Font = Fonts.MontserratAlternatesBold(12f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonBackup.Font = Fonts.MontserratAlternatesBold(12f);
            buttonStructure.Font = Fonts.MontserratAlternatesBold(12f);
            buttonImport.Font = Fonts.MontserratAlternatesBold(12f);
            buttonExport.Font = Fonts.MontserratAlternatesBold(12f);
            labelExport.Font = Fonts.MontserratAlternatesRegular(14f);
            labelImport.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxExport.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxImport.Font = Fonts.MontserratAlternatesRegular(14f);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {

        }
    }
}