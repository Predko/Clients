namespace Clients
{
    partial class Clients
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
            this.tabControlClients = new System.Windows.Forms.TabControl();
            this.tabPageClients = new System.Windows.Forms.TabPage();
            this.labelListContractsTotals = new System.Windows.Forms.Label();
            this.labelListContracts = new System.Windows.Forms.Label();
            this.buttonDeleteContract = new System.Windows.Forms.Button();
            this.buttonDeleteClient = new System.Windows.Forms.Button();
            this.buttonEditClient = new System.Windows.Forms.Button();
            this.buttonNewClient = new System.Windows.Forms.Button();
            this.buttonEditContract = new System.Windows.Forms.Button();
            this.buttonNewContract = new System.Windows.Forms.Button();
            this.labelClient = new System.Windows.Forms.Label();
            this.labelContract = new System.Windows.Forms.Label();
            this.labelContracts = new System.Windows.Forms.Label();
            this.listBoxContracts = new System.Windows.Forms.ListBox();
            this.comboBoxClients = new System.Windows.Forms.ComboBox();
            this.tabPageContractEdit = new System.Windows.Forms.TabPage();
            this.dataGridViewContract = new System.Windows.Forms.DataGridView();
            this.columnNumb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNameService = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnServiceNumb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnServiceSumm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSubdivision = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlClients.SuspendLayout();
            this.tabPageClients.SuspendLayout();
            this.tabPageContractEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContract)).BeginInit();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlClients
            // 
            this.tabControlClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlClients.Controls.Add(this.tabPageClients);
            this.tabControlClients.Controls.Add(this.tabPageContractEdit);
            this.tabControlClients.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControlClients.Location = new System.Drawing.Point(12, 50);
            this.tabControlClients.Name = "tabControlClients";
            this.tabControlClients.SelectedIndex = 0;
            this.tabControlClients.Size = new System.Drawing.Size(1263, 575);
            this.tabControlClients.TabIndex = 2;
            // 
            // tabPageClients
            // 
            this.tabPageClients.Controls.Add(this.labelListContractsTotals);
            this.tabPageClients.Controls.Add(this.labelListContracts);
            this.tabPageClients.Controls.Add(this.buttonDeleteContract);
            this.tabPageClients.Controls.Add(this.buttonDeleteClient);
            this.tabPageClients.Controls.Add(this.buttonEditClient);
            this.tabPageClients.Controls.Add(this.buttonNewClient);
            this.tabPageClients.Controls.Add(this.buttonEditContract);
            this.tabPageClients.Controls.Add(this.buttonNewContract);
            this.tabPageClients.Controls.Add(this.labelClient);
            this.tabPageClients.Controls.Add(this.labelContract);
            this.tabPageClients.Controls.Add(this.labelContracts);
            this.tabPageClients.Controls.Add(this.listBoxContracts);
            this.tabPageClients.Controls.Add(this.comboBoxClients);
            this.tabPageClients.Location = new System.Drawing.Point(4, 32);
            this.tabPageClients.Name = "tabPageClients";
            this.tabPageClients.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageClients.Size = new System.Drawing.Size(1255, 539);
            this.tabPageClients.TabIndex = 0;
            this.tabPageClients.Text = "Клиенты";
            this.tabPageClients.UseVisualStyleBackColor = true;
            // 
            // labelListContractsTotals
            // 
            this.labelListContractsTotals.AutoSize = true;
            this.labelListContractsTotals.Location = new System.Drawing.Point(6, 496);
            this.labelListContractsTotals.Name = "labelListContractsTotals";
            this.labelListContractsTotals.Size = new System.Drawing.Size(307, 23);
            this.labelListContractsTotals.TabIndex = 13;
            this.labelListContractsTotals.Text = "Договоров:        на сумму:";
            // 
            // labelListContracts
            // 
            this.labelListContracts.AutoSize = true;
            this.labelListContracts.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelListContracts.Location = new System.Drawing.Point(6, 137);
            this.labelListContracts.Name = "labelListContracts";
            this.labelListContracts.Size = new System.Drawing.Size(351, 23);
            this.labelListContracts.TabIndex = 12;
            this.labelListContracts.Text = "  Дата      Номер         Сумма";
            // 
            // buttonDeleteContract
            // 
            this.buttonDeleteContract.Location = new System.Drawing.Point(863, 87);
            this.buttonDeleteContract.Name = "buttonDeleteContract";
            this.buttonDeleteContract.Size = new System.Drawing.Size(182, 47);
            this.buttonDeleteContract.TabIndex = 11;
            this.buttonDeleteContract.Text = "Удалить";
            this.buttonDeleteContract.UseVisualStyleBackColor = true;
            // 
            // buttonDeleteClient
            // 
            this.buttonDeleteClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteClient.Location = new System.Drawing.Point(1041, 2);
            this.buttonDeleteClient.Name = "buttonDeleteClient";
            this.buttonDeleteClient.Size = new System.Drawing.Size(182, 38);
            this.buttonDeleteClient.TabIndex = 10;
            this.buttonDeleteClient.Text = "Удалить";
            this.buttonDeleteClient.UseVisualStyleBackColor = true;
            // 
            // buttonEditClient
            // 
            this.buttonEditClient.Location = new System.Drawing.Point(325, 2);
            this.buttonEditClient.Name = "buttonEditClient";
            this.buttonEditClient.Size = new System.Drawing.Size(182, 38);
            this.buttonEditClient.TabIndex = 9;
            this.buttonEditClient.Text = "Редактировать";
            this.buttonEditClient.UseVisualStyleBackColor = true;
            // 
            // buttonNewClient
            // 
            this.buttonNewClient.Location = new System.Drawing.Point(141, 2);
            this.buttonNewClient.Name = "buttonNewClient";
            this.buttonNewClient.Size = new System.Drawing.Size(166, 38);
            this.buttonNewClient.TabIndex = 8;
            this.buttonNewClient.Text = "Добавить";
            this.buttonNewClient.UseVisualStyleBackColor = true;
            // 
            // buttonEditContract
            // 
            this.buttonEditContract.Location = new System.Drawing.Point(666, 87);
            this.buttonEditContract.Name = "buttonEditContract";
            this.buttonEditContract.Size = new System.Drawing.Size(191, 47);
            this.buttonEditContract.TabIndex = 7;
            this.buttonEditContract.Text = "Редактировать";
            this.buttonEditContract.UseVisualStyleBackColor = true;
            // 
            // buttonNewContract
            // 
            this.buttonNewContract.Location = new System.Drawing.Point(298, 87);
            this.buttonNewContract.Name = "buttonNewContract";
            this.buttonNewContract.Size = new System.Drawing.Size(142, 47);
            this.buttonNewContract.TabIndex = 6;
            this.buttonNewContract.Text = "Новый";
            this.buttonNewContract.UseVisualStyleBackColor = true;
            // 
            // labelClient
            // 
            this.labelClient.AutoSize = true;
            this.labelClient.Location = new System.Drawing.Point(6, 13);
            this.labelClient.Name = "labelClient";
            this.labelClient.Size = new System.Drawing.Size(76, 23);
            this.labelClient.TabIndex = 5;
            this.labelClient.Text = "Клиент";
            // 
            // labelContract
            // 
            this.labelContract.AutoSize = true;
            this.labelContract.Location = new System.Drawing.Point(481, 99);
            this.labelContract.Name = "labelContract";
            this.labelContract.Size = new System.Drawing.Size(109, 23);
            this.labelContract.TabIndex = 4;
            this.labelContract.Text = "Договор №";
            // 
            // labelContracts
            // 
            this.labelContracts.AutoSize = true;
            this.labelContracts.Location = new System.Drawing.Point(6, 99);
            this.labelContracts.Name = "labelContracts";
            this.labelContracts.Size = new System.Drawing.Size(98, 23);
            this.labelContracts.TabIndex = 3;
            this.labelContracts.Text = "Договоры";
            // 
            // listBoxContracts
            // 
            this.listBoxContracts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxContracts.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxContracts.FormattingEnabled = true;
            this.listBoxContracts.ItemHeight = 23;
            this.listBoxContracts.Location = new System.Drawing.Point(6, 163);
            this.listBoxContracts.Name = "listBoxContracts";
            this.listBoxContracts.Size = new System.Drawing.Size(454, 326);
            this.listBoxContracts.TabIndex = 1;
            this.listBoxContracts.SelectedIndexChanged += new System.EventHandler(this.listBoxContracts_SelectedIndexChanged);
            // 
            // comboBoxClients
            // 
            this.comboBoxClients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxClients.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxClients.Location = new System.Drawing.Point(6, 46);
            this.comboBoxClients.Name = "comboBoxClients";
            this.comboBoxClients.Size = new System.Drawing.Size(1217, 31);
            this.comboBoxClients.TabIndex = 0;
            this.comboBoxClients.SelectedIndexChanged += new System.EventHandler(this.ComboBoxClients_SelectedIndexChanged);
            // 
            // tabPageContractEdit
            // 
            this.tabPageContractEdit.Controls.Add(this.dataGridViewContract);
            this.tabPageContractEdit.Controls.Add(this.comboBox1);
            this.tabPageContractEdit.Location = new System.Drawing.Point(4, 32);
            this.tabPageContractEdit.Name = "tabPageContractEdit";
            this.tabPageContractEdit.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageContractEdit.Size = new System.Drawing.Size(1255, 539);
            this.tabPageContractEdit.TabIndex = 1;
            this.tabPageContractEdit.Text = "Договор";
            this.tabPageContractEdit.UseVisualStyleBackColor = true;
            // 
            // dataGridViewContract
            // 
            this.dataGridViewContract.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewContract.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewContract.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNumb,
            this.columnNameService,
            this.columnServiceNumb,
            this.columnServiceSumm,
            this.columnSubdivision});
            this.dataGridViewContract.Location = new System.Drawing.Point(6, 71);
            this.dataGridViewContract.Name = "dataGridViewContract";
            this.dataGridViewContract.RowTemplate.Height = 28;
            this.dataGridViewContract.Size = new System.Drawing.Size(1243, 462);
            this.dataGridViewContract.TabIndex = 1;
            // 
            // ColumnNumb
            // 
            this.columnNumb.Frozen = true;
            this.columnNumb.HeaderText = "№";
            this.columnNumb.Name = "ColumnNumb";
            this.columnNumb.ReadOnly = true;
            this.columnNumb.Width = 57;
            // 
            // ColumnNameService
            // 
            this.columnNameService.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnNameService.HeaderText = "Оказанные услуги";
            this.columnNameService.Name = "ColumnNameService";
            this.columnNameService.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnNameService.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ColumnServiceNumb
            // 
            this.columnServiceNumb.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.columnServiceNumb.HeaderText = "№ услуги";
            this.columnServiceNumb.Name = "ColumnServiceNumb";
            this.columnServiceNumb.Width = 80;
            // 
            // ColumnServiceSumm
            // 
            this.columnServiceSumm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.columnServiceSumm.HeaderText = "Сумма";
            this.columnServiceSumm.Name = "ColumnServiceSumm";
            // 
            // ColumnSubdivision
            // 
            this.columnSubdivision.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.columnSubdivision.HeaderText = "Подразделение (кабинет)";
            this.columnSubdivision.Name = "ColumnSubdivision";
            this.columnSubdivision.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnSubdivision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnSubdivision.Width = 150;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Договор №",
            "Акт выполненных работ №"});
            this.comboBox1.Location = new System.Drawing.Point(368, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(418, 31);
            this.comboBox1.TabIndex = 0;
            // 
            // menuStripMain
            // 
            this.menuStripMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFile});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1364, 33);
            this.menuStripMain.TabIndex = 3;
            this.menuStripMain.Text = "Главное меню";
            // 
            // toolStripMenuItemFile
            // 
            this.toolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSave,
            this.toolStripMenuItemLoad,
            this.toolStripMenuItemExit});
            this.toolStripMenuItemFile.Name = "toolStripMenuItemFile";
            this.toolStripMenuItemFile.Size = new System.Drawing.Size(65, 29);
            this.toolStripMenuItemFile.Text = "Файл";
            // 
            // ToolStripMenuItemSave
            // 
            this.toolStripMenuItemSave.Name = "ToolStripMenuItemSave";
            this.toolStripMenuItemSave.Size = new System.Drawing.Size(183, 30);
            this.toolStripMenuItemSave.Text = "Сохранить";
            // 
            // ToolStripMenuItemLoad
            // 
            this.toolStripMenuItemLoad.Name = "ToolStripMenuItemLoad";
            this.toolStripMenuItemLoad.Size = new System.Drawing.Size(183, 30);
            this.toolStripMenuItemLoad.Text = "Загрузить";
            this.toolStripMenuItemLoad.Click += new System.EventHandler(this.ToolStripMenuItemLoad_Click);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(183, 30);
            this.toolStripMenuItemExit.Text = "Выход";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.ToolStripMenuItemExit_Click);
            // 
            // Clients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1364, 637);
            this.Controls.Add(this.tabControlClients);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "Clients";
            this.Text = "Клиенты";
            this.tabControlClients.ResumeLayout(false);
            this.tabPageClients.ResumeLayout(false);
            this.tabPageClients.PerformLayout();
            this.tabPageContractEdit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContract)).EndInit();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlClients;
        private System.Windows.Forms.TabPage tabPageClients;
        private System.Windows.Forms.TabPage tabPageContractEdit;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoad;
        private System.Windows.Forms.Button buttonDeleteContract;
        private System.Windows.Forms.Button buttonDeleteClient;
        private System.Windows.Forms.Button buttonEditClient;
        private System.Windows.Forms.Button buttonNewClient;
        private System.Windows.Forms.Button buttonEditContract;
        private System.Windows.Forms.Button buttonNewContract;
        private System.Windows.Forms.Label labelClient;
        private System.Windows.Forms.Label labelContract;
        private System.Windows.Forms.Label labelContracts;
        private System.Windows.Forms.ListBox listBoxContracts;
        private System.Windows.Forms.ComboBox comboBoxClients;
        private System.Windows.Forms.Label labelListContracts;
        private System.Windows.Forms.Label labelListContractsTotals;
        private System.Windows.Forms.DataGridView dataGridViewContract;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNumb;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnNameService;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnServiceNumb;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnServiceSumm;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnSubdivision;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

