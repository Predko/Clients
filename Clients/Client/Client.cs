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
    public partial class Clients : Form
    {
        private Contract _currentContract;  // текущий договор
    }

    public class Client: IComparable<Client>
    {
        public int Id;                      // идентификатор клиента
        public string Name;                 // Название организации
        public string SettlementAccount;    // Расчётный счёт
        public string City;                 // Населённый пункт
        public string Address;              // Адрес

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

        // Добавляет имя подразделения в список. Если такое есть - возвращает его id(индекс в списке).
        // Возвращает индекс подразделения в списке
        public int AddSubdision(string name)
        {
            int id = Subdivisions.IndexOfValue(name);   // получаем индекс в списке для name
            if (id == -1)
            {
                id = freeIdSubdiv.Id;
                Subdivisions.Add(id, name);

                if (Subdivisions.IndexOfKey(id) == -1)
                    return -1;      // добавление нового элемента не удалось
            }
            else
            {
                id = Subdivisions.Keys[id]; // ключ для индекса
            }

            return id;
        }

        // Удаляет подразделение из списка
        public void RemoveSubdision(int id)
        {
            Subdivisions.Remove(id);
            freeIdSubdiv.Id = id;       // освободившийся id заносим в список
        }

        public int GetIdSubdivision(string name)
        {
            return Subdivisions.Keys[Subdivisions.IndexOfValue(name)];
        }

        public string GetNameSubdivision(int id)
        {
            return Subdivisions[id];
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
