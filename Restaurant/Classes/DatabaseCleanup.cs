using MySql.Data.MySqlClient;
using System;

namespace Restaurant
{
    public static class DatabaseCleanup
    {
        public static int CleanExpiredBookings()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM booking WHERE BookingDate < NOW()",
                        con);

                    int deletedCount = cmd.ExecuteNonQuery();

                    if (deletedCount > 0)
                    {
                        Console.WriteLine($"Автоматически удалено {deletedCount} устаревших бронирований");
                    }

                    return deletedCount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении устаревших бронирований: {ex.Message}");
                return -1;
            }
        }
    }
}