namespace Restaurant
{
    partial class OrderInsert
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
            this.buttonOrderItem = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.dateTimePickerOrder = new System.Windows.Forms.DateTimePicker();
            this.labelStatusPayment = new System.Windows.Forms.Label();
            this.labelStatusOrder = new System.Windows.Forms.Label();
            this.labelTable = new System.Windows.Forms.Label();
            this.labelClient = new System.Windows.Forms.Label();
            this.labelDateOrder = new System.Windows.Forms.Label();
            this.labelWaiter = new System.Windows.Forms.Label();
            this.comboBoxStatusPayment = new System.Windows.Forms.ComboBox();
            this.comboBoxStatusOrder = new System.Windows.Forms.ComboBox();
            this.comboBoxTable = new System.Windows.Forms.ComboBox();
            this.comboBoxClient = new System.Windows.Forms.ComboBox();
            this.comboBoxWaiter = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonOrderItem
            // 
            this.buttonOrderItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonOrderItem.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonOrderItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOrderItem.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOrderItem.ForeColor = System.Drawing.Color.White;
            this.buttonOrderItem.Location = new System.Drawing.Point(596, 362);
            this.buttonOrderItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonOrderItem.Name = "buttonOrderItem";
            this.buttonOrderItem.Size = new System.Drawing.Size(167, 68);
            this.buttonOrderItem.TabIndex = 33;
            this.buttonOrderItem.Text = "Сохранить";
            this.buttonOrderItem.UseVisualStyleBackColor = false;
            this.buttonOrderItem.Click += new System.EventHandler(this.buttonOrderItem_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonBack.ForeColor = System.Drawing.Color.White;
            this.buttonBack.Location = new System.Drawing.Point(16, 362);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(147, 68);
            this.buttonBack.TabIndex = 32;
            this.buttonBack.Text = "Назад";
            this.buttonBack.UseVisualStyleBackColor = false;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // dateTimePickerOrder
            // 
            this.dateTimePickerOrder.CalendarForeColor = System.Drawing.Color.White;
            this.dateTimePickerOrder.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.dateTimePickerOrder.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.dateTimePickerOrder.Enabled = false;
            this.dateTimePickerOrder.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.dateTimePickerOrder.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerOrder.Location = new System.Drawing.Point(417, 204);
            this.dateTimePickerOrder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateTimePickerOrder.Name = "dateTimePickerOrder";
            this.dateTimePickerOrder.Size = new System.Drawing.Size(300, 36);
            this.dateTimePickerOrder.TabIndex = 31;
            // 
            // labelStatusPayment
            // 
            this.labelStatusPayment.AutoSize = true;
            this.labelStatusPayment.BackColor = System.Drawing.Color.Transparent;
            this.labelStatusPayment.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStatusPayment.ForeColor = System.Drawing.Color.White;
            this.labelStatusPayment.Location = new System.Drawing.Point(412, 90);
            this.labelStatusPayment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatusPayment.Name = "labelStatusPayment";
            this.labelStatusPayment.Size = new System.Drawing.Size(204, 29);
            this.labelStatusPayment.TabIndex = 25;
            this.labelStatusPayment.Text = "Статус оплаты:";
            // 
            // labelStatusOrder
            // 
            this.labelStatusOrder.AutoSize = true;
            this.labelStatusOrder.BackColor = System.Drawing.Color.Transparent;
            this.labelStatusOrder.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStatusOrder.ForeColor = System.Drawing.Color.White;
            this.labelStatusOrder.Location = new System.Drawing.Point(412, 9);
            this.labelStatusOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatusOrder.Name = "labelStatusOrder";
            this.labelStatusOrder.Size = new System.Drawing.Size(196, 29);
            this.labelStatusOrder.TabIndex = 26;
            this.labelStatusOrder.Text = "Статус заказа:";
            // 
            // labelTable
            // 
            this.labelTable.AutoSize = true;
            this.labelTable.BackColor = System.Drawing.Color.Transparent;
            this.labelTable.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTable.ForeColor = System.Drawing.Color.White;
            this.labelTable.Location = new System.Drawing.Point(35, 172);
            this.labelTable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(83, 29);
            this.labelTable.TabIndex = 27;
            this.labelTable.Text = "Стол:";
            // 
            // labelClient
            // 
            this.labelClient.AutoSize = true;
            this.labelClient.BackColor = System.Drawing.Color.Transparent;
            this.labelClient.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelClient.ForeColor = System.Drawing.Color.White;
            this.labelClient.Location = new System.Drawing.Point(35, 90);
            this.labelClient.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelClient.Name = "labelClient";
            this.labelClient.Size = new System.Drawing.Size(112, 29);
            this.labelClient.TabIndex = 28;
            this.labelClient.Text = "Клиент:";
            // 
            // labelDateOrder
            // 
            this.labelDateOrder.AutoSize = true;
            this.labelDateOrder.BackColor = System.Drawing.Color.Transparent;
            this.labelDateOrder.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDateOrder.ForeColor = System.Drawing.Color.White;
            this.labelDateOrder.Location = new System.Drawing.Point(412, 172);
            this.labelDateOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDateOrder.Name = "labelDateOrder";
            this.labelDateOrder.Size = new System.Drawing.Size(172, 29);
            this.labelDateOrder.TabIndex = 29;
            this.labelDateOrder.Text = "Дата заказа:";
            // 
            // labelWaiter
            // 
            this.labelWaiter.AutoSize = true;
            this.labelWaiter.BackColor = System.Drawing.Color.Transparent;
            this.labelWaiter.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelWaiter.ForeColor = System.Drawing.Color.White;
            this.labelWaiter.Location = new System.Drawing.Point(35, 9);
            this.labelWaiter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWaiter.Name = "labelWaiter";
            this.labelWaiter.Size = new System.Drawing.Size(149, 29);
            this.labelWaiter.TabIndex = 30;
            this.labelWaiter.Text = "Официант:";
            // 
            // comboBoxStatusPayment
            // 
            this.comboBoxStatusPayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxStatusPayment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatusPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxStatusPayment.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxStatusPayment.ForeColor = System.Drawing.Color.White;
            this.comboBoxStatusPayment.FormattingEnabled = true;
            this.comboBoxStatusPayment.Location = new System.Drawing.Point(417, 122);
            this.comboBoxStatusPayment.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxStatusPayment.Name = "comboBoxStatusPayment";
            this.comboBoxStatusPayment.Size = new System.Drawing.Size(300, 37);
            this.comboBoxStatusPayment.TabIndex = 20;
            this.comboBoxStatusPayment.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatusPayment_SelectedIndexChanged);
            // 
            // comboBoxStatusOrder
            // 
            this.comboBoxStatusOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxStatusOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatusOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxStatusOrder.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxStatusOrder.ForeColor = System.Drawing.Color.White;
            this.comboBoxStatusOrder.FormattingEnabled = true;
            this.comboBoxStatusOrder.Location = new System.Drawing.Point(417, 41);
            this.comboBoxStatusOrder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxStatusOrder.Name = "comboBoxStatusOrder";
            this.comboBoxStatusOrder.Size = new System.Drawing.Size(300, 37);
            this.comboBoxStatusOrder.TabIndex = 21;
            this.comboBoxStatusOrder.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatusOrder_SelectedIndexChanged);
            // 
            // comboBoxTable
            // 
            this.comboBoxTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxTable.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxTable.ForeColor = System.Drawing.Color.White;
            this.comboBoxTable.FormattingEnabled = true;
            this.comboBoxTable.Location = new System.Drawing.Point(40, 204);
            this.comboBoxTable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxTable.Name = "comboBoxTable";
            this.comboBoxTable.Size = new System.Drawing.Size(300, 37);
            this.comboBoxTable.TabIndex = 22;
            // 
            // comboBoxClient
            // 
            this.comboBoxClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxClient.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxClient.ForeColor = System.Drawing.Color.White;
            this.comboBoxClient.FormattingEnabled = true;
            this.comboBoxClient.Location = new System.Drawing.Point(40, 122);
            this.comboBoxClient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxClient.Name = "comboBoxClient";
            this.comboBoxClient.Size = new System.Drawing.Size(300, 37);
            this.comboBoxClient.TabIndex = 23;
            this.comboBoxClient.SelectedIndexChanged += new System.EventHandler(this.comboBoxClient_SelectedIndexChanged);
            // 
            // comboBoxWaiter
            // 
            this.comboBoxWaiter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxWaiter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWaiter.Enabled = false;
            this.comboBoxWaiter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxWaiter.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxWaiter.ForeColor = System.Drawing.Color.White;
            this.comboBoxWaiter.FormattingEnabled = true;
            this.comboBoxWaiter.Location = new System.Drawing.Point(40, 41);
            this.comboBoxWaiter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxWaiter.Name = "comboBoxWaiter";
            this.comboBoxWaiter.Size = new System.Drawing.Size(300, 37);
            this.comboBoxWaiter.TabIndex = 24;
            // 
            // OrderInsert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Restaurant.Properties.Resources.fonMain;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(779, 444);
            this.ControlBox = false;
            this.Controls.Add(this.buttonOrderItem);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.dateTimePickerOrder);
            this.Controls.Add(this.labelStatusPayment);
            this.Controls.Add(this.labelStatusOrder);
            this.Controls.Add(this.labelTable);
            this.Controls.Add(this.labelClient);
            this.Controls.Add(this.labelDateOrder);
            this.Controls.Add(this.labelWaiter);
            this.Controls.Add(this.comboBoxStatusPayment);
            this.Controls.Add(this.comboBoxStatusOrder);
            this.Controls.Add(this.comboBoxTable);
            this.Controls.Add(this.comboBoxClient);
            this.Controls.Add(this.comboBoxWaiter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "OrderInsert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование заказа";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OrderInsert_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOrderItem;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.DateTimePicker dateTimePickerOrder;
        private System.Windows.Forms.Label labelStatusPayment;
        private System.Windows.Forms.Label labelStatusOrder;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.Label labelClient;
        private System.Windows.Forms.Label labelDateOrder;
        private System.Windows.Forms.Label labelWaiter;
        private System.Windows.Forms.ComboBox comboBoxStatusPayment;
        private System.Windows.Forms.ComboBox comboBoxStatusOrder;
        private System.Windows.Forms.ComboBox comboBoxTable;
        private System.Windows.Forms.ComboBox comboBoxClient;
        private System.Windows.Forms.ComboBox comboBoxWaiter;
    }
}