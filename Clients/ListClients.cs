using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clients
{

    // Класс-обёртка списка клиентов
    // предназначен для отображения списка в комбобоксе при изменении
    // Вместо binding...
    class ListClients
    {
        private List<Client> clients = new List<Client>();  // список клиентов

        public event EventHandler ListClientsChanged;       // событие для обработки(отображения) изменений


        #region Begin - End - ClientListChanging

        private bool IsBeginClientListChanging = false;     // true - если был вызван метод BeginClientListChanging
        private event EventHandler SaveEventHandler;        // временно сохраняется событие до конца изменений
        
        // начало изменения списка
        // Используется чтобы предотвратить вызов события ListClientsChanged
        // при каждом изменении списка в случае множественного изменения
        // работает в паре с методом EndClientListChanging, который событие
        public void BeginClientListChanging()
        {
            IsBeginClientListChanging = true;
            SaveEventHandler = ListClientsChanged;
            ListClientsChanged = null;
        }

        public void EndClientListChanging(EventArgs e)
        {
            if (IsBeginClientListChanging)
            {
                ListClientsChanged = SaveEventHandler;
                OnChangeListClients(e);
            }
        }
        #endregion


        public ListClients()
        {

        }


        // запуск обработки изменений
        protected virtual void OnChangeListClients(EventArgs e)
        {
            ListClientsChanged?.Invoke(this, e);
        }

        // очистка списка
        public void Clear()
        {
            clients.Clear();

            OnChangeListClients(EventArgs.Empty);
        }

        // добавления нового клиента в список
        public void Add(Client client)
        {
            clients.Add(client);

            OnChangeListClients(EventArgs.Empty);
        }
    }
}
