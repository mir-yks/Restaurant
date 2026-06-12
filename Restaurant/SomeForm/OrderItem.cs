using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class OrderItem : Form
    {
        private int roleId;
        private int orderId;
        private DataTable dishesTable;
        private DataTable allDishesTable;
        private DataTable offersTable;
        private DataTable orderItemsData;
        private List<int> originalItemIds = new List<int>();

        public OrderItem(int role, int orderId)
        {
            InitializeComponent();
            roleId = role;
            this.orderId = orderId;
            ConfigureButtons();
            InactivityManager.Init();

            SetupFonts();
            SetFormTitle();
        }

        private void SetFormTitle()
        {
            switch (roleId)
            {
                case 2:
                    this.Text = "Состав заказа";
                    break;
                case 3:
                    try
                    {
                        using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                        {
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand(
                                "SELECT COUNT(*) FROM `Order` WHERE OrderId = @OrderId",
                                con);
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            int count = Convert.ToInt32(cmd.ExecuteScalar());

                            if (count > 0)
                            {
                                this.Text = "Состав заказа при его редактировании";
                            }
                            else
                            {
                                this.Text = "Состав заказа при его создании";
                            }
                        }
                    }
                    catch
                    {
                        this.Text = "Состав заказа";
                    }
                    break;
                default:
                    this.Text = "Состав заказа";
                    break;
            }
        }

        private void SetupFonts()
        {
            label1.Font = Fonts.MontserratAlternatesBold(12f);
            labelSumma.Font = Fonts.MontserratAlternatesBold(12f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);
            dataGridView1.Font = Fonts.MontserratAlternatesRegular(12f);
            labelTotal.Font = Fonts.MontserratAlternatesBold(12f);
        }

        private void ConfigureButtons()
        {
            if (roleId == 3)
            {
                buttonWrite.Visible = true;
            }
            else if (roleId == 2)
            {
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                buttonWrite.Visible = false;
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OrderItem_Load(object sender, EventArgs e)
        {
            LoadDishesData();
            LoadOffersData();
            LoadOrderItems();
            LoadOrderTotalSum();
            UpdateTotalCount();
            ConfigureComboBoxColumn();
        }

        private void LoadDishesData()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    MySqlCommand cmdAll = new MySqlCommand(@"
                        SELECT 
                            DishId, 
                            COALESCE(OriginalDishName, DishName) as DisplayName,
                            DishName as CurrentName,
                            DishPrice, 
                            OffersDish, 
                            IsActive
                        FROM MenuDish", con);
                    allDishesTable = new DataTable();
                    MySqlDataAdapter daAll = new MySqlDataAdapter(cmdAll);
                    daAll.Fill(allDishesTable);

                    MySqlCommand cmdActive = new MySqlCommand(@"
                        SELECT 
                            DishId, 
                            DishName,
                            DishPrice, 
                            OffersDish
                        FROM MenuDish 
                        WHERE IsActive = 1", con);
                    dishesTable = new DataTable();
                    MySqlDataAdapter daActive = new MySqlDataAdapter(cmdActive);
                    daActive.Fill(dishesTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке блюд: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOffersData()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                        SELECT OffersDishId, OffersDishName, OffersDishDicsount
                        FROM OffersDish", con);

                    offersTable = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(offersTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке акций: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrderItems()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(@"
                        SELECT 
                            i.DishId,
                            i.DishCount,
                            i.OriginalPrice,
                            i.OriginalDiscount,
                            i.OriginalDishName,
                            m.DishName,
                            m.OffersDish
                        FROM OrderItems i
                        JOIN MenuDish m ON i.DishId = m.DishId
                        WHERE i.OrderId = @OrderId
                        ORDER BY i.OrderItemId;
                    ", con);

                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    orderItemsData = new DataTable();
                    da.Fill(orderItemsData);

                    dataGridView1.Rows.Clear();
                    originalItemIds.Clear();

                    foreach (DataRow row in orderItemsData.Rows)
                    {
                        int dishId = Convert.ToInt32(row["DishId"]);
                        int quantity = Convert.ToInt32(row["DishCount"]);
                        decimal price;
                        int discount;
                        string displayName;

                        if (row["OriginalPrice"] != DBNull.Value && Convert.ToDecimal(row["OriginalPrice"]) > 0)
                        {
                            price = Convert.ToDecimal(row["OriginalPrice"]);
                            discount = Convert.ToInt32(row["OriginalDiscount"]);

                            string originalDishName = row["OriginalDishName"] != DBNull.Value ?
                                row["OriginalDishName"].ToString() : row["DishName"].ToString();

                            if (discount > 0)
                            {
                                displayName = $"{originalDishName} (-{discount}%)";
                            }
                            else
                            {
                                displayName = originalDishName;
                            }
                        }
                        else
                        {
                            DataRow[] dishRows = dishesTable.Select($"DishId = {dishId}");
                            if (dishRows.Length > 0)
                            {
                                price = Convert.ToDecimal(dishRows[0]["DishPrice"]);
                                discount = PriceCalculator.Instance.GetDiscountForDish(dishId, allDishesTable, offersTable);
                                string dishName = dishRows[0]["DishName"].ToString();

                                if (discount > 0)
                                {
                                    displayName = $"{dishName} (-{discount}%)";
                                }
                                else
                                {
                                    displayName = dishName;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }

                        decimal sum = price * quantity * (100 - discount) / 100;

                        int rowIndex = dataGridView1.Rows.Add();
                        DataGridViewRow newRow = dataGridView1.Rows[rowIndex];

                        newRow.Cells["ColumnDish"].Value = dishId;
                        newRow.Cells["ColumnDish"].Tag = displayName;
                        newRow.Cells["ColumnQuantity"].Value = quantity;
                        newRow.Cells["ColumnPrice"].Value = Math.Round(price, 2);
                        newRow.Cells["ColumnSum"].Value = Math.Round(sum, 2);

                        originalItemIds.Add(dishId);
                    }

                    UpdateTotalCount();
                    SetOriginalItemsReadOnly();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetOriginalItemsReadOnly()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                object dishIdValue = row.Cells["ColumnDish"].Value;
                if (dishIdValue != null && dishIdValue != DBNull.Value)
                {
                    try
                    {
                        int dishId = Convert.ToInt32(dishIdValue);
                        if (originalItemIds.Contains(dishId))
                        {
                            row.Cells["ColumnDish"].ReadOnly = true;
                            row.Cells["ColumnQuantity"].ReadOnly = true;
                        }
                    }
                    catch { }
                }
            }
        }

        private void ConfigureComboBoxColumn()
        {
            if (dataGridView1.Columns["ColumnDish"] is DataGridViewComboBoxColumn dishColumn)
            {
                DataTable displayTable = dishesTable.Copy();
                if (!displayTable.Columns.Contains("DisplayName"))
                {
                    displayTable.Columns.Add("DisplayName", typeof(string));
                }

                foreach (DataRow row in displayTable.Rows)
                {
                    string dishName = row["DishName"].ToString();
                    object offersDish = row["OffersDish"];
                    string displayName = dishName;

                    if (offersDish != null && offersDish != DBNull.Value && Convert.ToInt32(offersDish) > 0)
                    {
                        int offerId = Convert.ToInt32(offersDish);
                        DataRow[] offerRows = offersTable.Select($"OffersDishId = {offerId}");
                        if (offerRows.Length > 0)
                        {
                            int discount = Convert.ToInt32(offerRows[0]["OffersDishDicsount"]);
                            displayName = $"{dishName} (-{discount}%)";
                        }
                    }
                    row["DisplayName"] = displayName;
                }

                dishColumn.DataSource = displayTable;
                dishColumn.DisplayMember = "DisplayName";
                dishColumn.ValueMember = "DishId";
            }
        }

        private void UpdateTotalCount()
        {
            int totalCount = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow &&
                    row.Cells["ColumnDish"].Value != null &&
                    row.Cells["ColumnDish"].Value != DBNull.Value)
                {
                    totalCount++;
                }
            }
            labelTotal.Text = $"Всего: {totalCount}";
        }

        private void LoadOrderTotalSum()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();
                    decimal totalSum = PriceCalculator.Instance.CalculateOrderTotalSumFromDatabase(orderId, con);
                    label1.Text = totalSum.ToString("F2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке суммы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label1.Text = "0.00";
            }
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите сохранить запись?", "Подтверждение записи",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (SaveAllChanges())
                {
                    this.Close();
                }
            }
        }

        private bool SaveAllChanges()
        {
            try
            {
                dataGridView1.EndEdit();
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    MySqlCommand deleteCmd = new MySqlCommand(
                        "DELETE FROM OrderItems WHERE OrderId = @OrderId", con);

                    deleteCmd.Parameters.AddWithValue("@OrderId", orderId);
                    deleteCmd.ExecuteNonQuery();

                    decimal totalSum = 0;
                    int itemsSaved = 0;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["ColumnDish"].Value == null ||
                            row.Cells["ColumnDish"].Value == DBNull.Value)
                        {
                            continue;
                        }

                        if (row.Cells["ColumnQuantity"].Value == null ||
                            row.Cells["ColumnQuantity"].Value == DBNull.Value)
                        {
                            continue;
                        }

                        object value = row.Cells["ColumnDish"].Value;

                        if (value is DataRowView drv)
                        {
                            value = drv["DishId"];
                        }

                        int dishId = Convert.ToInt32(value);

                        int quantity = Convert.ToInt32(row.Cells["ColumnQuantity"].Value);

                        decimal displayPrice = 0;

                        if (row.Cells["ColumnPrice"].Value != null &&
                            row.Cells["ColumnPrice"].Value != DBNull.Value)
                        {
                            displayPrice = Convert.ToDecimal(row.Cells["ColumnPrice"].Value);
                        }
                        else
                        {
                            DataRow[] priceRows = dishesTable.Select($"DishId = {dishId}");

                            if (priceRows.Length > 0)
                            {
                                displayPrice = Convert.ToDecimal(priceRows[0]["DishPrice"]);
                            }
                        }

                        int discount = PriceCalculator.Instance.GetDiscountForDish(
                            dishId,
                            allDishesTable,
                            offersTable);

                        DataRow[] dishRows = dishesTable.Select($"DishId = {dishId}");

                        string dishName = "";

                        if (dishRows.Length > 0)
                        {
                            dishName = dishRows[0]["DishName"].ToString();
                        }

                        decimal itemSum = displayPrice * quantity * (100 - discount) / 100;

                        totalSum += itemSum;

                        string insertQuery = @"
                    INSERT INTO OrderItems
                    (
                        OrderId,
                        DishId,
                        DishCount,
                        OriginalPrice,
                        OriginalDiscount,
                        OriginalDishName
                    )
                    VALUES
                    (
                        @OrderId,
                        @DishId,
                        @DishCount,
                        @OriginalPrice,
                        @OriginalDiscount,
                        @OriginalDishName
                    )";

                        MySqlCommand cmd = new MySqlCommand(insertQuery, con);

                        cmd.Parameters.AddWithValue("@OrderId", orderId);
                        cmd.Parameters.AddWithValue("@DishId", dishId);
                        cmd.Parameters.AddWithValue("@DishCount", quantity);
                        cmd.Parameters.AddWithValue("@OriginalPrice", displayPrice);
                        cmd.Parameters.AddWithValue("@OriginalDiscount", discount);
                        cmd.Parameters.AddWithValue("@OriginalDishName", dishName);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            itemsSaved++;
                        }
                    }

                    MySqlCommand updateOrderCmd = new MySqlCommand(
                        "UPDATE `Order` SET OrderPrice = @OrderPrice WHERE OrderId = @OrderId",
                        con);

                    updateOrderCmd.Parameters.AddWithValue(
                        "@OrderPrice",
                        Math.Round(totalSum, 2));

                    updateOrderCmd.Parameters.AddWithValue(
                        "@OrderId",
                        orderId);

                    updateOrderCmd.ExecuteNonQuery();

                    MessageBox.Show(
                        $"Сохранено {itemsSaved} позиций! Сумма заказа: {totalSum:F2} руб.",
                        "Успех",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    RefreshOrderData();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при сохранении: {ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        private void RefreshOrderData()
        {
            if (orderItemsData != null)
                orderItemsData.Clear();
            originalItemIds.Clear();
            dataGridView1.Rows.Clear();

            LoadOrderItems();
            LoadOrderTotalSum();
            UpdateTotalCount();
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count) return;

            if (e.ColumnIndex == dataGridView1.Columns["ColumnDish"].Index)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                if (row.Cells["ColumnDish"].Value != null && row.Cells["ColumnDish"].Value != DBNull.Value)
                {
                    int dishId = Convert.ToInt32(row.Cells["ColumnDish"].Value);

                    if (originalItemIds.Contains(dishId))
                    {
                        DataRow[] originalRows = orderItemsData.Select($"DishId = {dishId}");
                        if (originalRows.Length > 0)
                        {
                            string originalDishName = originalRows[0]["OriginalDishName"] != DBNull.Value ?
                                originalRows[0]["OriginalDishName"].ToString() : originalRows[0]["DishName"].ToString();
                            int discount = Convert.ToInt32(originalRows[0]["OriginalDiscount"]);

                            if (discount > 0)
                            {
                                e.Value = $"{originalDishName} (-{discount}%)";
                            }
                            else
                            {
                                e.Value = originalDishName;
                            }
                            e.FormattingApplied = true;
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count - 1) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            if (row.Cells["ColumnDish"].ReadOnly)
                return;

            try
            {
                if (e.ColumnIndex == dataGridView1.Columns["ColumnDish"].Index)
                {
                    if (row.Cells["ColumnDish"].Value != null && row.Cells["ColumnDish"].Value != DBNull.Value)
                    {
                        int dishId = Convert.ToInt32(row.Cells["ColumnDish"].Value);

                        DataRow[] dishRows = dishesTable.Select($"DishId = {dishId}");
                        if (dishRows.Length > 0)
                        {
                            decimal currentPrice = Convert.ToDecimal(dishRows[0]["DishPrice"]);
                            int currentDiscount = PriceCalculator.Instance.GetDiscountForDish(dishId, allDishesTable, offersTable);

                            row.Cells["ColumnPrice"].Value = Math.Round(currentPrice, 2);

                            int quantity = 1;
                            if (row.Cells["ColumnQuantity"].Value != null && row.Cells["ColumnQuantity"].Value != DBNull.Value)
                            {
                                quantity = Convert.ToInt32(row.Cells["ColumnQuantity"].Value);
                            }
                            else
                            {
                                row.Cells["ColumnQuantity"].Value = 1;
                            }

                            decimal totalSum = currentPrice * quantity * (100 - currentDiscount) / 100;
                            row.Cells["ColumnSum"].Value = Math.Round(totalSum, 2);
                        }
                    }
                }

                if (e.ColumnIndex == dataGridView1.Columns["ColumnQuantity"].Index)
                {
                    if (row.Cells["ColumnQuantity"].Value != null && row.Cells["ColumnQuantity"].Value != DBNull.Value &&
                        row.Cells["ColumnPrice"].Value != null && row.Cells["ColumnPrice"].Value != DBNull.Value &&
                        row.Cells["ColumnDish"].Value != null && row.Cells["ColumnDish"].Value != DBNull.Value)
                    {
                        int dishId = Convert.ToInt32(row.Cells["ColumnDish"].Value);
                        int quantity = Convert.ToInt32(row.Cells["ColumnQuantity"].Value);
                        decimal price = Convert.ToDecimal(row.Cells["ColumnPrice"].Value);

                        int discount = PriceCalculator.Instance.GetDiscountForDish(dishId, allDishesTable, offersTable);

                        decimal totalSum = price * quantity * (100 - discount) / 100;
                        row.Cells["ColumnSum"].Value = Math.Round(totalSum, 2);
                    }
                }

                UpdateTotalCount();

                decimal total = 0;
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    if (!r.IsNewRow && r.Cells["ColumnSum"].Value != null && r.Cells["ColumnSum"].Value != DBNull.Value)
                    {
                        total += Convert.ToDecimal(r.Cells["ColumnSum"].Value);
                    }
                }
                label1.Text = total.ToString("F2");
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateTotalCount();
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateTotalCount();
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["ColumnQuantity"].Value = 1;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count - 1)
            {
                if (e.ColumnIndex == dataGridView1.Columns["ColumnQuantity"].Index)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    if (row.Cells["ColumnQuantity"].ReadOnly)
                        return;

                    string value = e.FormattedValue.ToString();

                    if (string.IsNullOrEmpty(value))
                    {
                        MessageBox.Show("Введите количество!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }

                    if (!int.TryParse(value, out int quantity) || quantity <= 0)
                    {
                        MessageBox.Show("Введите корректное количество (целое число больше 0)!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == dataGridView1.Columns["ColumnDish"].Index)
            {
                ComboBox combo = e.Control as ComboBox;

                if (combo != null)
                {
                    combo.DropDownStyle = ComboBoxStyle.DropDown;
                    combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    combo.AutoCompleteSource = AutoCompleteSource.ListItems;

                    combo.KeyPress -= ComboBox_KeyPress;
                    combo.KeyPress += ComboBox_KeyPress;

                    combo.SelectionChangeCommitted -= ComboBox_SelectionChangeCommitted;
                    combo.SelectionChangeCommitted += ComboBox_SelectionChangeCommitted;

                    combo.Leave -= ComboBox_Leave;
                    combo.Leave += ComboBox_Leave;
                }
            }
        }

        private void TextBoxQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!char.IsControl(e.KeyChar))
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                    return;
                }

                if (textBox != null && textBox.Text.Length >= 3)
                {
                    e.Handled = true;
                }
            }
        }

        private void ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void ComboBox_Leave(object sender, EventArgs e)
        {
            ComboBox combo = sender as ComboBox;

            if (combo == null)
                return;

            string text = combo.Text.Trim();

            foreach (DataRow row in dishesTable.Rows)
            {
                string dishName = row["DishName"].ToString();

                if (dishName.Equals(text, StringComparison.OrdinalIgnoreCase))
                {
                    int dishId = Convert.ToInt32(row["DishId"]);

                    DataGridViewRow gridRow = dataGridView1.CurrentRow;

                    if (gridRow == null)
                        return;

                    gridRow.Cells["ColumnDish"].Value = dishId;

                    decimal currentPrice = Convert.ToDecimal(row["DishPrice"]);
                    int discount = PriceCalculator.Instance.GetDiscountForDish(
                        dishId,
                        allDishesTable,
                        offersTable);

                    gridRow.Cells["ColumnPrice"].Value = Math.Round(currentPrice, 2);

                    int quantity = 1;

                    if (gridRow.Cells["ColumnQuantity"].Value != null &&
                        gridRow.Cells["ColumnQuantity"].Value != DBNull.Value)
                    {
                        quantity = Convert.ToInt32(gridRow.Cells["ColumnQuantity"].Value);
                    }
                    else
                    {
                        gridRow.Cells["ColumnQuantity"].Value = 1;
                    }

                    decimal totalSum = currentPrice * quantity * (100 - discount) / 100;

                    gridRow.Cells["ColumnSum"].Value = Math.Round(totalSum, 2);

                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    dataGridView1.EndEdit();

                    decimal total = 0;

                    foreach (DataGridViewRow r in dataGridView1.Rows)
                    {
                        if (!r.IsNewRow &&
                            r.Cells["ColumnSum"].Value != null &&
                            r.Cells["ColumnSum"].Value != DBNull.Value)
                        {
                            total += Convert.ToDecimal(r.Cells["ColumnSum"].Value);
                        }
                    }

                    label1.Text = total.ToString("F2");

                    return;
                }
            }
        }

        private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ComboBox combo = sender as ComboBox;

            if (!char.IsControl(e.KeyChar))
            {
                bool isValid = (e.KeyChar >= 'а' && e.KeyChar <= 'я') ||
                               (e.KeyChar >= 'А' && e.KeyChar <= 'Я') ||
                               e.KeyChar == 'ё' || e.KeyChar == 'Ё' ||
                               e.KeyChar == ' ';

                if (!isValid)
                {
                    e.Handled = true;
                    return;
                }

                if (combo != null && combo.Text.Length >= 50)
                {
                    e.Handled = true;
                }
            }
        }
    }
}