namespace Clients
{
    partial class FormShowDataTable
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
            this.dataGridViewFile_xls = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFile_xls)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewFile_xls
            // 
            this.dataGridViewFile_xls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFile_xls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFile_xls.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewFile_xls.Name = "dataGridViewFile_xls";
            this.dataGridViewFile_xls.RowTemplate.Height = 28;
            this.dataGridViewFile_xls.Size = new System.Drawing.Size(1741, 662);
            this.dataGridViewFile_xls.TabIndex = 0;
            // 
            // FormShowDataTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1741, 662);
            this.Controls.Add(this.dataGridViewFile_xls);
            this.Name = "FormShowDataTable";
            this.Text = "Файл xls";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFile_xls)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridViewFile_xls;
    }
}