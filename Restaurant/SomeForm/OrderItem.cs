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
                        WHERE i.OrderId = @OrderId;
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
                    SetReadOnlyForOriginalItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetReadOnlyForOriginalItems()
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
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    MySqlCommand deleteCmd = new MySqlCommand(
                        "DELETE FROM OrderItems WHERE OrderId = @OrderId", con);
                    deleteCmd.Parameters.AddWithValue("@OrderId", orderId);
                    deleteCmd.ExecuteNonQuery();

                    decimal totalSum = 0;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        if (row.Cells["ColumnDish"].Value != null &&
                            row.Cells["ColumnDish"].Value != DBNull.Value &&
                            row.Cells["ColumnQuantity"].Value != null &&
                            row.Cells["ColumnQuantity"].Value != DBNull.Value)
                        {
                            int dishId = Convert.ToInt32(row.Cells["ColumnDish"].Value);
                            int quantity = Convert.ToInt32(row.Cells["ColumnQuantity"].Value);
                            decimal displayPrice = Convert.ToDecimal(row.Cells["ColumnPrice"].Value);

                            decimal originalPrice;
                            int originalDiscount;
                            string originalDishName;

                            DataRow[] originalRows = orderItemsData.Select($"DishId = {dishId}");
                            if (originalRows.Length > 0)
                            {
                                originalPrice = Convert.ToDecimal(originalRows[0]["OriginalPrice"]);
                                originalDiscount = Convert.ToInt32(originalRows[0]["OriginalDiscount"]);
                                originalDishName = originalRows[0]["OriginalDishName"] != DBNull.Value ?
                                    originalRows[0]["OriginalDishName"].ToString() : originalRows[0]["DishName"].ToString();

                                if (originalPrice == 0)
                                {
                                    originalPrice = displayPrice;
                                }
                            }
                            else
                            {
                                DataRow[] dishRows = dishesTable.Select($"DishId = {dishId}");
                                originalPrice = displayPrice;
                                originalDiscount = PriceCalculator.Instance.GetDiscountForDish(dishId, allDishesTable, offersTable);
                                originalDishName = dishRows[0]["DishName"].ToString();
                            }

                            decimal sum = displayPrice * quantity * (100 - originalDiscount) / 100;
                            totalSum += sum;

                            string insertQuery = @"INSERT INTO OrderItems 
                                (OrderId, DishId, DishCount, OriginalPrice, OriginalDiscount, OriginalDishName) 
                                VALUES (@OrderId, @DishId, @DishCount, @OriginalPrice, @OriginalDiscount, @OriginalDishName)";

                            MySqlCommand cmd = new MySqlCommand(insertQuery, con);
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            cmd.Parameters.AddWithValue("@DishId", dishId);
                            cmd.Parameters.AddWithValue("@DishCount", quantity);
                            cmd.Parameters.AddWithValue("@OriginalPrice", originalPrice);
                            cmd.Parameters.AddWithValue("@OriginalDiscount", originalDiscount);
                            cmd.Parameters.AddWithValue("@OriginalDishName", originalDishName);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MySqlCommand updateOrderCmd = new MySqlCommand(
                        "UPDATE `Order` SET OrderPrice = @OrderPrice WHERE OrderId = @OrderId", con);
                    updateOrderCmd.Parameters.AddWithValue("@OrderPrice", Math.Round(totalSum, 2));
                    updateOrderCmd.Parameters.AddWithValue("@OrderId", orderId);
                    updateOrderCmd.ExecuteNonQuery();

                    MessageBox.Show("Изменения успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
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

            try
            {
                if (e.ColumnIndex == dataGridView1.Columns["ColumnDish"].Index)
                {
                    if (row.Cells["ColumnDish"].Value != null && row.Cells["ColumnDish"].Value != DBNull.Value)
                    {
                        int dishId = Convert.ToInt32(row.Cells["ColumnDish"].Value);

                        if (!originalItemIds.Contains(dishId))
                        {
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

                        int discount;
                        if (originalItemIds.Contains(dishId))
                        {
                            DataRow[] originalRows = orderItemsData.Select($"DishId = {dishId}");
                            if (originalRows.Length > 0)
                            {
                                discount = Convert.ToInt32(originalRows[0]["OriginalDiscount"]);
                            }
                            else
                            {
                                discount = 0;
                            }
                        }
                        else
                        {
                            discount = PriceCalculator.Instance.GetDiscountForDish(dishId, allDishesTable, offersTable);
                        }

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
                    if (string.IsNullOrEmpty(e.FormattedValue.ToString()) ||
                        !int.TryParse(e.FormattedValue.ToString(), out int quantity) || quantity <= 0)
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
                }
            }
        }

        private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar))
            {
                bool isValid = (e.KeyChar >= 'а' && e.KeyChar <= 'я') ||
                               (e.KeyChar >= 'А' && e.KeyChar <= 'Я') ||
                               e.KeyChar == 'ё' || e.KeyChar == 'Ё' ||
                               e.KeyChar == ' ';

                if (!isValid)
                {
                    e.Handled = true;
                }
            }
        }
    }
}