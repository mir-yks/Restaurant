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

        public decimal CalculatePriceWithDiscount(decimal originalPrice, int discount)
        {
            if (discount > 0)
            {
                return Math.Round(originalPrice * (100 - discount) / 100, 2);
            }
            return Math.Round(originalPrice, 2);
        }

        public decimal CalculateTotalSumForDish(decimal originalPrice, int quantity, int discount = 0)
        {
            decimal finalPrice = CalculatePriceWithDiscount(originalPrice, discount);
            return Math.Round(quantity * finalPrice, 2);
        }

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

        public string GetDishDisplayName(string dishName, object offersDish, DataTable offersTable)
        {
            if (offersDish != null && offersDish != DBNull.Value && Convert.ToInt32(offersDish) > 0)
            {
                int offerId = Convert.ToInt32(offersDish);
                DataRow[] offerRows = offersTable.Select($"OffersDishId = {offerId}");
                if (offerRows.Length > 0)
                {
                    int discount = Convert.ToInt32(offerRows[0]["OffersDishDicsount"]);
                    return $"★ {dishName} (-{discount}%)";
                }
                else
                {
                    return $"★ {dishName}";
                }
            }
            return dishName;
        }

        public string GetToolTipText(int discount, decimal finalPrice, int quantity, decimal totalSum)
        {
            if (discount > 0)
            {
                return $"Цена со скидкой {discount}%: {finalPrice:F2} × {quantity} = {totalSum:F2}";
            }
            return null;
        }

        public decimal CalculateCurrentTotalSum(DataGridView dataGridView)
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["ColumnSum"].Value != null && row.Cells["ColumnSum"].Value != DBNull.Value)
                {
                    total += Convert.ToDecimal(row.Cells["ColumnSum"].Value);
                }
            }
            return Math.Round(total, 2);
        }

        public decimal CalculateOrderTotalSumFromDatabase(int orderId, MySqlConnection connection)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(@"
                    SELECT 
                        SUM(
                            CASE 
                                WHEN m.OffersDish IS NOT NULL AND m.OffersDish > 0 THEN
                                    ROUND(i.DishCount * m.DishPrice * (100 - od.OffersDishDicsount) / 100, 2)
                                ELSE
                                    ROUND(i.DishCount * m.DishPrice, 2)
                            END
                        ) AS TotalSum
                    FROM OrderItems i
                    JOIN MenuDish m ON i.DishId = m.DishId
                    LEFT JOIN OffersDish od ON m.OffersDish = od.OffersDishId
                    WHERE i.OrderId = @OrderId;", connection);

                cmd.Parameters.AddWithValue("@OrderId", orderId);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToDecimal(result);
                }
            }
            catch (Exception)
            {
                
            }
            return 0;
        }
    }
}