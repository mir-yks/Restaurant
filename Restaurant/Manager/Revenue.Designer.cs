namespace Restaurant
{
    partial class Revenue
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Revenue));
            this.dateTimePickerMin = new System.Windows.Forms.DateTimePicker();
            this.labelReport = new System.Windows.Forms.Label();
            this.labelS = new System.Windows.Forms.Label();
            this.labelPo = new System.Windows.Forms.Label();
            this.dateTimePickerMax = new System.Windows.Forms.DateTimePicker();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.labelPeriod = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dateTimePickerMin
            // 
            this.dateTimePickerMin.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.dateTimePickerMin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerMin.Location = new System.Drawing.Point(76, 153);
            this.dateTimePickerMin.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePickerMin.Name = "dateTimePickerMin";
            this.dateTimePickerMin.Size = new System.Drawing.Size(265, 36);
            this.dateTimePickerMin.TabIndex = 1;
            this.dateTimePickerMin.ValueChanged += new System.EventHandler(this.dateTimePickerMin_ValueChanged);
            // 
            // labelReport
            // 
            this.labelReport.AutoSize = true;
            this.labelReport.BackColor = System.Drawing.Color.Transparent;
            this.labelReport.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelReport.ForeColor = System.Drawing.Color.White;
            this.labelReport.Location = new System.Drawing.Point(50, 20);
            this.labelReport.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelReport.Name = "labelReport";
            this.labelReport.Size = new System.Drawing.Size(236, 29);
            this.labelReport.TabIndex = 1;
            this.labelReport.Text = "Отчёт по выручке";
            // 
            // labelS
            // 
            this.labelS.AutoSize = true;
            this.labelS.BackColor = System.Drawing.Color.Transparent;
            this.labelS.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelS.ForeColor = System.Drawing.Color.White;
            this.labelS.Location = new System.Drawing.Point(39, 160);
            this.labelS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelS.Name = "labelS";
            this.labelS.Size = new System.Drawing.Size(30, 29);
            this.labelS.TabIndex = 1;
            this.labelS.Text = "С";
            // 
            // labelPo
            // 
            this.labelPo.AutoSize = true;
            this.labelPo.BackColor = System.Drawing.Color.Transparent;
            this.labelPo.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPo.ForeColor = System.Drawing.Color.White;
            this.labelPo.Location = new System.Drawing.Point(21, 224);
            this.labelPo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPo.Name = "labelPo";
            this.labelPo.Size = new System.Drawing.Size(46, 29);
            this.labelPo.TabIndex = 3;
            this.labelPo.Text = "По";
            // 
            // dateTimePickerMax
            // 
            this.dateTimePickerMax.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.dateTimePickerMax.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerMax.Location = new System.Drawing.Point(76, 224);
            this.dateTimePickerMax.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePickerMax.Name = "dateTimePickerMax";
            this.dateTimePickerMax.Size = new System.Drawing.Size(265, 36);
            this.dateTimePickerMax.TabIndex = 2;
            this.dateTimePickerMax.ValueChanged += new System.EventHandler(this.dateTimePickerMax_ValueChanged);
            // 
            // buttonCreate
            // 
            this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonCreate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreate.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCreate.ForeColor = System.Drawing.Color.White;
            this.buttonCreate.Location = new System.Drawing.Point(211, 362);
            this.buttonCreate.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(147, 68);
            this.buttonCreate.TabIndex = 3;
            this.buttonCreate.Text = "Создать отчёт";
            this.buttonCreate.UseVisualStyleBackColor = false;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonBack.ForeColor = System.Drawing.Color.White;
            this.buttonBack.Location = new System.Drawing.Point(16, 362);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(147, 68);
            this.buttonBack.TabIndex = 4;
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
            this.labelPeriod.Location = new System.Drawing.Point(50, 76);
            this.labelPeriod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPeriod.Name = "labelPeriod";
            this.labelPeriod.Size = new System.Drawing.Size(241, 29);
            this.labelPeriod.TabIndex = 1;
            this.labelPeriod.Text = "Выберите период:";
            // 
            // Revenue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(32)))), ((int)(((byte)(44)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(379, 444);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.labelPo);
            this.Controls.Add(this.dateTimePickerMax);
            this.Controls.Add(this.labelS);
            this.Controls.Add(this.labelPeriod);
            this.Controls.Add(this.labelReport);
            this.Controls.Add(this.dateTimePickerMin);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(379, 444);
            this.Name = "Revenue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выручка";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.DateTimePicker dateTimePickerMin;
        private System.Windows.Forms.Label labelReport;
        private System.Windows.Forms.Label labelS;
        private System.Windows.Forms.Label labelPo;
        private System.Windows.Forms.DateTimePicker dateTimePickerMax;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label labelPeriod;
    }
}