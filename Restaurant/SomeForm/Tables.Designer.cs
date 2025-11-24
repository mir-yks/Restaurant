namespace Restaurant
{
    partial class Tables
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
            this.comboBoxPlaceCount = new System.Windows.Forms.ComboBox();
            this.buttonBack = new System.Windows.Forms.Button();
            this.labelTotal = new System.Windows.Forms.Label();
            this.textBoxTable = new System.Windows.Forms.TextBox();
            this.labelPlaceCount = new System.Windows.Forms.Label();
            this.labelTable = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.comboBoxStatus = new System.Windows.Forms.ComboBox();
            this.buttonClearFilters = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxPlaceCount
            // 
            this.comboBoxPlaceCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxPlaceCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlaceCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxPlaceCount.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxPlaceCount.ForeColor = System.Drawing.Color.White;
            this.comboBoxPlaceCount.FormattingEnabled = true;
            this.comboBoxPlaceCount.Location = new System.Drawing.Point(267, 567);
            this.comboBoxPlaceCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxPlaceCount.Name = "comboBoxPlaceCount";
            this.comboBoxPlaceCount.Size = new System.Drawing.Size(252, 37);
            this.comboBoxPlaceCount.TabIndex = 29;
            this.comboBoxPlaceCount.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // buttonBack
            // 
            this.buttonBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonBack.ForeColor = System.Drawing.Color.White;
            this.buttonBack.Location = new System.Drawing.Point(17, 660);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(147, 62);
            this.buttonBack.TabIndex = 28;
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
            this.labelTotal.TabIndex = 26;
            this.labelTotal.Text = "Всего:";
            // 
            // textBoxTable
            // 
            this.textBoxTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.textBoxTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTable.Font = new System.Drawing.Font("Verdana", 14F);
            this.textBoxTable.ForeColor = System.Drawing.Color.White;
            this.textBoxTable.Location = new System.Drawing.Point(19, 567);
            this.textBoxTable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxTable.MaxLength = 2;
            this.textBoxTable.Name = "textBoxTable";
            this.textBoxTable.Size = new System.Drawing.Size(239, 36);
            this.textBoxTable.TabIndex = 25;
            this.textBoxTable.TextChanged += new System.EventHandler(this.textBoxTable_TextChanged);
            this.textBoxTable.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxTable_KeyPress);
            // 
            // labelPlaceCount
            // 
            this.labelPlaceCount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPlaceCount.AutoSize = true;
            this.labelPlaceCount.BackColor = System.Drawing.Color.Transparent;
            this.labelPlaceCount.Font = new System.Drawing.Font("Verdana", 14F);
            this.labelPlaceCount.ForeColor = System.Drawing.Color.White;
            this.labelPlaceCount.Location = new System.Drawing.Point(261, 535);
            this.labelPlaceCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPlaceCount.Name = "labelPlaceCount";
            this.labelPlaceCount.Size = new System.Drawing.Size(233, 29);
            this.labelPlaceCount.TabIndex = 23;
            this.labelPlaceCount.Text = "Количество мест:";
            // 
            // labelTable
            // 
            this.labelTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTable.AutoSize = true;
            this.labelTable.BackColor = System.Drawing.Color.Transparent;
            this.labelTable.Font = new System.Drawing.Font("Verdana", 14F);
            this.labelTable.ForeColor = System.Drawing.Color.White;
            this.labelTable.Location = new System.Drawing.Point(13, 535);
            this.labelTable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(112, 29);
            this.labelTable.TabIndex = 24;
            this.labelTable.Text = "Столик:";
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
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1067, 505);
            this.dataGridView1.TabIndex = 22;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonUpdate.Enabled = false;
            this.buttonUpdate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUpdate.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonUpdate.ForeColor = System.Drawing.Color.White;
            this.buttonUpdate.Location = new System.Drawing.Point(732, 660);
            this.buttonUpdate.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(153, 62);
            this.buttonUpdate.TabIndex = 27;
            this.buttonUpdate.Text = "Изменить";
            this.buttonUpdate.UseVisualStyleBackColor = false;
            this.buttonUpdate.Visible = false;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonNew.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNew.Font = new System.Drawing.Font("Verdana", 14F);
            this.buttonNew.ForeColor = System.Drawing.Color.White;
            this.buttonNew.Location = new System.Drawing.Point(896, 660);
            this.buttonNew.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(153, 62);
            this.buttonNew.TabIndex = 27;
            this.buttonNew.Text = "Добавить";
            this.buttonNew.UseVisualStyleBackColor = false;
            this.buttonNew.Visible = false;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.AutoSize = true;
            this.labelStatus.BackColor = System.Drawing.Color.Transparent;
            this.labelStatus.Font = new System.Drawing.Font("Verdana", 14F);
            this.labelStatus.ForeColor = System.Drawing.Color.White;
            this.labelStatus.Location = new System.Drawing.Point(521, 535);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(106, 29);
            this.labelStatus.TabIndex = 23;
            this.labelStatus.Text = "Cтатус:";
            // 
            // comboBoxStatus
            // 
            this.comboBoxStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxStatus.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxStatus.ForeColor = System.Drawing.Color.White;
            this.comboBoxStatus.FormattingEnabled = true;
            this.comboBoxStatus.Location = new System.Drawing.Point(527, 567);
            this.comboBoxStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxStatus.Name = "comboBoxStatus";
            this.comboBoxStatus.Size = new System.Drawing.Size(252, 37);
            this.comboBoxStatus.TabIndex = 29;
            this.comboBoxStatus.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // buttonClearFilters
            // 
            this.buttonClearFilters.BackColor = System.Drawing.Color.Transparent;
            this.buttonClearFilters.BackgroundImage = global::Restaurant.Properties.Resources.exit;
            this.buttonClearFilters.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClearFilters.FlatAppearance.BorderSize = 0;
            this.buttonClearFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearFilters.ForeColor = System.Drawing.Color.Transparent;
            this.buttonClearFilters.Location = new System.Drawing.Point(788, 570);
            this.buttonClearFilters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonClearFilters.Name = "buttonClearFilters";
            this.buttonClearFilters.Size = new System.Drawing.Size(40, 37);
            this.buttonClearFilters.TabIndex = 56;
            this.buttonClearFilters.UseVisualStyleBackColor = false;
            this.buttonClearFilters.Click += new System.EventHandler(this.buttonClearFilters_Click);
            // 
            // Tables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Restaurant.Properties.Resources.fonMain;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1067, 738);
            this.ControlBox = false;
            this.Controls.Add(this.buttonClearFilters);
            this.Controls.Add(this.comboBoxStatus);
            this.Controls.Add(this.comboBoxPlaceCount);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.textBoxTable);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelPlaceCount);
            this.Controls.Add(this.labelTable);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Tables";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Столы";
            this.Load += new System.EventHandler(this.Tables_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxPlaceCount;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.TextBox textBoxTable;
        private System.Windows.Forms.Label labelPlaceCount;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ComboBox comboBoxStatus;
        private System.Windows.Forms.Button buttonClearFilters;
    }
}