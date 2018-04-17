using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Data;


namespace Clients
{
    public static partial class ExtensionMethods
    {
        public static void IncrementValue(this Dictionary<int, int> ns, int key)
        {
            if (!ns.ContainsKey(key))
            {
                ns[key] = 1;
            }
            else
            {
                ns[key]++;
            }
        }
    }

    public partial class Clients : Form
    {
        private Contract _currentContract;  // текущий договор
        public event EventHandler ChangeCurrentContract_EventHandler;    // событие, вызываемое при изменении текущего контракта

        public Contract CurrentContract // Текущий договор
        {
            get => _currentContract;
            set
            {
                if (_currentContract != value)
                {
                    _currentContract = value;
                    OnChangeCurrentContractInfo(EventArgs.Empty); // обрабатываем подключённые события после изменения текущего договора
                }
            }
        }
    }

    public class Client: IComparable<Client>
    {
        public int Id;                      // идентификатор клиента
        public string Name;                 // Название организации
        public string SettlementAccount;    // Расчётный счёт
        public string City;                 // Населённый пункт
        public string Address;              // Адрес

        // Статистические данные: Количество использования названий работ(NameWork),
        // названия устройства(NameDevice) и дополнительной информации услуги(AddInfo)
        // Key - Id, Value - count
        public Dictionary<int, int> NWCounts = new Dictionary<int, int>();
        public Dictionary<int, int> NDCounts = new Dictionary<int, int>();
        public Dictionary<int, int> AICounts = new Dictionary<int, int>();

        public Contracts contracts = new Contracts(); // список договоров с данным клиентом

        public SortedList<int, string> Subdivisions = new SortedList<int, string>() { { 0, "" } };  // список всех подразделений клиента
        private readonly FreeID freeIdSubdiv = new FreeID();                   // список свободных id подразделений


        public Client(int id = -1)
        {
            this.Id = id;

            this.Name = String.Empty;
            this.SettlementAccount = String.Empty;
            this.City = String.Empty;
            this.Address = String.Empty;
        }

        public Client(string Name, int Id, string SettlementAccount = "", string City = "", string Address = "")
        {
            this.Id = Id;
            this.Name = Name;
            this.SettlementAccount = SettlementAccount;
            this.City = City;
            this.Address = Address;
        }

        // Клонирует экземпляр
        public Client Clone()
        {
            return new Client(Name, Id, SettlementAccount, City, Address)
            {
                contracts = contracts,
                Subdivisions = Subdivisions
            };
        }

        // Добавляет имя подразделения в список. Если такое есть - возвращает его id(индекс в списке).
        // Возвращает индекс подразделения в списке
        public int AddSubdivision(string name, int id = -1)
        {
            if(id != -1) // Добавляем подразделение с определённым id
            {
                freeIdSubdiv.SetLastId(id);

                if (!Subdivisions.ContainsKey(id))
                {
                    Subdivisions.Add(id, name);
                }
            }
            else
            {
                int index = Subdivisions.IndexOfValue(name);   // получаем индекс в списке для name

                if (index == -1)
                {
                    id = freeIdSubdiv.Id;
                    Subdivisions.Add(id, name);

                    if (Subdivisions.IndexOfKey(id) == -1)
                        return -1;      // добавление нового элемента не удалось
                }
                else
                {
                    id = Subdivisions.Keys[index]; // ключ для индекса

                }
            }

            return id;
        }

        // Удаляет подразделение из списка
        public void RemoveSubdision(int id)
        {
            Subdivisions.Remove(id);
            freeIdSubdiv.Id = id;       // освободившийся id заносим в список
        }
 
        // Очистка списка подразделений
        public void ClearSubdisions()
        {
            foreach (var key in Subdivisions.Keys.ToArray())
            {
                Subdivisions.Remove(key);
                freeIdSubdiv.Id = key;       // освободившийся id заносим в список
            }
        }

        public int GetIdSubdivision(string name)
        {
            return Subdivisions.Keys[Subdivisions.IndexOfValue(name)];
        }

        public string GetNameSubdivision(int id)
        {
            return Subdivisions[id];
        }

        // Увеличения счётчика использования имени...
        public void IncrementNameService(Object sender, IncrCountNameServiceEventArgs e)
        {
            switch (sender)
            {
                case NameWork nw:

                    NWCounts[e.Id]++;

                    break;

                case NameDevice nd:

                    NDCounts[e.Id]++;

                    break;

                case AddInfo ai:

                    AICounts[e.Id]++;

                    break;
            }
        }

        public int CompareTo(Client other)
        {
            return this.Name.CompareTo(other.Name);
        }

        public override string ToString()
        {
            return String.Format(this.Name);
        }
    }
}
