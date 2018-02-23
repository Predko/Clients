using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Clients
{
    public partial class Clients : Form
    {
        private static Client _currentClient;

        public static event EventHandler ChangedCurrentClient_EventHandler;    // Событие, вызываемое после изменения текущего клиента

        public static Client CurrentClient        // Текущий клиент
        {
            get
            {
                return _currentClient;
            }
            set
            {
                if (_currentClient != value)
                {
                    _currentClient = value;

                    OnChangeCurrentClient(EventArgs.Empty);
                }
            }
        }

        private static void OnChangeCurrentClient(EventArgs args)
        {
            ChangedCurrentClient_EventHandler?.Invoke(null, args);
        }
    }

    // перечисление вариантов изменения списка - добавить, очистить, установить по индексу
    public enum Change { Add, Del, Clear, Set }

    // класс аргумента события изменения списка
    public class ChangedListClientsEventArgs : EventArgs
    {
        public Change change;  // вид изменения
        public Client client;   // опционально. добавляемый или устанавливаемый по индексу объект
        public int index;       // индекс для события Set
    }

    // Класс-обёртка списка клиентов
    // предназначен для отображения списка в комбобоксе при изменении
    // Вместо binding...
    public class ListClients:IEnumerable<Client>
    {
        private static readonly SortedList<int, Client> clients = new SortedList<int, Client>();  // Список всех клиентов

        public event EventHandler<ChangedListClientsEventArgs> ListClientsChanged;       // событие для обработки(отображения) изменений
        private readonly ChangedListClientsEventArgs ChangedEventArgs = new ChangedListClientsEventArgs();

        // очистка списка
        public void Clear()
        {
            clients.Clear();

            ChangedEventArgs.change = Change.Clear;
            OnChangeListClients(ChangedEventArgs);
        }

        public int Count { get => clients.Count; }

        public int IndexOfKey(int key) => clients.IndexOfKey(key);

        // добавления клиента в список. Если добавляем нового, isNewClient устанавливаем в true
        public void Add(Client client, bool isNewClient = false)
        {
            if (clients.ContainsValue(client))
                return;                             // такой элемент есть, ничего не делаем

            if (isNewClient)    // Если добавляется новый клиент,
            {
                int id = clients.Count;
                while (clients.ContainsKey(id)) // ищем свободный идентификатор
                    id++;

                client.Id = id;
            }

            if (clients.ContainsKey(client.Id))
            {
                // Ошибка - такой клиент уже есть
                // заносим ошибку в log-файл
                return;    // Ничего не делаем
            }

            clients.Add(client.Id, client);

            ChangedEventArgs.change = Change.Add;
            ChangedEventArgs.client = client;
            OnChangeListClients(ChangedEventArgs);
        }

        // Удаление клиента из списка. Удаляются также договоры из списка, услуги и очищается список подразделений
        public void Remove(Client client)
        {
            if (!clients.ContainsValue(client))
                return;                             // такого элемента нет

            client.contracts.Clear();

            client.ClearSubdisions();

            clients.Remove(client.Id);

            ChangedEventArgs.change = Change.Del;
            ChangedEventArgs.client = client;
            OnChangeListClients(ChangedEventArgs);
        }

        public bool Contains(int id) => clients.ContainsKey(id);

        // Первый клиент в списке
        public Client First() => clients.First().Value;

        // индексатор(получение/установка(замена данных клиента) значения по Id клиента)
        public Client this[int id]
        {
            get { return clients[id]; }
            set
            {
                value.Id = id;          // Соблюдаем соответствие Id ключу в списке
                clients[id] = value;

                ChangedEventArgs.change = Change.Set;
                ChangedEventArgs.client = value;
                ChangedEventArgs.index = id;
                OnChangeListClients(ChangedEventArgs);
            }
        }

        // запуск обработки изменений
        protected virtual void OnChangeListClients(ChangedListClientsEventArgs e)
        {
            ListClientsChanged?.Invoke(this, e);
        }


        public IEnumerator<Client> GetEnumerator() => clients.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
