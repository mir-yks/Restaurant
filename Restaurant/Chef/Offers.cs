using MySql.Data.MySqlClient;
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
    public partial class Offers : Form
    {
        public Offers()
        {
            InitializeComponent();

            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            button3.Font = Fonts.MontserratAlternatesBold(12f);
            button8.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(10f);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OffersInsert OfferInsert = new OffersInsert();
            this.Visible = true;
            OfferInsert.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OffersInsert OfferInsert = new OffersInsert();
            this.Visible = true;
            OfferInsert.ShowDialog();
            this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Offers_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(connStr.ConnectionString);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"SELECT OffersDishName AS 'Предложение блюд', OffersDishDicsount AS 'Скидка' FROM OffersDish", con);
                DataTable t = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(t);
                dataGridView1.DataSource = t;

                label2.Text = $"Всего: {t.Rows.Count}";

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
