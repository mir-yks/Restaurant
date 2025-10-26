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
    public partial class OrderItem : Form
    {
        private int roleId;
        private int orderId;
        private DataTable orderItemsTable;
        private DataTable dishesTable;
        private DataTable offersTable;
        private bool isFirstLoad = true;

        public OrderItem(int role, int orderId)
        {
            InitializeComponent();
            roleId = role;
            this.orderId = orderId;
            ConfigureButtons();

            textBoxSum.Font = Fonts.MontserratAlternatesRegular(12f);
            labelSum.Font = Fonts.MontserratAlternatesBold(12f);
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
                dataGridView1.AllowUserToDeleteRows = false;
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void OrderItem_Load(object sender, EventArgs e)
        {
            LoadDishesData();
            LoadOffersData();
            LoadOrderItems();
            LoadOrderTotalSum();
            UpdateTotalCount();
        }

        private void LoadDishesData()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                        SELECT DishId, DishName, DishPrice, OffersDish
                        FROM MenuDish", con);

                    dishesTable = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dishesTable);
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
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
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
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                SELECT 
                    i.DishId AS 'DishId',
                    m.DishName AS 'Блюдо',
                    i.DishCount AS 'Количество',
                    m.DishPrice AS 'Цена',
                    CASE 
                        WHEN m.OffersDish IS NOT NULL AND m.OffersDish > 0 THEN
                            (i.DishCount * m.DishPrice * (100 - od.OffersDishDicsount) / 100)
                        ELSE
                            (i.DishCount * m.DishPrice)
                    END AS 'Сумма',
                    m.OffersDish AS 'OffersDish'
                FROM OrderItems i
                JOIN MenuDish m ON i.DishId = m.DishId
                LEFT JOIN OffersDish od ON m.OffersDish = od.OffersDishId
                WHERE i.OrderId = @OrderId;", con);

                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    orderItemsTable = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(orderItemsTable);

                    dataGridView1.DataSource = orderItemsTable;

                    ConfigureDataGridView();

                    UpdateTotalCount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView()
        {
            if (dataGridView1.Columns.Contains("DishId"))
                dataGridView1.Columns["DishId"].Visible = false;

            if (dataGridView1.Columns.Contains("OffersDish"))
                dataGridView1.Columns["OffersDish"].Visible = false;

            if (dataGridView1.Columns.Contains("Блюдо"))
            {
                DataGridViewComboBoxColumn dishColumn = new DataGridViewComboBoxColumn();
                dishColumn.HeaderText = "Блюдо";
                dishColumn.Name = "Блюдо";

                DataTable displayTable = dishesTable.Copy();
                if (!displayTable.Columns.Contains("DisplayName"))
                {
                    displayTable.Columns.Add("DisplayName", typeof(string));
                }

                foreach (DataRow row in displayTable.Rows)
                {
                    string dishName = row["DishName"].ToString();
                    object offersDish = row["OffersDish"];

                    if (offersDish != null && offersDish != DBNull.Value && Convert.ToInt32(offersDish) > 0)
                    {
                        int offerId = Convert.ToInt32(offersDish);
                        DataRow[] offerRows = offersTable.Select($"OffersDishId = {offerId}");
                        if (offerRows.Length > 0)
                        {
                            int discount = Convert.ToInt32(offerRows[0]["OffersDishDicsount"]);
                            row["DisplayName"] = $"★ {dishName} (-{discount}%)";
                        }
                        else
                        {
                            row["DisplayName"] = $"★ {dishName}";
                        }
                    }
                    else
                    {
                        row["DisplayName"] = dishName;
                    }
                }

                dishColumn.DataSource = displayTable;
                dishColumn.DisplayMember = "DisplayName";
                dishColumn.ValueMember = "DishId";
                dishColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                dishColumn.DisplayStyleForCurrentCellOnly = true;
                dishColumn.FlatStyle = FlatStyle.Flat;

                int columnIndex = dataGridView1.Columns["Блюдо"].Index;
                dataGridView1.Columns.Remove("Блюдо");
                dataGridView1.Columns.Insert(columnIndex, dishColumn);

                dishColumn.DataPropertyName = "DishId";
            }

            if (dataGridView1.Columns.Contains("Цена"))
                dataGridView1.Columns["Цена"].ReadOnly = true;

            if (dataGridView1.Columns.Contains("Сумма"))
                dataGridView1.Columns["Сумма"].ReadOnly = true;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.Name != "Блюдо" && column.Name != "Количество")
                {
                    column.ReadOnly = true;
                }
            }

            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            dataGridView1.EditingControlShowing += DataGridView1_EditingControlShowing;
            dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;

            this.ActiveControl = buttonBack;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Блюдо"].Index)
            {
                if (e.Value != null && e.Value != DBNull.Value)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    object dishIdValue = row.Cells["Блюдо"].Value;

                    if (dishIdValue != null && dishIdValue != DBNull.Value)
                    {
                        try
                        {
                            int dishId = Convert.ToInt32(dishIdValue);
                            DataRow[] rows = dishesTable.Select($"DishId = {dishId}");
                            if (rows.Length > 0)
                            {
                                string dishName = rows[0]["DishName"].ToString();
                                object offersDish = rows[0]["OffersDish"];

                                if (offersDish != null && offersDish != DBNull.Value && Convert.ToInt32(offersDish) > 0)
                                {
                                    int offerId = Convert.ToInt32(offersDish);
                                    DataRow[] offerRows = offersTable.Select($"OffersDishId = {offerId}");
                                    if (offerRows.Length > 0)
                                    {
                                        int discount = Convert.ToInt32(offerRows[0]["OffersDishDicsount"]);
                                        e.Value = $"★ {dishName} (-{discount}%)";
                                    }
                                    else
                                    {
                                        e.Value = $"★ {dishName}";
                                    }
                                }
                                else
                                {
                                    e.Value = dishName;
                                }
                                e.FormattingApplied = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка форматирования: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == dataGridView1.Columns["Блюдо"].Index)
            {
                ComboBox combo = e.Control as ComboBox;
                if (combo != null)
                {
                    combo.DropDownStyle = ComboBoxStyle.DropDownList;

                    combo.DrawMode = DrawMode.OwnerDrawFixed;
                    combo.DrawItem += ComboBox_DrawItem;
                    combo.MeasureItem += ComboBox_MeasureItem;
                }
            }
        }

        private void ComboBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 20;
        }

        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox combo = sender as ComboBox;
            if (combo == null) return;

            e.DrawBackground();

            string itemText = combo.GetItemText(combo.Items[e.Index]);

            Brush textBrush = (e.State & DrawItemState.Selected) == DrawItemState.Selected ?
                SystemBrushes.HighlightText : SystemBrushes.ControlText;

            e.Graphics.DrawString(itemText, combo.Font, textBrush, e.Bounds.X, e.Bounds.Y);

            e.DrawFocusRectangle();
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Блюдо"].Index)
            {
                dataGridView1.InvalidateCell(e.ColumnIndex, e.RowIndex);
            }
        }

        private void UpdateTotalCount()
        {
            int totalCount = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow &&
                    row.Cells["Блюдо"].Value != null &&
                    row.Cells["Блюдо"].Value != DBNull.Value)
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
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                SELECT 
                    SUM(
                        CASE 
                            WHEN m.OffersDish IS NOT NULL AND m.OffersDish > 0 THEN
                                (i.DishCount * m.DishPrice * (100 - od.OffersDishDicsount) / 100)
                            ELSE
                                (i.DishCount * m.DishPrice)
                        END
                    ) AS TotalSum
                FROM OrderItems i
                JOIN MenuDish m ON i.DishId = m.DishId
                LEFT JOIN OffersDish od ON m.OffersDish = od.OffersDishId
                WHERE i.OrderId = @OrderId;", con);

                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        decimal totalSum = Convert.ToDecimal(result);
                        textBoxSum.Text = totalSum.ToString("F2");
                    }
                    else
                    {
                        textBoxSum.Text = "0.00";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке суммы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateOrderTotalSumInDatabase()
        {
            try
            {
                decimal totalSum = CalculateCurrentTotalSum();

                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                        UPDATE `Order` 
                        SET OrderPrice = @OrderPrice 
                        WHERE OrderId = @OrderId", con);

                    cmd.Parameters.AddWithValue("@OrderPrice", totalSum);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении суммы заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetDiscountForDish(int dishId)
        {
            DataRow[] dishRows = dishesTable.Select($"DishId = {dishId}");
            if (dishRows.Length > 0)
            {
                object offersDish = dishRows[0]["OffersDish"];
                if (offersDish != null && offersDish != DBNull.Value && Convert.ToInt32(offersDish) > 0)
                {
                    int offerId = Convert.ToInt32(offersDish);
                    DataRow[] offerRows = offersTable.Select($"OffersDishId = {offerId}");
                    if (offerRows.Length > 0)
                    {
                        return Convert.ToInt32(offerRows[0]["OffersDishDicsount"]);
                    }
                }
            }
            return 0;
        }

        private decimal CalculatePriceWithDiscount(decimal originalPrice, int discount)
        {
            if (discount > 0)
            {
                return originalPrice * (100 - discount) / 100;
            }
            return originalPrice;
        }

        private decimal CalculateCurrentTotalSum()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["Сумма"].Value != null && row.Cells["Сумма"].Value != DBNull.Value)
                {
                    total += Convert.ToDecimal(row.Cells["Сумма"].Value);
                }
            }
            return total;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите сохранить запись?", "Подтверждение записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (SaveAllChanges())
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private bool SaveAllChanges()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand deleteCmd = new MySqlCommand(
                        "DELETE FROM OrderItems WHERE OrderId = @OrderId", con);
                    deleteCmd.Parameters.AddWithValue("@OrderId", orderId);
                    deleteCmd.ExecuteNonQuery();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        if (row.Cells["Блюдо"].Value != null &&
                            row.Cells["Блюдо"].Value != DBNull.Value &&
                            row.Cells["Количество"].Value != null &&
                            row.Cells["Количество"].Value != DBNull.Value)
                        {
                            int dishId = Convert.ToInt32(row.Cells["Блюдо"].Value);
                            int quantity = Convert.ToInt32(row.Cells["Количество"].Value);

                            string insertQuery = @"INSERT INTO OrderItems 
                                        (OrderId, DishId, DishCount) 
                                        VALUES (@OrderId, @DishId, @DishCount)";

                            MySqlCommand cmd = new MySqlCommand(insertQuery, con);
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            cmd.Parameters.AddWithValue("@DishId", dishId);
                            cmd.Parameters.AddWithValue("@DishCount", quantity);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    UpdateOrderTotalSumInDatabase();

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

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count - 1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                try
                {
                    if (e.ColumnIndex == dataGridView1.Columns["Блюдо"].Index)
                    {
                        if (row.Cells["Блюдо"].Value != null && row.Cells["Блюдо"].Value != DBNull.Value)
                        {
                            int dishId = Convert.ToInt32(row.Cells["Блюдо"].Value);
                            DataRow[] dishRows = dishesTable.Select($"DishId = {dishId}");
                            if (dishRows.Length > 0)
                            {
                                decimal originalPrice = Convert.ToDecimal(dishRows[0]["DishPrice"]);

                                int discount = GetDiscountForDish(dishId);
                                decimal finalPrice = CalculatePriceWithDiscount(originalPrice, discount);

                                row.Cells["Цена"].Value = originalPrice;

                                if (row.Cells["Количество"].Value != null && row.Cells["Количество"].Value != DBNull.Value)
                                {
                                    int quantity = Convert.ToInt32(row.Cells["Количество"].Value);
                                    decimal totalSum = quantity * finalPrice;
                                    row.Cells["Сумма"].Value = totalSum;

                                    if (discount > 0)
                                    {
                                        row.Cells["Сумма"].ToolTipText = $"Цена со скидкой {discount}%: {finalPrice:F2} × {quantity} = {totalSum:F2}";
                                    }
                                    else
                                    {
                                        row.Cells["Сумма"].ToolTipText = null;
                                    }
                                }
                                else
                                {
                                    row.Cells["Количество"].Value = 1;
                                    decimal totalSum = finalPrice;
                                    row.Cells["Сумма"].Value = totalSum;

                                    if (discount > 0)
                                    {
                                        row.Cells["Сумма"].ToolTipText = $"Цена со скидкой {discount}%: {finalPrice:F2}";
                                    }
                                    else
                                    {
                                        row.Cells["Сумма"].ToolTipText = null;
                                    }
                                }
                            }
                        }
                    }

                    if (e.ColumnIndex == dataGridView1.Columns["Количество"].Index)
                    {
                        if (row.Cells["Количество"].Value != null && row.Cells["Количество"].Value != DBNull.Value &&
                            row.Cells["Цена"].Value != null && row.Cells["Цена"].Value != DBNull.Value &&
                            row.Cells["Блюдо"].Value != null && row.Cells["Блюдо"].Value != DBNull.Value)
                        {
                            int dishId = Convert.ToInt32(row.Cells["Блюдо"].Value);
                            int quantity = Convert.ToInt32(row.Cells["Количество"].Value);
                            decimal originalPrice = Convert.ToDecimal(row.Cells["Цена"].Value);

                            int discount = GetDiscountForDish(dishId);
                            decimal finalPrice = CalculatePriceWithDiscount(originalPrice, discount);
                            decimal totalSum = quantity * finalPrice;

                            row.Cells["Сумма"].Value = totalSum;

                            if (discount > 0)
                            {
                                row.Cells["Сумма"].ToolTipText = $"Цена со скидкой {discount}%: {finalPrice:F2} × {quantity} = {totalSum:F2}";
                            }
                            else
                            {
                                row.Cells["Сумма"].ToolTipText = null;
                            }
                        }
                    }

                    UpdateTotalCount();
                    textBoxSum.Text = CalculateCurrentTotalSum().ToString("F2");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в CellValueChanged: {ex.Message}");
                }
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
            e.Row.Cells["Количество"].Value = 1;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count - 1)
            {
                if (e.ColumnIndex == dataGridView1.Columns["Количество"].Index)
                {
                    if (string.IsNullOrEmpty(e.FormattedValue.ToString()) ||
                        !int.TryParse(e.FormattedValue.ToString(), out int quantity) || quantity <= 0)
                    {
                        MessageBox.Show("Введите корректное количество (целое число больше 0)!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            Console.WriteLine($"DataError: {e.Exception.Message}");
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.ActiveControl = buttonBack;
        }
    }
}