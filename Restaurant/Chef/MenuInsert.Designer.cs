namespace Restaurant
{
    partial class MenuInsert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuInsert));
            this.textBoxPrice = new System.Windows.Forms.TextBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonWrite = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.labelOffers = new System.Windows.Forms.Label();
            this.labelCategory = new System.Windows.Forms.Label();
            this.labelPrice = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.comboBoxOffers = new System.Windows.Forms.ComboBox();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.buttonImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxPrice
            // 
            this.textBoxPrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.textBoxPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPrice.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPrice.ForeColor = System.Drawing.Color.White;
            this.textBoxPrice.Location = new System.Drawing.Point(21, 208);
            this.textBoxPrice.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPrice.MaxLength = 5;
            this.textBoxPrice.Name = "textBoxPrice";
            this.textBoxPrice.Size = new System.Drawing.Size(302, 36);
            this.textBoxPrice.TabIndex = 46;
            this.textBoxPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPrice_KeyPress);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.textBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDescription.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxDescription.ForeColor = System.Drawing.Color.White;
            this.textBoxDescription.Location = new System.Drawing.Point(21, 121);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(302, 36);
            this.textBoxDescription.TabIndex = 47;
            this.textBoxDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDescription_KeyPress);
            // 
            // textBoxName
            // 
            this.textBoxName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.textBoxName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxName.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxName.ForeColor = System.Drawing.Color.White;
            this.textBoxName.Location = new System.Drawing.Point(21, 42);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxName.MaxLength = 100;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(302, 36);
            this.textBoxName.TabIndex = 48;
            this.textBoxName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxName_KeyPress);
            // 
            // buttonWrite
            // 
            this.buttonWrite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonWrite.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonWrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonWrite.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonWrite.ForeColor = System.Drawing.Color.White;
            this.buttonWrite.Location = new System.Drawing.Point(543, 363);
            this.buttonWrite.Margin = new System.Windows.Forms.Padding(4);
            this.buttonWrite.Name = "buttonWrite";
            this.buttonWrite.Size = new System.Drawing.Size(153, 62);
            this.buttonWrite.TabIndex = 45;
            this.buttonWrite.Text = "Добавить";
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
            this.buttonBack.Location = new System.Drawing.Point(16, 363);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(147, 62);
            this.buttonBack.TabIndex = 44;
            this.buttonBack.Text = "Назад";
            this.buttonBack.UseVisualStyleBackColor = false;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // labelOffers
            // 
            this.labelOffers.AutoSize = true;
            this.labelOffers.BackColor = System.Drawing.Color.Transparent;
            this.labelOffers.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelOffers.ForeColor = System.Drawing.Color.White;
            this.labelOffers.Location = new System.Drawing.Point(365, 10);
            this.labelOffers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOffers.Name = "labelOffers";
            this.labelOffers.Size = new System.Drawing.Size(98, 29);
            this.labelOffers.TabIndex = 39;
            this.labelOffers.Text = "Акция:";
            // 
            // labelCategory
            // 
            this.labelCategory.AutoSize = true;
            this.labelCategory.BackColor = System.Drawing.Color.Transparent;
            this.labelCategory.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCategory.ForeColor = System.Drawing.Color.White;
            this.labelCategory.Location = new System.Drawing.Point(16, 262);
            this.labelCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(238, 29);
            this.labelCategory.TabIndex = 40;
            this.labelCategory.Text = "Категория блюда:";
            // 
            // labelPrice
            // 
            this.labelPrice.AutoSize = true;
            this.labelPrice.BackColor = System.Drawing.Color.Transparent;
            this.labelPrice.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPrice.ForeColor = System.Drawing.Color.White;
            this.labelPrice.Location = new System.Drawing.Point(16, 176);
            this.labelPrice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(85, 29);
            this.labelPrice.TabIndex = 41;
            this.labelPrice.Text = "Цена:";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.BackColor = System.Drawing.Color.Transparent;
            this.labelDescription.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDescription.ForeColor = System.Drawing.Color.White;
            this.labelDescription.Location = new System.Drawing.Point(16, 89);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(144, 29);
            this.labelDescription.TabIndex = 42;
            this.labelDescription.Text = "Описание:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.BackColor = System.Drawing.Color.Transparent;
            this.labelName.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelName.ForeColor = System.Drawing.Color.White;
            this.labelName.Location = new System.Drawing.Point(16, 10);
            this.labelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(141, 29);
            this.labelName.TabIndex = 43;
            this.labelName.Text = "Название:";
            // 
            // comboBoxOffers
            // 
            this.comboBoxOffers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxOffers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOffers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxOffers.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxOffers.ForeColor = System.Drawing.Color.White;
            this.comboBoxOffers.FormattingEnabled = true;
            this.comboBoxOffers.Location = new System.Drawing.Point(371, 42);
            this.comboBoxOffers.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxOffers.Name = "comboBoxOffers";
            this.comboBoxOffers.Size = new System.Drawing.Size(300, 37);
            this.comboBoxOffers.TabIndex = 37;
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.comboBoxCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxCategory.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxCategory.ForeColor = System.Drawing.Color.White;
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(21, 294);
            this.comboBoxCategory.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(300, 37);
            this.comboBoxCategory.TabIndex = 38;
            this.comboBoxCategory.MaxLength = 50;
            this.comboBoxCategory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxCategory_KeyPress);
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxImage.Location = new System.Drawing.Point(371, 94);
            this.pictureBoxImage.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(300, 192);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 49;
            this.pictureBoxImage.TabStop = false;
            // 
            // buttonImage
            // 
            this.buttonImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.buttonImage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(99)))), ((int)(((byte)(107)))));
            this.buttonImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImage.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonImage.ForeColor = System.Drawing.Color.White;
            this.buttonImage.Location = new System.Drawing.Point(370, 294);
            this.buttonImage.Margin = new System.Windows.Forms.Padding(4);
            this.buttonImage.Name = "buttonImage";
            this.buttonImage.Size = new System.Drawing.Size(301, 37);
            this.buttonImage.TabIndex = 45;
            this.buttonImage.Text = "Выбрать фото";
            this.buttonImage.UseVisualStyleBackColor = false;
            this.buttonImage.Click += new System.EventHandler(this.buttonImage_Click);
            // 
            // MenuInsert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Restaurant.Properties.Resources.fonMain;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(712, 439);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBoxImage);
            this.Controls.Add(this.textBoxPrice);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.buttonImage);
            this.Controls.Add(this.buttonWrite);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.labelOffers);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.comboBoxOffers);
            this.Controls.Add(this.comboBoxCategory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MenuInsert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование блюда";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MenuInsert_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPrice;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonWrite;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label labelOffers;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ComboBox comboBoxOffers;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.Button buttonImage;
    }
}