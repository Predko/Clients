using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clients
{

    // Класс-обёртка списка клиентов
    // предназначен для отображения списка в комбобоксе при изменении
    // Вместо binding...
    public class ListClients:IEnumerable<Client>
    {
        private readonly List<Client> clients = new List<Client>();  // список клиентов

        public event EventHandler<ChangedListClientsEventArgs> ListClientsChanged;       // событие для обработки(отображения) изменений
        private ChangedListClientsEventArgs ChangedEventArgs = new ChangedListClientsEventArgs();

 

        public ListClients()
        {

        }


        public List<Client> GetListClients()
        {
            return clients;
        }

        // запуск обработки изменений
        protected virtual void OnChangeListClients(ChangedListClientsEventArgs e)
        {
            ListClientsChanged?.Invoke(this, e);
        }

        // очистка списка
        public void Clear()
        {
            clients.Clear();

            ChangedEventArgs.changed = Change.Clear;
            OnChangeListClients(ChangedEventArgs);
        }

        public int Count { get => clients.Count; }

        // добавления нового клиента в список
        public void Add(Client client)
        {
            clients.Add(client);

            ChangedEventArgs.changed = Change.Add;
            ChangedEventArgs.client = client;
            OnChangeListClients(ChangedEventArgs);
        }

        public Client this[int i]
        {
            get { return clients[i]; }
            set
            {
                clients[i] = value;

                ChangedEventArgs.changed = Change.Set;
                ChangedEventArgs.client = value;
                ChangedEventArgs.index = i;
                OnChangeListClients(ChangedEventArgs);
            }
        }


        public IEnumerator<Client> GetEnumerator() => ((IEnumerable<Client>)clients).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public enum Change { Add, Clear, Set}

    public class ChangedListClientsEventArgs : EventArgs
    {
        public Change changed;
        public Client client;
        public int index;
    }
}
