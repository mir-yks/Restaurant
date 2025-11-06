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
                        "DELETE FROM booking WHERE BookingDate < DATE_SUB(NOW(), INTERVAL 30 MINUTE)", con);

                    int deletedCount = cmd.ExecuteNonQuery();

                    return deletedCount;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}