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
        private DataTable allDishesTable;
        private DataTable offersTable;
        private List<int> originalDishIds = new List<int>();

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

            ConfigureComboBoxColumn();
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
                    row["DisplayName"] = PriceCalculator.Instance.GetDishDisplayName(dishName, offersDish, offersTable);
                }

                dishColumn.DataSource = displayTable;
                dishColumn.DisplayMember = "DisplayName";
                dishColumn.ValueMember = "DishId";
            }
        }

        private void LoadDishesData()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand cmdAll = new MySqlCommand(@"
                        SELECT DishId, DishName, DishPrice, OffersDish, IsActive
                        FROM MenuDish", con);
                    allDishesTable = new DataTable();
                    MySqlDataAdapter daAll = new MySqlDataAdapter(cmdAll);
                    daAll.Fill(allDishesTable);

                    MySqlCommand cmdActive = new MySqlCommand(@"
                        SELECT DishId, DishName, DishPrice, OffersDish
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
    i.DishCount AS 'Количество'
FROM OrderItems i
WHERE i.OrderId = @OrderId;", con);

                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    orderItemsTable = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(orderItemsTable);

                    foreach (DataRow row in orderItemsTable.Rows)
                    {
                        if (row["DishId"] != DBNull.Value)
                        {
                            originalDishIds.Add(Convert.ToInt32(row["DishId"]));
                        }
                    }

                    dataGridView1.DataSource = orderItemsTable;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        if (row.Cells["ColumnDish"].Value != null && row.Cells["ColumnDish"].Value != DBNull.Value)
                        {
                            int dishId = Convert.ToInt32(row.Cells["ColumnDish"].Value);
                            DataRow[] dishRows = allDishesTable.Select($"DishId = {dishId}");
                            if (dishRows.Length > 0)
                            {
                                decimal originalPrice = Convert.ToDecimal(dishRows[0]["DishPrice"]);
                                int quantity = Convert.ToInt32(row.Cells["ColumnQuantity"].Value);
                                int discount = PriceCalculator.Instance.GetDiscountForDish(dishId, allDishesTable, offersTable);
                                decimal totalSum = PriceCalculator.Instance.CalculateTotalSumForDish(originalPrice, quantity, discount);

                                row.Cells["ColumnPrice"].Value = originalPrice;
                                row.Cells["ColumnSum"].Value = totalSum;
                            }
                        }
                    }

                    UpdateTotalCount();
                    SetReadOnlyForOriginalDishes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count) return;

            if (!dataGridView1.Columns.Contains("ColumnDish")) return;

            if (e.ColumnIndex != dataGridView1.Columns["ColumnDish"].Index) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            if (row == null) return;

            if (row.Cells["ColumnDish"] == null) return;

            object dishIdValue = row.Cells["ColumnDish"].Value;

            if (dishIdValue != null && dishIdValue != DBNull.Value)
            {
                try
                {
                    int dishId = Convert.ToInt32(dishIdValue);

                    if (allDishesTable == null) return;

                    DataRow[] rows = allDishesTable.Select($"DishId = {dishId}");

                    if (rows.Length > 0)
                    {
                        string dishName = rows[0]["DishName"].ToString();
                        object offersDish = rows[0]["OffersDish"];
                        string displayName = PriceCalculator.Instance.GetDishDisplayName(dishName, offersDish, offersTable);
                        e.Value = displayName;
                        e.FormattingApplied = true;
                    }
                    else
                    {
                        e.Value = $"Блюдо удалено (ID: {dishId})";
                        e.FormattingApplied = true;
                    }
                }
                catch (Exception)
                {
                    e.Value = $"Ошибка (ID: {dishIdValue})";
                    e.FormattingApplied = true;
                }
            }
        }

        private void SetReadOnlyForOriginalDishes()
        {
            if (!dataGridView1.Columns.Contains("ColumnDish")) return;

            int dishColumnIndex = dataGridView1.Columns["ColumnDish"].Index;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow || row.Cells["ColumnDish"] == null) continue;

                object dishIdValue = row.Cells["ColumnDish"].Value;
                if (dishIdValue != null && dishIdValue != DBNull.Value)
                {
                    try
                    {
                        int dishId = Convert.ToInt32(dishIdValue);
                        if (originalDishIds.Contains(dishId))
                        {
                            row.Cells["ColumnDish"].ReadOnly = true;
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == dataGridView1.Columns["ColumnDish"].Index)
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
            if (e.ColumnIndex == dataGridView1.Columns["ColumnDish"].Index)
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
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    decimal totalSum = PriceCalculator.Instance.CalculateOrderTotalSumFromDatabase(orderId, con);
                    textBoxSum.Text = totalSum.ToString("F2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке суммы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSum.Text = "0.00";
            }
        }

        private void UpdateOrderTotalSumInDatabase()
        {
            try
            {
                decimal totalSum = PriceCalculator.Instance.CalculateCurrentTotalSum(dataGridView1);

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

                        if (row.Cells["ColumnDish"].Value != null &&
                            row.Cells["ColumnDish"].Value != DBNull.Value &&
                            row.Cells["ColumnQuantity"].Value != null &&
                            row.Cells["ColumnQuantity"].Value != DBNull.Value)
                        {
                            int dishId = Convert.ToInt32(row.Cells["ColumnDish"].Value);
                            int quantity = Convert.ToInt32(row.Cells["ColumnQuantity"].Value);

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
                    if (e.ColumnIndex == dataGridView1.Columns["ColumnDish"].Index)
                    {
                        if (row.Cells["ColumnDish"].Value != null && row.Cells["ColumnDish"].Value != DBNull.Value)
                        {
                            int dishId = Convert.ToInt32(row.Cells["ColumnDish"].Value);
                            DataRow[] dishRows = dishesTable.Select($"DishId = {dishId}");
                            if (dishRows.Length > 0)
                            {
                                decimal originalPrice = Convert.ToDecimal(dishRows[0]["DishPrice"]);

                                int discount = PriceCalculator.Instance.GetDiscountForDish(dishId, allDishesTable, offersTable);
                                decimal finalPrice = PriceCalculator.Instance.CalculatePriceWithDiscount(originalPrice, discount);

                                row.Cells["ColumnPrice"].Value = Math.Round(originalPrice, 2);

                                if (row.Cells["ColumnQuantity"].Value != null && row.Cells["ColumnQuantity"].Value != DBNull.Value)
                                {
                                    int quantity = Convert.ToInt32(row.Cells["ColumnQuantity"].Value);
                                    decimal totalSum = PriceCalculator.Instance.CalculateTotalSumForDish(originalPrice, quantity, discount);
                                    row.Cells["ColumnSum"].Value = totalSum;

                                    string toolTipText = PriceCalculator.Instance.GetToolTipText(discount, finalPrice, quantity, totalSum);
                                    row.Cells["ColumnSum"].ToolTipText = toolTipText;
                                }
                                else
                                {
                                    row.Cells["ColumnQuantity"].Value = 1;
                                    decimal totalSum = PriceCalculator.Instance.CalculateTotalSumForDish(originalPrice, 1, discount);
                                    row.Cells["ColumnSum"].Value = totalSum;

                                    string toolTipText = PriceCalculator.Instance.GetToolTipText(discount, finalPrice, 1, totalSum);
                                    row.Cells["ColumnSum"].ToolTipText = toolTipText;
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
                            decimal originalPrice = Convert.ToDecimal(row.Cells["ColumnPrice"].Value);

                            int discount = PriceCalculator.Instance.GetDiscountForDish(dishId, allDishesTable, offersTable);
                            decimal totalSum = PriceCalculator.Instance.CalculateTotalSumForDish(originalPrice, quantity, discount);

                            row.Cells["ColumnSum"].Value = totalSum;

                            decimal finalPrice = PriceCalculator.Instance.CalculatePriceWithDiscount(originalPrice, discount);
                            string toolTipText = PriceCalculator.Instance.GetToolTipText(discount, finalPrice, quantity, totalSum);
                            row.Cells["ColumnSum"].ToolTipText = toolTipText;
                        }
                    }

                    UpdateTotalCount();
                    textBoxSum.Text = PriceCalculator.Instance.CalculateCurrentTotalSum(dataGridView1).ToString("F2");
                }
                catch (Exception)
                {

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
                        MessageBox.Show("Введите корректное количество (целое число больше 0)!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}