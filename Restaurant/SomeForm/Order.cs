using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Restaurant
{
    public partial class Order : Form
    {
        private int currentWorkerId;
        private int roleId;
        private DataTable orderTable;
        private DataView currentView;
        private PaginationManager pagination;
        public Order(int role, int currentWorkerId = 0)
        {
            InitializeComponent();

            pagination = new PaginationManager(
                dataGridView1,
                paginationPanel,
                labelPageInfo,
                labelTotal);

            InitPerformanceTweaks();

            roleId = role;
            this.currentWorkerId = currentWorkerId;
            ConfigureButtons();
            InactivityManager.Init();

            labelOrder.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTotal.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPageInfo.Font = Fonts.MontserratAlternatesRegular(14f);
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
            buttonClearFilters.Font = Fonts.MontserratAlternatesBold(12f);
            labelPageInfo.Font = Fonts.MontserratAlternatesRegular(14f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);

            KeyboardLayoutManager.AttachRussianLayout(textBoxOrder);
        }

        private void InitPerformanceTweaks()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
            this.UpdateStyles();

            typeof(DataGridView)
                .GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic)
                .SetValue(dataGridView1, true, null);

            dataGridView1.SizeChanged += (s, e) => pagination.RecalculateLayoutOnly();
            this.Resize += (s, e) => pagination.RecalculateLayoutOnly();
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

        private void UpdateButtonsState()
        {
            if (dataGridView1.CurrentRow == null)
            {
                buttonUpdate.Enabled = false;
                buttonOrderItem.Enabled = false;
                buttonCheck.Enabled = false;
                return;
            }

            string status = dataGridView1.CurrentRow.Cells["Статус заказа"].Value.ToString();

            if (status == "Завершен" || status == "Отменен")
            {
                buttonUpdate.Enabled = false;
            }
            else
            {
                buttonUpdate.Enabled = true;
            }

            if (status == "Отменен")
            {
                buttonCheck.Enabled = false;
            }
            else
            {
                buttonCheck.Enabled = true;
            }

            buttonOrderItem.Enabled = true;
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

            if (status == "Отменен")
            {
                MessageBox.Show("Нельзя сформировать чек для отмененного заказа!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (status == "Завершен" && payment == "Оплачен")
            {
                DialogResult viewResult = MessageBox.Show(
                    "Заказ уже завершен и оплачен. Чек был ранее сформирован.\n\nВы хотите просмотреть чек заново?",
                    "Просмотр чека",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (viewResult == DialogResult.Yes)
                {
                    DialogResult formatResult = MessageBox.Show(
                        "Выберите формат:\nДа - Word\nНет - PDF",
                        "Формат чека",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Information);

                    if (formatResult == DialogResult.Cancel)
                        return;

                    string format = formatResult == DialogResult.Yes ? "Word" : "PDF";

                    GenerateCheck.GenerateOrderCheck(orderId, false, format, OrderStatusUpdater.UpdateOrderStatus, LoadOrders);
                }
                return;
            }

            if (status != "Завершен" || payment != "Оплачен")
            {
                var payResult = MessageBox.Show(
                    "Заказ не оплачен. Отметить как оплаченный и сформировать чек?",
                    "Оплата",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (payResult == DialogResult.Yes)
                {
                    OrderStatusUpdater.UpdateOrderStatus(orderId, "Завершен", "Оплачен");

                    DialogResult formatResult = MessageBox.Show(
                        "Выберите формат:\nДа - Word\nНет - PDF",
                        "Формат чека",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Information);

                    if (formatResult == DialogResult.Cancel)
                        return;

                    string format = formatResult == DialogResult.Yes ? "Word" : "PDF";

                    GenerateCheck.GenerateOrderCheck(orderId, false, format, OrderStatusUpdater.UpdateOrderStatus, LoadOrders);
                    LoadOrders();
                }
                return;
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            OrderInsert orderInsert = new OrderInsert("add", currentWorkerId, this);
            orderInsert.ShowDialog();
            LoadOrders();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            string status = dataGridView1.CurrentRow.Cells["Статус заказа"].Value.ToString();

            if (status == "Завершен")
            {
                MessageBox.Show("Нельзя редактировать завершенный заказ!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (status == "Отменен")
            {
                MessageBox.Show("Нельзя редактировать отмененный заказ!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dataGridView1.CurrentRow;

            OrderInsert orderInsert = new OrderInsert("edit", 0, this)
            {
                OrderID = Convert.ToInt32(row.Cells["ID"].Value),
                WorkerName = row.Cells["Сотрудник"].Value.ToString(),
                ClientName = row.Cells["Клиент"].Value.ToString(),
                TableNumber = row.Cells["Номер столика"].Value?.ToString() ?? "",
                OrderDate = Convert.ToDateTime(row.Cells["Дата заказа"].Value),
                OrderStatus = row.Cells["Статус заказа"].Value.ToString(),
                OrderStatusPayment = row.Cells["Статус оплаты заказа"].Value.ToString()
            };

            orderInsert.ShowDialog();
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
            Revenue revenue = new Revenue("revenue", this);
            revenue.ShowDialog();
        }

        private void buttonReportPopular_Click(object sender, EventArgs e)
        {
            Revenue revenue = new Revenue("popular", this);
            revenue.ShowDialog();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    currentView = new DataView(orderTable);

                    currentView.Sort = "ID ASC";

                    pagination.SetData(currentView);

                    if (dataGridView1.Columns.Contains("ID"))
                        dataGridView1.Columns["ID"].Visible = false;

                    comboBoxStatus.Items.Clear();
                    comboBoxStatus.Items.Add("");
                    comboBoxStatus.Items.Add("Новый");
                    comboBoxStatus.Items.Add("Завершен");
                    comboBoxStatus.Items.Add("Отменен");
                    comboBoxStatus.SelectedIndex = 0;

                    comboBoxSum.Items.Clear();
                    comboBoxSum.Items.Add("");
                    comboBoxSum.Items.Add("По возрастанию");
                    comboBoxSum.Items.Add("По убыванию");
                    comboBoxSum.SelectedIndex = 0;

                    UpdateButtonsState();
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
            if (orderTable == null)
                return;

            string searchText = textBoxOrder.Text.Trim().Replace("'", "''");
            string selectedStatus = comboBoxStatus.SelectedItem?.ToString() ?? "";
            string sortOption = comboBoxSum.SelectedItem?.ToString() ?? "";

            currentView = new DataView(orderTable);

            currentView.Sort = "ID ASC";

            string filter = "";

            if (!string.IsNullOrEmpty(searchText))
            {
                filter =
                    $"Convert([Номер заказа], 'System.String') LIKE '%{searchText}%'" +
                    $" OR [Клиент] LIKE '%{searchText}%'" +
                    $" OR [Сотрудник] LIKE '%{searchText}%'" +
                    $" OR Convert([Номер столика], 'System.String') LIKE '%{searchText}%'";
            }

            if (!string.IsNullOrEmpty(selectedStatus))
            {
                string statusFilter = $"[Статус заказа] = '{selectedStatus}'";

                if (!string.IsNullOrEmpty(filter))
                    filter = $"({filter}) AND ({statusFilter})";
                else
                    filter = statusFilter;
            }

            currentView.RowFilter = filter;

            if (sortOption == "По возрастанию")
                currentView.Sort = "[Стоимость заказа] ASC";
            else if (sortOption == "По убыванию")
                currentView.Sort = "[Стоимость заказа] DESC";

            pagination.SetData(currentView);
        }

        private void buttonClearFilters_Click(object sender, EventArgs e)
        {
            textBoxOrder.Text = "";
            comboBoxStatus.SelectedIndex = 0;
            comboBoxSum.SelectedIndex = 0;

            currentView = new DataView(orderTable);

            pagination.SetData(currentView);
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
                UpdateButtonsState();
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