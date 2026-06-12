    using MySql.Data.MySqlClient;
    using System;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;

    namespace Restaurant
    {
        public partial class ManagementBD : Form
        {
            private string exportPath = "";
            private string importPath = "";

            public ManagementBD()
            {
                InitializeComponent();

                buttonImportFile.Font = Fonts.MontserratAlternatesBold(12f);
                buttonExportFile.Font = Fonts.MontserratAlternatesBold(12f);
                buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
                buttonBackup.Font = Fonts.MontserratAlternatesBold(12f);
                buttonStructure.Font = Fonts.MontserratAlternatesBold(12f);
                buttonImport.Font = Fonts.MontserratAlternatesBold(12f);
                buttonExport.Font = Fonts.MontserratAlternatesBold(12f);
                buttonRestore.Font = Fonts.MontserratAlternatesBold(12f);

                labelExport.Font = Fonts.MontserratAlternatesRegular(14f);
                labelImport.Font = Fonts.MontserratAlternatesRegular(14f);

                comboBoxExport.Font = Fonts.MontserratAlternatesRegular(14f);
                comboBoxImport.Font = Fonts.MontserratAlternatesRegular(14f);

                LoadTables();
            }

            private void buttonBack_Click(object sender, EventArgs e)
            {
                Close();
            }

            private void LoadTables()
            {
                try
                {
                    comboBoxExport.Items.Clear();
                    comboBoxImport.Items.Clear();

                    using (MySqlConnection con =
                        new MySqlConnection(connStr.GetConnectionString("db57")))
                    {
                        con.Open();

                        MySqlCommand cmd =
                            new MySqlCommand("SHOW TABLES", con);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string table = reader.GetString(0);

                                comboBoxExport.Items.Add(table);
                                comboBoxImport.Items.Add(table);
                            }
                        }
                    }

                    if (comboBoxExport.Items.Count > 0)
                        comboBoxExport.SelectedIndex = 0;

                    if (comboBoxImport.Items.Count > 0)
                        comboBoxImport.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            private void buttonExportFile_Click(object sender, EventArgs e)
            {
                string exportFolder =
                    Path.Combine(Application.StartupPath, "Export");

                if (!Directory.Exists(exportFolder))
                    Directory.CreateDirectory(exportFolder);

                SaveFileDialog save = new SaveFileDialog();

                save.Filter = "CSV files (*.csv)|*.csv";
                save.Title = "Сохранить экспорт";
                save.InitialDirectory = exportFolder;

                if (save.ShowDialog() == DialogResult.OK)
                {
                    exportPath = save.FileName;
                }
            }

            private void buttonExport_Click(object sender, EventArgs e)
            {
                try
                {
                    if (comboBoxExport.SelectedItem == null)
                    {
                        MessageBox.Show(
                            "Выберите таблицу!",
                            "Внимание",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }

                    string table =
                        comboBoxExport.SelectedItem.ToString();

                    string exportFolder =
                        Path.Combine(
                            Application.StartupPath,
                            "Export");

                    if (!Directory.Exists(exportFolder))
                        Directory.CreateDirectory(exportFolder);

                    if (string.IsNullOrEmpty(exportPath))
                    {
                        exportPath =
                            Path.Combine(
                                exportFolder,
                                table + "_" +
                                DateTime.Now.ToString(
                                    "yyyy-MM-dd_HH-mm-ss") +
                                ".csv");
                    }

                    using (MySqlConnection con =
                        new MySqlConnection(
                            connStr.GetConnectionString("db57")))
                    {
                        con.Open();

                        DataTable dt =
                            new DataTable();

                        new MySqlDataAdapter(
                            "SELECT * FROM `" + table + "`",
                            con)
                            .Fill(dt);

                        using (StreamWriter sw =
                            new StreamWriter(
                                exportPath,
                                false,
                                new UTF8Encoding(false)))
                        {
                            for (int i = 0;
                                i < dt.Columns.Count;
                                i++)
                            {
                                sw.Write(
                                    dt.Columns[i].ColumnName);

                                if (i < dt.Columns.Count - 1)
                                    sw.Write(";");
                            }

                            sw.WriteLine();

                            foreach (DataRow row in dt.Rows)
                            {
                                for (int i = 0;
                                    i < dt.Columns.Count;
                                    i++)
                                {
                                    object val =
                                        row[i];

                                    string value = "";

                                    if (val == DBNull.Value)
                                        value = "";
                                    else if (val is bool)
                                        value =
                                            (bool)val ? "1" : "0";
                                    else if (val is DateTime)
                                        value =
                                            ((DateTime)val)
                                            .ToString(
                                                "yyyy-MM-dd HH:mm:ss");
                                    else if (
                                        val is decimal ||
                                        val is double ||
                                        val is float)
                                    {
                                        value =
                                            Convert.ToDecimal(val)
                                            .ToString(
                                                System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        value =
                                            val.ToString();
                                    }

                                    value =
                                        value.Replace(";", ",");

                                    sw.Write(value);

                                    if (i < dt.Columns.Count - 1)
                                        sw.Write(";");
                                }

                                sw.WriteLine();
                            }
                        }
                    }

                    MessageBox.Show(
                        $"Экспорт завершен!\n\nФайл сохранен:\n{exportPath}",
                        "Успех",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    exportPath = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            private void buttonImportFile_Click(object sender, EventArgs e)
            {
                string exportFolder = Path.Combine(Application.StartupPath, "Export");

                if (!Directory.Exists(exportFolder))
                    Directory.CreateDirectory(exportFolder);

                OpenFileDialog open = new OpenFileDialog();

                open.Filter = "CSV (*.csv)|*.csv";
                open.InitialDirectory = exportFolder;

                if (open.ShowDialog() == DialogResult.OK)
                {
                    importPath = open.FileName;
                }
            }

            private void buttonImport_Click(object sender, EventArgs e)
            {
                try
                {
                    if (comboBoxImport.SelectedItem == null)
                    {
                        MessageBox.Show(
                            "Выберите таблицу!",
                            "Внимание",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }

                    if (string.IsNullOrEmpty(importPath))
                    {
                        MessageBox.Show(
                            "Выберите CSV файл!",
                            "Внимание",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }

                    string table =
                        comboBoxImport.SelectedItem.ToString();

                    string[] lines =
                        File.ReadAllLines(importPath, Encoding.UTF8);

                    if (lines.Length < 2)
                    {
                        MessageBox.Show(
                            "CSV файл пуст!",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }

                    string[] columns =
                        lines[0].Split(';');



                    int added = 0;
                    int skipped = 0;

                    using (MySqlConnection con =
                        new MySqlConnection(
                            connStr.GetConnectionString("db57")))
                    {
                        con.Open();

                    List<string> dbColumns = new List<string>();

                    MySqlCommand cmdColumns = new MySqlCommand(
                        $"SHOW COLUMNS FROM `{table}`",
                        con);

                    using (MySqlDataReader reader = cmdColumns.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dbColumns.Add(reader.GetString("Field"));
                        }
                    }

                    bool structureMatches =
                        columns.Length == dbColumns.Count &&
                        columns.SequenceEqual(dbColumns);

                    if (!structureMatches)
                    {
                        MessageBox.Show(
                            $"Импорт невозможен.\n\n" +
                            $"Файл не подходит для таблицы \"{table}\".\n" +
                            $"Вероятно, он был экспортирован из другой таблицы базы данных.",
                            "Неверный файл импорта",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }

                    string fkMsg;

                        if (!ImportManager.CheckForeignKeys(
                            con,
                            table,
                            columns,
                            lines,
                            out fkMsg))
                        {
                            MessageBox.Show(
                                fkMsg,
                                "FK ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }

                        for (int i = 1; i < lines.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(lines[i]))
                                continue;

                            string[] values =
                                lines[i].Split(';');

                            string where = "";

                            for (int c = 0; c < columns.Length; c++)
                            {
                                if (c > 0)
                                    where += " AND ";

                                where += $"`{columns[c]}`=@p{c}";
                            }

                            MySqlCommand check =
                                new MySqlCommand(
                                    $"SELECT COUNT(*) " +
                                    $"FROM `{table}` " +
                                    $"WHERE {where}",
                                    con);

                            for (int j = 0; j < columns.Length; j++)
                            {
                                string v =
                                    j < values.Length
                                    ? values[j]
                                    : "";

                                check.Parameters.AddWithValue(
                                    "@p" + j,
                                    ImportManager.ConvertValue(v));
                            }

                            int exists =
                                Convert.ToInt32(check.ExecuteScalar());

                            if (exists > 0)
                            {
                                skipped++;
                                continue;
                            }

                            string cols = "";
                            string pars = "";

                            for (int c = 0; c < columns.Length; c++)
                            {
                                if (c > 0)
                                {
                                    cols += ",";
                                    pars += ",";
                                }

                                cols += $"`{columns[c]}`";
                                pars += $"@v{c}";
                            }

                            MySqlCommand insert =
                                new MySqlCommand(
                                    $"INSERT INTO `{table}` ({cols}) " +
                                    $"VALUES ({pars})",
                                    con);

                            for (int j = 0; j < columns.Length; j++)
                            {
                                string v =
                                    j < values.Length
                                    ? values[j]
                                    : "";

                                insert.Parameters.AddWithValue(
                                    "@v" + j,
                                    ImportManager.ConvertValue(v));
                            }

                            insert.ExecuteNonQuery();

                            added++;
                        }
                    }

                    MessageBox.Show(
                        $"Импорт завершен!\n\n" +
                        $"Добавлено: {added}\n" +
                        $"Дубликаты: {skipped}",
                        "Успех",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            private void buttonBackup_Click(object sender, EventArgs e)
            {
                try
                {
                    string path =
                        BackupManager.CreateBackup();

                    MessageBox.Show(
                        $"Резервная копия создана:\n{path}",
                        "Успех",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            private void buttonRestore_Click(object sender, EventArgs e)
            {
                try
                {
                    string backupFolder =
                        Path.Combine(Application.StartupPath, "Backup");

                    if (!Directory.Exists(backupFolder))
                        Directory.CreateDirectory(backupFolder);

                    OpenFileDialog open = new OpenFileDialog();

                    open.Filter = "SQL files (*.sql)|*.sql";
                    open.InitialDirectory = backupFolder;

                    if (open.ShowDialog() != DialogResult.OK)
                        return;

                    string sql =
                        File.ReadAllText(open.FileName, Encoding.UTF8);

                    sql = sql.Replace("\uFEFF", "");

                    string host =
                        ConfigurationManager.AppSettings["host"];

                    string uid =
                        ConfigurationManager.AppSettings["uid"];

                    string pwd =
                        ConfigurationManager.AppSettings["pwd"];

                    using (MySqlConnection con =
                        new MySqlConnection(
                            $"server={host};uid={uid};pwd={pwd};Allow User Variables=True;"))
                    {
                        con.Open();

                        MySqlCommand disableFk =
                            new MySqlCommand(
                                "SET FOREIGN_KEY_CHECKS = 0;",
                                con);

                        disableFk.ExecuteNonQuery();

                        MySqlScript script =
                            new MySqlScript(con, sql);

                        script.Execute();

                        MySqlCommand enableFk =
                            new MySqlCommand(
                                "SET FOREIGN_KEY_CHECKS = 1;",
                                con);

                        enableFk.ExecuteNonQuery();
                    }

                    LoadTables();

                    MessageBox.Show(
                        "База данных успешно восстановлена!",
                        "Успех",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            private void buttonStructure_Click(object sender, EventArgs e)
            {
                try
                {
                    string structureFolder =
                        Path.Combine(Application.StartupPath, "Structure");

                    if (!Directory.Exists(structureFolder))
                        Directory.CreateDirectory(structureFolder);

                    OpenFileDialog open = new OpenFileDialog();

                    open.Filter = "SQL files (*.sql)|*.sql";
                    open.InitialDirectory = structureFolder;

                    if (open.ShowDialog() != DialogResult.OK)
                        return;

                    string sql =
                        File.ReadAllText(open.FileName, Encoding.UTF8);

                    sql = sql.Replace("\uFEFF", "");

                    string host =
                        ConfigurationManager.AppSettings["host"];

                    string uid =
                        ConfigurationManager.AppSettings["uid"];

                    string pwd =
                        ConfigurationManager.AppSettings["pwd"];

                    using (MySqlConnection con =
                        new MySqlConnection(
                            $"server={host};uid={uid};pwd={pwd};"))
                    {
                        con.Open();

                        MySqlCommand disableFk =
                            new MySqlCommand(
                                "SET FOREIGN_KEY_CHECKS = 0;",
                                con);

                        disableFk.ExecuteNonQuery();

                        MySqlCommand drop =
                            new MySqlCommand(
                                "DROP DATABASE IF EXISTS db57;",
                                con);

                        drop.ExecuteNonQuery();

                        MySqlCommand create =
                            new MySqlCommand(
                                "CREATE DATABASE db57 CHARACTER SET utf8mb4;",
                                con);

                        create.ExecuteNonQuery();

                        MySqlCommand enableFk =
                            new MySqlCommand(
                                "SET FOREIGN_KEY_CHECKS = 1;",
                                con);

                        enableFk.ExecuteNonQuery();
                    }

                    using (MySqlConnection con =
                        new MySqlConnection(connStr.GetConnectionString("db57")))
                    {
                        con.Open();

                        MySqlCommand disableFk =
                            new MySqlCommand(
                                "SET FOREIGN_KEY_CHECKS = 0;",
                                con);

                        disableFk.ExecuteNonQuery();

                        MySqlScript script =
                            new MySqlScript(con, sql);

                        script.Execute();

                        MySqlCommand enableFk =
                            new MySqlCommand(
                                "SET FOREIGN_KEY_CHECKS = 1;",
                                con);

                        enableFk.ExecuteNonQuery();
                    }

                    LoadTables();

                    MessageBox.Show(
                        "Структура базы данных восстановлена!",
                        "Успех",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }