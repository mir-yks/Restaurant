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
    public partial class Booking : Form
    {
        public Booking()
        {
            InitializeComponent();

            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            button3.Font = Fonts.MontserratAlternatesBold(12f);
            button8.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
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

        private void Booking_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(connStr.ConnectionString);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                        c.ClientFIO AS 'Клиент',
                                                        b.BookingDate AS 'Дата брони'
                                                    FROM booking b 
                                                    JOIN client c ON b.ClientId = c.ClientId;", con);
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

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись?", "Подтверждение удаления записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

            }
        }
    }
}
