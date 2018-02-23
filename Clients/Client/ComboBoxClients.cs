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
        //-------------------------------------------------------------
        //      Инициализация comboBoxClients
        //
        private void InitComboBoxClients()
        {
            comboBoxClients.SelectedIndexChanged += ComboBoxClients_SelectedIndexChanged;

            ChangedCurrentClient_EventHandler += ComboBoxClientChangeCurrentClient;

            // при изменении списка клиентов, вызывать это событие
            clients.ListClientsChanged += ChangeComboBoxClients;
        }

        private void ComboBoxClientChangeCurrentClient(object sender, EventArgs e)
        {
            comboBoxClients.SelectedItem = CurrentClient;
        }

        //-------------------------------------------------------------
        //      Выбор элемента comboBoxClients
        //
        private void ComboBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentClient = (Client)((ComboBox)sender).SelectedItem;
        }

        //---------------------------------------------------------------
        // Событие по изменению списка клиентов:
        // - добавление нового
        // - очистка списка
        // - установка по индексу
        public void ChangeComboBoxClients(Object sender, ChangedListClientsEventArgs e)
        {
            switch (e.change)
            {
                case Change.Add:
                    comboBoxClients.Items.Add(e.client);
                    break;

                case Change.Del:
                    comboBoxClients.Items.Remove(e.client);
                    break;

                case Change.Clear:                              // очищаем список
                    comboBoxClients.Items.Clear();
                    break;

                case Change.Set:                                // устанавливаем элемент с данным индексом
                    comboBoxClients.Items[e.index] = e.client;
                    break;
            }
        }
    }
}
