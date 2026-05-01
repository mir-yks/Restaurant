using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Restaurant
{
    public static class OrderStatusUpdater
    {
        public static void UpdateOrderStatus(int orderId, string orderStatus, string paymentStatus)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    MySqlCommand getTableCmd = new MySqlCommand(
                        "SELECT TableId FROM `Order` WHERE OrderId = @OrderId", con);
                    getTableCmd.Parameters.AddWithValue("@OrderId", orderId);
                    object tableIdObj = getTableCmd.ExecuteScalar();

                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE `Order` SET OrderStatus = @OrderStatus, OrderStatusPayment = @OrderStatusPayment WHERE OrderId = @OrderId",
                        con);
                    cmd.Parameters.AddWithValue("@OrderStatus", orderStatus);
                    cmd.Parameters.AddWithValue("@OrderStatusPayment", paymentStatus);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.ExecuteNonQuery();

                    if (orderStatus == "Завершен" && paymentStatus == "Оплачен" && tableIdObj != null && tableIdObj != DBNull.Value)
                    {
                        int tableId = Convert.ToInt32(tableIdObj);
                        MySqlCommand updateTableCmd = new MySqlCommand(
                            "UPDATE Tables SET TablesStatus = 'Свободен' WHERE TablesId = @TableId", con);
                        updateTableCmd.Parameters.AddWithValue("@TableId", tableId);
                        updateTableCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статуса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}