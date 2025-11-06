using MySql.Data.MySqlClient;
using System;

namespace Restaurant
{
    public static class TableStatusUpdater
    {
        private static void CheckDebugInfo(MySqlConnection con)
        {
            MySqlCommand debugCmd = new MySqlCommand(@"
                SELECT b.TableId, b.BookingDate, 
                       TIMESTAMPDIFF(MINUTE, NOW(), b.BookingDate) as MinutesUntil,
                       TIMESTAMPDIFF(MINUTE, b.BookingDate, NOW()) as MinutesPassed
                FROM Booking b 
                WHERE b.BookingDate BETWEEN DATE_SUB(NOW(), INTERVAL 30 MINUTE) AND DATE_ADD(NOW(), INTERVAL 1 HOUR)
                ORDER BY b.BookingDate",
                con);

            using (var reader = debugCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                   
                }
            }
        }

        public static void UpdateTablesStatus()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand updateCmd = new MySqlCommand(@"
                        UPDATE Tables t
                        SET TablesStatus = 
                            CASE 
                                WHEN EXISTS (
                                    SELECT 1 FROM `Order` o 
                                    WHERE o.TableId = t.TablesId 
                                    AND (o.OrderStatus != 'Завершен' 
                                         OR (o.OrderStatus = 'Завершен' AND o.OrderStatusPayment = 'Не оплачен'))
                                ) THEN 'Занят'
                                WHEN EXISTS (
                                    SELECT 1 FROM Booking b 
                                    WHERE b.TableId = t.TablesId 
                                    AND b.BookingDate BETWEEN DATE_SUB(NOW(), INTERVAL 30 MINUTE) AND DATE_ADD(NOW(), INTERVAL 1 HOUR)
                                ) THEN 'Забронирован'
                                ELSE 'Свободен'
                            END",
                        con);

                    int updatedCount = updateCmd.ExecuteNonQuery();

                    CheckDebugInfo(con);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}