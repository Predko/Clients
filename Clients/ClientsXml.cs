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
        XDocument xClients = null;
        private bool _load_Ok;

        public bool Load_Ok
        {
            get { return _load_Ok; }
        }

        public ClientsXml(List<Client> clients)
        {
            xClients = ToXml(clients, ""); // namespace = "" 
        }

        public ClientsXml(string filename)
        {
            if (!LoadXml(filename))
                _load_Ok = false;

            _load_Ok = true;
        }


        public XDocument ToXml(List<Client> clients, string ns)
        {
            XNamespace namesp = ns;

            return new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                        new XElement(ns + "Clients",
                        clients.Select(c => new XElement("Client",
                            new XElement("Id", c.id),
                            new XElement("Name", c.name),
                            new XElement("Contracts", new XAttribute("Count", c.contracts.Count),
                                                            c.contracts.Select(t =>
                                                                new XElement("Contract",
                                                                new XElement("Number", t.Numb),
                                                                new XElement("Date", t.Dt.ToString("d")),
                                                                new XElement("Summ", t.Summ),
                                                                new XElement("Services", 
                                                                    t.services.Select(s => 
                                                                    new XElement("Service",
                                                                    new XElement("Id",s.Id),
                                                                    new XElement("Number", s.Number),
                                                                    new XElement("NameWork", 
                                                                       new XElement("Id",s.Nw.Id),
                                                                       new XElement("Name", s.Nw.Name),
                                                                       new XElement("Price",
                                                                          new XElement("Id", s.Nw.Val.Id),
                                                                          new XElement("Summ", s.Nw.Val.Summ),
                                                                          new XElement("Date", s.Nw.Val.Date.ToString("d")))),
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

        public void SaveXml(string filename)
        {
            if (xClients == null)
                return;

            using (StreamWriter sw = new StreamWriter(filename, false))
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.Indent = true;
                xws.IndentChars = "\t";

                using (XmlWriter xw = XmlWriter.Create(sw, xws))
                {
                    xClients.Save(xw);
                }
            }
        }


        // Load filename(xml) to XDocument and fill CollectionClients and CollectionContracts (foreach(Element))
        public bool LoadXml(string filename, ref List<Client> clients)
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

            XmlToClientsAndContracts(ref clients);


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
        public void XmlToClientsAndContracts(ref List<Client> clients)
        {
            clients.Clear();
 
            #region CultureInfo
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;

            ci = (CultureInfo)ci.Clone();

            ci.NumberFormat.CurrencyGroupSeparator = " ";
            ci.NumberFormat.NumberDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = ci;
            #endregion

            foreach (XElement xe in xClients.Element("Clients").Elements("Client"))
            {// 
                int id = int.Parse(xe.Element("Id").Value);
                string name = xe.Element("Name").Value;

                Client client = new Client(name, id);

                foreach (XElement element in xe.Element("Contracts").Elements())
                {
                    DateTime dt = DateTime.Parse(element.Element("Date").Value);
                    decimal summ = decimal.Parse(element.Element("Summ").Value);

                    int number = (int)double.Parse(element.Element("Number").Value);

                    client.contracts.Add(new Contract(id, dt, number, summ));
                }
                clients.Add(client);
            }
        }

        // Fill List clients and BindingList contracts (foreach(Element))
        public void XmlToClientsAndContracts(ref BindingList<Client> clients)
        {
            clients.Clear();

            #region CultureInfo
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;

            ci = (CultureInfo)ci.Clone();

            ci.NumberFormat.CurrencyGroupSeparator = " ";
            ci.NumberFormat.NumberDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = ci;
            #endregion

            foreach (XElement xe in xClients.Element("Clients").Elements("Client"))
            {// 
                int id = int.Parse(xe.Element("Id").Value);
                string name = xe.Element("Name").Value;

                Client client = new Client(name, id);

                foreach (XElement element in xe.Element("Contracts").Elements())
                {
                    DateTime dt = DateTime.Parse(element.Element("Date").Value);
                    decimal summ = decimal.Parse(element.Element("Summ").Value);

                    int number = (int)double.Parse(element.Element("Number").Value);

                    client.contracts.Add(new Contract(id, dt, number, summ));
                }
                clients.Add(client);
            }
        }


    }
}
