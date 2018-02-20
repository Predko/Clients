using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clients
{
    [Flags]
    public enum FlagChangeTextBoxes
    {
        None = 0,
        Name = 1,
        City = 2,
        Address = 4,
        SettlementAccount = 8
    }

    public partial class DialogBoxClients : Form
    {
        private Client _currentClient;

        private FlagChangeTextBoxes isChangeTextBox;


        private Client CurrentClient
        {
            get
            {
                return _currentClient;
            }

            set
            {
                _currentClient = value;

                if(value == null)
                {
                    return;
                }

                if(_currentClient != value)
                {
                    SetCurrentClientInfo();
                }
            }
        }

        public DialogBoxClients(Client client)
        {
            InitializeComponent();

            InitCurrentClientInfo(client);
        }

        private void InitCurrentClientInfo(Client client)
        {
            CurrentClient = client;

            listBoxClients.BeginUpdate();

            foreach (Client cl in Clients.clients)
            {
                listBoxClients.Items.Add(cl);
            }

            SetCurrentClientInfo();

            listBoxClients.EndUpdate();
        }

        private void SetCurrentClientInfo()
        {
            switch (isChangeTextBox)
            {
                case FlagChangeTextBoxes.Name:
                    break;

                case FlagChangeTextBoxes.City:
                    break;

                case FlagChangeTextBoxes.Address:
                    break;

                case FlagChangeTextBoxes.SettlementAccount:
                    break;

                case FlagChangeTextBoxes.None:
                    break;
            }

            SetTextBoxAddTextChangedEvent(false);   // Удаляем события

            textBoxClientName.Text = CurrentClient.Name;
            textBoxClientCity.Text = CurrentClient.City;
            textBoxClientAddress.Text = CurrentClient.Address;
            textBoxClientSettlementAccount.Text = CurrentClient.SettlementAccount;

            listBoxClients.SelectedItem = CurrentClient;

            SetTextBoxAddTextChangedEvent(true);    // Добавляем события

            isChangeTextBox = FlagChangeTextBoxes.None;
        }

        private void SetTextBoxAddTextChangedEvent(bool add)
        {
            textBoxClientName.TextChanged -= TextBoxClientName_TextChanged;
            textBoxClientCity.TextChanged -= TextBoxClientCity_TextChanged;
            textBoxClientAddress.TextChanged -= TextBoxClientAddress_TextChanged;
            textBoxClientSettlementAccount.TextChanged -= TextBoxClientSettlementAccount_TextChanged;

            if (add)
            {
                textBoxClientName.TextChanged += TextBoxClientName_TextChanged;
                textBoxClientCity.TextChanged += TextBoxClientCity_TextChanged;
                textBoxClientAddress.TextChanged += TextBoxClientAddress_TextChanged;
                textBoxClientSettlementAccount.TextChanged += TextBoxClientSettlementAccount_TextChanged;
            }
        }

        private void ButtonAddClientCancel_Click(object sender, EventArgs e)
        {
            if(isChangeTextBox != 0)
            {
                var dlg = new DialogBoxSaveOrNo();

                dlg.ShowDialog();

                var res = dlg.DialogResult;
                if (res == DialogResult.OK)
                {
                    DialogBoxClientSaveChanges();
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                if (res == DialogResult.Ignore)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
                else
                if (res == DialogResult.Cancel)
                {
                    buttonAddClientCancel.DialogResult = DialogResult.Cancel;
                    return;
                }
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        // Сохраняем изменения внесённые в текстовые поля
        private void DialogBoxClientSaveChanges()
        {
            CurrentClient.Name = textBoxClientName.Text;
            CurrentClient.City = textBoxClientCity.Text;
            CurrentClient.Address = textBoxClientAddress.Text;
            CurrentClient.SettlementAccount = textBoxClientSettlementAccount.Text;

            SetTextBoxAddTextChangedEvent(true);    // Добавляем события

            isChangeTextBox = FlagChangeTextBoxes.None;
        }

        private void TextBoxClientName_TextChanged(object sender, EventArgs e)
        {
            isChangeTextBox ^= FlagChangeTextBoxes.Name;
            textBoxClientName.TextChanged -= TextBoxClientName_TextChanged; // Удаляем событие
        }

        private void TextBoxClientCity_TextChanged(object sender, EventArgs e)
        {
            isChangeTextBox ^= FlagChangeTextBoxes.City;
            textBoxClientCity.TextChanged -= TextBoxClientCity_TextChanged;
        }

        private void TextBoxClientAddress_TextChanged(object sender, EventArgs e)
        {
            isChangeTextBox ^= FlagChangeTextBoxes.Address;
            textBoxClientAddress.TextChanged -= TextBoxClientAddress_TextChanged;
        }

        private void TextBoxClientSettlementAccount_TextChanged(object sender, EventArgs e)
        {
            isChangeTextBox ^= FlagChangeTextBoxes.SettlementAccount;
            textBoxClientSettlementAccount.TextChanged -= TextBoxClientSettlementAccount_TextChanged;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            DialogBoxClientSaveChanges();
        }
    }
}
