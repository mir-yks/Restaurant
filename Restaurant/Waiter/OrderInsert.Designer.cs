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
            this.dateTimePickerOder = new System.Windows.Forms.DateTimePicker();
            this.labelStatusPayment = new System.Windows.Forms.Label();
            this.labelStatusOrder = new System.Windows.Forms.Label();
            this.labelTable = new System.Windows.Forms.Label();
            this.labelClient = new System.Windows.Forms.Label();
            this.labelDateOrder = new System.Windows.Forms.Label();
            this.labelWaiter = new System.Windows.Forms.Label();
            this.comboBoxStatusPayment = new System.Windows.Forms.ComboBox();
            this.comboBoxStatusOrder = new System.Windows.Forms.ComboBox();
            this.comboBoxWorker = new System.Windows.Forms.ComboBox();
            this.comboBoxClient = new System.Windows.Forms.ComboBox();
            this.comboBoxWaiter = new System.Windows.Forms.ComboBox();
            this.labelSum = new System.Windows.Forms.Label();
            this.textBoxSum = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonOrderItem
            // 
            this.buttonOrderItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonOrderItem.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonOrderItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOrderItem.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOrderItem.ForeColor = System.Drawing.Color.White;
            this.buttonOrderItem.Location = new System.Drawing.Point(462, 294);
            this.buttonOrderItem.Name = "buttonOrderItem";
            this.buttonOrderItem.Size = new System.Drawing.Size(110, 55);
            this.buttonOrderItem.TabIndex = 33;
            this.buttonOrderItem.Text = "Состав заказа";
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
            this.buttonBack.Location = new System.Drawing.Point(12, 294);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(110, 55);
            this.buttonBack.TabIndex = 32;
            this.buttonBack.Text = "Назад";
            this.buttonBack.UseVisualStyleBackColor = false;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // dateTimePickerOder
            // 
            this.dateTimePickerOder.CalendarForeColor = System.Drawing.Color.White;
            this.dateTimePickerOder.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.dateTimePickerOder.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.dateTimePickerOder.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerOder.Location = new System.Drawing.Point(313, 166);
            this.dateTimePickerOder.Name = "dateTimePickerOder";
            this.dateTimePickerOder.Size = new System.Drawing.Size(226, 31);
            this.dateTimePickerOder.TabIndex = 31;
            // 
            // labelStatusPayment
            // 
            this.labelStatusPayment.AutoSize = true;
            this.labelStatusPayment.BackColor = System.Drawing.Color.Transparent;
            this.labelStatusPayment.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStatusPayment.ForeColor = System.Drawing.Color.White;
            this.labelStatusPayment.Location = new System.Drawing.Point(309, 73);
            this.labelStatusPayment.Name = "labelStatusPayment";
            this.labelStatusPayment.Size = new System.Drawing.Size(160, 23);
            this.labelStatusPayment.TabIndex = 25;
            this.labelStatusPayment.Text = "Статус оплаты:";
            // 
            // labelStatusOrder
            // 
            this.labelStatusOrder.AutoSize = true;
            this.labelStatusOrder.BackColor = System.Drawing.Color.Transparent;
            this.labelStatusOrder.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStatusOrder.ForeColor = System.Drawing.Color.White;
            this.labelStatusOrder.Location = new System.Drawing.Point(309, 7);
            this.labelStatusOrder.Name = "labelStatusOrder";
            this.labelStatusOrder.Size = new System.Drawing.Size(153, 23);
            this.labelStatusOrder.TabIndex = 26;
            this.labelStatusOrder.Text = "Статус заказа:";
            // 
            // labelTable
            // 
            this.labelTable.AutoSize = true;
            this.labelTable.BackColor = System.Drawing.Color.Transparent;
            this.labelTable.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTable.ForeColor = System.Drawing.Color.White;
            this.labelTable.Location = new System.Drawing.Point(26, 140);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(65, 23);
            this.labelTable.TabIndex = 27;
            this.labelTable.Text = "Стол:";
            // 
            // labelClient
            // 
            this.labelClient.AutoSize = true;
            this.labelClient.BackColor = System.Drawing.Color.Transparent;
            this.labelClient.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelClient.ForeColor = System.Drawing.Color.White;
            this.labelClient.Location = new System.Drawing.Point(26, 73);
            this.labelClient.Name = "labelClient";
            this.labelClient.Size = new System.Drawing.Size(88, 23);
            this.labelClient.TabIndex = 28;
            this.labelClient.Text = "Клиент:";
            // 
            // labelDateOrder
            // 
            this.labelDateOrder.AutoSize = true;
            this.labelDateOrder.BackColor = System.Drawing.Color.Transparent;
            this.labelDateOrder.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDateOrder.ForeColor = System.Drawing.Color.White;
            this.labelDateOrder.Location = new System.Drawing.Point(309, 140);
            this.labelDateOrder.Name = "labelDateOrder";
            this.labelDateOrder.Size = new System.Drawing.Size(135, 23);
            this.labelDateOrder.TabIndex = 29;
            this.labelDateOrder.Text = "Дата заказа:";
            // 
            // labelWaiter
            // 
            this.labelWaiter.AutoSize = true;
            this.labelWaiter.BackColor = System.Drawing.Color.Transparent;
            this.labelWaiter.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelWaiter.ForeColor = System.Drawing.Color.White;
            this.labelWaiter.Location = new System.Drawing.Point(26, 7);
            this.labelWaiter.Name = "labelWaiter";
            this.labelWaiter.Size = new System.Drawing.Size(119, 23);
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
            this.comboBoxStatusPayment.Location = new System.Drawing.Point(313, 99);
            this.comboBoxStatusPayment.Name = "comboBoxStatusPayment";
            this.comboBoxStatusPayment.Size = new System.Drawing.Size(226, 31);
            this.comboBoxStatusPayment.TabIndex = 20;
            // 
            // comboBoxStatusOrder
            // 
            this.comboBoxStatusOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxStatusOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatusOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxStatusOrder.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxStatusOrder.ForeColor = System.Drawing.Color.White;
            this.comboBoxStatusOrder.FormattingEnabled = true;
            this.comboBoxStatusOrder.Location = new System.Drawing.Point(313, 33);
            this.comboBoxStatusOrder.Name = "comboBoxStatusOrder";
            this.comboBoxStatusOrder.Size = new System.Drawing.Size(226, 31);
            this.comboBoxStatusOrder.TabIndex = 21;
            // 
            // comboBoxWorker
            // 
            this.comboBoxWorker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxWorker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWorker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxWorker.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxWorker.ForeColor = System.Drawing.Color.White;
            this.comboBoxWorker.FormattingEnabled = true;
            this.comboBoxWorker.Location = new System.Drawing.Point(30, 166);
            this.comboBoxWorker.Name = "comboBoxWorker";
            this.comboBoxWorker.Size = new System.Drawing.Size(226, 31);
            this.comboBoxWorker.TabIndex = 22;
            // 
            // comboBoxClient
            // 
            this.comboBoxClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxClient.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxClient.ForeColor = System.Drawing.Color.White;
            this.comboBoxClient.FormattingEnabled = true;
            this.comboBoxClient.Location = new System.Drawing.Point(30, 99);
            this.comboBoxClient.Name = "comboBoxClient";
            this.comboBoxClient.Size = new System.Drawing.Size(226, 31);
            this.comboBoxClient.TabIndex = 23;
            // 
            // comboBoxWaiter
            // 
            this.comboBoxWaiter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxWaiter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWaiter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxWaiter.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxWaiter.ForeColor = System.Drawing.Color.White;
            this.comboBoxWaiter.FormattingEnabled = true;
            this.comboBoxWaiter.Location = new System.Drawing.Point(30, 33);
            this.comboBoxWaiter.Name = "comboBoxWaiter";
            this.comboBoxWaiter.Size = new System.Drawing.Size(226, 31);
            this.comboBoxWaiter.TabIndex = 24;
            // 
            // labelSum
            // 
            this.labelSum.AutoSize = true;
            this.labelSum.BackColor = System.Drawing.Color.Transparent;
            this.labelSum.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSum.ForeColor = System.Drawing.Color.White;
            this.labelSum.Location = new System.Drawing.Point(309, 215);
            this.labelSum.Name = "labelSum";
            this.labelSum.Size = new System.Drawing.Size(80, 23);
            this.labelSum.TabIndex = 27;
            this.labelSum.Text = "Сумма:";
            // 
            // textBoxSum
            // 
            this.textBoxSum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.textBoxSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSum.Enabled = false;
            this.textBoxSum.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxSum.ForeColor = System.Drawing.Color.White;
            this.textBoxSum.Location = new System.Drawing.Point(313, 241);
            this.textBoxSum.Name = "textBoxSum";
            this.textBoxSum.Size = new System.Drawing.Size(100, 31);
            this.textBoxSum.TabIndex = 34;
            // 
            // OrderInsert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Restaurant.Properties.Resources.fonMain;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.ControlBox = false;
            this.Controls.Add(this.textBoxSum);
            this.Controls.Add(this.buttonOrderItem);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.dateTimePickerOder);
            this.Controls.Add(this.labelStatusPayment);
            this.Controls.Add(this.labelStatusOrder);
            this.Controls.Add(this.labelSum);
            this.Controls.Add(this.labelTable);
            this.Controls.Add(this.labelClient);
            this.Controls.Add(this.labelDateOrder);
            this.Controls.Add(this.labelWaiter);
            this.Controls.Add(this.comboBoxStatusPayment);
            this.Controls.Add(this.comboBoxStatusOrder);
            this.Controls.Add(this.comboBoxWorker);
            this.Controls.Add(this.comboBoxClient);
            this.Controls.Add(this.comboBoxWaiter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OrderInsert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование заказа";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOrderItem;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.DateTimePicker dateTimePickerOder;
        private System.Windows.Forms.Label labelStatusPayment;
        private System.Windows.Forms.Label labelStatusOrder;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.Label labelClient;
        private System.Windows.Forms.Label labelDateOrder;
        private System.Windows.Forms.Label labelWaiter;
        private System.Windows.Forms.ComboBox comboBoxStatusPayment;
        private System.Windows.Forms.ComboBox comboBoxStatusOrder;
        private System.Windows.Forms.ComboBox comboBoxWorker;
        private System.Windows.Forms.ComboBox comboBoxClient;
        private System.Windows.Forms.ComboBox comboBoxWaiter;
        private System.Windows.Forms.Label labelSum;
        private System.Windows.Forms.TextBox textBoxSum;
    }
}