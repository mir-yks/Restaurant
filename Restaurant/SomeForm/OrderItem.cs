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
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void OrderItem_Load(object sender, EventArgs e)
        {
            LoadDishesData();
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
                        SELECT DishId, DishName, DishPrice 
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
                        (i.DishCount * m.DishPrice) AS 'Сумма'
                    FROM OrderItems i
                    JOIN MenuDish m ON i.DishId = m.DishId
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
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;

            if (dataGridView1.Columns.Contains("DishId"))
                dataGridView1.Columns["DishId"].Visible = false;

            if (dataGridView1.Columns.Contains("Блюдо"))
            {
                DataGridViewComboBoxColumn dishColumn = new DataGridViewComboBoxColumn();
                dishColumn.HeaderText = "Блюдо";
                dishColumn.Name = "Блюдо";
                dishColumn.DataSource = dishesTable;
                dishColumn.DisplayMember = "DishName";
                dishColumn.ValueMember = "DishId";
                dishColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;

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
                    SELECT SUM(i.DishCount * m.DishPrice) AS TotalSum
                    FROM OrderItems i
                    JOIN MenuDish m ON i.DishId = m.DishId
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
                                decimal price = Convert.ToDecimal(dishRows[0]["DishPrice"]);
                                row.Cells["Цена"].Value = price;

                                if (row.Cells["Количество"].Value != null && row.Cells["Количество"].Value != DBNull.Value)
                                {
                                    int quantity = Convert.ToInt32(row.Cells["Количество"].Value);
                                    row.Cells["Сумма"].Value = quantity * price;
                                }
                                else
                                {
                                    row.Cells["Количество"].Value = 1;
                                    row.Cells["Сумма"].Value = price;
                                }
                            }
                        }
                    }

                    if (e.ColumnIndex == dataGridView1.Columns["Количество"].Index)
                    {
                        if (row.Cells["Количество"].Value != null && row.Cells["Количество"].Value != DBNull.Value &&
                            row.Cells["Цена"].Value != null && row.Cells["Цена"].Value != DBNull.Value)
                        {
                            int quantity = Convert.ToInt32(row.Cells["Количество"].Value);
                            decimal price = Convert.ToDecimal(row.Cells["Цена"].Value);
                            row.Cells["Сумма"].Value = quantity * price;
                        }
                    }

                    UpdateTotalCount();
                    textBoxSum.Text = CalculateCurrentTotalSum().ToString("F2");
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
        }
    }
}