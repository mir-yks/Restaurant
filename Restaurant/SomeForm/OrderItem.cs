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
        public OrderItem(int role)
        {
            InitializeComponent();
            roleId = role;
            ConfigureButtons();

            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }
        private void ConfigureButtons()
        {
            button2.Visible = false;
            button1.Visible = true;

            if (roleId == 3)
            {
                button2.Visible = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void OrderItem_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(connStr.ConnectionString);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                        o.OrderId AS 'Номер заказа',
                                                        m.DishName AS 'Блюдо',
                                                        i.DishCount AS 'Количество'
                                                    FROM OrderItems i
                                                    JOIN `Order` o ON i.OrderId = o.OrderId
                                                    LEFT JOIN MenuDish m ON i.DishId = m.DishId;", con);
                DataTable t = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(t);
                dataGridView1.DataSource = t;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
