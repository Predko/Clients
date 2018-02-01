using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Globalization;
using System.Threading;
using System.ComponentModel;


namespace Clients
{
    public class ClientsXml
    {
        XDocument xClients = null;  // xml документ
        private bool _load_Ok;

        public bool Load_Ok         // true - операция загрузки файла успешна
        {
            get { return _load_Ok; }
        }

        // конструктор с созданием документа XDocument из списка клиентов (ListClients)clients
        public ClientsXml(ListClients clients)
        {
            xClients = ToXml(clients, ""); // namespace = "" 
        }

        // конструктор с загрузкой документа из xml файла filename
        public ClientsXml(string filename)
        {
            if (!LoadXml(filename))
                _load_Ok = false;

            _load_Ok = true;
        }

        // Создание XDocument из списка клиентов (ListClients)clients
        public XDocument ToXml(ListClients clients, string ns)
        {
            XNamespace namesp = ns;

            return new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                        new XElement(ns + "Clients",
                        clients.Select(c => new XElement("Client",
                            new XElement("ClientId", c.Id),
                            new XElement("ClientName", c.Name),
                            new XElement("SettlementAccount", c.SettlementAccount),
                            new XElement("City", c.City),
                            new XElement("Address", c.Address),
                            new XElement("Contracts", 
                                new XAttribute("Count", c.contracts.Count),
                                c.contracts.Select(t =>
                                new XElement("Contract",
                                    new XElement("ContractId", t.Id),
                                    new XElement("DateContract", t.Dt.ToString("d")),
                                    new XElement("Number", t.Numb),
                                    new XElement("Summ", t.Summ),
                                    new XElement("Signed", t.Signed),
                                    new XElement("FileName", t.FileName),
                                    new XElement("Services", 
                                        t.services.Select(s => 
                                            new XElement("Service",
                                                new XElement("Id",s.Id),
                                                new XElement("Number", s.Number),

                                                new XElement("Value",
                                                    new XElement("Id", s.Value.Id),
                                                    new XElement("Summ", s.Value.Summ),
                                                    new XElement("Date", s.Value.Date.ToString("d"))),

                                                new XElement("NameWork", 
                                                    new XElement("Id",s.Nw.Id),
                                                    new XElement("Name", s.Nw.Name)),

                                                new XElement("NameDevice",
                                                    new XElement("Id", s.Nd.Id),
                                                    new XElement("Name", s.Nd.Name)),

                                                new XElement("Subdivision",
                                                    new XElement("Id", s.Sd.Id),
                                                    new XElement("Name", s.Sd.Name))
                                                        )
                                                ))
                                        ))
                                )))));

        }

