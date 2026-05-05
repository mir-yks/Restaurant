using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Restaurant
{
    public class PriceCalculator
    {
        private static PriceCalculator _instance;
        private static readonly object _lock = new object();

        public static PriceCalculator Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new PriceCalculator();
                    }
                    return _instance;
                }
            }
        }

        private PriceCalculator() { }


        public int GetDiscountForDish(int dishId, DataTable allDishesTable, DataTable offersTable)
        {
            DataRow[] dishRows = allDishesTable.Select($"DishId = {dishId}");

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

        public decimal CalculateOrderTotalSumFromDatabase(int orderId, MySqlConnection connection)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(@"
                    SELECT 
                        SUM(
                            i.DishCount * 
                            CASE 
                                WHEN i.OriginalPrice > 0 THEN i.OriginalPrice
                                ELSE m.DishPrice
                            END *
                            (100 - 
                                CASE 
                                    WHEN i.OriginalDiscount > 0 THEN i.OriginalDiscount
                                    WHEN m.OffersDish IS NOT NULL AND m.OffersDish > 0 THEN 
                                        (SELECT OffersDishDicsount FROM OffersDish WHERE OffersDishId = m.OffersDish)
                                    ELSE 0
                                END
                            ) / 100
                        ) AS TotalSum
                    FROM OrderItems i
                    JOIN MenuDish m ON i.DishId = m.DishId
                    WHERE i.OrderId = @OrderId;
                ", connection);

                cmd.Parameters.AddWithValue("@OrderId", orderId);
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return Math.Round(Convert.ToDecimal(result), 2);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error calculating order sum: {ex.Message}");
            }
            return 0;
        }
    }
}