﻿namespace Clients
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControlClients = new System.Windows.Forms.TabControl();
            this.tabPageClients = new System.Windows.Forms.TabPage();
            this.labelFileName = new System.Windows.Forms.Label();
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
            this.buttonLoadContractFrom_xls = new System.Windows.Forms.Button();
            this.labelClientName = new System.Windows.Forms.Label();
            this.dataGridViewContract = new System.Windows.Forms.DataGridView();
            this.ColumnNameService = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnNameDevice = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnAddInfo = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnSubdivision = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnServiceNumb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnServiceSumm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIdService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBoxTypeContract = new System.Windows.Forms.ComboBox();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSaveXml = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadXmlAccess = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRead_xls = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemReadXlsOLEDB = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.PanelTabs = new System.Windows.Forms.Panel();
            this.richTextBoxDebug = new System.Windows.Forms.RichTextBox();
            this.tabControlClients.SuspendLayout();
            this.tabPageClients.SuspendLayout();
            this.tabPageContractEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContract)).BeginInit();
            this.menuStripMain.SuspendLayout();
            this.PanelTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlClients
            // 
            this.tabControlClients.Controls.Add(this.tabPageClients);
            this.tabControlClients.Controls.Add(this.tabPageContractEdit);
            this.tabControlClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlClients.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControlClients.Location = new System.Drawing.Point(0, 0);
            this.tabControlClients.Name = "tabControlClients";
            this.tabControlClients.SelectedIndex = 0;
            this.tabControlClients.Size = new System.Drawing.Size(1512, 709);
            this.tabControlClients.TabIndex = 2;
            // 
            // tabPageClients
            // 
            this.tabPageClients.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageClients.Controls.Add(this.labelFileName);
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
            this.tabPageClients.Margin = new System.Windows.Forms.Padding(5);
            this.tabPageClients.Name = "tabPageClients";
            this.tabPageClients.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageClients.Size = new System.Drawing.Size(1504, 673);
            this.tabPageClients.TabIndex = 0;
            this.tabPageClients.Text = "Клиенты";
            this.tabPageClients.UseVisualStyleBackColor = true;
            // 
            // labelFileName
            // 
            this.labelFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(4, 627);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(186, 23);
            this.labelFileName.TabIndex = 14;
            this.labelFileName.Text = "Файл отсутствует";
            // 
            // labelListContractsTotals
            // 
            this.labelListContractsTotals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelListContractsTotals.AutoSize = true;
            this.labelListContractsTotals.Location = new System.Drawing.Point(4, 588);
            this.labelListContractsTotals.Name = "labelListContractsTotals";
            this.labelListContractsTotals.Size = new System.Drawing.Size(307, 23);
            this.labelListContractsTotals.TabIndex = 13;
            this.labelListContractsTotals.Text = "Договоров:        на сумму:";
            // 
            // labelListContracts
            // 
            this.labelListContracts.AutoSize = true;
            this.labelListContracts.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelListContracts.Location = new System.Drawing.Point(8, 139);
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
            this.buttonDeleteClient.Location = new System.Drawing.Point(1637, 4);
            this.buttonDeleteClient.Name = "buttonDeleteClient";
            this.buttonDeleteClient.Size = new System.Drawing.Size(182, 38);
            this.buttonDeleteClient.TabIndex = 10;
            this.buttonDeleteClient.Text = "Удалить";
            this.buttonDeleteClient.UseVisualStyleBackColor = true;
            // 
            // buttonEditClient
            // 
            this.buttonEditClient.Location = new System.Drawing.Point(317, 7);
            this.buttonEditClient.Name = "buttonEditClient";
            this.buttonEditClient.Size = new System.Drawing.Size(182, 38);
            this.buttonEditClient.TabIndex = 9;
            this.buttonEditClient.Text = "Редактировать";
            this.buttonEditClient.UseVisualStyleBackColor = true;
            // 
            // buttonNewClient
            // 
            this.buttonNewClient.Location = new System.Drawing.Point(134, 7);
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
            this.labelClient.Location = new System.Drawing.Point(8, 15);
            this.labelClient.Name = "labelClient";
            this.labelClient.Size = new System.Drawing.Size(76, 23);
            this.labelClient.TabIndex = 5;
            this.labelClient.Text = "Клиент";
            // 
            // labelContract
            // 
            this.labelContract.AutoSize = true;
            this.labelContract.Location = new System.Drawing.Point(483, 101);
            this.labelContract.Name = "labelContract";
            this.labelContract.Size = new System.Drawing.Size(109, 23);
            this.labelContract.TabIndex = 4;
            this.labelContract.Text = "Договор №";
            // 
            // labelContracts
            // 
            this.labelContracts.AutoSize = true;
            this.labelContracts.Location = new System.Drawing.Point(8, 101);
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
            this.listBoxContracts.Location = new System.Drawing.Point(8, 165);
            this.listBoxContracts.Name = "listBoxContracts";
            this.listBoxContracts.Size = new System.Drawing.Size(454, 395);
            this.listBoxContracts.TabIndex = 1;
            // 
            // comboBoxClients
            // 
            this.comboBoxClients.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxClients.Location = new System.Drawing.Point(12, 50);
            this.comboBoxClients.Name = "comboBoxClients";
            this.comboBoxClients.Size = new System.Drawing.Size(1217, 31);
            this.comboBoxClients.TabIndex = 0;
            // 
            // tabPageContractEdit
            // 
            this.tabPageContractEdit.BackColor = System.Drawing.Color.Transparent;
            this.tabPageContractEdit.Controls.Add(this.buttonLoadContractFrom_xls);
            this.tabPageContractEdit.Controls.Add(this.labelClientName);
            this.tabPageContractEdit.Controls.Add(this.dataGridViewContract);
            this.tabPageContractEdit.Controls.Add(this.comboBoxTypeContract);
            this.tabPageContractEdit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPageContractEdit.Location = new System.Drawing.Point(4, 32);
            this.tabPageContractEdit.Name = "tabPageContractEdit";
            this.tabPageContractEdit.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageContractEdit.Size = new System.Drawing.Size(1504, 673);
            this.tabPageContractEdit.TabIndex = 1;
            this.tabPageContractEdit.Text = "Договор";
            // 
            // buttonLoadContractFrom_xls
            // 
            this.buttonLoadContractFrom_xls.AutoSize = true;
            this.buttonLoadContractFrom_xls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonLoadContractFrom_xls.Location = new System.Drawing.Point(6, 43);
            this.buttonLoadContractFrom_xls.Name = "buttonLoadContractFrom_xls";
            this.buttonLoadContractFrom_xls.Size = new System.Drawing.Size(218, 33);
            this.buttonLoadContractFrom_xls.TabIndex = 3;
            this.buttonLoadContractFrom_xls.Text = "Загрузить из файла";
            this.buttonLoadContractFrom_xls.UseVisualStyleBackColor = true;
            this.buttonLoadContractFrom_xls.Click += new System.EventHandler(this.ButtonLoadContractFrom_xls_Click);
            // 
            // labelClientName
            // 
            this.labelClientName.AutoSize = true;
            this.labelClientName.Location = new System.Drawing.Point(7, 7);
            this.labelClientName.Name = "labelClientName";
            this.labelClientName.Size = new System.Drawing.Size(186, 23);
            this.labelClientName.TabIndex = 2;
            this.labelClientName.Text = "Название клиента";
            // 
            // dataGridViewContract
            // 
            this.dataGridViewContract.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewContract.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewContract.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNameService,
            this.ColumnNameDevice,
            this.ColumnAddInfo,
            this.ColumnSubdivision,
            this.ColumnServiceNumb,
            this.ColumnServiceSumm,
            this.ColumnIdService});
            this.dataGridViewContract.Location = new System.Drawing.Point(6, 157);
            this.dataGridViewContract.Name = "dataGridViewContract";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewContract.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewContract.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewContract.RowTemplate.Height = 28;
            this.dataGridViewContract.Size = new System.Drawing.Size(1492, 516);
            this.dataGridViewContract.StandardTab = true;
            this.dataGridViewContract.TabIndex = 1;
            // 
            // ColumnNameService
            // 
            this.ColumnNameService.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnNameService.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.ColumnNameService.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColumnNameService.HeaderText = "Оказанные услуги";
            this.ColumnNameService.Name = "ColumnNameService";
            this.ColumnNameService.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnNameService.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ColumnNameDevice
            // 
            this.ColumnNameDevice.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.ColumnNameDevice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColumnNameDevice.HeaderText = "Имя устройства";
            this.ColumnNameDevice.Name = "ColumnNameDevice";
            this.ColumnNameDevice.Width = 200;
            // 
            // ColumnAddInfo
            // 
            this.ColumnAddInfo.HeaderText = "Доп. информация";
            this.ColumnAddInfo.Name = "ColumnAddInfo";
            this.ColumnAddInfo.Width = 200;
            // 
            // ColumnSubdivision
            // 
            this.ColumnSubdivision.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnSubdivision.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.ColumnSubdivision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColumnSubdivision.HeaderText = "Подразделение (кабинет)";
            this.ColumnSubdivision.Name = "ColumnSubdivision";
            this.ColumnSubdivision.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnSubdivision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColumnSubdivision.Width = 200;
            // 
            // ColumnServiceNumb
            // 
            this.ColumnServiceNumb.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnServiceNumb.HeaderText = "№ услуги";
            this.ColumnServiceNumb.Name = "ColumnServiceNumb";
            this.ColumnServiceNumb.Width = 80;
            // 
            // ColumnServiceSumm
            // 
            this.ColumnServiceSumm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnServiceSumm.HeaderText = "Сумма";
            this.ColumnServiceSumm.Name = "ColumnServiceSumm";
            // 
            // ColumnIdService
            // 
            this.ColumnIdService.HeaderText = "Id услуги";
            this.ColumnIdService.Name = "ColumnIdService";
            this.ColumnIdService.Visible = false;
            // 
            // comboBoxTypeContract
            // 
            this.comboBoxTypeContract.FormattingEnabled = true;
            this.comboBoxTypeContract.Items.AddRange(new object[] {
            "Договор №",
            "Акт выполненных работ №"});
            this.comboBoxTypeContract.Location = new System.Drawing.Point(367, 114);
            this.comboBoxTypeContract.Name = "comboBoxTypeContract";
            this.comboBoxTypeContract.Size = new System.Drawing.Size(418, 31);
            this.comboBoxTypeContract.TabIndex = 0;
            // 
            // menuStripMain
            // 
            this.menuStripMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFile});
            this.menuStripMain.Location = new System.Drawing.Point(5, 5);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1900, 33);
            this.menuStripMain.TabIndex = 3;
            this.menuStripMain.Text = "Главное меню";
            // 
            // toolStripMenuItemFile
            // 
            this.toolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSaveXml,
            this.toolStripMenuItemLoad,
            this.toolStripMenuItemLoadXmlAccess,
            this.toolStripMenuItemRead_xls,
            this.toolStripMenuItemReadXlsOLEDB,
            this.toolStripMenuItemExit});
            this.toolStripMenuItemFile.Name = "toolStripMenuItemFile";
            this.toolStripMenuItemFile.Size = new System.Drawing.Size(65, 29);
            this.toolStripMenuItemFile.Text = "Файл";
            // 
            // toolStripMenuItemSaveXml
            // 
            this.toolStripMenuItemSaveXml.Name = "toolStripMenuItemSaveXml";
            this.toolStripMenuItemSaveXml.Size = new System.Drawing.Size(295, 30);
            this.toolStripMenuItemSaveXml.Text = "Сохранить xml";
            // 
            // toolStripMenuItemLoad
            // 
            this.toolStripMenuItemLoad.Name = "toolStripMenuItemLoad";
            this.toolStripMenuItemLoad.Size = new System.Drawing.Size(295, 30);
            this.toolStripMenuItemLoad.Text = "Загрузить xml(мой)";
            // 
            // toolStripMenuItemLoadXmlAccess
            // 
            this.toolStripMenuItemLoadXmlAccess.Name = "toolStripMenuItemLoadXmlAccess";
            this.toolStripMenuItemLoadXmlAccess.Size = new System.Drawing.Size(295, 30);
            this.toolStripMenuItemLoadXmlAccess.Text = "Загрузить xml(Access)";
            // 
            // toolStripMenuItemRead_xls
            // 
            this.toolStripMenuItemRead_xls.Name = "toolStripMenuItemRead_xls";
            this.toolStripMenuItemRead_xls.Size = new System.Drawing.Size(295, 30);
            this.toolStripMenuItemRead_xls.Text = "Читать .xls через Interop";
            // 
            // toolStripMenuItemReadXlsOLEDB
            // 
            this.toolStripMenuItemReadXlsOLEDB.Name = "toolStripMenuItemReadXlsOLEDB";
            this.toolStripMenuItemReadXlsOLEDB.Size = new System.Drawing.Size(295, 30);
            this.toolStripMenuItemReadXlsOLEDB.Text = "Читать .xls через OLEDB";
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(295, 30);
            this.toolStripMenuItemExit.Text = "Выход";
            // 
            // PanelTabs
            // 
            this.PanelTabs.Controls.Add(this.tabControlClients);
            this.PanelTabs.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelTabs.Location = new System.Drawing.Point(5, 38);
            this.PanelTabs.Name = "PanelTabs";
            this.PanelTabs.Size = new System.Drawing.Size(1512, 709);
            this.PanelTabs.TabIndex = 4;
            // 
            // richTextBoxDebug
            // 
            this.richTextBoxDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxDebug.Location = new System.Drawing.Point(1517, 38);
            this.richTextBoxDebug.Name = "richTextBoxDebug";
            this.richTextBoxDebug.Size = new System.Drawing.Size(388, 709);
            this.richTextBoxDebug.TabIndex = 5;
            this.richTextBoxDebug.Text = "";
            // 
            // Clients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1910, 752);
            this.Controls.Add(this.richTextBoxDebug);
            this.Controls.Add(this.PanelTabs);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "Clients";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Клиенты";
            this.tabControlClients.ResumeLayout(false);
            this.tabPageClients.ResumeLayout(false);
            this.tabPageClients.PerformLayout();
            this.tabPageContractEdit.ResumeLayout(false);
            this.tabPageContractEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContract)).EndInit();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.PanelTabs.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveXml;
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
        private System.Windows.Forms.ComboBox comboBoxTypeContract;
        private System.Windows.Forms.Panel PanelTabs;
        private System.Windows.Forms.RichTextBox richTextBoxDebug;
        private System.Windows.Forms.Label labelClientName;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRead_xls;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemReadXlsOLEDB;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadXmlAccess;
        private System.Windows.Forms.Button buttonLoadContractFrom_xls;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnNameService;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnNameDevice;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnAddInfo;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnSubdivision;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnServiceNumb;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnServiceSumm;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIdService;
    }
}

