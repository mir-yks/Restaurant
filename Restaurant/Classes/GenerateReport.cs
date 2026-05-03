using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Restaurant
{
    public class GenerateReport
    {
        public static void GenerateRevenueReport(DateTime startDate, DateTime endDate)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                DataTable dataTable = GetRevenueData(startDate, endDate);

                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Нет оплаченных заказов за выбранный период", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.ActiveSheet as Excel.Worksheet;
                worksheet.Name = "Отчёт по выручке";

                worksheet.Cells[1, 1] = "Отчёт по выручке";
                SetRangeStyle(worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 6]],
                            16, true, Excel.XlHAlign.xlHAlignCenter);
                worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 6]].Merge();

                worksheet.Cells[2, 1] = $"Период: с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}";
                SetRangeStyle(worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[2, 6]],
                            12, false, Excel.XlHAlign.xlHAlignCenter);
                worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[2, 6]].Merge();

                worksheet.Cells[3, 1] = "";

                int currentRow = 4;
                string[] headers = { "Номер заказа", "Дата заказа", "Сотрудник", "Клиент", "Столик", "Сумма заказа (руб.)" };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[currentRow, i + 1] = headers[i];
                    var cell = worksheet.Cells[currentRow, i + 1];
                    cell.Font.Bold = true;
                    cell.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                    cell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                currentRow++;
                decimal totalRevenue = 0;

                dataTable.DefaultView.Sort = "ДатаЗаказа ASC";
                DataTable sortedTable = dataTable.DefaultView.ToTable();

                foreach (DataRow row in sortedTable.Rows)
                {
                    worksheet.Cells[currentRow, 1] = row["НомерЗаказа"].ToString();
                    worksheet.Cells[currentRow, 2] = Convert.ToDateTime(row["ДатаЗаказа"]).ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cells[currentRow, 3] = row["Сотрудник"].ToString();

                    string client = row["Клиент"].ToString();
                    worksheet.Cells[currentRow, 4] = string.IsNullOrEmpty(client) ? "" : client;

                    worksheet.Cells[currentRow, 5] = row["Столик"].ToString();

                    decimal amount = Convert.ToDecimal(row["СуммаЗаказа"]);
                    worksheet.Cells[currentRow, 6] = $"{amount:N2} руб.";
                    worksheet.Cells[currentRow, 6].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                    for (int col = 1; col <= 6; col++)
                    {
                        worksheet.Cells[currentRow, col].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    }

                    totalRevenue += amount;
                    currentRow++;
                }

                currentRow++;
                worksheet.Cells[currentRow, 1] = "ИТОГО:";
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                worksheet.Cells[currentRow, 6] = $"{totalRevenue:N2} руб.";
                worksheet.Cells[currentRow, 6].Font.Bold = true;
                worksheet.Cells[currentRow, 6].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                currentRow++;
                worksheet.Cells[currentRow, 1] = "Количество заказов:";
                worksheet.Cells[currentRow, 2] = dataTable.Rows.Count.ToString();
                worksheet.Cells[currentRow, 2].Font.Bold = true;

                currentRow++;
                worksheet.Cells[currentRow, 1] = "Средний чек:";
                decimal averageCheck = dataTable.Rows.Count > 0 ? totalRevenue / dataTable.Rows.Count : 0;
                worksheet.Cells[currentRow, 2] = $"{averageCheck:N2} руб.";
                worksheet.Cells[currentRow, 2].Font.Bold = true;
                worksheet.Cells[currentRow, 2].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                worksheet.Columns.AutoFit();

                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчёта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (excelApp != null && !excelApp.Visible)
                {
                    ReleaseObject(worksheet);
                    ReleaseObject(workbook);
                    ReleaseObject(excelApp);
                }
                else
                {
                    worksheet = null;
                    workbook = null;
                    excelApp = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public static void GeneratePopularDishesReport(DateTime startDate, DateTime endDate)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                DataTable dataTable = GetPopularDishesData(startDate, endDate);

                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Нет данных о заказах за выбранный период", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.ActiveSheet as Excel.Worksheet;
                worksheet.Name = "Популярные блюда";

                worksheet.Cells[1, 1] = "Топ-5 популярных блюд";
                worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 4]].Merge();
                worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 4]].Font.Size = 16;
                worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 4]].Font.Bold = true;
                worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 4]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                worksheet.Cells[2, 1] = $"Период: с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}";
                worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[2, 4]].Merge();
                worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[2, 4]].Font.Size = 12;
                worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[2, 4]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                int currentRow = 4;
                string[] headers = { "№", "Блюдо", "Количество заказов", "Общая выручка (руб.)" };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[currentRow, i + 1] = headers[i];
                    worksheet.Cells[currentRow, i + 1].Font.Bold = true;
                    worksheet.Cells[currentRow, i + 1].Interior.Color = Excel.XlRgbColor.rgbLightGray;
                    worksheet.Cells[currentRow, i + 1].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    worksheet.Cells[currentRow, i + 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                currentRow++;
                int counter = 1;
                decimal totalRevenue = 0;

                foreach (DataRow row in dataTable.Rows)
                {
                    worksheet.Cells[currentRow, 1] = counter++;
                    worksheet.Cells[currentRow, 2] = row["Блюдо"].ToString();
                    worksheet.Cells[currentRow, 3] = row["Количество"].ToString();
                    worksheet.Cells[currentRow, 3].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    decimal amount = Convert.ToDecimal(row["Выручка"]);
                    worksheet.Cells[currentRow, 4] = $"{amount:N2} руб.";
                    worksheet.Cells[currentRow, 4].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                    for (int col = 1; col <= 4; col++)
                    {
                        worksheet.Cells[currentRow, col].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    }

                    totalRevenue += amount;
                    currentRow++;
                }

                currentRow++;
                worksheet.Cells[currentRow, 1] = "ИТОГО по топ-5:";
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, 3]].Merge();
                worksheet.Cells[currentRow, 4] = $"{totalRevenue:N2} руб.";
                worksheet.Cells[currentRow, 4].Font.Bold = true;
                worksheet.Cells[currentRow, 4].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                int dataStartRow = 5;
                int dataEndRow = dataStartRow + dataTable.Rows.Count - 1;

                Excel.Range chartDataRange = worksheet.Range[worksheet.Cells[dataStartRow, 2], worksheet.Cells[dataEndRow, 3]];

                Excel.ChartObjects chartObjects = (Excel.ChartObjects)worksheet.ChartObjects();
                Excel.ChartObject chartObject = chartObjects.Add(350, 60, 350, 250);
                Excel.Chart chart = chartObject.Chart;

                chart.SetSourceData(chartDataRange);
                chart.ChartType = Excel.XlChartType.xlPie;

                chart.HasTitle = true;
                chart.ChartTitle.Text = "Распределение заказов по блюдам";
                chart.ChartTitle.Font.Size = 12;

                chart.HasLegend = true;
                chart.Legend.Position = Excel.XlLegendPosition.xlLegendPositionRight;

                Excel.Series series = (Excel.Series)chart.SeriesCollection(1);
                series.HasDataLabels = true;
                series.ApplyDataLabels(Excel.XlDataLabelsType.xlDataLabelsShowPercent);

                Excel.DataLabels dataLabels = series.DataLabels();
                dataLabels.ShowCategoryName = false;
                dataLabels.ShowPercentage = true;
                dataLabels.ShowValue = false;

                worksheet.Columns.AutoFit();

                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчёта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (excelApp != null && !excelApp.Visible)
                {
                    ReleaseObject(worksheet);
                    ReleaseObject(workbook);
                    ReleaseObject(excelApp);
                }
                else
                {
                    worksheet = null;
                    workbook = null;
                    excelApp = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private static DataTable GetPopularDishesData(DateTime startDate, DateTime endDate)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    string query = @"
                        SELECT 
                            md.DishName AS 'Блюдо',
                            SUM(oi.DishCount) AS 'Количество',
                            SUM(
                                CASE 
                                    WHEN md.OffersDish IS NOT NULL AND md.OffersDish > 0 THEN
                                        ROUND(oi.DishCount * md.DishPrice * (100 - od.OffersDishDicsount) / 100, 2)
                                    ELSE
                                        ROUND(oi.DishCount * md.DishPrice, 2)
                                END
                            ) AS 'Выручка'
                        FROM OrderItems oi
                        JOIN `Order` o ON oi.OrderId = o.OrderId
                        JOIN MenuDish md ON oi.DishId = md.DishId
                        LEFT JOIN OffersDish od ON md.OffersDish = od.OffersDishId
                        WHERE DATE(o.OrderDate) BETWEEN @StartDate AND @EndDate
                        AND o.OrderStatusPayment = 'Оплачен'
                        GROUP BY md.DishId, md.DishName
                        ORDER BY SUM(oi.DishCount) DESC
                        LIMIT 5";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                        cmd.Parameters.AddWithValue("@EndDate", endDate.Date);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dataTable;
        }

        private static DataTable GetRevenueData(DateTime startDate, DateTime endDate)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    string query = @"
                        SELECT 
                            o.OrderId AS 'НомерЗаказа',
                            o.OrderDate AS 'ДатаЗаказа',
                            COALESCE(w.OriginalWorkerFIO, w.WorkerFIO) AS 'Сотрудник',
                            COALESCE(c.OriginalClientFIO, c.ClientFIO) AS 'Клиент',
                            t.TablesId AS 'Столик',
                            o.OrderPrice AS 'СуммаЗаказа'
                        FROM `Order` o
                        JOIN Worker w ON o.WorkerId = w.WorkerId
                        LEFT JOIN Client c ON o.ClientId = c.ClientId
                        LEFT JOIN Tables t ON o.TableId = t.TablesId
                        WHERE DATE(o.OrderDate) BETWEEN @StartDate AND @EndDate
                        AND o.OrderStatusPayment = 'Оплачен'
                        ORDER BY o.OrderDate ASC";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                        cmd.Parameters.AddWithValue("@EndDate", endDate.Date);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dataTable;
        }

        private static void SetRangeStyle(Excel.Range range, int fontSize, bool bold, Excel.XlHAlign alignment)
        {
            range.Font.Size = fontSize;
            range.Font.Bold = bold;
            range.HorizontalAlignment = alignment;
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
        }
    }
}