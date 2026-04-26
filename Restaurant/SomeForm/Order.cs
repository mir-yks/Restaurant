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
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
            InactivityManager.Init();

            labelOrder.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            labelStatus.Font = Fonts.MontserratAlternatesRegular(14f);
            labelSum.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxOrder.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxStatus.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxSum.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonReportRevenue.Font = Fonts.MontserratAlternatesBold(12f);
            buttonReportPopular.Font = Fonts.MontserratAlternatesBold(12f);
            buttonOrderItem.Font = Fonts.MontserratAlternatesBold(12f);
            buttonNew.Font = Fonts.MontserratAlternatesBold(12f);
            buttonUpdate.Font = Fonts.MontserratAlternatesBold(12f);
            buttonCheck.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
        }

        private void ConfigureButtons()
        {
            if (roleId == 2)
            {
                buttonReportRevenue.Visible = true;
                buttonReportPopular.Visible = true;
                buttonReportRevenue.Location = new System.Drawing.Point(552, 533);
                buttonReportPopular.Location = new System.Drawing.Point(412, 533);
                buttonOrderItem.Location = new System.Drawing.Point(673, 533);
            }
            else if (roleId == 3)
            {
                buttonNew.Visible = true;
                buttonUpdate.Visible = true;
                buttonCheck.Visible = true;
                buttonOrderItem.Location = new System.Drawing.Point(431, 533);
            }
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите заказ для формирования чека!");
                return;
            }

            int orderId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string status = dataGridView1.CurrentRow.Cells["Статус заказа"].Value.ToString();
            string payment = dataGridView1.CurrentRow.Cells["Статус оплаты заказа"].Value.ToString();

            if (status != "Завершен" || payment != "Оплачен")
            {
                var payResult = MessageBox.Show(
                    "Заказ не оплачен. Отметить как оплаченный?",
                    "Оплата",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (payResult == DialogResult.Yes)
                {
                    UpdateOrderStatus(orderId, "Завершен", "Оплачен");
                    payment = "Оплачен";
                }
                else
                {
                    return; 
                }
            }

            DialogResult formatResult = MessageBox.Show(
                "Выберите формат:\nДа - Word\nНет - PDF",
                "Формат чека",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

            if (formatResult == DialogResult.Cancel)
                return;

            string format = formatResult == DialogResult.Yes ? "Word" : "PDF";

            GenerateOrderCheck(orderId, false, format);
            LoadOrders();
        }

        private void GenerateOrderCheck(int orderId, bool askForPayment, string format)
        {
            try
            {
                var orderData = GetOrderData(orderId);
                if (orderData == null)
                {
                    MessageBox.Show("Не удалось получить данные о заказе", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var orderItems = GetOrderItemsWithDiscounts(orderId);

                InactivityManager.PauseTimer();

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                string folder = Path.Combine(baseDir, "Resources", "check");
                Directory.CreateDirectory(folder);

                string filePath = Path.Combine(folder, $"Чек_{orderId}");

                if (format == "Word")
                {
                    GenerateWordCheck(filePath, orderData, orderItems);
                }
                else
                {
                    GeneratePdfCheck(filePath, orderData, orderItems);
                }

                if (askForPayment)
                {
                    DialogResult paymentResult = MessageBox.Show(
                        "Клиент оплатил заказ?",
                        "Подтверждение оплаты",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (paymentResult == DialogResult.Yes)
                    {
                        UpdateOrderStatus(orderId, "Завершен", "Оплачен");
                        MessageBox.Show("Статус заказа обновлен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrders();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании чека: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                InactivityManager.ResumeTimer();
            }
        }

        private void GenerateWordCheck(string filePath, OrderData orderData, List<OrderItemWithDiscount> orderItems)
        {
            Word.Application wordApp = null;
            Word.Document document = null;

            try
            {
                wordApp = new Word.Application();
                wordApp.Visible = false;

                document = wordApp.Documents.Add();

                document.PageSetup.Orientation = Word.WdOrientation.wdOrientPortrait;
                document.PageSetup.PageWidth = wordApp.CentimetersToPoints(8f);
                document.PageSetup.PageHeight = wordApp.CentimetersToPoints(29.7f);
                document.PageSetup.TopMargin = wordApp.CentimetersToPoints(0.3f);
                document.PageSetup.BottomMargin = wordApp.CentimetersToPoints(0.3f);
                document.PageSetup.LeftMargin = wordApp.CentimetersToPoints(0.3f);
                document.PageSetup.RightMargin = wordApp.CentimetersToPoints(0.3f);

                void AddLine(string text, Word.WdParagraphAlignment align = Word.WdParagraphAlignment.wdAlignParagraphLeft, bool bold = false, int size = 8)
                {
                    Word.Paragraph p = document.Content.Paragraphs.Add();
                    p.Range.Text = text;
                    p.Range.Font.Name = "Courier New";
                    p.Range.Font.Size = size;
                    p.Range.Font.Bold = bold ? 1 : 0;
                    p.Alignment = align;
                    p.Format.SpaceBefore = 0;
                    p.Format.SpaceAfter = 0;
                    p.Range.InsertParagraphAfter();
                }

                AddLine("MIRYKS", Word.WdParagraphAlignment.wdAlignParagraphCenter, true, 10);
                AddLine("Ресторан европейской кухни", Word.WdParagraphAlignment.wdAlignParagraphCenter, false, 7);
                AddLine("----------------------", Word.WdParagraphAlignment.wdAlignParagraphCenter);

                AddLine($"Чек №{orderData.OrderNumber}");
                AddLine($"{orderData.OrderDate:dd.MM.yy HH:mm}");
                AddLine($"Официант: {orderData.WorkerName}");

                AddLine("----------------------", Word.WdParagraphAlignment.wdAlignParagraphCenter);

                decimal total = 0;
                decimal discountTotal = 0;

                foreach (var item in orderItems)
                {
                    string name = item.DishName;

                    AddLine(name);

                    string line;

                    if (item.Discount > 0)
                    {
                        decimal original = item.Quantity * item.OriginalPrice;
                        decimal discount = original - item.TotalPrice;

                        line = $"{item.Quantity} x {item.OriginalPrice:N0} = {item.TotalPrice:N0} (-{discount:N0})";
                        discountTotal += discount;
                    }
                    else
                    {
                        line = $"{item.Quantity} x {item.OriginalPrice:N0} = {item.TotalPrice:N0}";
                    }

                    AddLine(line, Word.WdParagraphAlignment.wdAlignParagraphRight);

                    total += item.TotalPrice;
                }

                AddLine("======================", Word.WdParagraphAlignment.wdAlignParagraphCenter);
                AddLine($"ИТОГО: {total:N0} руб.", Word.WdParagraphAlignment.wdAlignParagraphCenter, true);

                if (discountTotal > 0)
                {
                    AddLine($"Скидка: -{discountTotal:N0} руб.", Word.WdParagraphAlignment.wdAlignParagraphCenter);
                }

                AddLine("======================", Word.WdParagraphAlignment.wdAlignParagraphCenter);

                AddLine("Спасибо за посещение!", Word.WdParagraphAlignment.wdAlignParagraphCenter);
                AddLine("Ждем Вас снова!", Word.WdParagraphAlignment.wdAlignParagraphCenter);

                document.SaveAs(filePath + ".docx");

                wordApp.Visible = true;
                wordApp.Activate();
            }
            finally
            {
                ReleaseObject(document);
                ReleaseObject(wordApp);
            }
        }

        private void GeneratePdfCheck(string filePath, OrderData orderData, List<OrderItemWithDiscount> orderItems)
        {
            string fullPath = filePath + ".pdf";

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(226, 842);

                    page.Margin(5);

                    page.DefaultTextStyle(x =>
                        x.FontSize(8).FontFamily("Courier New"));

                    page.Content().Column(col =>
                    {
                        col.Spacing(1);

                        col.Item().AlignCenter().Text("MIRYKS").Bold();
                        col.Item().AlignCenter().Text("Ресторан европейской кухни");

                        col.Item().Text("----------------------").AlignCenter();

                        col.Item().Text($"Чек №{orderData.OrderNumber}");
                        col.Item().Text($"{orderData.OrderDate:dd.MM.yy HH:mm}");
                        col.Item().Text($"Официант: {orderData.WorkerName}");

                        col.Item().Text("----------------------").AlignCenter();

                        decimal total = 0;
                        decimal discountTotal = 0;

                        foreach (var item in orderItems)
                        {
                            string name = item.Discount > 0 ? $"★ {item.DishName}" : item.DishName;

                            col.Item().Text(name);

                            string line;

                            if (item.Discount > 0)
                            {
                                decimal original = item.Quantity * item.OriginalPrice;
                                decimal discount = original - item.TotalPrice;

                                line = $"{item.Quantity} x {item.OriginalPrice:N0} = {item.TotalPrice:N0} (-{discount:N0})";
                                discountTotal += discount;
                            }
                            else
                            {
                                line = $"{item.Quantity} x {item.OriginalPrice:N0} = {item.TotalPrice:N0}";
                            }

                            col.Item().AlignRight().Text(line);

                            total += item.TotalPrice;
                        }

                        col.Item().Text("======================").AlignCenter();

                        col.Item().AlignCenter().Text($"ИТОГО: {total:N0} руб.").Bold();

                        if (discountTotal > 0)
                        {
                            col.Item().AlignCenter().Text($"Скидка: -{discountTotal:N0} руб.");
                        }

                        col.Item().AlignCenter().Text("======================");

                        col.Item().AlignCenter().Text("Спасибо за посещение!");
                        col.Item().AlignCenter().Text("Ждем Вас снова!"); ;
                    });
                });
            }).GeneratePdf(fullPath);

            System.Diagnostics.Process.Start(fullPath);
        }

        private void UpdateOrderStatus(int orderId, string orderStatus, string paymentStatus)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    MySqlCommand getTableCmd = new MySqlCommand(
                        "SELECT TableId FROM `Order` WHERE OrderId = @OrderId", con);
                    getTableCmd.Parameters.AddWithValue("@OrderId", orderId);
                    object tableIdObj = getTableCmd.ExecuteScalar();

                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE `Order` SET OrderStatus = @OrderStatus, OrderStatusPayment = @OrderStatusPayment WHERE OrderId = @OrderId",
                        con);
                    cmd.Parameters.AddWithValue("@OrderStatus", orderStatus);
                    cmd.Parameters.AddWithValue("@OrderStatusPayment", paymentStatus);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.ExecuteNonQuery();

                    if (orderStatus == "Завершен" && paymentStatus == "Оплачен" && tableIdObj != null && tableIdObj != DBNull.Value)
                    {
                        int tableId = Convert.ToInt32(tableIdObj);
                        MySqlCommand updateTableCmd = new MySqlCommand(
                            "UPDATE Tables SET TablesStatus = 'Свободен' WHERE TablesId = @TableId", con);
                        updateTableCmd.Parameters.AddWithValue("@TableId", tableId);
                        updateTableCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статуса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
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
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
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
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
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
            if (dataGridView1.CurrentRow == null) return;

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
            OrderItem orderItemForm = new OrderItem(2, selectedOrderId);

            if (orderItemForm.ShowDialog() == DialogResult.OK)
            {
                LoadOrders();
            }
        }

        private void buttonReportRevenue_Click(object sender, EventArgs e)
        {
            Revenue Revenue = new Revenue("revenue");
            Revenue.ShowDialog();
        }

        private void buttonReportPopular_Click(object sender, EventArgs e)
        {
            Revenue Revenue = new Revenue("popular");
            Revenue.ShowDialog();
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
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT 
                                                o.OrderId AS 'ID',
                                                o.OrderId AS 'Номер заказа',
                                                COALESCE(
                                                    w.OriginalWorkerFIO, 
                                                    w.WorkerFIO
                                                ) AS 'Сотрудник',
                                                COALESCE(
                                                    c.OriginalClientFIO,
                                                    c.ClientFIO
                                                ) AS 'Клиент',
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
                buttonOrderItem.Enabled = true;
                buttonCheck.Enabled = true;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null) return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            string text = e.Value.ToString();

            if (columnName == "Сотрудник" || columnName == "Клиент")
            {
                if (!string.IsNullOrEmpty(text))
                {
                    e.Value = ConvertToInitials(text);
                }
            }
        }

        private string ConvertToInitials(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return string.Empty;

            string[] parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 3)
            {
                return $"{parts[0]} {parts[1][0]}.{parts[2][0]}.";
            }
            else if (parts.Length == 2)
            {
                return $"{parts[0]} {parts[1][0]}.";
            }
            else
            {
                return fullName;
            }
        }
    }
}