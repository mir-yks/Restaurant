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
        private int currentWorkerId;
        private int roleId;
        private DataTable orderTable;

        public Order(int role, int currentWorkerId = 0)
        {
            InitializeComponent();
            roleId = role;
            this.currentWorkerId = currentWorkerId;
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
            if (roleId == 2)
            {
                buttonReport.Visible = true;
                buttonReport.Location = new System.Drawing.Point(552, 533);
                buttonOrderItem.Location = new System.Drawing.Point(673, 533);
            }
            else if (roleId == 3)
            {
                buttonNew.Visible = true;
                buttonUpdate.Visible = true;
                buttonCheck.Visible = true;
                buttonDelete.Visible = true;
                buttonOrderItem.Location = new System.Drawing.Point(431, 533);
                buttonDelete.Location = new System.Drawing.Point(673, 472);
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            OrderInsert OrderInsert = new OrderInsert("add", currentWorkerId);
            OrderInsert.ShowDialog();
            LoadOrders();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите заказ для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dataGridView1.CurrentRow;

            OrderInsert OrderInsert = new OrderInsert("edit")
            {
                OrderID = Convert.ToInt32(row.Cells["ID"].Value),
                WorkerName = row.Cells["Сотрудник"].Value.ToString(),
                ClientName = row.Cells["Клиент"].Value.ToString(),
                TableNumber = row.Cells["Номер столика"].Value?.ToString() ?? "",
                OrderDate = Convert.ToDateTime(row.Cells["Дата заказа"].Value),
                OrderStatus = row.Cells["Статус заказа"].Value.ToString(),
                OrderStatusPayment = row.Cells["Статус оплаты заказа"].Value.ToString()
            };

            OrderInsert.ShowDialog();
            LoadOrders();
        }


        private void buttonOrderItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите заказ для просмотра состава!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedOrderId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Номер заказа"].Value);
            OrderItem orderItemForm = new OrderItem(roleId, selectedOrderId);

            if (orderItemForm.ShowDialog() == DialogResult.OK)
            {
                LoadOrders(); 
            }
        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            Revenue Revenue = new Revenue();
            Revenue.ShowDialog();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите заказ для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedOrderId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string orderNumber = dataGridView1.CurrentRow.Cells["Номер заказа"].Value.ToString();

            DialogResult result = MessageBox.Show($"Вы действительно хотите удалить заказ №{orderNumber}?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand deleteOrderItemsCmd = new MySqlCommand("DELETE FROM OrderItems WHERE OrderId = @id", con);
                    deleteOrderItemsCmd.Parameters.AddWithValue("@id", selectedOrderId);
                    deleteOrderItemsCmd.ExecuteNonQuery();

                    MySqlCommand cmd = new MySqlCommand("DELETE FROM `Order` WHERE OrderId = @id", con);
                    cmd.Parameters.AddWithValue("@id", selectedOrderId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Заказ №{orderNumber} успешно удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOrders();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Order_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                        o.OrderId AS 'ID',
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

                    if (dataGridView1.Columns.Contains("ID"))
                        dataGridView1.Columns["ID"].Visible = false;

                    labelTotal.Text = $"Всего: {orderTable.Rows.Count}";

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                buttonUpdate.Enabled = true;
                buttonDelete.Enabled = true;
                buttonOrderItem.Enabled = true;
                buttonCheck.Enabled = true;
            }
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {

        }
    }
}