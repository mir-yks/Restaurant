using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Word = Microsoft.Office.Interop.Word;
using System.IO;

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

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите заказ для формирования чека!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedOrderId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            GenerateOrderCheck(selectedOrderId);
        }

        private void GenerateOrderCheck(int orderId)
        {
            Word.Application wordApp = null;
            Word.Document document = null;

            try
            {
                var orderData = GetOrderData(orderId);
                if (orderData == null)
                {
                    MessageBox.Show("Не удалось получить данные о заказе", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var orderItems = GetOrderItemsWithDiscounts(orderId);

                wordApp = new Word.Application();
                wordApp.Visible = false;

                document = wordApp.Documents.Add();

                document.PageSetup.Orientation = Word.WdOrientation.wdOrientPortrait;
                document.PageSetup.PageWidth = wordApp.CentimetersToPoints(8f);
                document.PageSetup.PageHeight = wordApp.CentimetersToPoints(29.7f);
                document.PageSetup.TopMargin = wordApp.CentimetersToPoints(0.5f);
                document.PageSetup.BottomMargin = wordApp.CentimetersToPoints(0.5f);
                document.PageSetup.LeftMargin = wordApp.CentimetersToPoints(0.5f);
                document.PageSetup.RightMargin = wordApp.CentimetersToPoints(0.5f);

                Word.Paragraph content = document.Content.Paragraphs.Add();
                content.Format.SpaceAfter = 1f;
                content.Format.SpaceBefore = 0f;
                content.Format.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;

                content.Range.Text = "MIRYKS";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 10;
                content.Range.Font.Bold = 1;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                content.Range.InsertParagraphAfter();

                content.Range.Text = "Ресторан европейской кухни";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 7;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                content.Range.InsertParagraphAfter();

                content.Range.Text = "----------------------";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 7;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                content.Range.InsertParagraphAfter();

                content.Range.Text = $"Чек №{orderData.OrderNumber}";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 7;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                content.Range.InsertParagraphAfter();

                content.Range.Text = $"{orderData.OrderDate:dd.MM.yy HH:mm}";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 7;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                content.Range.InsertParagraphAfter();

                content.Range.Text = $"Официант: {orderData.WorkerName}";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 7;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                content.Range.InsertParagraphAfter();

                content.Range.Text = "----------------------";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 7;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                content.Range.InsertParagraphAfter();

                decimal totalAmount = 0;
                decimal totalDiscount = 0;

                foreach (var item in orderItems)
                {
                    string dishName = item.DishName;
                    if (item.Discount > 0)
                    {
                        dishName = $"★ {dishName}";
                    }

                    content.Range.Text = dishName;
                    content.Range.Font.Name = "Courier New";
                    content.Range.Font.Size = 7;
                    content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    content.Range.InsertParagraphAfter();

                    string priceLine;
                    if (item.Discount > 0)
                    {
                        decimal originalTotal = item.Quantity * item.OriginalPrice;
                        decimal discountAmount = originalTotal - item.TotalPrice;
                        priceLine = $"{item.Quantity} x {item.OriginalPrice:N0} = {item.TotalPrice:N0} (-{discountAmount:N0})";
                        totalDiscount += discountAmount;
                    }
                    else
                    {
                        priceLine = $"{item.Quantity} x {item.OriginalPrice:N0} = {item.TotalPrice:N0}";
                    }

                    content.Range.Text = priceLine;
                    content.Range.Font.Name = "Courier New";
                    content.Range.Font.Size = 7;
                    content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                    content.Range.InsertParagraphAfter();

                    totalAmount += item.TotalPrice;
                }

                content.Range.Text = "======================";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 7;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                content.Range.InsertParagraphAfter();

                content.Range.Text = $"ИТОГО: {totalAmount:N0} руб.";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 8;
                content.Range.Font.Bold = 1;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                content.Range.InsertParagraphAfter();

                if (totalDiscount > 0)
                {
                    content.Range.Text = $"Скидка: -{totalDiscount:N0} руб.";
                    content.Range.Font.Name = "Courier New";
                    content.Range.Font.Size = 7;
                    content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    content.Range.InsertParagraphAfter();
                }

                content.Range.Text = "======================";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 7;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                content.Range.InsertParagraphAfter();

                content.Range.Text = "Спасибо за посещение!";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 7;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                content.Range.InsertParagraphAfter();

                content.Range.Text = "Ждем Вас снова!";
                content.Range.Font.Name = "Courier New";
                content.Range.Font.Size = 7;
                content.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                wordApp.Visible = true;

                wordApp.Activate();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании чека: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (document != null)
                {
                    document.Close(false);
                    document = null;
                }
                if (wordApp != null)
                {
                    wordApp.Quit();
                    wordApp = null;
                }
            }
            finally
            {
                try
                {
                    if (document != null)
                    {
                        if (!wordApp.Visible)
                        {
                            document.Close(false);
                        }
                        ReleaseObject(document);
                        document = null;
                    }

                    if (wordApp != null)
                    {
                        if (wordApp.Documents.Count == 0)
                        {
                            wordApp.Quit();
                        }
                        ReleaseObject(wordApp);
                        wordApp = null;
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
                catch (Exception)
                {

                }
            }
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception)
            {
                obj = null;
            }
            finally
            {
                obj = null;
            }
        }
        private List<OrderItemWithDiscount> GetOrderItemsWithDiscounts(int orderId)
        {
            var items = new List<OrderItemWithDiscount>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                SELECT 
                    md.DishName,
                    oi.DishCount,
                    md.DishPrice as OriginalPrice,
                    CASE 
                        WHEN md.OffersDish IS NOT NULL AND md.OffersDish > 0 THEN
                            (oi.DishCount * md.DishPrice * (100 - od.OffersDishDicsount) / 100)
                        ELSE
                            (oi.DishCount * md.DishPrice)
                    END as TotalPrice,
                    COALESCE(od.OffersDishDicsount, 0) as Discount
                FROM OrderItems oi
                JOIN MenuDish md ON oi.DishId = md.DishId
                LEFT JOIN OffersDish od ON md.OffersDish = od.OffersDishId
                WHERE oi.OrderId = @orderId", con);

                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new OrderItemWithDiscount
                            {
                                DishName = reader.GetString("DishName"),
                                Quantity = reader.GetInt32("DishCount"),
                                OriginalPrice = reader.GetDecimal("OriginalPrice"),
                                TotalPrice = reader.GetDecimal("TotalPrice"),
                                Discount = reader.GetInt32("Discount")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении состава заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return items;
        }

        private class OrderItemWithDiscount
        {
            public string DishName { get; set; }
            public int Quantity { get; set; }
            public decimal OriginalPrice { get; set; }
            public decimal TotalPrice { get; set; }
            public int Discount { get; set; }
        }

        private OrderData GetOrderData(int orderId)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                        o.OrderId,
                                                        o.OrderDate,
                                                        w.WorkerFIO,
                                                        c.ClientFIO,
                                                        t.TablesId,
                                                        o.OrderPrice,
                                                        o.OrderStatus,
                                                        o.OrderStatusPayment
                                                    FROM `Order` o
                                                    JOIN Worker w ON o.WorkerId = w.WorkerId
                                                    LEFT JOIN Client c ON o.ClientId = c.ClientId
                                                    LEFT JOIN Tables t ON o.TableId = t.TablesId
                                                    WHERE o.OrderId = @orderId", con);
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new OrderData
                            {
                                OrderNumber = reader.GetInt32("OrderId"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                WorkerName = reader.GetString("WorkerFIO"),
                                ClientName = reader.IsDBNull(reader.GetOrdinal("ClientFIO")) ? null : reader.GetString("ClientFIO"),
                                TableNumber = reader.GetInt32("TablesId"),
                                TotalPrice = reader.GetDecimal("OrderPrice"),
                                OrderStatus = reader.GetString("OrderStatus"),
                                PaymentStatus = reader.GetString("OrderStatusPayment")
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении данных заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        private List<OrderItemData> GetOrderItems(int orderId)
        {
            var items = new List<OrderItemData>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                        md.DishName,
                                                        oi.DishCount,
                                                        md.DishPrice,
                                                        (md.DishPrice * oi.DishCount) as TotalPrice
                                                    FROM OrderItems oi
                                                    JOIN MenuDish md ON oi.DishId = md.DishId
                                                    WHERE oi.OrderId = @orderId", con);
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new OrderItemData
                            {
                                DishName = reader.GetString("DishName"),
                                Quantity = reader.GetInt32("DishCount"),
                                Price = reader.GetDecimal("DishPrice"),
                                TotalPrice = reader.GetDecimal("TotalPrice")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении состава заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return items;
        }
        private class OrderData
        {
            public int OrderNumber { get; set; }
            public DateTime OrderDate { get; set; }
            public string WorkerName { get; set; }
            public string ClientName { get; set; }
            public int TableNumber { get; set; }
            public decimal TotalPrice { get; set; }
            public string OrderStatus { get; set; }
            public string PaymentStatus { get; set; }
        }

        private class OrderItemData
        {
            public string DishName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal TotalPrice { get; set; }
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

            string orderStatus = row.Cells["Статус заказа"].Value.ToString();
            string paymentStatus = row.Cells["Статус оплаты заказа"].Value.ToString();

            if (orderStatus == "Завершен" && paymentStatus == "Оплачен")
            {
                MessageBox.Show("Заказ завершен и оплачен. Редактирование невозможно!", "Информация",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OrderInsert OrderInsert = new OrderInsert("edit")
            {
                OrderID = Convert.ToInt32(row.Cells["ID"].Value),
                WorkerName = row.Cells["Сотрудник"].Value.ToString(),
                ClientName = row.Cells["Клиент"].Value.ToString(),
                TableNumber = row.Cells["Номер столика"].Value?.ToString() ?? "",
                OrderDate = Convert.ToDateTime(row.Cells["Дата заказа"].Value),
                OrderStatus = orderStatus,
                OrderStatusPayment = paymentStatus
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
            OrderItem orderItemForm = new OrderItem(2, selectedOrderId);

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

            DataGridViewRow row = dataGridView1.CurrentRow;
            string orderStatus = row.Cells["Статус заказа"].Value.ToString();
            int selectedOrderId = Convert.ToInt32(row.Cells["ID"].Value);
            string orderNumber = row.Cells["Номер заказа"].Value.ToString();

            if (orderStatus != "Новый")
            {
                MessageBox.Show("Можно удалить только заказ со статусом 'Новый'!",
                               "Удаление невозможно",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Вы действительно хотите удалить заказ №{orderNumber}?",
                                                "Удаление записи",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

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
                    comboBoxStatus.Items.Add("Новый");
                    comboBoxStatus.Items.Add("На кухне");
                    comboBoxStatus.Items.Add("Готов");
                    comboBoxStatus.Items.Add("Завершен");
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
    }
}