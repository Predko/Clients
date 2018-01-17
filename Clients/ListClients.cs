using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clients
{
    // перечисление вариантов изменения списка - добавить, очистить, установить по индексу
    public enum Change { Add, Clear, Set }

    // класс аргумента события изменения списка
    public class ChangedListClientsEventArgs : EventArgs
    {
        public Change changed;  // вид изменения
        public Client client;   // опционально. добавляемый или устанавливаемый по индексу объект
        public int index;       // индекс для события Set
    }

    // Класс-обёртка списка клиентов
    // предназначен для отображения списка в комбобоксе при изменении
    // Вместо binding...
    public class ListClients:IEnumerable<Client>
    {
        private readonly List<Client> clients = new List<Client>();  // список клиентов

        public event EventHandler<ChangedListClientsEventArgs> ListClientsChanged;       // событие для обработки(отображения) изменений
        private ChangedListClientsEventArgs ChangedEventArgs = new ChangedListClientsEventArgs();

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


        // индексатор(установка значения по индексу - Set)
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

        // запуск обработки изменений
        protected virtual void OnChangeListClients(ChangedListClientsEventArgs e)
        {
            ListClientsChanged?.Invoke(this, e);
        }


        public IEnumerator<Client> GetEnumerator() => ((IEnumerable<Client>)clients).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
