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
using System.Text.RegularExpressions;

namespace Clients
{
    public class ClientsXml
    {
        private XDocument xClients = null;  // xml документ
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

                            new XElement("CommonData",

                                new XElement("ListNameWorks",
                                    Clients.AllNameWorks.Select(n =>
                                    new XElement("NameWork",
                                        new XAttribute("Id", n.Key),
                                        new XElement("Name", n.Value)))),

                                new XElement("ListNameDevices",
                                    Clients.AllNameDevices.Select(n =>
                                    new XElement("NameDevice",
                                        new XAttribute("Id", n.Key),
                                        new XElement("Name", n.Value)))),

                                new XElement("ListAddInfo",
                                    Clients.AllAddInfo.Select(n =>
                                    new XElement("AddInfo",
                                        new XAttribute("Id", n.Key),
                                        new XElement("InfoString", n.Value)))),

                                new XElement("ListServices",
                                    Clients.AllServices.Select(n =>
                                    new XElement("Service",
                                        new XAttribute("Id", n.Key),

                                        new XElement("NameWork",
                                        new XAttribute("Id", n.Value.Nw.Id)),

                                        new XElement("NameDevice",
                                        new XAttribute("Id", n.Value.Nd.Id)),

                                        new XElement("Subdivision",
                                        new XAttribute("Id", n.Value.Sd)),

                                        new XElement("AddInfo",
                                        new XAttribute("Id", n.Value.Ai.Id)),

                                        new XElement("Number", n.Value.Number),

                                        new XElement("Value", n.Value.Value)
                                        )))
                            ),

