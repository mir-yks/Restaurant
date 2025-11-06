using MySql.Data.MySqlClient;
using System;

namespace Restaurant
{
    public static class TableStatusUpdater
    {
        private static void CheckDebugInfo(MySqlConnection con)
        {
            MySqlCommand debugCmd = new MySqlCommand(@"
                SELECT b.TableId, b.BookingDate, TIMESTAMPDIFF(MINUTE, NOW(), b.BookingDate) as MinutesUntil 
                FROM Booking b 
                WHERE b.BookingDate BETWEEN NOW() AND DATE_ADD(NOW(), INTERVAL 1 HOUR)
                ORDER BY b.BookingDate",
                con);

            using (var reader = debugCmd.ExecuteReader())
            {
                Console.WriteLine("Бронирования в ближайший час:");
                while (reader.Read())
                {
                    int tableId = reader.GetInt32("TableId");
                    DateTime bookingDate = reader.GetDateTime("BookingDate");
                    int minutesUntil = reader.GetInt32("MinutesUntil");
                    Console.WriteLine($"Стол {tableId}: бронь через {minutesUntil} мин ({bookingDate})");
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
                                    AND b.BookingDate BETWEEN NOW() AND DATE_ADD(NOW(), INTERVAL 1 HOUR)
                                ) THEN 'Забронирован'
                                ELSE 'Свободен'
                            END",
                        con);

                    int updatedCount = updateCmd.ExecuteNonQuery();
                    Console.WriteLine($"Обновлены статусы для {updatedCount} столов");

                    CheckDebugInfo(con);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении статусов столов: {ex.Message}");
            }
        }
    }
}