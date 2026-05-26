using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Restaurant
{
    public static class BackupManager
    {
        public static string CreateBackup()
        {
            string backupFolder =
                Path.Combine(Application.StartupPath, "Backup");

            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);

            string fileName =
                $"db57_backup_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.sql";

            string fullPath =
                Path.Combine(backupFolder, fileName);

            using (StreamWriter sw =
                new StreamWriter(fullPath, false, new UTF8Encoding(false)))
            {
                sw.WriteLine("CREATE DATABASE IF NOT EXISTS `db57`;");
                sw.WriteLine("USE `db57`;");
                sw.WriteLine();

                using (MySqlConnection con =
                    new MySqlConnection(
                        connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    List<string> tables =
                        new List<string>();

                    MySqlCommand tablesCmd =
                        new MySqlCommand(
                            "SHOW TABLES",
                            con);

                    using (MySqlDataReader reader =
                        tablesCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }

                    foreach (string table in tables)
                    {
                        MySqlCommand structureCmd =
                            new MySqlCommand(
                                $"SHOW CREATE TABLE `{table}`",
                                con);

                        using (MySqlDataReader reader =
                            structureCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string createTable =
                                    reader.GetString(1);

                                sw.WriteLine(
                                    $"DROP TABLE IF EXISTS `{table}`;");

                                sw.WriteLine(createTable + ";");
                                sw.WriteLine();
                            }
                        }
                    }

                    foreach (string table in tables)
                    {
                        MySqlCommand dataCmd =
                            new MySqlCommand(
                                $"SELECT * FROM `{table}`",
                                con);

                        using (MySqlDataReader reader =
                            dataCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> values =
                                    new List<string>();

                                for (int i = 0;
                                    i < reader.FieldCount;
                                    i++)
                                {
                                    if (reader[i] == DBNull.Value)
                                    {
                                        values.Add("NULL");
                                    }
                                    else
                                    {
                                        object val =
                                            reader[i];

                                        if (val is bool)
                                        {
                                            values.Add(
                                                (bool)val ? "1" : "0");
                                        }
                                        else if (
                                            val is decimal ||
                                            val is double ||
                                            val is float)
                                        {
                                            values.Add(
                                                Convert.ToString(
                                                    val,
                                                    System.Globalization.CultureInfo.InvariantCulture));
                                        }
                                        else if (val is DateTime)
                                        {
                                            values.Add(
                                                $"'{((DateTime)val):yyyy-MM-dd HH:mm:ss}'");
                                        }
                                        else
                                        {
                                            string value =
                                                val.ToString()
                                                .Replace("\\", "\\\\")
                                                .Replace("'", "\\'");

                                            values.Add($"'{value}'");
                                        }
                                    }
                                }

                                sw.WriteLine(
                                    $"INSERT INTO `{table}` VALUES ({string.Join(",", values)});");
                            }
                        }

                        sw.WriteLine();
                    }
                }
            }
            CreateStructureBackup();

            return fullPath;
        }

        public static void CreateStructureBackup()
        {
            string structureFolder =
                Path.Combine(Application.StartupPath, "Structure");

            if (!Directory.Exists(structureFolder))
                Directory.CreateDirectory(structureFolder);

            string fileName =
                $"db57_structure_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.sql";

            string fullPath =
                Path.Combine(structureFolder, fileName);

            using (StreamWriter sw =
                new StreamWriter(fullPath, false, new UTF8Encoding(false)))
            {
                using (MySqlConnection con =
                    new MySqlConnection(
                        connStr.GetConnectionString("db57")))
                {
                    con.Open();

                    List<string> tables =
                        new List<string>();

                    MySqlCommand tablesCmd =
                        new MySqlCommand(
                            "SHOW TABLES",
                            con);

                    using (MySqlDataReader reader =
                        tablesCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }

                    sw.WriteLine("SET FOREIGN_KEY_CHECKS=0;");
                    sw.WriteLine("CREATE DATABASE IF NOT EXISTS `db57`;");
                    sw.WriteLine("USE `db57`;");
                    sw.WriteLine();

                    foreach (string table in tables)
                    {
                        MySqlCommand structureCmd =
                            new MySqlCommand(
                                $"SHOW CREATE TABLE `{table}`",
                                con);

                        using (MySqlDataReader reader =
                            structureCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string createTable =
                                    reader.GetString(1);

                                sw.WriteLine(
                                    $"DROP TABLE IF EXISTS `{table}`;");

                                sw.WriteLine(createTable + ";");

                                sw.WriteLine();
                            }
                        }
                    }

                    sw.WriteLine("SET FOREIGN_KEY_CHECKS=1;");
                }
            }
        }
    }
}