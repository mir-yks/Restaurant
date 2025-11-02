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
    public partial class OrderInsert : Form
    {
        private string mode;
        private bool isUpdatingStatus = false;
        private string initialOrderStatus;
        private string initialOrderStatusPayment;

        public string WorkerName
        {
            get => comboBoxWaiter.Text;
            set => comboBoxWaiter.Text = value;
        }

        public string ClientName
        {
            get => comboBoxClient.Text;
            set => comboBoxClient.Text = value;
        }

        public string TableNumber
        {
            get => comboBoxTable.Text;
            set => comboBoxTable.Text = value;
        }

        public DateTime OrderDate
        {
            get => dateTimePickerOrder.Value;
            set => dateTimePickerOrder.Value = value;
        }

        public string OrderStatus
        {
            get => comboBoxStatusOrder.Text;
            set => comboBoxStatusOrder.Text = value;
        }

        public string OrderStatusPayment
        {
            get => comboBoxStatusPayment.Text;
            set => comboBoxStatusPayment.Text = value;
        }
        public int OrderID { get; set; }

        public OrderInsert(string mode, int currentWorkerId = 0)
        {
            InitializeComponent();
            this.mode = mode;

            labelWaiter.Font = Fonts.MontserratAlternatesRegular(14f);
            labelClient.Font = Fonts.MontserratAlternatesRegular(14f);
            labelTable.Font = Fonts.MontserratAlternatesRegular(14f);
            labelDateOrder.Font = Fonts.MontserratAlternatesRegular(14f);
            labelStatusOrder.Font = Fonts.MontserratAlternatesRegular(14f);
            labelStatusPayment.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxWaiter.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxClient.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxTable.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxStatusOrder.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBoxStatusPayment.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonOrderItem.Font = Fonts.MontserratAlternatesBold(12f);
            dateTimePickerOrder.Font = Fonts.MontserratAlternatesRegular(12f);

            LoadComboBoxData(currentWorkerId);
            SetControlsState();
        }

        private void SetControlsState()
        {
            if (mode == "add")
            {
                buttonOrderItem.Text = "Состав заказа";
                comboBoxStatusOrder.Enabled = false;
                comboBoxStatusPayment.Enabled = false;
                comboBoxClient.Enabled = true;
                comboBoxTable.Enabled = true;

                comboBoxStatusOrder.SelectedIndex = 0;
                comboBoxStatusPayment.SelectedIndex = 1;

                comboBoxClient.SelectedIndex = -1;
                comboBoxClient.Text = "";
            }
            else if (mode == "edit")
            {
                buttonOrderItem.Text = "Сохранить";
                comboBoxStatusOrder.Enabled = true;
                comboBoxStatusPayment.Enabled = true;
                comboBoxClient.Enabled = false;
                comboBoxTable.Enabled = false;

                UpdateControlsState();
            }
        }

        private void UpdateControlsState()
        {
            UpdatePaymentStatusControlState();
            UpdateOrderStatusControlState();
        }

        private void UpdatePaymentStatusControlState()
        {
            if (comboBoxStatusPayment.Text == "Оплачен")
            {
                comboBoxStatusPayment.Enabled = false;
            }
            else
            {
                comboBoxStatusPayment.Enabled = true;
            }
        }

        private void UpdateOrderStatusControlState()
        {
            if (comboBoxStatusOrder.Text == "Завершен")
            {
                comboBoxStatusOrder.Enabled = false;
            }
            else
            {
                comboBoxStatusOrder.Enabled = true;
            }
        }

        private void LoadComboBoxData(int currentWorkerId = 0)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand cmdWorkers = new MySqlCommand("SELECT WorkerId, WorkerFIO FROM Worker", con);
                    MySqlDataAdapter daWorkers = new MySqlDataAdapter(cmdWorkers);
                    DataTable workersTable = new DataTable();
                    daWorkers.Fill(workersTable);

                    comboBoxWaiter.DisplayMember = "WorkerFIO";
                    comboBoxWaiter.ValueMember = "WorkerId";
                    comboBoxWaiter.DataSource = workersTable;

                    if (mode == "add" && currentWorkerId > 0)
                    {
                        comboBoxWaiter.SelectedValue = currentWorkerId;
                    }

                    MySqlCommand cmdClients = new MySqlCommand("SELECT ClientId, ClientFIO FROM Client", con);
                    MySqlDataAdapter daClients = new MySqlDataAdapter(cmdClients);
                    DataTable clientsTable = new DataTable();
                    daClients.Fill(clientsTable);

                    comboBoxClient.Items.Clear();
                    comboBoxClient.Items.Add("");
                    foreach (DataRow row in clientsTable.Rows)
                    {
                        comboBoxClient.Items.Add(new KeyValuePair<int, string>(
                            Convert.ToInt32(row["ClientId"]),
                            row["ClientFIO"].ToString()
                        ));
                    }

                    comboBoxClient.DisplayMember = "Value";
                    comboBoxClient.ValueMember = "Key";

                    MySqlCommand cmdTables = new MySqlCommand("SELECT TablesId FROM Tables", con);
                    MySqlDataAdapter daTables = new MySqlDataAdapter(cmdTables);
                    DataTable tablesTable = new DataTable();
                    daTables.Fill(tablesTable);

                    comboBoxTable.Items.Clear();
                    comboBoxTable.Items.Add("");
                    foreach (DataRow row in tablesTable.Rows)
                    {
                        comboBoxTable.Items.Add(row["TablesId"].ToString());
                    }

                    comboBoxStatusOrder.Items.Clear();
                    comboBoxStatusOrder.Items.Add("Принят");
                    comboBoxStatusOrder.Items.Add("В обработке");
                    comboBoxStatusOrder.Items.Add("На кухне");
                    comboBoxStatusOrder.Items.Add("Готов");
                    comboBoxStatusOrder.Items.Add("Завершен");

                    comboBoxStatusPayment.Items.Clear();
                    comboBoxStatusPayment.Items.Add("Оплачен");
                    comboBoxStatusPayment.Items.Add("Не оплачен");

                    if (mode == "edit")
                    {
                        LoadOrderDetails();
                    }
                    else if (mode == "add")
                    {
                        dateTimePickerOrder.Value = DateTime.Now;
                        comboBoxClient.SelectedIndex = 0;
                    }

                    SetControlsState();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrderDetails()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(@"
                SELECT WorkerId, ClientId, TableId, OrderDate, OrderPrice, OrderStatus, OrderStatusPayment
                FROM `Order` 
                WHERE OrderId = @OrderId", con);
                    cmd.Parameters.AddWithValue("@OrderId", OrderID);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        comboBoxWaiter.SelectedValue = reader.GetInt32("WorkerId");

                        initialOrderStatus = reader.GetString("OrderStatus");
                        initialOrderStatusPayment = reader.GetString("OrderStatusPayment");

                        if (!reader.IsDBNull(reader.GetOrdinal("ClientId")))
                        {
                            int clientId = reader.GetInt32("ClientId");
                            foreach (KeyValuePair<int, string> item in comboBoxClient.Items)
                            {
                                if (item.Key == clientId)
                                {
                                    comboBoxClient.SelectedItem = item;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            comboBoxClient.SelectedIndex = 0;
                        }

                        dateTimePickerOrder.Value = reader.GetDateTime("OrderDate");
                        comboBoxStatusOrder.Text = initialOrderStatus;
                        comboBoxStatusPayment.Text = initialOrderStatusPayment;
                    }
                    reader.Close();

                    UpdateControlsState();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonOrderItem_Click(object sender, EventArgs e)
        {
            if (mode == "add")
            {
                if (!ValidateInput())
                    return;

                DialogResult confirmResult = MessageBox.Show(
                    "Вы уверены, что хотите создать новый заказ?",
                    "Подтверждение сохранения",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes)
                    return;

                if (SaveOrder())
                {
                    OrderItem orderItemForm = new OrderItem(3, OrderID);
                    this.Hide();
                    orderItemForm.ShowDialog();
                    this.Close();
                }
            }
            else if (mode == "edit")
            {
                bool isCompletedNow = (comboBoxStatusOrder.Text == "Завершен" && comboBoxStatusPayment.Text == "Оплачен");
                bool wasCompletedInitially = (initialOrderStatus == "Завершен" && initialOrderStatusPayment == "Оплачен");

                if (!wasCompletedInitially && isCompletedNow)
                {
                    if (!ValidateInput())
                        return;

                    DialogResult confirmResult = MessageBox.Show(
                        "Вы уверены, что хотите сохранить изменения?",
                        "Подтверждение сохранения",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmResult != DialogResult.Yes)
                        return;

                    if (SaveOrder())
                    {
                        MessageBox.Show($"Заказ №{OrderID} успешно обновлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    return;
                }

                if (!ValidateInput())
                    return;

                DialogResult confirmResultOther = MessageBox.Show(
                    "Вы уверены, что хотите сохранить изменения?",
                    "Подтверждение сохранения",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResultOther != DialogResult.Yes)
                    return;

                if (SaveOrder())
                {
                    MessageBox.Show($"Заказ №{OrderID} успешно обновлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DialogResult result = MessageBox.Show(
                        "Есть изменения в составе заказа?",
                        "Состав заказа",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        OrderItem orderItemForm = new OrderItem(3, OrderID);
                        this.Hide();
                        orderItemForm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }

        private bool ValidateInput()
        {
            if (comboBoxWaiter.SelectedValue == null)
            {
                MessageBox.Show("Выберите сотрудника!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxWaiter.Focus();
                return false;
            }

            if (mode == "edit")
            {
                if (comboBoxStatusOrder.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите статус заказа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxStatusOrder.Focus();
                    return false;
                }

                if (comboBoxStatusPayment.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите статус оплаты!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxStatusPayment.Focus();
                    return false;
                }
            }

            return true;
        }

        private bool SaveOrder()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    if (mode == "add")
                    {
                        string query = @"INSERT INTO `Order` 
                            (WorkerId, ClientId, TableId, OrderDate, OrderPrice, OrderStatus, OrderStatusPayment)
                            VALUES (@WorkerId, @ClientId, @TableId, @OrderDate, @OrderPrice, @OrderStatus, @OrderStatusPayment);
                            SELECT LAST_INSERT_ID();";

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@WorkerId", comboBoxWaiter.SelectedValue);

                        object clientIdParam = string.IsNullOrEmpty(comboBoxClient.Text) ? (object)DBNull.Value : comboBoxClient.SelectedValue;
                        cmd.Parameters.AddWithValue("@ClientId", clientIdParam);

                        object tableIdParam = string.IsNullOrEmpty(comboBoxTable.Text) ? (object)DBNull.Value : Convert.ToInt32(comboBoxTable.Text);
                        cmd.Parameters.AddWithValue("@TableId", tableIdParam);

                        cmd.Parameters.AddWithValue("@OrderDate", dateTimePickerOrder.Value);
                        cmd.Parameters.AddWithValue("@OrderPrice", 0);
                        cmd.Parameters.AddWithValue("@OrderStatus", "Принят");
                        cmd.Parameters.AddWithValue("@OrderStatusPayment", "Не оплачен");

                        OrderID = Convert.ToInt32(cmd.ExecuteScalar());
                        MessageBox.Show($"Заказ №{OrderID} успешно создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        string query = @"UPDATE `Order` 
                            SET WorkerId = @WorkerId,
                                ClientId = @ClientId,
                                TableId = @TableId,
                                OrderDate = @OrderDate,
                                OrderStatus = @OrderStatus,
                                OrderStatusPayment = @OrderStatusPayment
                            WHERE OrderId = @OrderId";

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@WorkerId", comboBoxWaiter.SelectedValue);

                        object clientIdParam = string.IsNullOrEmpty(comboBoxClient.Text) ? (object)DBNull.Value : comboBoxClient.SelectedValue;
                        cmd.Parameters.AddWithValue("@ClientId", clientIdParam);

                        object tableIdParam = string.IsNullOrEmpty(comboBoxTable.Text) ? (object)DBNull.Value : Convert.ToInt32(comboBoxTable.Text);
                        cmd.Parameters.AddWithValue("@TableId", tableIdParam);

                        cmd.Parameters.AddWithValue("@OrderDate", dateTimePickerOrder.Value);
                        cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus);
                        cmd.Parameters.AddWithValue("@OrderStatusPayment", OrderStatusPayment);
                        cmd.Parameters.AddWithValue("@OrderId", OrderID);

                        cmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void comboBoxStatusPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingStatus) return;

            isUpdatingStatus = true;

            if (comboBoxStatusPayment.Text == "Оплачен" && comboBoxStatusOrder.Text != "Завершен")
            {
                comboBoxStatusOrder.Text = "Завершен";
            }

            UpdateControlsState();
            isUpdatingStatus = false;
        }

        private void comboBoxStatusOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingStatus) return;

            isUpdatingStatus = true;

            if (comboBoxStatusOrder.Text == "Завершен" && comboBoxStatusPayment.Text != "Оплачен")
            {
                comboBoxStatusPayment.Text = "Оплачен";
            }

            UpdateControlsState();
            isUpdatingStatus = false;
        }
    }
}