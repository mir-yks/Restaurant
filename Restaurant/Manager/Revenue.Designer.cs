namespace Restaurant
{
    partial class Revenue
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
            this.dateTimePickerS = new System.Windows.Forms.DateTimePicker();
            this.labelReport = new System.Windows.Forms.Label();
            this.labelS = new System.Windows.Forms.Label();
            this.labelPo = new System.Windows.Forms.Label();
            this.dateTimePickerPo = new System.Windows.Forms.DateTimePicker();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.labelPeriod = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dateTimePickerS
            // 
            this.dateTimePickerS.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerS.Location = new System.Drawing.Point(57, 124);
            this.dateTimePickerS.Name = "dateTimePickerS";
            this.dateTimePickerS.Size = new System.Drawing.Size(200, 31);
            this.dateTimePickerS.TabIndex = 0;
            // 
            // labelReport
            // 
            this.labelReport.AutoSize = true;
            this.labelReport.BackColor = System.Drawing.Color.Transparent;
            this.labelReport.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelReport.ForeColor = System.Drawing.Color.White;
            this.labelReport.Location = new System.Drawing.Point(36, 9);
            this.labelReport.Name = "labelReport";
            this.labelReport.Size = new System.Drawing.Size(187, 23);
            this.labelReport.TabIndex = 1;
            this.labelReport.Text = "Отчёт по выручке";
            // 
            // labelS
            // 
            this.labelS.AutoSize = true;
            this.labelS.BackColor = System.Drawing.Color.Transparent;
            this.labelS.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelS.ForeColor = System.Drawing.Color.White;
            this.labelS.Location = new System.Drawing.Point(29, 124);
            this.labelS.Name = "labelS";
            this.labelS.Size = new System.Drawing.Size(23, 23);
            this.labelS.TabIndex = 1;
            this.labelS.Text = "С";
            // 
            // labelPo
            // 
            this.labelPo.AutoSize = true;
            this.labelPo.BackColor = System.Drawing.Color.Transparent;
            this.labelPo.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPo.ForeColor = System.Drawing.Color.White;
            this.labelPo.Location = new System.Drawing.Point(16, 176);
            this.labelPo.Name = "labelPo";
            this.labelPo.Size = new System.Drawing.Size(36, 23);
            this.labelPo.TabIndex = 3;
            this.labelPo.Text = "По";
            // 
            // dateTimePickerPo
            // 
            this.dateTimePickerPo.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerPo.Location = new System.Drawing.Point(57, 176);
            this.dateTimePickerPo.Name = "dateTimePickerPo";
            this.dateTimePickerPo.Size = new System.Drawing.Size(200, 31);
            this.dateTimePickerPo.TabIndex = 2;
            // 
            // buttonCreate
            // 
            this.buttonCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonCreate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreate.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCreate.ForeColor = System.Drawing.Color.White;
            this.buttonCreate.Location = new System.Drawing.Point(158, 294);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(110, 55);
            this.buttonCreate.TabIndex = 10;
            this.buttonCreate.Text = "Создать отчёт";
            this.buttonCreate.UseVisualStyleBackColor = false;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
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
            this.buttonBack.TabIndex = 11;
            this.buttonBack.Text = "Назад";
            this.buttonBack.UseVisualStyleBackColor = false;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // labelPeriod
            // 
            this.labelPeriod.AutoSize = true;
            this.labelPeriod.BackColor = System.Drawing.Color.Transparent;
            this.labelPeriod.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPeriod.ForeColor = System.Drawing.Color.White;
            this.labelPeriod.Location = new System.Drawing.Point(31, 56);
            this.labelPeriod.Name = "labelPeriod";
            this.labelPeriod.Size = new System.Drawing.Size(192, 23);
            this.labelPeriod.TabIndex = 1;
            this.labelPeriod.Text = "Выберите период:";
            // 
            // Revenue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Restaurant.Properties.Resources.fonAutorization;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(284, 361);
            this.ControlBox = false;
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.labelPo);
            this.Controls.Add(this.dateTimePickerPo);
            this.Controls.Add(this.labelS);
            this.Controls.Add(this.labelPeriod);
            this.Controls.Add(this.labelReport);
            this.Controls.Add(this.dateTimePickerS);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Revenue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выручка";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePickerS;
        private System.Windows.Forms.Label labelReport;
        private System.Windows.Forms.Label labelS;
        private System.Windows.Forms.Label labelPo;
        private System.Windows.Forms.DateTimePicker dateTimePickerPo;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label labelPeriod;
    }
}