        // Запись xml документа в файл filename
        public void SaveXml(string filename)
        {
            if (xClients == null)
                return;

            using (StreamWriter sw = new StreamWriter(filename, false))
            {
                XmlWriterSettings xws = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "\t"
                };

                using (XmlWriter xw = XmlWriter.Create(sw, xws))
                {
                    xClients.Save(xw);
                }
            }
        }

        // Load filename(xml) to XDocument and fill ListClients (foreach(Element))
        public bool LoadXml(string filename, ListClients clients)
        {
            _load_Ok = true;
            try
            {
                xClients = XDocument.Load(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + '\n' + $"Error loading file {filename}");
                _load_Ok = false;
                return _load_Ok;
            }

            XmlToClientsAndContracts(clients);

            return _load_Ok;
        }

        // Load filename(xml) to XDocument
        public bool LoadXml(string filename)
        {
            _load_Ok = true;
            try
            {
                xClients = XDocument.Load(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + '\n' + $"Error loading file {filename}");
                _load_Ok = true;
                return _load_Ok;
            }

            return _load_Ok;
        }


        // Fill List clients and List contracts (foreach(Element))
        public void XmlToClientsAndContracts(ListClients clients)
        {
            clients.Clear();

            #region CultureInfo setting
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;

            ci = (CultureInfo)ci.Clone();

            ci.NumberFormat.CurrencyGroupSeparator = " ";
            ci.NumberFormat.NumberDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = ci;
            #endregion

            foreach (XElement xe in xClients.Element("Clients").Elements("Client"))
            {// 
                int id = int.Parse(xe.Element("ClientId").Value);
                string name = xe.Element("ClientName").Value;
                string settlementAccount = xe.Element("SettlementAccount")?.Value;
                string city = xe.Element("City")?.Value;
                string address = xe.Element("Address")?.Value;

                Client client = new Client(name, id, settlementAccount, city, address);

                string svalue;

                IEnumerable<XElement> xelement = xe.Element("Contracts")?.Elements();
                if (xelement != null)
                {
                    foreach (XElement element in xelement)
                    {
                        id = int.Parse(element.Element("ContractId").Value);

                        svalue = element.Element("DateContract")?.Value;
                        DateTime dt;

                        try
                        {
                            dt = DateTime.Parse(svalue);
                        }
                        catch
                        {
                            dt = DateTime.MinValue;
                        }

                        svalue = element.Element("Number")?.Value;
                        int number = (int)double.Parse(svalue ?? "0");

                        svalue = element.Element("Summ")?.Value;
                        decimal summ = decimal.Parse(svalue ?? "0");

                        bool signed = bool.Parse(element.Element("Signed").Value);

                        string filename = element.Element("FileName")?.Value;

                        TypeContract tc;
                        if (filename == null)
                            tc = TypeContract.Contract;
                        else
                            tc = (filename.Contains("Договор"))
                                                    ? TypeContract.Contract     // Договор
                                                    : TypeContract.СWC;         // Акт приёмки сдачи работ

                        //
                        //  Здесь загрузить Services
                        //
                        //

                        client.contracts.Add(new Contract(client, id, dt, number, summ, signed, filename, tc));
                    } // end foreach
                }

                clients.Add(client);
            }
        }

        // Загрузка списка клиентов и контрактов из xml документа сформированного MS Access
        public void AccessXmlToClients(ListClients clients)
        {
            clients.Clear();

            #region CultureInfo setting
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;

            ci = (CultureInfo)ci.Clone();

            ci.NumberFormat.CurrencyGroupSeparator = " ";
            ci.NumberFormat.NumberDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = ci;
            #endregion

            foreach (XElement xe in xClients.Element("dataroot").Elements("Clients"))
            {// 
                int id = int.Parse(xe.Element("ClientId").Value);
                string name = xe.Element("ClientName").Value;
                string settlementAccount = xe.Element("SettlementAccount")?.Value;
                string city = xe.Element("City")?.Value;
                string address = xe.Element("Address")?.Value;

                Client client = new Client(name, id, settlementAccount, city, address);

                string svalue;

                IEnumerable<XElement> xelement = xe.Elements("Contracts");
                if (xelement != null)
                {
                    foreach (XElement element in xelement)
                    {
                        id = int.Parse(element.Element("ContractId").Value);

                        svalue = element.Element("DateContract")?.Value;

                        if (!DateTime.TryParse(svalue, out DateTime dt))
                            dt = DateTime.MinValue;

                        svalue = element.Element("Number")?.Value;
                        int number = (int)double.Parse(svalue ?? "0");

                        svalue = element.Element("Summ")?.Value;
                        decimal summ = decimal.Parse(svalue ?? "0");

                        bool signed = (int.Parse(element.Element("Signed").Value) == 1);

                        string filename = GetFileName(element.Element("FileName")?.Value);

                        TypeContract tc;
                        if (filename == null)
                            tc = TypeContract.Contract;
                        else
                            tc = (filename.Contains("Договор"))
                                                    ? TypeContract.Contract     // Договор
                                                    : TypeContract.СWC;         // Акт приёмки сдачи работ

                        //
                        //  Здесь загрузить Services
                        //
                        //

                        client.contracts.Add(new Contract(client, id, dt, number, summ, signed, filename, tc));
                    } // end foreach
                }

                clients.Add(client);
            }
        }


        // Выделяем имя файла из строки
        // Примерная строка: "../Договоры/Договор%20№= [Договоры]![НомерЗаказа].xls#..\Договоры%202015\Акт%20%20№289.xls#"
        // возможная строка: "../Договоры/Договор%20№= [Договоры]![НомерЗаказа].xls" - нет имени файла - возвращаем пустую строку
        private string GetFileName(string filename)
        {
            if (filename == null)
                return String.Empty;

            int first = filename.IndexOf('#');
            int last = filename.LastIndexOf('#');

            if (last == -1 || first == last)
                return String.Empty;

            return filename.Substring(first + 1, last - (first + 1));
        }
    }
}
