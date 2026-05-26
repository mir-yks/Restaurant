namespace Restaurant
{
    partial class ManagementBD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagementBD));
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonBackup = new System.Windows.Forms.Button();
            this.buttonStructure = new System.Windows.Forms.Button();
            this.comboBoxImport = new System.Windows.Forms.ComboBox();
            this.labelImport = new System.Windows.Forms.Label();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.labelExport = new System.Windows.Forms.Label();
            this.comboBoxExport = new System.Windows.Forms.ComboBox();
            this.buttonImportFile = new System.Windows.Forms.Button();
            this.buttonExportFile = new System.Windows.Forms.Button();
            this.buttonRestore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonBack
            // 
            this.buttonBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonBack.ForeColor = System.Drawing.Color.White;
            this.buttonBack.Location = new System.Drawing.Point(14, 272);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(147, 79);
            this.buttonBack.TabIndex = 9;
            this.buttonBack.Text = "Назад";
            this.buttonBack.UseVisualStyleBackColor = false;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonBackup
            // 
            this.buttonBackup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonBackup.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBackup.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonBackup.ForeColor = System.Drawing.Color.White;
            this.buttonBackup.Location = new System.Drawing.Point(404, 272);
            this.buttonBackup.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonBackup.Name = "buttonBackup";
            this.buttonBackup.Size = new System.Drawing.Size(189, 79);
            this.buttonBackup.TabIndex = 8;
            this.buttonBackup.Text = "Резервное копирование";
            this.buttonBackup.UseVisualStyleBackColor = false;
            this.buttonBackup.Click += new System.EventHandler(this.buttonBackup_Click);
            // 
            // buttonStructure
            // 
            this.buttonStructure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonStructure.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonStructure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStructure.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonStructure.ForeColor = System.Drawing.Color.White;
            this.buttonStructure.Location = new System.Drawing.Point(831, 272);
            this.buttonStructure.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonStructure.Name = "buttonStructure";
            this.buttonStructure.Size = new System.Drawing.Size(222, 79);
            this.buttonStructure.TabIndex = 7;
            this.buttonStructure.Text = "Восстановить структуру БД";
            this.buttonStructure.UseVisualStyleBackColor = false;
            this.buttonStructure.Click += new System.EventHandler(this.buttonStructure_Click);
            // 
            // comboBoxImport
            // 
            this.comboBoxImport.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxImport.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxImport.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxImport.ForeColor = System.Drawing.Color.White;
            this.comboBoxImport.FormattingEnabled = true;
            this.comboBoxImport.Location = new System.Drawing.Point(13, 57);
            this.comboBoxImport.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxImport.Name = "comboBoxImport";
            this.comboBoxImport.Size = new System.Drawing.Size(499, 37);
            this.comboBoxImport.TabIndex = 1;
            // 
            // labelImport
            // 
            this.labelImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelImport.AutoSize = true;
            this.labelImport.BackColor = System.Drawing.Color.Transparent;
            this.labelImport.Font = new System.Drawing.Font("Verdana", 14F);
            this.labelImport.ForeColor = System.Drawing.Color.White;
            this.labelImport.Location = new System.Drawing.Point(8, 9);
            this.labelImport.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelImport.Name = "labelImport";
            this.labelImport.Size = new System.Drawing.Size(415, 29);
            this.labelImport.TabIndex = 46;
            this.labelImport.Text = "Выберите таблицу для импорта:";
            // 
            // buttonImport
            // 
            this.buttonImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonImport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImport.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonImport.ForeColor = System.Drawing.Color.White;
            this.buttonImport.Location = new System.Drawing.Point(272, 107);
            this.buttonImport.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(240, 75);
            this.buttonImport.TabIndex = 3;
            this.buttonImport.Text = "Импортировать данные";
            this.buttonImport.UseVisualStyleBackColor = false;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonExport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExport.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonExport.ForeColor = System.Drawing.Color.White;
            this.buttonExport.Location = new System.Drawing.Point(813, 107);
            this.buttonExport.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(240, 75);
            this.buttonExport.TabIndex = 6;
            this.buttonExport.Text = "Экспортировать данные";
            this.buttonExport.UseVisualStyleBackColor = false;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // labelExport
            // 
            this.labelExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelExport.AutoSize = true;
            this.labelExport.BackColor = System.Drawing.Color.Transparent;
            this.labelExport.Font = new System.Drawing.Font("Verdana", 14F);
            this.labelExport.ForeColor = System.Drawing.Color.White;
            this.labelExport.Location = new System.Drawing.Point(549, 9);
            this.labelExport.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelExport.Name = "labelExport";
            this.labelExport.Size = new System.Drawing.Size(423, 29);
            this.labelExport.TabIndex = 46;
            this.labelExport.Text = "Выберите таблицу для экспорта:";
            // 
            // comboBoxExport
            // 
            this.comboBoxExport.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxExport.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxExport.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxExport.ForeColor = System.Drawing.Color.White;
            this.comboBoxExport.FormattingEnabled = true;
            this.comboBoxExport.Location = new System.Drawing.Point(554, 57);
            this.comboBoxExport.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxExport.Name = "comboBoxExport";
            this.comboBoxExport.Size = new System.Drawing.Size(499, 37);
            this.comboBoxExport.TabIndex = 4;
            // 
            // buttonImportFile
            // 
            this.buttonImportFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonImportFile.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonImportFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImportFile.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonImportFile.ForeColor = System.Drawing.Color.White;
            this.buttonImportFile.Location = new System.Drawing.Point(13, 107);
            this.buttonImportFile.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonImportFile.Name = "buttonImportFile";
            this.buttonImportFile.Size = new System.Drawing.Size(240, 75);
            this.buttonImportFile.TabIndex = 2;
            this.buttonImportFile.Text = "Выберите файл для импорта";
            this.buttonImportFile.UseVisualStyleBackColor = false;
            this.buttonImportFile.Click += new System.EventHandler(this.buttonImportFile_Click);
            // 
            // buttonExportFile
            // 
            this.buttonExportFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonExportFile.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonExportFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExportFile.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonExportFile.ForeColor = System.Drawing.Color.White;
            this.buttonExportFile.Location = new System.Drawing.Point(554, 107);
            this.buttonExportFile.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonExportFile.Name = "buttonExportFile";
            this.buttonExportFile.Size = new System.Drawing.Size(240, 75);
            this.buttonExportFile.TabIndex = 5;
            this.buttonExportFile.Text = "Выберите файл для экспорта";
            this.buttonExportFile.UseVisualStyleBackColor = false;
            this.buttonExportFile.Click += new System.EventHandler(this.buttonExportFile_Click);
            // 
            // buttonRestore
            // 
            this.buttonRestore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonRestore.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRestore.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonRestore.ForeColor = System.Drawing.Color.White;
            this.buttonRestore.Location = new System.Drawing.Point(603, 272);
            this.buttonRestore.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonRestore.Name = "buttonRestore";
            this.buttonRestore.Size = new System.Drawing.Size(218, 79);
            this.buttonRestore.TabIndex = 8;
            this.buttonRestore.Text = "Восстановить базу данных";
            this.buttonRestore.UseVisualStyleBackColor = false;
            this.buttonRestore.Click += new System.EventHandler(this.buttonRestore_Click);
            // 
            // ManagementBD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(32)))), ((int)(((byte)(44)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1067, 364);
            this.ControlBox = false;
            this.Controls.Add(this.comboBoxExport);
            this.Controls.Add(this.labelExport);
            this.Controls.Add(this.comboBoxImport);
            this.Controls.Add(this.labelImport);
            this.Controls.Add(this.buttonExportFile);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonImportFile);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.buttonRestore);
            this.Controls.Add(this.buttonBackup);
            this.Controls.Add(this.buttonStructure);
            this.Controls.Add(this.buttonBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ManagementBD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Управление БД";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonBackup;
        private System.Windows.Forms.Button buttonStructure;
        private System.Windows.Forms.ComboBox comboBoxImport;
        private System.Windows.Forms.Label labelImport;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Label labelExport;
        private System.Windows.Forms.ComboBox comboBoxExport;
        private System.Windows.Forms.Button buttonImportFile;
        private System.Windows.Forms.Button buttonExportFile;
        private System.Windows.Forms.Button buttonRestore;
    }
}