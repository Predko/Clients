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
        private Client _currentClient = null;

        private Client CurrentClient        // Текущий клиент
        {
            get
            {
                return _currentClient;
            }
            set
            {
                _currentClient = value;
            }
        }

        private Contract CurrentContract { get; set; } // Текущий договор

        //-------------------------------------------------------------
        //      Инициализация comboBoxClients
        //
        private void InitComboBoxClients()
        {
            clients.ListClientsChanged += ChangeComboBoxClients;
        }

        //-------------------------------------------------------------
        //      Выбор элемента comboBoxClients
        //
        private void ComboBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentClient = (Client)((ComboBox)sender).SelectedItem;

            // заполняем listBoxContracts текущим списком договоров
            listBoxContracts.BeginUpdate();

            SetClientContracts(CurrentClient);

            listBoxContracts.EndUpdate();
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
                case Change.Add:                            // добавляем элемент в список не нарушая сортировки
                    SortedInsertItem(comboBoxClients, e.client);
                    break;

                case Change.Clear:                          // очищаем список
                    comboBoxClients.Items.Clear();
                    break;

                case Change.Set:                            // устанавливаем элемент с данным индексом
                    comboBoxClients.Items[e.index] = e.client;
                    break;
            }
        }

        // Вставка нового элемента без нарушения сортировки списка
        private void SortedInsertItem(ComboBox cb, Client cl)
        {
            if (cb.Items.Count != 0)
            {
                for (int i = 0; i < cb.Items.Count; i++)
                {
                    Client c = (Client)cb.Items[i];

                    int res = c.Name.CompareTo(cl.Name);

                    if (res == 0)
                        return;     // такой элемент есть. Ничего не делаем

                    if (res > 0)
                    {
                        cb.Items.Insert(i, cl); // найден элемент с большим весом, вставляем новый до него
                        return;
                    }
                }
            }

            cb.Items.Add(cl);   // добавляем, если первый или если не найден больший чем данный
        }
    }
}
