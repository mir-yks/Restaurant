namespace Restaurant
{
    partial class TablesInsert
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
            this.buttonWrite = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.labelNumberTable = new System.Windows.Forms.Label();
            this.textBoxNumberTables = new System.Windows.Forms.TextBox();
            this.textBoxPlaceCount = new System.Windows.Forms.TextBox();
            this.labelPlaceCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonWrite
            // 
            this.buttonWrite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonWrite.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonWrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonWrite.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonWrite.ForeColor = System.Drawing.Color.White;
            this.buttonWrite.Location = new System.Drawing.Point(173, 138);
            this.buttonWrite.Name = "buttonWrite";
            this.buttonWrite.Size = new System.Drawing.Size(115, 50);
            this.buttonWrite.TabIndex = 19;
            this.buttonWrite.Text = "Записать";
            this.buttonWrite.UseVisualStyleBackColor = false;
            this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonBack.ForeColor = System.Drawing.Color.White;
            this.buttonBack.Location = new System.Drawing.Point(12, 138);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(110, 50);
            this.buttonBack.TabIndex = 18;
            this.buttonBack.Text = "Назад";
            this.buttonBack.UseVisualStyleBackColor = false;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // labelNumberTable
            // 
            this.labelNumberTable.AutoSize = true;
            this.labelNumberTable.BackColor = System.Drawing.Color.Transparent;
            this.labelNumberTable.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberTable.ForeColor = System.Drawing.Color.White;
            this.labelNumberTable.Location = new System.Drawing.Point(35, 3);
            this.labelNumberTable.Name = "labelNumberTable";
            this.labelNumberTable.Size = new System.Drawing.Size(165, 23);
            this.labelNumberTable.TabIndex = 17;
            this.labelNumberTable.Text = "Номер столика:";
            // 
            // textBoxNumberTables
            // 
            this.textBoxNumberTables.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.textBoxNumberTables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxNumberTables.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxNumberTables.ForeColor = System.Drawing.Color.White;
            this.textBoxNumberTables.Location = new System.Drawing.Point(39, 29);
            this.textBoxNumberTables.MaxLength = 3;
            this.textBoxNumberTables.Name = "textBoxNumberTables";
            this.textBoxNumberTables.Size = new System.Drawing.Size(219, 31);
            this.textBoxNumberTables.TabIndex = 16;
            this.textBoxNumberTables.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // textBoxPlaceCount
            // 
            this.textBoxPlaceCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.textBoxPlaceCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPlaceCount.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPlaceCount.ForeColor = System.Drawing.Color.White;
            this.textBoxPlaceCount.Location = new System.Drawing.Point(39, 92);
            this.textBoxPlaceCount.MaxLength = 2;
            this.textBoxPlaceCount.Name = "textBoxPlaceCount";
            this.textBoxPlaceCount.Size = new System.Drawing.Size(219, 31);
            this.textBoxPlaceCount.TabIndex = 16;
            this.textBoxPlaceCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // labelPlaceCount
            // 
            this.labelPlaceCount.AutoSize = true;
            this.labelPlaceCount.BackColor = System.Drawing.Color.Transparent;
            this.labelPlaceCount.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPlaceCount.ForeColor = System.Drawing.Color.White;
            this.labelPlaceCount.Location = new System.Drawing.Point(35, 66);
            this.labelPlaceCount.Name = "labelPlaceCount";
            this.labelPlaceCount.Size = new System.Drawing.Size(183, 23);
            this.labelPlaceCount.TabIndex = 17;
            this.labelPlaceCount.Text = "Количество мест:";
            // 
            // TablesInsert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Restaurant.Properties.Resources.fonMain;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.ControlBox = false;
            this.Controls.Add(this.buttonWrite);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.labelPlaceCount);
            this.Controls.Add(this.labelNumberTable);
            this.Controls.Add(this.textBoxPlaceCount);
            this.Controls.Add(this.textBoxNumberTables);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TablesInsert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование столика";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonWrite;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label labelNumberTable;
        private System.Windows.Forms.TextBox textBoxNumberTables;
        private System.Windows.Forms.TextBox textBoxPlaceCount;
        private System.Windows.Forms.Label labelPlaceCount;
    }
}