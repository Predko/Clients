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
        private event EventHandler ChangingCurrentClientEventHandler;

        private FlagChangeTextBoxes isChangeTextBox;

        public Client CurrentClient
        {
            get
            {
                return _currentClient;
            }

            private set
            {
                if(value == null)
                {
                    _currentClient = null;

                    return;
                }

                if (_currentClient != value)
                {
                    // Меняем текущего клиента, только в случае согласия с сохранением или игнорированием изменений
                    var res = IsChangeAndNoSave();

                    if (res == DialogResult.Cancel) // Ничего не делаем
                    {
                        if(_currentClient.Id != -1)
                        {
                            listBoxClients.SelectedItem = _currentClient;
                        }

                        return;
                    }

                    if (res == DialogResult.OK) // Сохраняем изменения
                    {
                        DialogBoxClientSaveChanges();
                    }

                    OnChangingCurrentClient();

                    _currentClient = value;

                    SetCurrentClientInfo();
                }
            }
        }

        private void OnChangingCurrentClient()
        {
            ChangingCurrentClientEventHandler?.Invoke(this, EventArgs.Empty);
        }

        public DialogBoxClients(Client client)
        {
            InitializeComponent();

            InitCurrentClientInfo(client);
        }

        private void InitCurrentClientInfo(Client client)
        {
            listBoxClients.BeginUpdate();

            foreach (Client cl in Clients.clients)
            {
                listBoxClients.Items.Add(cl);
            }

            _currentClient = client;    // устанавливаем первоначального клиента
            SetCurrentClientInfo();     // Обновляем информацию о нём

            listBoxClients.SelectedItem = client;

            listBoxClients.EndUpdate();

            textBoxClientName.Select();
        }

        private void SetCurrentClientInfo()
        {
            SetTextBoxAddTextChangedEvent(false);   // Удаляем события

            textBoxClientName.Text = CurrentClient.Name;
            textBoxClientCity.Text = CurrentClient.City;
            textBoxClientAddress.Text = CurrentClient.Address;
            textBoxClientSettlementAccount.Text = CurrentClient.SettlementAccount;

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

        // Проверяет, были ли изменения и сохранены ли они
        // и предлагает согранить или выйти без сохранения или перкатить смену/(выход из) текущего клиента
        // true - можно менять текущего клиента
        // false - возврат к редактированию
        // Также устанавливает DialogResult
        private DialogResult IsChangeAndNoSave()
        {
            if (isChangeTextBox != FlagChangeTextBoxes.None)
            {
                var dlg = new DialogBoxSaveOrNo();

                dlg.ShowDialog();

                return dlg.DialogResult;
            }

            return DialogResult.No;   // Не было изменений
        }

        // Сохраняем изменения внесённые в текстовые поля
        private void DialogBoxClientSaveChanges()
        {
            CurrentClient.Name = textBoxClientName.Text;
            CurrentClient.City = textBoxClientCity.Text;
            CurrentClient.Address = textBoxClientAddress.Text;
            CurrentClient.SettlementAccount = textBoxClientSettlementAccount.Text;

            if (CurrentClient.Id == -1)  // Это новый клиент?
            {
                Clients.clients.Add(CurrentClient, true); // Добавляем в список всех клиентов

                listBoxClients.Items.Add(CurrentClient);

                listBoxClients.SelectedItem = CurrentClient;
            }

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

        private void ButtonAddClientExit_Click(object sender, EventArgs e)
        {
            // Проверяем на изменения, предлагаем сохранить.
            //  Устанавливаем DialogResult.OK если надо сохранить изменения перед выходом.
            var res = IsChangeAndNoSave();

            if (res == DialogResult.OK)
            {
                DialogResult = DialogResult.OK;

                DialogBoxClientSaveChanges();   // сохраняем изменения

                Close();
            }
            else
            if (res == DialogResult.Ignore || res == DialogResult.No)
            {
                DialogResult = DialogResult.Cancel;

                Close();
            }
        }

        // Сохраняем
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            DialogBoxClientSaveChanges();
        }

        // Сохраняем и выходим
        private void ButtonClientSaveAndExit_Click(object sender, EventArgs e)
        {
            DialogBoxClientSaveChanges();

            Close();
        }

        // Предыдущий клиент
        private void ButtonClientPrevious_Click(object sender, EventArgs e)
        {
            int PrevIndex = listBoxClients.SelectedIndex - 1;

            if (PrevIndex >= 0)
                listBoxClients.SelectedIndex = PrevIndex;
        }

        // Следующий клиент
        private void ButtonClientNext_Click(object sender, EventArgs e)
        {
            int NextIndex = listBoxClients.SelectedIndex + 1;

            if (NextIndex < listBoxClients.Items.Count)
                listBoxClients.SelectedIndex = NextIndex;
        }

        // Добавляем нового клиента
        private void ButtonAddNewClient_Click(object sender, EventArgs e)
        {
            CurrentClient = new Client();

            textBoxClientName.Select();
        }

        // При изменении текущего пункта listBoxClient, меняем CurrentClient
        private void ListBoxClients_SelectedValueChanged(object sender, EventArgs e)
        {
            var cl = (Client)listBoxClients.SelectedItem;

            if (cl != null)
            {
                CurrentClient = cl;
            }
        }

        // Удаляет выбранного клиента, кроме последнего
        private void ButtonRemoveCurrentClient_Click(object sender, EventArgs e)
        {
            if(CurrentClient != null)
            {
                var dlg = new DialogRemoveClient(CurrentClient.Name);

                dlg.ShowDialog();
                if(dlg.DialogResult == DialogResult.OK)
                {
                    if(listBoxClients.Items.Count == 1)
                    {
                        Clients.clients.Remove(CurrentClient);  // Удаляем последнего из списка и выходим из формы редактирования

                        CurrentClient = null;

                        Close();

                        return;
                    }

                    int index = listBoxClients.SelectedIndex;
                    if(index == listBoxClients.Items.Count - 1)
                    {
                        index--;
                    }

                    Clients.clients.Remove(CurrentClient);

                    listBoxClients.Items.Remove(CurrentClient);

                    isChangeTextBox = FlagChangeTextBoxes.None;

                    listBoxClients.SelectedIndex = index;
                }
            }
        }
    }
}
