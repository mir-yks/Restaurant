using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Windows.Forms.DataVisualization.Charting;

namespace Restaurant
{
    public partial class Revenue : Form
    {
        private string reportMode;
        private Form parentForm;
        private Chart chartPopular;

        public Revenue(string mode = "revenue", Form parentForm = null)
        {
            InitializeComponent();
            this.reportMode = mode;
            this.parentForm = parentForm;

            if (parentForm != null)
            {
                BlurEffect.ShowDimmed(parentForm);
            }

            SetupFonts();

            if (mode == "popular")
            {
                labelReport.Text = "Популярные блюда";
                this.ClientSize = new System.Drawing.Size(800, 360);
                InitChart();
            }

            LoadDateRangeFromDatabase();

            if (mode == "popular")
            {
                LoadPopularChart(dateTimePickerMin.Value, dateTimePickerMax.Value);
            }
        }

        private void SetupFonts()
        {
            labelReport.Font = Fonts.MontserratAlternatesBold(14f);
            labelPeriod.Font = Fonts.MontserratAlternatesRegular(14f);
            labelS.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPo.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonCreate.Font = Fonts.MontserratAlternatesBold(12f);
        }

        private void InitChart()
        {
            chartPopular = new Chart();
            chartPopular.Location = new System.Drawing.Point(280, 10);
            chartPopular.Size = new System.Drawing.Size(500, 340);

            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.BackColor = System.Drawing.Color.Transparent;

            chartArea.Position = new ElementPosition(10, 35, 80, 70);
            chartArea.InnerPlotPosition = new ElementPosition(10, 10, 80, 80);

            chartPopular.ChartAreas.Add(chartArea);

            Series series = new Series("Dishes");
            series.ChartType = SeriesChartType.Pie;
            series.Label = "#PERCENT{P1}";
            series["PieLabelStyle"] = "Inside";
            series["PieLineColor"] = "Transparent";
            series["PieDrawingStyle"] = "Default";
            series["PieStartAngle"] = "270";
            series.LegendText = "#VALX";
            series.Font = Fonts.MontserratAlternatesRegular(11f);
            series.LabelForeColor = System.Drawing.Color.White;

            chartPopular.Series.Add(series);

            Legend legend = new Legend();
            legend.Docking = Docking.Top;
            legend.Alignment = System.Drawing.StringAlignment.Far;
            legend.Font = Fonts.MontserratAlternatesRegular(12f);
            legend.ForeColor = System.Drawing.Color.White;
            legend.BackColor = System.Drawing.Color.Transparent;

            chartPopular.Legends.Add(legend);

            chartPopular.BackColor = System.Drawing.Color.Transparent;

            this.Controls.Add(chartPopular);
        }

        private void LoadPopularChart(DateTime startDate, DateTime endDate)
        {
            if (chartPopular == null) return;

            chartPopular.Series[0].Points.Clear();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    string query = @"
                        SELECT 
                            md.DishName,
                            SUM(oi.DishCount) AS Quantity
                        FROM OrderItems oi
                        JOIN `Order` o ON oi.OrderId = o.OrderId
                        JOIN MenuDish md ON oi.DishId = md.DishId
                        WHERE DATE(o.OrderDate) BETWEEN @Start AND @End
                        AND o.OrderStatusPayment = 'Оплачен'
                        GROUP BY md.DishName
                        ORDER BY Quantity DESC
                        LIMIT 5";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Start", startDate.Date);
                        cmd.Parameters.AddWithValue("@End", endDate.Date);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string dish = reader.GetString("DishName");
                                int qty = Convert.ToInt32(reader["Quantity"]);
                                chartPopular.Series[0].Points.AddXY(dish, qty);
                            }
                        }
                    }
                }

                var colors = new System.Drawing.Color[]
                {
                    System.Drawing.Color.FromArgb(79,129,189),
                    System.Drawing.Color.FromArgb(192,80,77),
                    System.Drawing.Color.FromArgb(155,187,89),
                    System.Drawing.Color.FromArgb(128,100,162),
                    System.Drawing.Color.FromArgb(75,172,198)
                };

                for (int i = 0; i < chartPopular.Series[0].Points.Count; i++)
                {
                    chartPopular.Series[0].Points[i].Color = colors[i % colors.Length];
                }

                if (chartPopular.Series[0].Points.Count == 0)
                {
                    chartPopular.Titles.Clear();
                    chartPopular.Titles.Add("Нет данных за выбранный период");
                }
                else
                {
                    chartPopular.Titles.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки диаграммы: {ex.Message}");
            }
        }

        private void LoadDateRangeFromDatabase()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    string minDateQuery = "SELECT MIN(DATE(OrderDate)) FROM `Order`";
                    using (MySqlCommand minCmd = new MySqlCommand(minDateQuery, con))
                    {
                        var minDateResult = minCmd.ExecuteScalar();
                        if (minDateResult != null && minDateResult != DBNull.Value)
                        {
                            DateTime minDate = Convert.ToDateTime(minDateResult);
                            dateTimePickerMin.MinDate = minDate;
                            dateTimePickerMax.MinDate = minDate;
                        }
                    }

                    string maxDateQuery = "SELECT MAX(DATE(OrderDate)) FROM `Order`";
                    using (MySqlCommand maxCmd = new MySqlCommand(maxDateQuery, con))
                    {
                        var maxDateResult = maxCmd.ExecuteScalar();
                        if (maxDateResult != null && maxDateResult != DBNull.Value)
                        {
                            DateTime maxDate = Convert.ToDateTime(maxDateResult);
                            dateTimePickerMin.MaxDate = maxDate;
                            dateTimePickerMax.MaxDate = maxDate;
                        }
                    }

                    dateTimePickerMin.Value = dateTimePickerMin.MinDate;
                    dateTimePickerMax.Value = dateTimePickerMax.MaxDate;
                }
            }
            catch
            {
                DateTime today = DateTime.Today;
                dateTimePickerMin.MinDate = today.AddYears(-1);
                dateTimePickerMax.MinDate = today.AddYears(-1);
                dateTimePickerMin.MaxDate = today;
                dateTimePickerMax.MaxDate = today;
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (dateTimePickerMin.Value > dateTimePickerMax.Value)
            {
                MessageBox.Show("Дата 'С' не может быть больше даты 'По'");
                return;
            }

            if (reportMode == "popular")
            {
                GenerateReport.GeneratePopularDishesReport(
                    dateTimePickerMin.Value,
                    dateTimePickerMax.Value);
            }
            else
            {
                GenerateReport.GenerateRevenueReport(
                    dateTimePickerMin.Value,
                    dateTimePickerMax.Value);
            }
        }

        private void dateTimePickerMin_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerMin.Value > dateTimePickerMax.Value)
                dateTimePickerMax.Value = dateTimePickerMin.Value;

            if (reportMode == "popular")
                LoadPopularChart(dateTimePickerMin.Value, dateTimePickerMax.Value);
        }

        private void dateTimePickerMax_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerMax.Value < dateTimePickerMin.Value)
                dateTimePickerMin.Value = dateTimePickerMax.Value;

            if (reportMode == "popular")
                LoadPopularChart(dateTimePickerMin.Value, dateTimePickerMax.Value);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (parentForm != null)
            {
                BlurEffect.HideDimmed();
            }
        }
    }
}