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
    public partial class WorkerInsert : Form
    {
        private string mode;
        public WorkerInsert(string mode)
        {
            InitializeComponent();
            this.mode = mode;

            label1.Font = Fonts.MontserratAlternatesRegular(14f);
            label2.Font = Fonts.MontserratAlternatesRegular(14f);
            label3.Font = Fonts.MontserratAlternatesRegular(14f);
            label4.Font = Fonts.MontserratAlternatesRegular(14f);
            label11.Font = Fonts.MontserratAlternatesRegular(14f);
            label6.Font = Fonts.MontserratAlternatesRegular(14f);
            label7.Font = Fonts.MontserratAlternatesRegular(14f);
            label12.Font = Fonts.MontserratAlternatesRegular(14f);
            label9.Font = Fonts.MontserratAlternatesRegular(14f);
            label10.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox4.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox5.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox6.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox8.Font = Fonts.MontserratAlternatesRegular(14f);
            textBox9.Font = Fonts.MontserratAlternatesRegular(14f);
            maskedTextBox1.Font = Fonts.MontserratAlternatesRegular(14f);
            comboBox3.Font = Fonts.MontserratAlternatesRegular(14f);
            dateTimePicker1.Font = Fonts.MontserratAlternatesRegular(14f);
            dateTimePicker2.Font = Fonts.MontserratAlternatesRegular(14f);
            button1.Font = Fonts.MontserratAlternatesBold(12f);
            button2.Font = Fonts.MontserratAlternatesBold(12f);

            LoadRoles();
            ApplyMode();
        }

        private void LoadRoles()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connStr.ConnectionString))
                {
                    con.Open();
                    DataTable rolesTable = new DataTable();
                    MySqlDataAdapter daRoles = new MySqlDataAdapter("SELECT RoleName FROM role;", con);
                    daRoles.Fill(rolesTable);

                    comboBox3.Items.Clear();
                    foreach (DataRow row in rolesTable.Rows)
                    {
                        comboBox3.Items.Add(row["RoleName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyMode()
        {
            switch (mode)
            {
                case "view":
                    label2.Visible = false;
                    label6.Visible = false;
                    textBox5.Visible = false;
                    textBox6.Visible = false;
                    button2.Visible = false;

                    textBox1.ReadOnly = true;
                    textBox4.ReadOnly = true;
                    textBox5.ReadOnly = true;
                    textBox6.ReadOnly = true;
                    maskedTextBox1.ReadOnly = true;
                    textBox8.ReadOnly = true;
                    textBox9.ReadOnly = true;

                    comboBox3.Enabled = false;
                    dateTimePicker1.Enabled = false;
                    dateTimePicker2.Enabled = false;
                    
                    comboBox3.Location = new System.Drawing.Point(16, 235);
                    label3.Location = new System.Drawing.Point(12, 209);
                    textBox9.Location = new System.Drawing.Point(16, 168);
                    label12.Location = new System.Drawing.Point(12, 142);

                    button1.Text = "Закрыть"; 
                    break;

                case "add":
                    textBox1.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    maskedTextBox1.Text = "";
                    textBox8.Text = "";
                    textBox9.Text = "";
                    comboBox3.SelectedIndex = 0;
                    dateTimePicker1.Value = DateTime.Today;
                    dateTimePicker2.Value = DateTime.Today;

                    button2.Visible = true; 
                    button1.Text = "Отмена";
                    break;

                case "edit":
                    textBox1.ReadOnly = false;
                    textBox4.ReadOnly = false;
                    textBox5.ReadOnly = false;
                    textBox6.ReadOnly = false;
                    maskedTextBox1.ReadOnly = false;
                    textBox8.ReadOnly = false;
                    textBox9.ReadOnly = false;

                    comboBox3.Enabled = true;
                    dateTimePicker1.Enabled = true;
                    dateTimePicker2.Enabled = true;

                    button2.Visible = true; 
                    button1.Text = "Отмена";
                    break;
            }
        }

        public string WorkerFIO
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }

        public string WorkerLogin
        {
            get => textBox4.Text;
            set => textBox4.Text = value;
        }

        public string WorkerPhone
        {
            get => maskedTextBox1.Text;
            set => maskedTextBox1.Text = value;
        }

        public string WorkerEmail
        {
            get => textBox8.Text;
            set => textBox8.Text = value;
        }

        public DateTime WorkerBirthday
        {
            get => dateTimePicker1.Value;
            set => dateTimePicker1.Value = value;
        }

        public DateTime WorkerDateEmployment
        {
            get => dateTimePicker2.Value;
            set => dateTimePicker2.Value = value;
        }

        public string WorkerAddress
        {
            get => textBox9.Text;
            set => textBox9.Text = value;
        }

        public string WorkerRole
        {
            get => comboBox3.Text;
            set => comboBox3.Text = value;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}