using System;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class Tables : Form
    {
        private int roleId;

        public Tables(int role)
        {
            InitializeComponent();
            roleId = role;
            ConfigureButtons(); 
        }

        private void ConfigureButtons()
        {
            button2.Visible = false; 
            button3.Visible = false;
            button4.Visible = false; 
            button1.Visible = true; 

            if (roleId == 1) 
            {
                button2.Visible = true;
                button3.Visible = true;
            }
            else if (roleId == 2) 
            {
                button4.Visible = true;
                button4.Location = new System.Drawing.Point(661, 492);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TablesInsert TablesInsert = new TablesInsert();
            this.Visible = true;
            TablesInsert.ShowDialog();
            this.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TablesInsert TablesInsert = new TablesInsert();
            this.Visible = true;
            TablesInsert.ShowDialog();
            this.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Вooking BookingManager = new Вooking();
            this.Visible = true;
            BookingManager.ShowDialog();
            this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }
    }
}
