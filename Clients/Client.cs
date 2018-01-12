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
    public class Client
    {
        public int id;
        public string name;

        public BindingList<Contract> contracts = new BindingList<Contract>(); // список договоров с данным клиентом



        public Client(string name, int id)
        {
            this.id = id;
            this.name = name;
        }

        public override string ToString()
        {
            return String.Format($"{id,5} {name}");
        }

    }


}
