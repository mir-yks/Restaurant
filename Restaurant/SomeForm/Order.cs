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
    public partial class Order : Form
    {
        private int roleId;
        private DataTable orderTable;
        public Order(int role)
        {
            InitializeComponent(); 
            roleId = role;
            ConfigureButtons();

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            label3.Font = Fonts.MontserratAlternatesRegular(14f);
            label4.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox2.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);
            button3.Font = Fonts.MontserratAlternatesBold(12f);
            button4.Font = Fonts.MontserratAlternatesBold(12f);
            button5.Font = Fonts.MontserratAlternatesBold(12f);
            button6.Font = Fonts.MontserratAlternatesBold(12f);
            button8.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }

        private void ConfigureButtons()
        {
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button8.Visible = false;
            button1.Visible = true;

            if (roleId == 1)
            {
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;
            }
            else if (roleId == 2)
            {
                button2.Visible = true;
                button3.Visible = true;
                button3.Location = new System.Drawing.Point(673, 533);
                button2.Location = new System.Drawing.Point(552, 533);
            }
            else if (roleId == 3)
            {
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;
                button8.Visible = true;
                button6.Location = new System.Drawing.Point(421, 533);
                button8.Location = new System.Drawing.Point(673, 472);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OrderInsert OrderInsert = new OrderInsert();
            this.Visible = true;
            OrderInsert.ShowDialog();
            this.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OrderInsert OrderInsert = new OrderInsert();
            this.Visible = true;
            OrderInsert.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OrderItem OrderItem = new OrderItem(roleId);
            this.Visible = false;
            OrderItem.ShowDialog();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Revenue Revenue = new Revenue();
            this.Visible = true;
            Revenue.ShowDialog();
            this.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись?", "Подтверждение удаления записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Order_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(connStr.ConnectionString);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                        o.OrderId AS 'Номер заказа',
                                                        w.WorkerFIO AS 'Сотрудник',
                                                        c.ClientFIO AS 'Клиент',
                                                        t.TablesId AS 'Номер столика',
                                                        o.OrderDate AS 'Дата заказа',
                                                        o.OrderPrice AS 'Стоимость заказа',
                                                        o.OrderStatus AS 'Статус заказа',
                                                        o.OrderStatusPayment AS 'Статус оплаты заказа'
                                                    FROM `Order` o
                                                    JOIN Worker w ON o.WorkerId = w.WorkerId
                                                    LEFT JOIN Client c ON o.ClientId = c.ClientId
                                                    LEFT JOIN Tables t ON o.TableId = t.TablesId;", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                orderTable = new DataTable();
                da.Fill(orderTable);
                dataGridView1.DataSource = orderTable;

                label2.Text = $"Всего: {orderTable.Rows.Count}";

                MySqlCommand cmdCategories = new MySqlCommand("SELECT CategoryDishName FROM CategoryDish;", con);
                MySqlDataReader reader = cmdCategories.ExecuteReader();

                comboBox1.Items.Clear();
                comboBox1.Items.Add("");
                comboBox1.Items.Add("Принят");
                comboBox1.Items.Add("В обработке");
                comboBox1.Items.Add("На кухне");
                comboBox1.Items.Add("Готов");

                comboBox1.Items.Add("Оплачен");
                comboBox1.Items.Add("Не оплачен");
                comboBox1.SelectedIndex = 0;


                comboBox2.Items.Clear();
                comboBox2.Items.Add("");
                comboBox2.Items.Add("По возрастанию");
                comboBox2.Items.Add("По убыванию");
                comboBox2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            if (orderTable == null) return;

            string searchText = textBox1.Text.Trim().Replace("'", "''");
            string selectedStatus = comboBox1.SelectedItem?.ToString() ?? "";
            string sortOption = comboBox2.SelectedItem?.ToString() ?? "";

            DataView view = new DataView(orderTable);
            string filter = "";

            if (!string.IsNullOrEmpty(searchText))
            {
                filter = $"Convert([Номер заказа], 'System.String') LIKE '%{searchText}%' " +
                         $"OR [Клиент] LIKE '%{searchText}%' " +
                         $"OR [Сотрудник] LIKE '%{searchText}%' " +
                         $"OR Convert([Номер столика], 'System.String') LIKE '%{searchText}%'";
            }

            if (!string.IsNullOrEmpty(selectedStatus))
            {
                string statusFilter = $"[Статус заказа] = '{selectedStatus}' OR [Статус оплаты заказа] = '{selectedStatus}'";
                if (!string.IsNullOrEmpty(filter))
                    filter = $"({filter}) AND ({statusFilter})";
                else
                    filter = statusFilter;
            }

            view.RowFilter = filter;

            if (sortOption == "По возрастанию")
                view.Sort = "[Стоимость заказа] ASC";
            else if (sortOption == "По убыванию")
                view.Sort = "[Стоимость заказа] DESC";
            else
                view.Sort = "";

            dataGridView1.DataSource = view;

            label2.Text = $"Всего: {view.Count}";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

            if (orderTable != null)
            {
                DataView view = new DataView(orderTable);
                view.RowFilter = "";
                view.Sort = "";
                dataGridView1.DataSource = view;

                label2.Text = $"Всего: {view.Count}";
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я0-9\s]$"))
            {
                e.Handled = true;
            }
        }
    }
}
