namespace Restaurant
{
    partial class Clients
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Clients));
            this.buttonBack = new System.Windows.Forms.Button();
            this.labelTotal = new System.Windows.Forms.Label();
            this.textBoxClient = new System.Windows.Forms.TextBox();
            this.labelClient = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonNew = new System.Windows.Forms.Button();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonBooking = new System.Windows.Forms.Button();
            this.buttonClearFilters = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonBack
            // 
            this.buttonBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonBack.ForeColor = System.Drawing.Color.White;
            this.buttonBack.Location = new System.Drawing.Point(17, 651);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(147, 71);
            this.buttonBack.TabIndex = 7;
            this.buttonBack.Text = "Назад";
            this.buttonBack.UseVisualStyleBackColor = false;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // labelTotal
            // 
            this.labelTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTotal.AutoSize = true;
            this.labelTotal.BackColor = System.Drawing.Color.Transparent;
            this.labelTotal.Font = new System.Drawing.Font("Verdana", 14F);
            this.labelTotal.ForeColor = System.Drawing.Color.White;
            this.labelTotal.Location = new System.Drawing.Point(893, 517);
            this.labelTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(93, 29);
            this.labelTotal.TabIndex = 29;
            this.labelTotal.Text = "Всего:";
            // 
            // textBoxClient
            // 
            this.textBoxClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.textBoxClient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxClient.Font = new System.Drawing.Font("Verdana", 14F);
            this.textBoxClient.ForeColor = System.Drawing.Color.White;
            this.textBoxClient.Location = new System.Drawing.Point(19, 567);
            this.textBoxClient.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxClient.MaxLength = 100;
            this.textBoxClient.Name = "textBoxClient";
            this.textBoxClient.Size = new System.Drawing.Size(437, 36);
            this.textBoxClient.TabIndex = 1;
            this.textBoxClient.TextChanged += new System.EventHandler(this.textBoxClient_TextChanged);
            this.textBoxClient.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxClient_KeyPress);
            // 
            // labelClient
            // 
            this.labelClient.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelClient.AutoSize = true;
            this.labelClient.BackColor = System.Drawing.Color.Transparent;
            this.labelClient.Font = new System.Drawing.Font("Verdana", 14F);
            this.labelClient.ForeColor = System.Drawing.Color.White;
            this.labelClient.Location = new System.Drawing.Point(13, 535);
            this.labelClient.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelClient.Name = "labelClient";
            this.labelClient.Size = new System.Drawing.Size(112, 29);
            this.labelClient.TabIndex = 27;
            this.labelClient.Text = "Клиент:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1067, 505);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // buttonNew
            // 
            this.buttonNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonNew.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNew.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonNew.ForeColor = System.Drawing.Color.White;
            this.buttonNew.Location = new System.Drawing.Point(900, 652);
            this.buttonNew.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(153, 71);
            this.buttonNew.TabIndex = 3;
            this.buttonNew.Text = "Добавить";
            this.buttonNew.UseVisualStyleBackColor = false;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonUpdate.Enabled = false;
            this.buttonUpdate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUpdate.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonUpdate.ForeColor = System.Drawing.Color.White;
            this.buttonUpdate.Location = new System.Drawing.Point(737, 652);
            this.buttonUpdate.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(153, 71);
            this.buttonUpdate.TabIndex = 4;
            this.buttonUpdate.Text = "Изменить";
            this.buttonUpdate.UseVisualStyleBackColor = false;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonDelete.Enabled = false;
            this.buttonDelete.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonDelete.ForeColor = System.Drawing.Color.White;
            this.buttonDelete.Location = new System.Drawing.Point(898, 570);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(155, 72);
            this.buttonDelete.TabIndex = 5;
            this.buttonDelete.Text = "Удалить";
            this.buttonDelete.UseVisualStyleBackColor = false;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonBooking
            // 
            this.buttonBooking.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonBooking.Enabled = false;
            this.buttonBooking.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBooking.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonBooking.ForeColor = System.Drawing.Color.White;
            this.buttonBooking.Location = new System.Drawing.Point(564, 652);
            this.buttonBooking.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonBooking.Name = "buttonBooking";
            this.buttonBooking.Size = new System.Drawing.Size(163, 71);
            this.buttonBooking.TabIndex = 6;
            this.buttonBooking.Text = "Оформить бронь";
            this.buttonBooking.UseVisualStyleBackColor = false;
            this.buttonBooking.Click += new System.EventHandler(this.buttonBooking_Click);
            // 
            // buttonClearFilters
            // 
            this.buttonClearFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonClearFilters.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonClearFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearFilters.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonClearFilters.ForeColor = System.Drawing.Color.White;
            this.buttonClearFilters.Location = new System.Drawing.Point(465, 566);
            this.buttonClearFilters.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonClearFilters.Name = "buttonClearFilters";
            this.buttonClearFilters.Size = new System.Drawing.Size(98, 38);
            this.buttonClearFilters.TabIndex = 2;
            this.buttonClearFilters.Text = "Сброс";
            this.buttonClearFilters.UseVisualStyleBackColor = false;
            this.buttonClearFilters.Click += new System.EventHandler(this.buttonClearFilters_Click);
            // 
            // Clients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Restaurant.Properties.Resources.fonMain;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1067, 738);
            this.ControlBox = false;
            this.Controls.Add(this.buttonClearFilters);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonBooking);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.textBoxClient);
            this.Controls.Add(this.labelClient);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Clients";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Клиенты";
            this.Load += new System.EventHandler(this.Clients_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.TextBox textBoxClient;
        private System.Windows.Forms.Label labelClient;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonBooking;
        private System.Windows.Forms.Button buttonClearFilters;
    }
}