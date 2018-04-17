namespace PrintGraphics
{
    partial class PrintGraphics
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.GetPrintData = new System.Windows.Forms.Button();
            this.ButtonInfo1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // GetPrintData
            // 
            this.GetPrintData.AutoSize = true;
            this.GetPrintData.Location = new System.Drawing.Point(13, 13);
            this.GetPrintData.Name = "GetPrintData";
            this.GetPrintData.Size = new System.Drawing.Size(204, 30);
            this.GetPrintData.TabIndex = 0;
            this.GetPrintData.Text = "Извлечь данные печати";
            this.GetPrintData.UseVisualStyleBackColor = true;
            this.GetPrintData.Click += new System.EventHandler(this.GetPrintData_Click);
            // 
            // ButtonInfo1
            // 
            this.ButtonInfo1.AutoSize = true;
            this.ButtonInfo1.Location = new System.Drawing.Point(247, 13);
            this.ButtonInfo1.Name = "ButtonInfo1";
            this.ButtonInfo1.Size = new System.Drawing.Size(132, 30);
            this.ButtonInfo1.TabIndex = 1;
            this.ButtonInfo1.Text = "Информация 1";
            this.ButtonInfo1.UseVisualStyleBackColor = true;
            this.ButtonInfo1.Click += new System.EventHandler(this.ButtonInfo1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(682, 15);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(637, 28);
            this.comboBox1.TabIndex = 2;
            // 
            // PrintGraphics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1331, 682);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.ButtonInfo1);
            this.Controls.Add(this.GetPrintData);
            this.Name = "PrintGraphics";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PrintGraphics_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetPrintData;
        private System.Windows.Forms.Button ButtonInfo1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

