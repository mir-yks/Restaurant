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

            labelOrder.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            labelStatus.Font = Fonts.MontserratAlternatesRegular(14f);
            labelSum.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxOrder.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxStatus.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxSum.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonReport.Font = Fonts.MontserratAlternatesBold(12f);
            buttonOrderItem.Font = Fonts.MontserratAlternatesBold(12f);
            buttonNew.Font = Fonts.MontserratAlternatesBold(12f);
            buttonUpdate.Font = Fonts.MontserratAlternatesBold(12f);
            buttonCheck.Font = Fonts.MontserratAlternatesBold(12f);
            buttonDelete.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }

        private void ConfigureButtons()
        {
            buttonReport.Visible = false;
            buttonOrderItem.Visible = false;
            buttonNew.Visible = false;
            buttonUpdate.Visible = false;
            buttonCheck.Visible = false;
            buttonDelete.Visible = false;
            buttonBack.Visible = true;

            if (roleId == 1)
            {
                buttonReport.Visible = true;
                buttonOrderItem.Visible = true;
                buttonNew.Visible = true;
                buttonUpdate.Visible = true;
                buttonCheck.Visible = true;
            }
            else if (roleId == 2)
            {
                buttonReport.Visible = true;
                buttonOrderItem.Visible = true;
                buttonOrderItem.Location = new System.Drawing.Point(673, 533);
                buttonReport.Location = new System.Drawing.Point(552, 533);
            }
            else if (roleId == 3)
            {
                buttonNew.Visible = true;
                buttonUpdate.Visible = true;
                buttonCheck.Visible = true;
                buttonDelete.Visible = true;
                buttonCheck.Location = new System.Drawing.Point(421, 533);
                buttonDelete.Location = new System.Drawing.Point(673, 472);
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            OrderInsert OrderInsert = new OrderInsert();
            this.Visible = true;
            OrderInsert.ShowDialog();
            this.Visible = true;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            OrderInsert OrderInsert = new OrderInsert();
            this.Visible = true;
            OrderInsert.ShowDialog();
            this.Visible = true;
        }

        private void buttonOrderItem_Click(object sender, EventArgs e)
        {
            OrderItem OrderItem = new OrderItem(roleId);
            this.Visible = false;
            OrderItem.ShowDialog();
            this.Visible = true;
        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            Revenue Revenue = new Revenue();
            this.Visible = true;
            Revenue.ShowDialog();
            this.Visible = true;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
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

                labelTotal.Text = $"Всего: {orderTable.Rows.Count}";

                MySqlCommand cmdCategories = new MySqlCommand("SELECT CategoryDishName FROM CategoryDish;", con);
                MySqlDataReader reader = cmdCategories.ExecuteReader();

                comboBoxStatus.Items.Clear();
                comboBoxStatus.Items.Add("");
                comboBoxStatus.Items.Add("Принят");
                comboBoxStatus.Items.Add("В обработке");
                comboBoxStatus.Items.Add("На кухне");
                comboBoxStatus.Items.Add("Готов");

                comboBoxStatus.Items.Add("Оплачен");
                comboBoxStatus.Items.Add("Не оплачен");
                comboBoxStatus.SelectedIndex = 0;


                comboBoxSum.Items.Clear();
                comboBoxSum.Items.Add("");
                comboBoxSum.Items.Add("По возрастанию");
                comboBoxSum.Items.Add("По убыванию");
                comboBoxSum.SelectedIndex = 0;
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

        private void textBoxOrder_TextChanged(object sender, EventArgs e)
        {
            int cursorPos = textBoxOrder.SelectionStart;

            string input = textBoxOrder.Text;
            bool showSpaceWarning = false;
            bool showDashWarning = false;

            int spaceCount = input.Count(c => c == ' ');
            if (spaceCount > 2)
            {
                int lastSpace = input.LastIndexOf(' ');
                input = input.Remove(lastSpace, 1);
                showSpaceWarning = true;
            }

            int dashCount = input.Count(c => c == '-');
            if (dashCount > 1)
            {
                int lastDash = input.LastIndexOf('-');
                input = input.Remove(lastDash, 1);
                showDashWarning = true;
            }

            string[] parts = input
                .Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => char.ToUpper(p[0]) + p.Substring(1).ToLower())
                .ToArray();

            string formatted = input;
            int index = 0;
            foreach (string part in parts)
            {
                int pos = formatted.IndexOf(part, index, StringComparison.OrdinalIgnoreCase);
                if (pos >= 0)
                {
                    formatted = formatted.Remove(pos, part.Length).Insert(pos, part);
                    index = pos + part.Length;
                }
            }

            textBoxOrder.TextChanged -= textBoxOrder_TextChanged;
            textBoxOrder.Text = formatted;
            textBoxOrder.SelectionStart = Math.Min(cursorPos, textBoxOrder.Text.Length);
            textBoxOrder.TextChanged += textBoxOrder_TextChanged;

            if (showSpaceWarning)
                InputTooltipHelper.Show(textBoxOrder, "Можно использовать не более двух пробелов.");
            if (showDashWarning)
                InputTooltipHelper.Show(textBoxOrder, "Можно использовать только одно тире.");

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (orderTable == null) return;

            string searchText = textBoxOrder.Text.Trim().Replace("'", "''");
            string selectedStatus = comboBoxStatus.SelectedItem?.ToString() ?? "";
            string sortOption = comboBoxSum.SelectedItem?.ToString() ?? "";

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

            labelTotal.Text = $"Всего: {view.Count}";
        }

        private void buttonClearFilters_Click(object sender, EventArgs e)
        {
            textBoxOrder.Text = "";

            comboBoxStatus.SelectedIndex = 0;
            comboBoxSum.SelectedIndex = 0;

            if (orderTable != null)
            {
                DataView view = new DataView(orderTable);
                view.RowFilter = "";
                view.Sort = "";
                dataGridView1.DataSource = view;

                labelTotal.Text = $"Всего: {view.Count}";
            }
        }

        private void textBoxOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я0-9-\s]$"))
            {
                e.Handled = true;
            }
        }
    }
}
