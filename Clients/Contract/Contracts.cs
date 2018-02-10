﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Clients
{

    public partial class Clients : Form
    {
        // Здесь хранится список всех договоров, всех клиентов
        public static readonly SortedList<int, Contract> AllContracts = new SortedList<int, Contract>();
    }

    // Класс аргумента события для ChangedContracts
    public class ChangedContractsEventArgs: EventArgs
    {
        public Change change;       // Add, Clear, Set
        public Contract contract;   // Изменяемый/добавляемый Contract
        public int index;           // индекс в списке
    }

    // Класс - список контрактов
    // перечисление работает со списком договоров данного клиента - contracts
    public class Contracts:IEnumerable<Contract>
    {
        private static readonly ChangedContractsEventArgs eventArgs = new ChangedContractsEventArgs();
        public static event EventHandler<ChangedContractsEventArgs> ChangedContracts;

        private SortedList<int, Contract> _contracts => Clients.AllContracts;
        private static int LastUnusedKey { get; set; } = 0;


        // Здесь хранится список Id(ключей из списка _contracts) всех договоров данного клиента
        private readonly List<int> contracts = new List<int>();

        public  int Count { get => contracts.Count;}

        public void Add(Contract contract, bool isNewContract = false)
        {
            if (isNewContract) // это новый договор?
            {   // да.
                // присваиваем Id последний не использованный ключ в списке 
                contract.Id = LastUnusedKey++;
            }

            _contracts.Add(contract.Id, contract);  // в общий список
            contracts.Add(contract.Id);             // в список ключей договоров данного клиента

            if (ChangedContracts != null)
            {
                eventArgs.change = Change.Add;
                eventArgs.contract = contract;

                OnChangedContracts(eventArgs);
            }
        }

        public void Remove(Contract contract)
        {
            _contracts.Remove(contract.Id);     // появляется неиспользованный Id.
            contracts.Remove(contract.Id);      // В дальнейшем можно создать список неиспользованных Id и использовать их

            if (ChangedContracts != null)
            {
                eventArgs.change = Change.Del;
                eventArgs.contract = contract;

                OnChangedContracts(eventArgs);
            }
        }

        public void Clear()
        {
            contracts.Select(c => _contracts.Remove(c)); // удаляем все договоры из списка в общем списке договоров всех клиентов
            LastUnusedKey = 0;  // использованных ключей нет

            contracts.Clear(); // удаляем список ключей договоров

            if (ChangedContracts != null)
            {
                eventArgs.change = Change.Clear;

                OnChangedContracts(eventArgs);
            }
        }

        // Индексатор списка. Получает индекс в списке договоров текущего клиента - contracts
        // Возвращает/присваивает договор из общего списка всех договоров: _contracts => Clients.AllContracts;
        public Contract this[int index]
        {
            get => _contracts[contracts[index]];

            set     // замена договора(редактирование)
            {
                _contracts[contracts[index]] = value;

                if (ChangedContracts != null)
                {
                    eventArgs.change = Change.Set;
                    eventArgs.contract = value;
                    eventArgs.index = index;

                    OnChangedContracts(eventArgs);
                }
            }
        }

        // Сортировка списка договоров данного клиента - contracts
        public void Sort()
        {
            contracts.Sort((id1, id2) => _contracts[id1].CompareTo(_contracts[id2]));

            foreach (int id in contracts)
            {
                _contracts[id].services.Sort();
            }
        }

        private void OnChangedContracts(ChangedContractsEventArgs e)
        {
            if (ChangedContracts == null)
                return;
            ChangedContracts(this, e);
        }

        public IEnumerator<Contract> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
 
    // класс итератора
        private class Enumerator : IEnumerator<Contract>
        {
            private readonly Contracts ListContracts;
            private int currentIndex = -1;

            public Enumerator(Contracts list)
            {
                ListContracts = list;
            }


            public Contract Current {
                get
                {
                    if (currentIndex == -1)
                        throw new InvalidOperationException("Enumeration not started");
                    if (currentIndex >= ListContracts.Count)
                        throw new InvalidOperationException("Past end of list");

                    return ListContracts[currentIndex];
                }
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (currentIndex == ListContracts.Count - 1)
                    return false;
                return ++currentIndex < ListContracts.Count;
            }

            void IEnumerator.Reset()
            {
                currentIndex = -1;
            }

            void IDisposable.Dispose() { }
        }

    }
}
