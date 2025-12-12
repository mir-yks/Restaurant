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
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace Restaurant
{
    public partial class Revenue : Form
    {
        public Revenue()
        {
            InitializeComponent();
            InactivityManager.Init();

            labelReport.Font = Fonts.MontserratAlternatesRegular(14f);
            labelS.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPo.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPeriod.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonCreate.Font = Fonts.MontserratAlternatesBold(12f);

            LoadDateRangeFromDatabase();
        }

        private void LoadDateRangeFromDatabase()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connStr.ConnectionString))
                {
                    connection.Open();

                    string minDateQuery = "SELECT MIN(DATE(OrderDate)) FROM `Order`";
                    using (MySqlCommand minCmd = new MySqlCommand(minDateQuery, connection))
                    {
                        var minDateResult = minCmd.ExecuteScalar();
                        if (minDateResult != null && minDateResult != DBNull.Value)
                        {
                            DateTime minDate = Convert.ToDateTime(minDateResult);
                            dateTimePickerMin.MinDate = minDate;
                            dateTimePickerMax.MinDate = minDate;
                        }
                        else
                        {
                            DateTime today = DateTime.Today;
                            dateTimePickerMin.MinDate = today;
                            dateTimePickerMax.MinDate = today;
                        }
                    }

                    string maxDateQuery = "SELECT MAX(DATE(OrderDate)) FROM `Order`";
                    using (MySqlCommand maxCmd = new MySqlCommand(maxDateQuery, connection))
                    {
                        var maxDateResult = maxCmd.ExecuteScalar();
                        if (maxDateResult != null && maxDateResult != DBNull.Value)
                        {
                            DateTime maxDate = Convert.ToDateTime(maxDateResult);
                            dateTimePickerMin.MaxDate = maxDate;
                            dateTimePickerMax.MaxDate = maxDate;
                        }
                        else
                        {
                            DateTime today = DateTime.Today;
                            dateTimePickerMin.MaxDate = today;
                            dateTimePickerMax.MaxDate = today;
                        }
                    }

                    dateTimePickerMin.Value = dateTimePickerMin.MinDate;
                    dateTimePickerMax.Value = dateTimePickerMax.MaxDate;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                DateTime today = DateTime.Today;
                dateTimePickerMin.MinDate = today.AddYears(-1);
                dateTimePickerMin.MaxDate = today;
                dateTimePickerMax.MinDate = today.AddYears(-1);
                dateTimePickerMax.MaxDate = today;
                dateTimePickerMin.Value = today.AddMonths(-1);
                dateTimePickerMax.Value = today;
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (dateTimePickerMin.Value > dateTimePickerMax.Value)
            {
                MessageBox.Show("Дата 'С' не может быть больше даты 'По'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Вы действительно хотите создать отчёт в Excel?", "Создание отчёта", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                GenerateExcelReport();
            }
        }

        private void GenerateExcelReport()
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                DataTable dataTable = GetRevenueData();

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
                SetRangeStyle(worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 8]],
                            16, true, Excel.XlHAlign.xlHAlignCenter);
                worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 8]].Merge();

                worksheet.Cells[2, 1] = $"Период: с {dateTimePickerMin.Value:dd.MM.yyyy} по {dateTimePickerMax.Value:dd.MM.yyyy}";
                SetRangeStyle(worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[2, 8]],
                            12, false, Excel.XlHAlign.xlHAlignCenter);
                worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[2, 8]].Merge();

                worksheet.Cells[3, 1] = "";

                int currentRow = 4;
                string[] headers = { "Номер заказа", "Дата заказа", "Сотрудник", "Клиент", "Столик", "Сумма заказа (руб.)", "Статус заказа", "Статус оплаты" };

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

                    worksheet.Cells[currentRow, 7] = row["СтатусЗаказа"].ToString();
                    worksheet.Cells[currentRow, 8] = row["СтатусОплаты"].ToString();

                    for (int col = 1; col <= 8; col++)
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

                try
                {
                    if (workbook != null) workbook.Close(false);
                    if (excelApp != null) excelApp.Quit();
                }
                catch { }
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

        private void SetRangeStyle(Excel.Range range, int fontSize, bool bold, Excel.XlHAlign alignment)
        {
            range.Font.Size = fontSize;
            range.Font.Bold = bold;
            range.HorizontalAlignment = alignment;
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
            catch (Exception )
            {
                obj = null;
            }
        }

        private DataTable GetRevenueData()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connStr.ConnectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            o.OrderId AS 'НомерЗаказа',
                            o.OrderDate AS 'ДатаЗаказа',
                            w.WorkerFIO AS 'Сотрудник',
                            c.ClientFIO AS 'Клиент',
                            t.TablesId AS 'Столик',
                            o.OrderPrice AS 'СуммаЗаказа',
                            o.OrderStatus AS 'СтатусЗаказа',
                            o.OrderStatusPayment AS 'СтатусОплаты'
                        FROM `Order` o
                        JOIN Worker w ON o.WorkerId = w.WorkerId
                        LEFT JOIN Client c ON o.ClientId = c.ClientId
                        LEFT JOIN Tables t ON o.TableId = t.TablesId
                        WHERE DATE(o.OrderDate) BETWEEN @StartDate AND @EndDate
                        AND o.OrderStatusPayment = 'Оплачен'
                        ORDER BY o.OrderDate ASC"; 

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", dateTimePickerMin.Value.Date);
                        cmd.Parameters.AddWithValue("@EndDate", dateTimePickerMax.Value.Date);

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

        private void dateTimePickerMin_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerMin.Value > dateTimePickerMax.Value)
            {
                dateTimePickerMax.Value = dateTimePickerMin.Value;
            }
        }

        private void dateTimePickerMax_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerMax.Value < dateTimePickerMin.Value) 
            {
                dateTimePickerMin.Value = dateTimePickerMax.Value;
            }
        }
    }
}