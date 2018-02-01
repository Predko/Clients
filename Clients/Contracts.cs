using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clients
{
    // аргумент события для ChangedContracts
    public class ChangedContractsEventArgs: EventArgs
    {
        public Change change;       // Add, Clear, Set
        public Contract contract;   // Изменяемый/добавляемый Contract
        public int index;           // индекс в списке
    }
    
    // Класс - список контрактов

    public class Contracts:IEnumerable<Contract>
    {
        private readonly List<Contract> contracts = new List<Contract>();

        private static ChangedContractsEventArgs eventArgs = new ChangedContractsEventArgs();
        public static event EventHandler<ChangedContractsEventArgs> ChangedContracts;

        public  int Count { get => contracts.Count;}

        public void Add(Contract contract)
        {
            contracts.Add(contract);

            if (ChangedContracts != null)
            {
                eventArgs.change = Change.Add;
                eventArgs.contract = contract;

                OnChangedContracts(eventArgs);
            }
        }

        public void Clear()
        {
            contracts.Clear();

            if (ChangedContracts != null)
            {
                eventArgs.change = Change.Clear;

                OnChangedContracts(eventArgs);
            }
        }

        public Contract this[int index]
        {
            get
            {
                if (index < 0 && index >= contracts.Count)
                    throw new IndexOutOfRangeException("Индекс за пределами списка Contracts");
                return contracts[index];
            }

            set
            {
                contracts[index] = value;

                if (ChangedContracts != null)
                {
                    eventArgs.change = Change.Set;
                    eventArgs.contract = value;
                    eventArgs.index = index;

                    OnChangedContracts(eventArgs);
                }
            }
        }

        public void Sort()
        {
            contracts.Sort();
            foreach(Contract c in contracts)
                c.services.Sort();
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
            private Contracts ListContracts;
            private int currentIndex = -1;

            public Enumerator(Contracts c)
            {
                ListContracts = c;
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
