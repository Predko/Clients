namespace Clients
{
    partial class DialogBoxClients
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
            this.textBoxClientName = new System.Windows.Forms.TextBox();
            this.labelClientName = new System.Windows.Forms.Label();
            this.labelClientCity = new System.Windows.Forms.Label();
            this.textBoxClientCity = new System.Windows.Forms.TextBox();
            this.textBoxClientAddress = new System.Windows.Forms.TextBox();
            this.labelClientAddress = new System.Windows.Forms.Label();
            this.textBoxClientSettlementAccount = new System.Windows.Forms.TextBox();
            this.labelClientSettlementAccount = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonAddClientCancel = new System.Windows.Forms.Button();
            this.listBoxClients = new System.Windows.Forms.ListBox();
            this.buttonClientPrevious = new System.Windows.Forms.Button();
            this.buttonClientNext = new System.Windows.Forms.Button();
            this.buttonAddNewClient = new System.Windows.Forms.Button();
            this.buttonClientSaveAndExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxClientName
            // 
            this.textBoxClientName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClientName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxClientName.Location = new System.Drawing.Point(12, 42);
            this.textBoxClientName.Name = "textBoxClientName";
            this.textBoxClientName.Size = new System.Drawing.Size(1141, 30);
            this.textBoxClientName.TabIndex = 0;
            this.textBoxClientName.TextChanged += new System.EventHandler(this.TextBoxClientName_TextChanged);
            // 
            // labelClientName
            // 
            this.labelClientName.AutoSize = true;
            this.labelClientName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelClientName.Location = new System.Drawing.Point(14, 14);
            this.labelClientName.Margin = new System.Windows.Forms.Padding(5);
            this.labelClientName.Name = "labelClientName";
            this.labelClientName.Size = new System.Drawing.Size(181, 25);
            this.labelClientName.TabIndex = 1;
            this.labelClientName.Text = "Название клиента";
            // 
            // labelClientCity
            // 
            this.labelClientCity.AutoSize = true;
            this.labelClientCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelClientCity.Location = new System.Drawing.Point(14, 76);
            this.labelClientCity.Margin = new System.Windows.Forms.Padding(5);
            this.labelClientCity.Name = "labelClientCity";
            this.labelClientCity.Size = new System.Drawing.Size(184, 25);
            this.labelClientCity.TabIndex = 2;
            this.labelClientCity.Text = "Населённый пункт";
            // 
            // textBoxClientCity
            // 
            this.textBoxClientCity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClientCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxClientCity.Location = new System.Drawing.Point(12, 104);
            this.textBoxClientCity.Name = "textBoxClientCity";
            this.textBoxClientCity.Size = new System.Drawing.Size(1141, 30);
            this.textBoxClientCity.TabIndex = 3;
            this.textBoxClientCity.TextChanged += new System.EventHandler(this.TextBoxClientCity_TextChanged);
            // 
            // textBoxClientAddress
            // 
            this.textBoxClientAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClientAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxClientAddress.Location = new System.Drawing.Point(12, 166);
            this.textBoxClientAddress.Multiline = true;
            this.textBoxClientAddress.Name = "textBoxClientAddress";
            this.textBoxClientAddress.Size = new System.Drawing.Size(1141, 78);
            this.textBoxClientAddress.TabIndex = 5;
            this.textBoxClientAddress.TextChanged += new System.EventHandler(this.TextBoxClientAddress_TextChanged);
            // 
            // labelClientAddress
            // 
            this.labelClientAddress.AutoSize = true;
            this.labelClientAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelClientAddress.Location = new System.Drawing.Point(14, 138);
            this.labelClientAddress.Margin = new System.Windows.Forms.Padding(5);
            this.labelClientAddress.Name = "labelClientAddress";
            this.labelClientAddress.Size = new System.Drawing.Size(69, 25);
            this.labelClientAddress.TabIndex = 4;
            this.labelClientAddress.Text = "Адрес";
            // 
            // textBoxClientSettlementAccount
            // 
            this.textBoxClientSettlementAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClientSettlementAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxClientSettlementAccount.Location = new System.Drawing.Point(12, 280);
            this.textBoxClientSettlementAccount.Name = "textBoxClientSettlementAccount";
            this.textBoxClientSettlementAccount.Size = new System.Drawing.Size(1141, 30);
            this.textBoxClientSettlementAccount.TabIndex = 7;
            this.textBoxClientSettlementAccount.TextChanged += new System.EventHandler(this.TextBoxClientSettlementAccount_TextChanged);
            // 
            // labelClientSettlementAccount
            // 
            this.labelClientSettlementAccount.AutoSize = true;
            this.labelClientSettlementAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelClientSettlementAccount.Location = new System.Drawing.Point(14, 252);
            this.labelClientSettlementAccount.Margin = new System.Windows.Forms.Padding(5);
            this.labelClientSettlementAccount.Name = "labelClientSettlementAccount";
            this.labelClientSettlementAccount.Size = new System.Drawing.Size(173, 25);
            this.labelClientSettlementAccount.TabIndex = 6;
            this.labelClientSettlementAccount.Text = "Рассчётный счёт";
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSave.Location = new System.Drawing.Point(326, 655);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(10);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(181, 42);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // buttonAddClientCancel
            // 
            this.buttonAddClientCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonAddClientCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddClientCancel.Location = new System.Drawing.Point(996, 655);
            this.buttonAddClientCancel.Margin = new System.Windows.Forms.Padding(10);
            this.buttonAddClientCancel.Name = "buttonAddClientCancel";
            this.buttonAddClientCancel.Size = new System.Drawing.Size(157, 42);
            this.buttonAddClientCancel.TabIndex = 9;
            this.buttonAddClientCancel.Text = "Отмена";
            this.buttonAddClientCancel.UseVisualStyleBackColor = true;
            this.buttonAddClientCancel.Click += new System.EventHandler(this.ButtonAddClientCancel_Click);
            // 
            // listBoxClients
            // 
            this.listBoxClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxClients.FormattingEnabled = true;
            this.listBoxClients.ItemHeight = 25;
            this.listBoxClients.Location = new System.Drawing.Point(12, 378);
            this.listBoxClients.Name = "listBoxClients";
            this.listBoxClients.Size = new System.Drawing.Size(1141, 254);
            this.listBoxClients.Sorted = true;
            this.listBoxClients.TabIndex = 10;
            // 
            // buttonClientPrevious
            // 
            this.buttonClientPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonClientPrevious.Location = new System.Drawing.Point(12, 319);
            this.buttonClientPrevious.Margin = new System.Windows.Forms.Padding(10);
            this.buttonClientPrevious.Name = "buttonClientPrevious";
            this.buttonClientPrevious.Size = new System.Drawing.Size(165, 42);
            this.buttonClientPrevious.TabIndex = 11;
            this.buttonClientPrevious.Text = "Предыдущий";
            this.buttonClientPrevious.UseVisualStyleBackColor = true;
            // 
            // buttonClientNext
            // 
            this.buttonClientNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonClientNext.Location = new System.Drawing.Point(197, 319);
            this.buttonClientNext.Margin = new System.Windows.Forms.Padding(10);
            this.buttonClientNext.Name = "buttonClientNext";
            this.buttonClientNext.Size = new System.Drawing.Size(165, 42);
            this.buttonClientNext.TabIndex = 12;
            this.buttonClientNext.Text = "Следующий";
            this.buttonClientNext.UseVisualStyleBackColor = true;
            // 
            // buttonAddNewClient
            // 
            this.buttonAddNewClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddNewClient.Location = new System.Drawing.Point(12, 655);
            this.buttonAddNewClient.Margin = new System.Windows.Forms.Padding(10);
            this.buttonAddNewClient.Name = "buttonAddNewClient";
            this.buttonAddNewClient.Size = new System.Drawing.Size(294, 42);
            this.buttonAddNewClient.TabIndex = 13;
            this.buttonAddNewClient.Text = "Добавить нового клиента";
            this.buttonAddNewClient.UseVisualStyleBackColor = true;
            // 
            // buttonClientSaveAndExit
            // 
            this.buttonClientSaveAndExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonClientSaveAndExit.Location = new System.Drawing.Point(744, 655);
            this.buttonClientSaveAndExit.Margin = new System.Windows.Forms.Padding(10);
            this.buttonClientSaveAndExit.Name = "buttonClientSaveAndExit";
            this.buttonClientSaveAndExit.Size = new System.Drawing.Size(232, 42);
            this.buttonClientSaveAndExit.TabIndex = 14;
            this.buttonClientSaveAndExit.Text = "Сохранить и выйти";
            this.buttonClientSaveAndExit.UseVisualStyleBackColor = true;
            // 
            // DialogBoxClients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1170, 709);
            this.ControlBox = false;
            this.Controls.Add(this.buttonClientSaveAndExit);
            this.Controls.Add(this.buttonAddNewClient);
            this.Controls.Add(this.buttonClientNext);
            this.Controls.Add(this.buttonClientPrevious);
            this.Controls.Add(this.listBoxClients);
            this.Controls.Add(this.buttonAddClientCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxClientSettlementAccount);
            this.Controls.Add(this.labelClientSettlementAccount);
            this.Controls.Add(this.textBoxClientAddress);
            this.Controls.Add(this.labelClientAddress);
            this.Controls.Add(this.textBoxClientCity);
            this.Controls.Add(this.labelClientCity);
            this.Controls.Add(this.labelClientName);
            this.Controls.Add(this.textBoxClientName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogBoxClients";
            this.Text = "Клиенты";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxClientName;
        private System.Windows.Forms.Label labelClientName;
        private System.Windows.Forms.Label labelClientCity;
        private System.Windows.Forms.TextBox textBoxClientCity;
        private System.Windows.Forms.TextBox textBoxClientAddress;
        private System.Windows.Forms.Label labelClientAddress;
        private System.Windows.Forms.TextBox textBoxClientSettlementAccount;
        private System.Windows.Forms.Label labelClientSettlementAccount;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonAddClientCancel;
        private System.Windows.Forms.ListBox listBoxClients;
        private System.Windows.Forms.Button buttonClientPrevious;
        private System.Windows.Forms.Button buttonClientNext;
        private System.Windows.Forms.Button buttonAddNewClient;
        private System.Windows.Forms.Button buttonClientSaveAndExit;
    }
}