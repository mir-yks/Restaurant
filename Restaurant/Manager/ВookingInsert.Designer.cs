namespace Restaurant
{
    partial class ВookingInsert
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
            this.labelClient = new System.Windows.Forms.Label();
            this.comboBoxClient = new System.Windows.Forms.ComboBox();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.labelDateBooking = new System.Windows.Forms.Label();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonArrange = new System.Windows.Forms.Button();
            this.textBoxClientsCount = new System.Windows.Forms.TextBox();
            this.labelClientsCount = new System.Windows.Forms.Label();
            this.comboBoxTable = new System.Windows.Forms.ComboBox();
            this.labelTable = new System.Windows.Forms.Label();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // labelClient
            // 
            this.labelClient.AutoSize = true;
            this.labelClient.BackColor = System.Drawing.Color.Transparent;
            this.labelClient.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelClient.ForeColor = System.Drawing.Color.White;
            this.labelClient.Location = new System.Drawing.Point(31, 9);
            this.labelClient.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelClient.Name = "labelClient";
            this.labelClient.Size = new System.Drawing.Size(112, 29);
            this.labelClient.TabIndex = 6;
            this.labelClient.Text = "Клиент:";
            // 
            // comboBoxClient
            // 
            this.comboBoxClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxClient.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxClient.ForeColor = System.Drawing.Color.White;
            this.comboBoxClient.FormattingEnabled = true;
            this.comboBoxClient.Location = new System.Drawing.Point(36, 41);
            this.comboBoxClient.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxClient.Name = "comboBoxClient";
            this.comboBoxClient.Size = new System.Drawing.Size(300, 37);
            this.comboBoxClient.TabIndex = 4;
            // 
            // datePicker
            // 
            this.datePicker.CustomFormat = "";
            this.datePicker.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker.Location = new System.Drawing.Point(36, 115);
            this.datePicker.Margin = new System.Windows.Forms.Padding(4);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(186, 36);
            this.datePicker.TabIndex = 8;
            this.datePicker.Value = new System.DateTime(2025, 11, 3, 0, 0, 0, 0);
            // 
            // labelDateBooking
            // 
            this.labelDateBooking.AutoSize = true;
            this.labelDateBooking.BackColor = System.Drawing.Color.Transparent;
            this.labelDateBooking.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDateBooking.ForeColor = System.Drawing.Color.White;
            this.labelDateBooking.Location = new System.Drawing.Point(31, 82);
            this.labelDateBooking.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDateBooking.Name = "labelDateBooking";
            this.labelDateBooking.Size = new System.Drawing.Size(165, 29);
            this.labelDateBooking.TabIndex = 7;
            this.labelDateBooking.Text = "Дата брони:";
            // 
            // buttonBack
            // 
            this.buttonBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonBack.ForeColor = System.Drawing.Color.White;
            this.buttonBack.Location = new System.Drawing.Point(13, 350);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(140, 68);
            this.buttonBack.TabIndex = 9;
            this.buttonBack.Text = "Назад";
            this.buttonBack.UseVisualStyleBackColor = false;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonArrange
            // 
            this.buttonArrange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonArrange.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonArrange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonArrange.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonArrange.ForeColor = System.Drawing.Color.White;
            this.buttonArrange.Location = new System.Drawing.Point(201, 350);
            this.buttonArrange.Margin = new System.Windows.Forms.Padding(4);
            this.buttonArrange.Name = "buttonArrange";
            this.buttonArrange.Size = new System.Drawing.Size(165, 68);
            this.buttonArrange.TabIndex = 9;
            this.buttonArrange.Text = "Добавить";
            this.buttonArrange.UseVisualStyleBackColor = false;
            this.buttonArrange.Click += new System.EventHandler(this.buttonArrange_Click);
            // 
            // textBoxClientsCount
            // 
            this.textBoxClientsCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.textBoxClientsCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxClientsCount.Font = new System.Drawing.Font("Verdana", 14.25F);
            this.textBoxClientsCount.ForeColor = System.Drawing.Color.White;
            this.textBoxClientsCount.Location = new System.Drawing.Point(36, 188);
            this.textBoxClientsCount.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxClientsCount.MaxLength = 2;
            this.textBoxClientsCount.Name = "textBoxClientsCount";
            this.textBoxClientsCount.Size = new System.Drawing.Size(300, 36);
            this.textBoxClientsCount.TabIndex = 10;
            this.textBoxClientsCount.TextChanged += new System.EventHandler(this.TextBoxClientsCount_TextChanged);
            // 
            // labelClientsCount
            // 
            this.labelClientsCount.AutoSize = true;
            this.labelClientsCount.BackColor = System.Drawing.Color.Transparent;
            this.labelClientsCount.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelClientsCount.ForeColor = System.Drawing.Color.White;
            this.labelClientsCount.Location = new System.Drawing.Point(31, 155);
            this.labelClientsCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelClientsCount.Name = "labelClientsCount";
            this.labelClientsCount.Size = new System.Drawing.Size(257, 29);
            this.labelClientsCount.TabIndex = 7;
            this.labelClientsCount.Text = "Количество гостей:";
            // 
            // comboBoxTable
            // 
            this.comboBoxTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxTable.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxTable.ForeColor = System.Drawing.Color.White;
            this.comboBoxTable.FormattingEnabled = true;
            this.comboBoxTable.Location = new System.Drawing.Point(36, 261);
            this.comboBoxTable.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxTable.Name = "comboBoxTable";
            this.comboBoxTable.Size = new System.Drawing.Size(300, 37);
            this.comboBoxTable.TabIndex = 4;
            // 
            // labelTable
            // 
            this.labelTable.AutoSize = true;
            this.labelTable.BackColor = System.Drawing.Color.Transparent;
            this.labelTable.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTable.ForeColor = System.Drawing.Color.White;
            this.labelTable.Location = new System.Drawing.Point(31, 228);
            this.labelTable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(112, 29);
            this.labelTable.TabIndex = 6;
            this.labelTable.Text = "Столик:";
            // 
            // timePicker
            // 
            this.timePicker.CustomFormat = "HH:mm";
            this.timePicker.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePicker.Location = new System.Drawing.Point(230, 115);
            this.timePicker.Margin = new System.Windows.Forms.Padding(4);
            this.timePicker.Name = "timePicker";
            this.timePicker.ShowUpDown = true;
            this.timePicker.Size = new System.Drawing.Size(106, 36);
            this.timePicker.TabIndex = 8;
            this.timePicker.Value = new System.DateTime(2025, 11, 3, 1, 15, 53, 0);
            // 
            // ВookingInsert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Restaurant.Properties.Resources.fonAutorization;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(379, 431);
            this.ControlBox = false;
            this.Controls.Add(this.textBoxClientsCount);
            this.Controls.Add(this.buttonArrange);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.timePicker);
            this.Controls.Add(this.datePicker);
            this.Controls.Add(this.labelClientsCount);
            this.Controls.Add(this.labelDateBooking);
            this.Controls.Add(this.labelTable);
            this.Controls.Add(this.labelClient);
            this.Controls.Add(this.comboBoxTable);
            this.Controls.Add(this.comboBoxClient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ВookingInsert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Бронирование столика";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelClient;
        private System.Windows.Forms.ComboBox comboBoxClient;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.Label labelDateBooking;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonArrange;
        private System.Windows.Forms.TextBox textBoxClientsCount;
        private System.Windows.Forms.Label labelClientsCount;
        private System.Windows.Forms.ComboBox comboBoxTable;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.DateTimePicker timePicker;
    }
}