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
    public class Client: IComparable<Client>
    {
        private static int lastId = 1;      // последний не использованный идентификатор.

        public int Id;                      // идентификатор записи
        public string Name;                 // Название организации
        public string SettlementAccount;    // Расчётный счёт
        public string City;                 // Населённый пункт
        public string Address;              // Адрес

        public Contracts contracts = new Contracts(); // список договоров с данным клиентом



        public Client()
        {
            this.Id = lastId++;

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

        public int CompareTo(Client other)
        {
            return this.Name.CompareTo(other.Name);
        }

        public override string ToString()
        {
            return String.Format($"{this.Name}");
        }

    }


}
