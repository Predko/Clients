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

            // при изменении списка клиентов, вызывать это событие
            clients.ListClientsChanged += ChangeComboBoxClients;
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
                    //SortedInsertItem(comboBoxClients, e.client); // добавляем элемент в список не нарушая сортировки
                    comboBoxClients.Items.Add(e.client);
                    break;

                case Change.Clear:                              // очищаем список
                    comboBoxClients.Items.Clear();
                    break;

                case Change.Set:                                // устанавливаем элемент с данным индексом
                    comboBoxClients.Items[e.index] = e.client;  // нарушается сортировка!
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
                    var c = (Client)cb.Items[i];

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
