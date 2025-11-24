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
        private Dictionary<int, int> clientTableMap = new Dictionary<int, int>();

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

                comboBoxStatusOrder.SelectedIndex = 0;
                comboBoxStatusPayment.SelectedIndex = 1;

                comboBoxClient.SelectedIndex = -1;
                comboBoxClient.Text = "";
                comboBoxTable.SelectedIndex = -1;
                comboBoxTable.Text = "";
            }
            else if (mode == "edit")
            {
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

                    MySqlCommand cmdWorkers = new MySqlCommand("SELECT WorkerId, WorkerFIO FROM Worker WHERE IsActive = 1", con);
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

                    LoadClientsWithTodayBooking(con);

                    if (mode == "edit")
                    {
                        string tablesQuery = "SELECT TablesId FROM Tables";
                        MySqlCommand cmdTables = new MySqlCommand(tablesQuery, con);
                        MySqlDataAdapter daTables = new MySqlDataAdapter(cmdTables);
                        DataTable tablesTable = new DataTable();
                        daTables.Fill(tablesTable);

                        comboBoxTable.Items.Clear();
                        comboBoxTable.Items.Add("");
                        foreach (DataRow row in tablesTable.Rows)
                        {
                            comboBoxTable.Items.Add(row["TablesId"].ToString());
                        }
                    }
                    else if (mode == "add")
                    {
                        LoadFreeTables(con);
                    }

                    comboBoxStatusOrder.Items.Clear();
                    comboBoxStatusOrder.Items.Add("Новый");
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
                        comboBoxStatusOrder.SelectedIndex = 0;
                        comboBoxStatusPayment.SelectedIndex = 1;
                        comboBoxTable.SelectedIndex = -1;
                        comboBoxTable.Text = "";
                    }

                    SetControlsState();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadClientsWithTodayBooking(MySqlConnection con)
        {
            try
            {
                clientTableMap.Clear();

                string query;

                if (mode == "add")
                {
                    query = @"
                SELECT DISTINCT c.ClientId, c.ClientFIO, b.TableId, b.BookingDate
                FROM Client c 
                INNER JOIN Booking b ON c.ClientId = b.ClientId 
                WHERE DATE(b.BookingDate) = CURDATE() 
                AND b.BookingDate BETWEEN DATE_SUB(NOW(), INTERVAL 30 MINUTE) AND DATE_ADD(NOW(), INTERVAL 1 HOUR)
                AND c.IsActive = 1
                ORDER BY c.ClientFIO";
                }
                else
                {
                    query = @"
                SELECT ClientId, ClientFIO
                FROM Client 
                WHERE IsActive = 1 
                ORDER BY ClientFIO";
                }

                MySqlCommand cmdClients = new MySqlCommand(query, con);
                MySqlDataAdapter daClients = new MySqlDataAdapter(cmdClients);
                DataTable clientsTable = new DataTable();
                daClients.Fill(clientsTable);

                comboBoxClient.Items.Clear();
                comboBoxClient.Items.Add("");

                foreach (DataRow row in clientsTable.Rows)
                {
                    int clientId = Convert.ToInt32(row["ClientId"]);
                    string clientName = row["ClientFIO"].ToString();

                    if (mode == "add" && row.Table.Columns.Contains("TableId"))
                    {
                        int tableId = Convert.ToInt32(row["TableId"]);
                        clientTableMap[clientId] = tableId;
                    }

                    comboBoxClient.Items.Add(new KeyValuePair<int, string>(clientId, clientName));
                }

                comboBoxClient.DisplayMember = "Value";
                comboBoxClient.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}",
                              "Ошибка",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
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

                            bool clientFound = false;
                            for (int i = 0; i < comboBoxClient.Items.Count; i++)
                            {
                                if (comboBoxClient.Items[i] is KeyValuePair<int, string> item && item.Key == clientId)
                                {
                                    comboBoxClient.SelectedIndex = i;
                                    clientFound = true;
                                    break;
                                }
                            }

                            if (!clientFound)
                            {
                                reader.Close();
                                LoadAllClients(con);
                                SetClientSelection(clientId);
                            }
                        }
                        else
                        {
                            comboBoxClient.SelectedIndex = 0;
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("TableId")))
                        {
                            int tableId = reader.GetInt32("TableId");

                            bool found = false;
                            for (int i = 0; i < comboBoxTable.Items.Count; i++)
                            {
                                if (comboBoxTable.Items[i].ToString() == tableId.ToString())
                                {
                                    comboBoxTable.SelectedIndex = i;
                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                            {
                                comboBoxTable.Items.Add(tableId.ToString());
                                comboBoxTable.SelectedItem = tableId.ToString();
                            }
                        }
                        else
                        {
                            comboBoxTable.SelectedIndex = 0;
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

        private void LoadAllClients(MySqlConnection con)
        {
            try
            {
                string query = "SELECT ClientId, ClientFIO FROM Client WHERE IsActive = 1 ORDER BY ClientFIO";
                MySqlCommand cmdClients = new MySqlCommand(query, con);
                MySqlDataAdapter daClients = new MySqlDataAdapter(cmdClients);
                DataTable clientsTable = new DataTable();
                daClients.Fill(clientsTable);

                comboBoxClient.Items.Clear();
                comboBoxClient.Items.Add("");

                foreach (DataRow row in clientsTable.Rows)
                {
                    int clientId = Convert.ToInt32(row["ClientId"]);
                    string clientName = row["ClientFIO"].ToString();
                    comboBoxClient.Items.Add(new KeyValuePair<int, string>(clientId, clientName));
                }

                comboBoxClient.DisplayMember = "Value";
                comboBoxClient.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetClientSelection(int clientId)
        {
            for (int i = 0; i < comboBoxClient.Items.Count; i++)
            {
                if (comboBoxClient.Items[i] is KeyValuePair<int, string> item && item.Key == clientId)
                {
                    comboBoxClient.SelectedIndex = i;
                    break;
                }
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

                if (string.IsNullOrEmpty(comboBoxTable.Text))
                {
                    MessageBox.Show("Выберите столик для создания заказа!",
                                   "Столик не выбран",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxTable.Focus();
                    return;
                }

                if (!IsTableAvailable())
                {
                    MessageBox.Show("Выбранный стол уже занят! Пожалуйста, выберите другой стол.",
                                   "Стол занят", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CheckTableBookingRestrictions())
                {
                    return;
                }

                DialogResult confirmResult = MessageBox.Show(
                    "Вы уверены, что хотите создать новый заказ?",
                    "Подтверждение сохранения",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes)
                    return;

                if (SaveOrder())
                {
                    MessageBox.Show($"Заказ №{OrderID} успешно создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    UpdateTableStatus(comboBoxTable.Text, "Занят");

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

            if (mode == "add" && string.IsNullOrEmpty(comboBoxTable.Text))
            {
                MessageBox.Show("Выберите столик!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxTable.Focus();
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
            bool wasCompletedInitially = false;
            bool isCompletedNow = false;
            int? tableId = null;
            int? clientId = null;
            DateTime? bookingDateTime = null;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    if (!string.IsNullOrEmpty(comboBoxTable.Text))
                    {
                        tableId = Convert.ToInt32(comboBoxTable.Text);
                    }

                    if (comboBoxClient.SelectedItem != null && comboBoxClient.SelectedIndex > 0)
                    {
                        clientId = ((KeyValuePair<int, string>)comboBoxClient.SelectedItem).Key;

                        if (clientId.HasValue && clientTableMap.ContainsKey(clientId.Value))
                        {
                            bookingDateTime = DateTime.Now;
                        }
                    }

                    if (mode == "edit")
                    {
                        MySqlCommand checkCmd = new MySqlCommand(
                            "SELECT OrderStatus, OrderStatusPayment, TableId FROM `Order` WHERE OrderId = @OrderId",
                            con);
                        checkCmd.Parameters.AddWithValue("@OrderId", OrderID);
                        using (var reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                wasCompletedInitially = (reader.GetString("OrderStatus") == "Завершен" &&
                                                       reader.GetString("OrderStatusPayment") == "Оплачен");

                                if (!reader.IsDBNull(reader.GetOrdinal("TableId")))
                                {
                                    tableId = reader.GetInt32("TableId");
                                }
                            }
                        }

                        isCompletedNow = (OrderStatus == "Завершен" && OrderStatusPayment == "Оплачен");
                    }

                    if (mode == "add")
                    {
                        string query = @"INSERT INTO `Order` 
                    (WorkerId, ClientId, TableId, OrderDate, OrderPrice, OrderStatus, OrderStatusPayment)
                    VALUES (@WorkerId, @ClientId, @TableId, @OrderDate, @OrderPrice, @OrderStatus, @OrderStatusPayment);
                    SELECT LAST_INSERT_ID();";

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@WorkerId", comboBoxWaiter.SelectedValue);

                        object clientIdParam = string.IsNullOrEmpty(comboBoxClient.Text) ? (object)DBNull.Value : ((KeyValuePair<int, string>)comboBoxClient.SelectedItem).Key;
                        cmd.Parameters.AddWithValue("@ClientId", clientIdParam);

                        object tableIdParam = string.IsNullOrEmpty(comboBoxTable.Text) ? (object)DBNull.Value : Convert.ToInt32(comboBoxTable.Text);
                        cmd.Parameters.AddWithValue("@TableId", tableIdParam);

                        cmd.Parameters.AddWithValue("@OrderDate", dateTimePickerOrder.Value);
                        cmd.Parameters.AddWithValue("@OrderPrice", 0);
                        cmd.Parameters.AddWithValue("@OrderStatus", "Новый");
                        cmd.Parameters.AddWithValue("@OrderStatusPayment", "Не оплачен");

                        OrderID = Convert.ToInt32(cmd.ExecuteScalar());

                        if (tableId.HasValue)
                        {
                            UpdateTableStatus(tableId.Value, "Занят");
                        }

                        if (clientId.HasValue && tableId.HasValue)
                        {
                            DeleteCurrentClientBooking(con, clientId.Value, tableId.Value);
                        }

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

                        object clientIdParam = string.IsNullOrEmpty(comboBoxClient.Text) ? (object)DBNull.Value : ((KeyValuePair<int, string>)comboBoxClient.SelectedItem).Key;
                        cmd.Parameters.AddWithValue("@ClientId", clientIdParam);

                        object tableIdParam = string.IsNullOrEmpty(comboBoxTable.Text) ? (object)DBNull.Value : Convert.ToInt32(comboBoxTable.Text);
                        cmd.Parameters.AddWithValue("@TableId", tableIdParam);

                        cmd.Parameters.AddWithValue("@OrderDate", dateTimePickerOrder.Value);
                        cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus);
                        cmd.Parameters.AddWithValue("@OrderStatusPayment", OrderStatusPayment);
                        cmd.Parameters.AddWithValue("@OrderId", OrderID);

                        cmd.ExecuteNonQuery();

                        if (tableId.HasValue)
                        {
                            if (isCompletedNow && !wasCompletedInitially)
                            {
                                UpdateTableStatus(tableId.Value, "Свободен");
                            }
                            else if (!isCompletedNow && wasCompletedInitially)
                            {
                                UpdateTableStatus(tableId.Value, "Занят");
                            }
                            else if (!isCompletedNow && !wasCompletedInitially)
                            {
                                UpdateTableStatus(tableId.Value, "Занят");
                            }
                        }
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

        private void DeleteCurrentClientBooking(MySqlConnection con, int clientId, int tableId)
        {
            try
            {
                MySqlCommand deleteCmd = new MySqlCommand(@"
            DELETE FROM booking 
            WHERE ClientId = @ClientId 
            AND TableId = @TableId 
            AND DATE(BookingDate) = CURDATE() 
            AND BookingDate BETWEEN DATE_SUB(NOW(), INTERVAL 30 MINUTE) AND DATE_ADD(NOW(), INTERVAL 1 HOUR)",
                    con);
                deleteCmd.Parameters.AddWithValue("@ClientId", clientId);
                deleteCmd.Parameters.AddWithValue("@TableId", tableId);

                int deletedCount = deleteCmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

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

        private void UpdateTableStatus(int tableId, string status)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE Tables SET TablesStatus = @Status WHERE TablesId = @TableId",
                        con);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@TableId", tableId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статуса стола: {ex.Message}", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTableStatus(string tableIdText, string status)
        {
            if (string.IsNullOrEmpty(tableIdText)) return;
            UpdateTableStatus(Convert.ToInt32(tableIdText), status);
        }

        private bool IsTableAvailable()
        {
            if (string.IsNullOrEmpty(comboBoxTable.Text)) return true;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT TablesStatus FROM Tables WHERE TablesId = @TableId",
                        con);
                    cmd.Parameters.AddWithValue("@TableId", Convert.ToInt32(comboBoxTable.Text));
                    var result = cmd.ExecuteScalar();

                    return result?.ToString() != "Занят";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке стола: {ex.Message}", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void OrderInsert_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mode == "add" && OrderID > 0 && !string.IsNullOrEmpty(comboBoxTable.Text))
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                    {
                        con.Open();
                        MySqlCommand checkCmd = new MySqlCommand(
                            "SELECT COUNT(*) FROM OrderItems WHERE OrderId = @OrderId",
                            con);
                        checkCmd.Parameters.AddWithValue("@OrderId", OrderID);
                        int itemCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (itemCount == 0)
                        {
                            UpdateTableStatus(comboBoxTable.Text, "Свободен");

                            MySqlCommand deleteCmd = new MySqlCommand(
                                "DELETE FROM `Order` WHERE OrderId = @OrderId",
                                con);
                            deleteCmd.Parameters.AddWithValue("@OrderId", OrderID);
                            deleteCmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void LoadFreeTables(MySqlConnection con)
        {
            string tablesQuery = "SELECT TablesId FROM Tables WHERE TablesStatus = 'Свободен'";
            MySqlCommand cmdTables = new MySqlCommand(tablesQuery, con);
            MySqlDataAdapter daTables = new MySqlDataAdapter(cmdTables);
            DataTable tablesTable = new DataTable();
            daTables.Fill(tablesTable);

            comboBoxTable.Items.Clear();
            comboBoxTable.Items.Add("");
            foreach (DataRow row in tablesTable.Rows)
            {
                comboBoxTable.Items.Add(row["TablesId"].ToString());
            }
        }

        private void LoadSpecificTable(MySqlConnection con, int tableId)
        {
            comboBoxTable.Items.Clear();
            comboBoxTable.Items.Add("");

            comboBoxTable.Items.Add(tableId.ToString());

            comboBoxTable.SelectedItem = tableId.ToString();
        }

        private void comboBoxClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mode != "add") return;

            if (comboBoxClient.SelectedItem == null ||
                comboBoxClient.SelectedIndex == 0 ||
                string.IsNullOrEmpty(comboBoxClient.Text))
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                    {
                        con.Open();
                        LoadFreeTables(con);
                        comboBoxTable.Enabled = true;
                        comboBoxTable.SelectedIndex = -1;
                        comboBoxTable.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке столов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            var selectedClient = (KeyValuePair<int, string>)comboBoxClient.SelectedItem;
            int clientId = selectedClient.Key;

            if (clientTableMap.ContainsKey(clientId))
            {
                int tableId = clientTableMap[clientId];

                try
                {
                    using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                    {
                        con.Open();

                        LoadSpecificTable(con, tableId);
                        comboBoxTable.Enabled = false;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при установке стола: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                    {
                        con.Open();
                        LoadFreeTables(con);
                        comboBoxTable.Enabled = true;
                        comboBoxTable.SelectedIndex = -1;
                        comboBoxTable.Text = "";

                        MessageBox.Show("Для выбранного клиента не найден забронированный стол. Пожалуйста, выберите стол вручную.",
                                      "Информация",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке столов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool CheckTableBookingRestrictions()
        {
            if (string.IsNullOrEmpty(comboBoxTable.Text))
                return true;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    int tableId = Convert.ToInt32(comboBoxTable.Text);

                    MySqlCommand checkBookingCmd = new MySqlCommand(@"
                SELECT 
                    BookingDate,
                    TIMESTAMPDIFF(MINUTE, NOW(), BookingDate) as MinutesUntilBooking,
                    c.ClientFIO
                FROM booking b
                JOIN client c ON b.ClientId = c.ClientId
                WHERE b.TableId = @TableId 
                AND DATE(b.BookingDate) = CURDATE()
                AND b.BookingDate > NOW()
                AND c.IsActive = 1 
                ORDER BY b.BookingDate", con);

                    checkBookingCmd.Parameters.AddWithValue("@TableId", tableId);

                    using (MySqlDataReader reader = checkBookingCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int minutesUntilBooking = reader.GetInt32("MinutesUntilBooking");

                            if (minutesUntilBooking < 120)
                            {
                                MessageBox.Show(
                                    $"Этот стол забронирован!\n" +
                                    $"Создание заказа невозможно!",
                                    "Стол забронирован",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                                return false;
                            }
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке бронирования стола: {ex.Message}",
                              "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}