                            clients.Select(c => new XElement("Client",
                            new XAttribute("ClientId", c.Id),
                            new XElement("ClientName", c.Name),
                            new XElement("SettlementAccount", c.SettlementAccount),
                            new XElement("City", c.City),
                            new XElement("Address", c.Address),

                            new XElement("Subdivisions",
                                c.Subdivisions.Select(sd =>
                                new XElement("Name", sd.Value,
                                    new XAttribute("Id", sd.Key)))),

                            new XElement("Contracts",
                                new XAttribute("Count", c.contracts.Count),
                                c.contracts.Select(t =>
                                new XElement("Contract",
                                    new XAttribute("ContractId", t.Id),
                                    new XElement("DateContract", t.Dt.ToString("d")),
                                    new XElement("Number", t.Numb),
                                    new XElement("Summ", t.Summ),
                                    new XElement("Signed", t.Signed),
                                    new XElement("FileName", t.FileName),
                                    new XElement("Services",
                                        t.services.Select(s =>
                                            new XElement("Service",
                                                new XAttribute("Id", Clients.AllServices[s].Id))
                                                ))
                                        ))
                                    )))
                                ));

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
            // Очищаем  все списки
            Clients.clients.Clear();
            Clients.AllContracts.Clear();
            Clients.AllServices.Clear();


            #region CultureInfo setting
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;

            ci = (CultureInfo)ci.Clone();

            ci.NumberFormat.CurrencyGroupSeparator = " ";
            ci.NumberFormat.NumberDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = ci;
            #endregion

            XElement xelm = xClients.Element("Clients").Element("CommonData");

            // загружаем общие данные, если они есть
            if (xelm != null)
            {
                // Загружаем списки наименований услуг, названий устройств, названий подразделений
                ReadListFromXml(Clients.AllNameWorks, xelm.Element("ListNameWorks")?.Elements());

                ReadListFromXml(Clients.AllNameDevices, xelm.Element("ListNameDevices")?.Elements());

                ReadListFromXml(Clients.AllAddInfo, xelm.Element("ListAddInfo")?.Elements(), "InfoString");

                // Загружаем список всех услуг
                IEnumerable<XElement> xelements = xelm.Element("ListServices")?.Elements();

                if (xelements != null)
                {
                    foreach (XElement xe in xelements)
                    {
                        int id = int.Parse(xe.Attribute("Id").Value);

                        var sv = new Service(Clients.AllNameWorks[int.Parse(xe.Element("NameWork").Attribute("Id").Value)].Name,
                                            Clients.AllNameDevices[int.Parse(xe.Element("NameDevice").Attribute("Id").Value)].Name,
                                            int.Parse(xe.Element("Subdivision").Attribute("Id").Value),
                                            int.Parse(xe.Element("Number").Value),
                                            decimal.Parse(xe.Element("Value").Value),
                                            id,
                                            Clients.AllAddInfo[int.Parse(xe.Element("AddInfo").Attribute("Id").Value)].Name);

                        sv.Add();
                    }
                }
            }

            // Загружаем список клиентов

            foreach (XElement xe in xClients.Element("Clients").Elements("Client"))
            {
                int id = int.Parse(xe.Attribute("ClientId").Value);
                string name = xe.Element("ClientName").Value;
                string settlementAccount = xe.Element("SettlementAccount")?.Value;
                string city = xe.Element("City")?.Value;
                string address = xe.Element("Address")?.Value;

                if(clients.Contains(id))
                {   // Ошибка! Повтор идентификатора в xml файле
                }

                var client = new Client(name, id, settlementAccount, city, address);

                clients.Add(client);

                client.Subdivisions.Clear();

                // загружаем список подразделений данного клиента
                IEnumerable<XElement> xelement = xe.Element("Subdivisions")?.Elements();
                if (xelement != null)
                {
                    foreach (XElement element in xelement)
                    {
                        if(client.Subdivisions.ContainsValue(element.Value))
                        {// Ошибка! Повтор имени подразделения в xml файле
                        }

                        client.AddSubdivision(element.Value, int.Parse(element.Attribute("Id").Value));
                    }
                }

                // загружаем список договоров данного клиента
                xelement = xe.Element("Contracts")?.Elements();
                if (xelement != null)
                {
                    foreach (XElement element in xelement)
                    {
                        id = int.Parse(element.Attribute("ContractId").Value);

                        string svalue = element.Element("DateContract")?.Value;

                        if (!DateTime.TryParse(svalue, out DateTime dt))
                        {
                            dt = DateTime.MinValue;
                        }

                        svalue = element.Element("Number")?.Value;
                        int number = (int)double.Parse(svalue ?? "0");

                        svalue = element.Element("Summ")?.Value;
                        decimal summ = decimal.Parse(svalue ?? "0");

                        bool signed = bool.Parse(element.Element("Signed").Value);

                        var regex = new Regex(@"(\.\.\\)|(/)|(\.\./)|%20");

                        svalue = regex.Replace(element.Element("FileName")?.Value,
                                                (m) =>
                                                {
                                                    if (m.Value == @"..\")
                                                        return @"\";
                                                    if (m.Value == "../")
                                                        return @"\";
                                                    if (m.Value == "%20")
                                                        return " ";
                                                    return @"\";
                                                });

                        TypeContract tc;
                        if (svalue == null)
                        {
                            tc = TypeContract.Contract;
                        }
                        else
                        {
                            tc = (svalue.Contains("Акт"))
                                                   ? TypeContract.CWC     // Акт приёмки сдачи работ
                                                   : TypeContract.Contract;         // Договор
                        }

                        var ctr = new Contract(client, id, dt, number, summ, signed, svalue, tc);

                        client.contracts.Add(ctr);

                        //
                        //  Здесь загружаем Services
                        //
                        IEnumerable<XElement> ServicesElements = element.Element("Services")?.Elements();

                        if (ServicesElements != null)
                        {
                            ctr.services.Clear();

                            foreach (XElement servelem in ServicesElements)
                            {
                                int idService = int.Parse(servelem.Attribute("Id").Value);

                                ctr.services.Add(idService);

                                var sv = Service.Svlist[idService];

                                // Увеличиваем счётчики использования компонентов услуги

                                client.NWCounts.IncrementValue(sv.Nw.Id);
                                client.NDCounts.IncrementValue(sv.Nd.Id);
                                client.AICounts.IncrementValue(sv.Ai.Id);
                            }
                        }
                    }
                }
            }
        }

        private void OnChangeServiceList()
        {
            Clients.ChangedServiceList?.Invoke();
        }

        private bool ReadListFromXml(SortedList<int, NameAndCount> sl, IEnumerable<XElement> xelement, string name = "Name")
        {
            if (xelement != null)
            {
                sl.Clear();
                foreach (XElement element in xelement)
                {
                    sl.Add(int.Parse(element.Attribute("Id").Value), new NameAndCount(element.Element(name).Value));
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        // Загрузка списка клиентов и контрактов из xml документа сформированного MS Access
        public void AccessXmlToClients(ListClients clients)
        {
            /* Закоментировано для варианта с добавлением новых записей 
            // Очищаем  все списки
            Clients.clients.Clear();

            Clients.AllContracts.Clear();
            Clients.AllServices.Clear();
            Clients.AllNameWorks.Clear();
            Clients.AllNameDevices.Clear();
            Clients.AllAddInfo.Clear();
            */

            #region CultureInfo setting
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;

            ci = (CultureInfo)ci.Clone();

            ci.NumberFormat.CurrencyGroupSeparator = " ";
            ci.NumberFormat.NumberDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = ci;
            #endregion

            foreach (XElement xe in xClients.Element("dataroot").Elements("Клиенты"))
            {// 
                int id = int.Parse(xe.Element("КодКлиента").Value);
                string name = xe.Element("НазваниеКомпании").Value;
                string settlementAccount = xe.Element("АдресВыставленияСчета")?.Value;
                string city = xe.Element("Город")?.Value;
                string address = xe.Element("ОбластьКрайРеспублика")?.Value;

                Client client;

                // Проверяем, есть ли клиент. Если нет - создаём
                if (clients.Contains(id))
                {
                    client = clients[id];
                }
                else
                {
                    client = new Client(name, id, settlementAccount, city, address);

                    clients.Add(client);
                }

                string svalue;

                IEnumerable<XElement> xelement = xe.Elements("Договоры");
                if (xelement != null)
                {
                    foreach (XElement element in xelement)
                    {
                        id = int.Parse(element.Element("КодЗаказа").Value);

                        if (Clients.AllContracts.ContainsKey(id))
                        {
                            continue;
                        }

                        svalue = element.Element("ДатаРазмещения")?.Value;

                        if (!DateTime.TryParse(svalue, out DateTime dt))
                            dt = DateTime.MinValue;

                        svalue = element.Element("НомерЗаказа")?.Value;
                        int number = (int)double.Parse(svalue ?? "0");

                        svalue = element.Element("Сумма")?.Value;
                        decimal summ = decimal.Parse(svalue ?? "0");

                        bool signed = (int.Parse(element.Element("Наличие_x0020_договора").Value) == 1);

                        string sfn = GetFileName(element.Element("ФайлДоговора")?.Value);

                        var regex = new Regex(@"(\.\.\\)|(/)|(\.\./)|%20");

                        string filename = regex.Replace(sfn,
                                                (m) => {
                                                    if (m.Value == @"..\")
                                                        return @"\";
                                                    if (m.Value == "../")
                                                        return @"\";
                                                    if (m.Value == "%20")
                                                        return " ";
                                                    return @"\";
                                                });

                        TypeContract tc;
                        if (filename == null)
                        {
                            tc = TypeContract.Contract;
                        }
                        else
                        {
                            tc = (filename.Contains("Договор"))
                                                   ? TypeContract.Contract     // Договор
                                                   : TypeContract.CWC;         // Акт приёмки сдачи работ
                        }

                        var contract = new Contract(client, id, dt, number, summ, signed, filename, tc);

                        // Получаем правильный путь к файлу договора
                        string path = Clients.GetPathFile(contract.FileName, Clients.DefaultPathFile);

                        if(path != null)
                        {
                            contract.LoadServicesFrom_xls(path); // загружаем список услуг договора
                        }

                        client.contracts.Add(contract);
                    }
                }
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
