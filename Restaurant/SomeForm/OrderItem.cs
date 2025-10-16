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
    public partial class OrderItem : Form
    {
        private int roleId;
        private int orderId;
        public OrderItem(int role, int orderId)
        {
            InitializeComponent();
            roleId = role;
            this.orderId = orderId;
            ConfigureButtons();

            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }
        private void ConfigureButtons()
        {
            buttonWrite.Visible = false;
            buttonBack.Visible = true;

            if (roleId == 3)
            {
                buttonWrite.Visible = true;
            }
        }
        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void OrderItem_Load(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                    SELECT 
                        o.OrderId AS 'Номер заказа',
                        m.DishName AS 'Блюдо',
                        i.DishCount AS 'Количество'
                    FROM OrderItems i
                    JOIN `Order` o ON i.OrderId = o.OrderId
                    LEFT JOIN MenuDish m ON i.DishId = m.DishId
                    WHERE o.OrderId = @OrderId;", con);

                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    DataTable t = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(t);
                    dataGridView1.DataSource = t;

                    if (dataGridView1.Columns.Contains("Номер заказа"))
                        dataGridView1.Columns["Номер заказа"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите сохранить запись?", "Подтверждение записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

            }
        }
    }
}
