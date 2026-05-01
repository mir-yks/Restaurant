using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Word = Microsoft.Office.Interop.Word;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Restaurant
{
    public class GenerateCheck
    {
        public static void GenerateOrderCheck(int orderId, bool askForPayment, string format, Action<int, string, string> updateOrderStatus = null, Action loadOrders = null)
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

                if (askForPayment && updateOrderStatus != null)
                {
                    DialogResult paymentResult = MessageBox.Show(
                        "Клиент оплатил заказ?",
                        "Подтверждение оплаты",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (paymentResult == DialogResult.Yes)
                    {
                        updateOrderStatus(orderId, "Завершен", "Оплачен");
                        MessageBox.Show("Статус заказа обновлен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadOrders?.Invoke();
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

        private static void GenerateWordCheck(string filePath, OrderData orderData, List<OrderItemWithDiscount> orderItems)
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
                AddLine($"Столик: {orderData.TableNumber}");
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

        private static void GeneratePdfCheck(string filePath, OrderData orderData, List<OrderItemWithDiscount> orderItems)
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
                        col.Item().Text($"Столик: {orderData.TableNumber}");
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
                        col.Item().AlignCenter().Text("Ждем Вас снова!");
                    });
                });
            }).GeneratePdf(fullPath);

            System.Diagnostics.Process.Start(fullPath);
        }

        private static List<OrderItemWithDiscount> GetOrderItemsWithDiscounts(int orderId)
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

        private static OrderData GetOrderData(int orderId)
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

        private static void ReleaseObject(object obj)
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

        private class OrderItemWithDiscount
        {
            public string DishName { get; set; }
            public int Quantity { get; set; }
            public decimal OriginalPrice { get; set; }
            public decimal TotalPrice { get; set; }
            public int Discount { get; set; }
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
    }
}