using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;

namespace Clients
{
    public partial class Clients : Form
    {
        private Client currentClient = null;      // текущий клиент


        private void InitComboBoxClients()
        {
            clients.ListClientsChanged += ChangeComboBoxClients;
        }

        //
        //      Выбор элемента comboBoxClients
        //
        private void ComboBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentClient = (Client)((ComboBox)sender).SelectedItem;

            // заполняем listBoxContracts текущим списком договоров
            listBoxContracts.BeginUpdate();

            SetClientContracts(currentClient);

            listBoxContracts.EndUpdate();
        }

        public void ChangeComboBoxClients(Object sender, ChangedListClientsEventArgs e)
        {
            switch (e.change)
            {
                case Change.Add:                            // добавляем элемент в список
                    comboBoxClients.Items.Add(e.client);
                    break;

                case Change.Clear:                          // очищаем список
                    comboBoxClients.Items.Clear();
                    break;

                case Change.Set:                            // устанавливаем элемент с данным индексом
                    comboBoxClients.Items[e.index] = e.client;
                    break;
            }
        }
    }
}
