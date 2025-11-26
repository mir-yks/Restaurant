using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class ClientsInsert : Form
    {
        private string mode;
        public int ClientID { get; set; }
        public ClientsInsert(string mode)
        {
            InitializeComponent();
            this.mode = mode;

            labelFIO.Font = Fonts.MontserratAlternatesRegular(14f);
            labelPhone.Font = Fonts.MontserratAlternatesRegular(14f);
            textBoxFIO.Font = Fonts.MontserratAlternatesRegular(14f);
            maskedTextBoxPhone.Font = Fonts.MontserratAlternatesRegular(14f);
            buttonBack.Font = Fonts.MontserratAlternatesBold(12f);
            buttonWrite.Font = Fonts.MontserratAlternatesBold(12f);

            ApplyMode();
        }

        private void ApplyMode()
        {
            switch (mode)
            {
                case "view":
                    textBoxFIO.ReadOnly = true;
                    maskedTextBoxPhone.ReadOnly = true;

                    buttonWrite.Visible = false;
                    break;

                case "add":
                    textBoxFIO.Text = "";
                    maskedTextBoxPhone.Text = "";
                    break;
            }
        }

        public string ClientFIO
        {
            get => textBoxFIO.Text;
            set => textBoxFIO.Text = value;
        }

        public string ClientPhone
        {
            get => new string(maskedTextBoxPhone.Text.Where(char.IsDigit).ToArray());
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    maskedTextBoxPhone.Text = "";
                    return;
                }

                string digits = new string(value.Where(char.IsDigit).ToArray());

                if (digits.StartsWith("7") && maskedTextBoxPhone.Mask.StartsWith("+7"))
                {
                    digits = digits.Substring(1);
                }

                maskedTextBoxPhone.Text = digits;
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ClientFIO))
            {
                MessageBox.Show("Введите ФИО клиента.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxFIO.Focus();
                return;
            }

            var fioParts = ClientFIO.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (fioParts.Length < 2)
            {
                MessageBox.Show("Введите полное ФИО (минимум фамилия и имя).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxFIO.Focus();
                return;
            }

            string phoneDigits = new string(maskedTextBoxPhone.Text.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(maskedTextBoxPhone.Text) || phoneDigits.Length < 11)
            {
                MessageBox.Show("Введите полный номер телефона!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                maskedTextBoxPhone.Focus();
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();

                    MySqlCommand checkCmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM client WHERE ClientPhone = @Phone" +
                        (mode == "edit" ? " AND ClientId != @Id" : ""),
                        con);

                    checkCmd.Parameters.AddWithValue("@Phone", phoneDigits);
                    if (mode == "edit")
                        checkCmd.Parameters.AddWithValue("@Id", ClientID);

                    int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        MessageBox.Show("Клиент с таким номером телефона уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        maskedTextBoxPhone.Focus();
                        return;
                    }

                    DialogResult result = MessageBox.Show(
                        "Вы действительно хотите сохранить запись?",
                        "Подтверждение записи",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result != DialogResult.Yes) return;

                    if (mode == "add")
                    {
                        MySqlCommand cmd = new MySqlCommand(
                            "INSERT INTO client (ClientFIO, OriginalClientFIO, ClientPhone) VALUES (@FIO, @OriginalFIO, @Phone)", con);
                        cmd.Parameters.AddWithValue("@FIO", ClientFIO);
                        cmd.Parameters.AddWithValue("@OriginalFIO", ClientFIO); 
                        cmd.Parameters.AddWithValue("@Phone", phoneDigits);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Клиент \"{ClientFIO}\" успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (mode == "edit")
                    {
                        bool fioChanged = false;
                        string originalFIO = ClientFIO; 

                        MySqlCommand getOriginalCmd = new MySqlCommand(
                            "SELECT ClientFIO, OriginalClientFIO FROM Client WHERE ClientId = @Id", con);
                        getOriginalCmd.Parameters.AddWithValue("@Id", ClientID);

                        using (var reader = getOriginalCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string currentFIO = reader.GetString("ClientFIO");
                                string storedOriginalFIO = reader.IsDBNull(reader.GetOrdinal("OriginalClientFIO"))
                                    ? currentFIO
                                    : reader.GetString("OriginalClientFIO");

                                if (currentFIO != ClientFIO)
                                {
                                    fioChanged = true;
                                    originalFIO = storedOriginalFIO; 
                                }
                                else
                                {
                                    originalFIO = storedOriginalFIO;
                                }
                            }
                        }

                        MySqlCommand cmd = new MySqlCommand(
                            "UPDATE client SET ClientFIO = @FIO, OriginalClientFIO = @OriginalFIO, ClientPhone = @Phone WHERE ClientId = @Id", con);
                        cmd.Parameters.AddWithValue("@FIO", ClientFIO);
                        cmd.Parameters.AddWithValue("@OriginalFIO", originalFIO);
                        cmd.Parameters.AddWithValue("@Phone", phoneDigits);
                        cmd.Parameters.AddWithValue("@Id", ClientID);
                        cmd.ExecuteNonQuery();

                        string message = fioChanged
                            ? $"Данные клиента успешно обновлены!\nФИО: \"{ClientFIO}\"\n\nПримечание: в существующих заказах останется предыдущее ФИО клиента."
                            : $"Данные клиента успешно обновлены!\nФИО: \"{ClientFIO}\"";

                        MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void textBoxFIO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[а-яА-Я-\s]$"))
            {
                e.Handled = true;
            }
        }

        private void textBoxFIO_TextChanged(object sender, EventArgs e)
        {
            int cursorPos = textBoxFIO.SelectionStart;

            string input = textBoxFIO.Text;

            int spaceCount = input.Count(c => c == ' ');
            if (spaceCount > 2)
            {
                int lastSpace = input.LastIndexOf(' ');
                input = input.Remove(lastSpace, 1);
            }

            int dashCount = input.Count(c => c == '-');
            if (dashCount > 1)
            {
                int lastDash = input.LastIndexOf('-');
                input = input.Remove(lastDash, 1);
            }

            string[] parts = input
                .Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => char.ToUpper(p[0]) + p.Substring(1).ToLower())
                .ToArray();

            string formatted = input;
            int index = 0;
            foreach (string part in parts)
            {
                int pos = formatted.IndexOf(part, index, StringComparison.OrdinalIgnoreCase);
                if (pos >= 0)
                {
                    formatted = formatted.Remove(pos, part.Length).Insert(pos, part);
                    index = pos + part.Length;
                }
            }

            textBoxFIO.TextChanged -= textBoxFIO_TextChanged;
            textBoxFIO.Text = formatted;
            textBoxFIO.SelectionStart = Math.Min(cursorPos, textBoxFIO.Text.Length);
            textBoxFIO.TextChanged += textBoxFIO_TextChanged;
        }
    }
}
