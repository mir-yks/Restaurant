using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant
{
    public static class ImportManager
    {
        public static object ConvertValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DBNull.Value;

            value = value.Trim();

            if (
                value.ToUpper() == "NULL" ||
                value == "\"\"")
            {
                return DBNull.Value;
            }

            if (value.ToLower() == "true")
                return 1;

            if (value.ToLower() == "false")
                return 0;

            int intValue;

            if (int.TryParse(value, out intValue))
                return intValue;

            decimal decimalValue;

            if (decimal.TryParse(
                value.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimalValue))
            {
                return decimalValue;
            }

            DateTime dt;

            string[] formats =
            {
                "dd.MM.yyyy HH:mm:ss",
                "dd.MM.yyyy",
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd"
            };

            if (DateTime.TryParseExact(
                value,
                formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out dt))
            {
                return dt;
            }

            return value;
        }

        public static bool CheckForeignKeys(
            MySqlConnection con,
            string table,
            string[] columns,
            string[] lines,
            out string message)
        {
            message = "";

            try
            {
                List<(string column,
                    string refTable,
                    string refColumn)> fks =
                    new List<(string, string, string)>();

                MySqlCommand cmd =
                    new MySqlCommand(@"
                SELECT 
                    COLUMN_NAME,
                    REFERENCED_TABLE_NAME,
                    REFERENCED_COLUMN_NAME
                FROM information_schema.KEY_COLUMN_USAGE
                WHERE TABLE_SCHEMA = 'db57'
                AND TABLE_NAME = @table
                AND REFERENCED_TABLE_NAME IS NOT NULL",
                    con);

                cmd.Parameters.AddWithValue(
                    "@table",
                    table);

                using (MySqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fks.Add((
                            reader.GetString("COLUMN_NAME"),
                            reader.GetString("REFERENCED_TABLE_NAME"),
                            reader.GetString("REFERENCED_COLUMN_NAME")
                        ));
                    }
                }

                List<string> missingTables =
                    new List<string>();

                foreach (string line in lines.Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] values =
                        line.Split(';');

                    foreach (var fk in fks)
                    {
                        int index =
                            Array.IndexOf(
                                columns,
                                fk.column);

                        if (index < 0)
                            continue;

                        if (index >= values.Length)
                            continue;

                        object value =
                            ConvertValue(values[index]);

                        if (
                            value == DBNull.Value ||
                            value == null)
                        {
                            continue;
                        }

                        MySqlCommand check =
                            new MySqlCommand(
                                $"SELECT COUNT(*) " +
                                $"FROM `{fk.refTable}` " +
                                $"WHERE `{fk.refColumn}`=@id",
                                con);

                        check.Parameters.AddWithValue(
                            "@id",
                            value);

                        int exists =
                            Convert.ToInt32(
                                check.ExecuteScalar());

                        if (exists == 0)
                        {
                            missingTables.Add(
                                fk.refTable);
                        }
                    }
                }

                missingTables =
                    missingTables
                    .Distinct()
                    .ToList();

                if (missingTables.Count > 0)
                {
                    message =
                        "Сначала заполните связанные таблицы:\n\n" +
                        string.Join("\n", missingTables);

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }
}