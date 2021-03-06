using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace Clients
{
    public enum TypeContract
    {
        Contract = 0,
        CWC = 1,         // Сompleted Works Certificate - акт выполненных работ
        None
    }

    public class ChangingListServicesEventArgs: EventArgs
    {
        public Change type;
        public Service service;
        public int index;
    }

    public partial class Clients : Form
    {
        // дата деноминации
        public readonly static DateTime dateDenomination = DateTime.Parse(@"07/01/2016", CultureInfo.CreateSpecificCulture("en-US"));
    }

    public class EventOnOff<TeventHandler>
    {
        // Здесь хранится событие и его состояние
        private struct EventElem:IEquatable<EventElem>
        {
            public TeventHandler ev;    // Событие
            public bool isOn;   // состояние события - вкл/выкл

            public EventElem(TeventHandler e)
            {
                ev = e;
                isOn = false;
            }

            bool IEquatable<EventElem>.Equals(EventElem other)
            {
                return ev.Equals(other.ev);
            }
        }

        private TeventHandler eventHandler;

        private bool isOn;

        public TeventHandler EventHand { get => (isOn) ? eventHandler : default(TeventHandler); set => eventHandler = value; }

        private readonly List<EventElem> listEvent = new List<EventElem>();

        public EventOnOff()
        {
            isOn = false;
        }

        public void Add(TeventHandler ev)
        {
            listEvent.Add(new EventElem(ev));
        }

        public void Remove(TeventHandler ev)
        {
            int i = listEvent.IndexOf(new EventElem(ev));

            if(i != -1)
            {
                listEvent.RemoveAt(i);
            }
        }

        public void On(TeventHandler ev)
        {
            EventElem evel = listEvent.First(e => e.ev.Equals(ev));

            evel.isOn = true;
        }


        public void Off(TeventHandler ev)
        {
            EventElem evel = listEvent.First(e => e.ev.Equals(ev));

            evel.isOn = false;
        }
    }


    // Договор с клиентом.
    // Включает идентификатор, дату, номер, сумму
    // и список оказанных по данному договору услуг
    // а также ссылку на владельца договора
    // Номер договора устанавливается при создании экземпляра
    // Нумерация начинается с 1 с начала года
    public class Contract: IComparable<Contract>
    {
        public static event EventHandler<ChangingListServicesEventArgs> ChangeServiceList;  // Событие, вызываемое при изменении списка услуг

        public static ChangingListServicesEventArgs ChangingLS_EventArgs = new ChangingListServicesEventArgs();

        public bool Signed;                                 // договор подписан и возвращён = true. Иначе - false
        public int Id { get; set; }                         // Идентификатор договора
        public DateTime Dt { get; set; }                    // дата договора(устанавливается на текущую)
        public int Numb { get; set; }

        public decimal Summ { get; set; }                   // Сумма по договору. 
                                                            // Вычисляется из сумм всех оказанных услуг

        public string FileName;                             // имя файла договора

        public TypeContract Type { get; set; }              // тип договора

        public List<int> services;                          // Список идентификаторов(ключей) оказанных услуг в данном договоре

        public Client Client { get; set; }      // ссылка на владельца договора


        public Contract(int id = -1)            // Id получаем при создании или будет получен позже, при добавлении в список
        {
            this.Signed = false;
            this.Id = id;
            this.Dt = DateTime.Now;
            this.Numb = Contracts.LastNumberContract;   // Получаем последний неиспользованный номер договора
            this.Summ = 0;
            this.FileName = String.Empty;
            Type = TypeContract.Contract;
            services = new List<int>();
        }

        public Contract(Client client, int Id, DateTime dt, int numb, decimal summ, bool signed = false, 
                                    string FileName = "", TypeContract type = TypeContract.Contract)
        {
            this.Client = client;
            this.Id = Id;
            this.Dt = dt;
            this.Numb = numb;
            this.Summ = summ;
            this.Signed = signed;
            this.FileName = FileName;
            this.Type = type;

            services = new List<int>();
        }

        public override string ToString()
        {
            decimal summ = (Dt >= Clients.dateDenomination) ? Summ
                                                    : Summ / 10000; // denomination after 07/01/2016

            return String.Format($"{Dt,-12:d} {Numb,5} {summ,20:c}");
        }


        private void OnChangingListServices(ChangingListServicesEventArgs e)
        {
            ChangeServiceList?.Invoke(null, e);
        }

        // добавить услугу
        public void AddService(Service sv)
        {
#if !DEBUG
            if (sv == null) // Добавляем только в релизе. В отладке ловим ошибки
                return;
#endif

            Summ += sv.Value;

            ChangingLS_EventArgs.type = Change.Add;
            ChangingLS_EventArgs.service = sv;

            sv.Add();   // добавляем услугу в общий список всех услуг

            OnChangingListServices(ChangingLS_EventArgs);

            services.Add(sv.Id); // добавляем Id услуги в список услуг данного договора
        }

        // удалить услугу
        public void DelService(Service sv)
        {
            int id = services.IndexOf(sv.Id);
#if DEBUG
            if (id == -1) // Ловим ошибки при отладке
            {
            }
#endif

            if (id != -1)        // если такая услуга есть в списке
            {
                Summ -= sv.Value;            // уменьшаем сумму договора на стоимость этой услуги

                ChangingLS_EventArgs.type = Change.Del;
                ChangingLS_EventArgs.index = sv.Id;
                ChangingLS_EventArgs.service = sv;

                OnChangingListServices(ChangingLS_EventArgs);

                sv.Remove();            // удаляем из списка всех услуг
                services.Remove(sv.Id); // удаляем
            }
        }

        // Обновляем значения услуги новыми данными
        public void SetService(string nw, string nd, int sd, int number, decimal value, int id, string ai)
        {

            int index = Clients.AllServices.IndexOfKey(id);
            if (index != -1)
            {
                var sv = Clients.AllServices.Values[index];

                Summ += value - sv.Value;   // Корректируем общую сумму

                sv.SetService(nw, nd, sd, number, value, ai);
            }
        }

        // Очищаем список услуг услуг договора
        public void ClearServices()
        {
            // удаление услуг из списка, начиная с конца списка, чтобы не нарушить индексацию списка
            for (int i = services.Count - 1; i >= 0; i--)
            {
                int id = services[i];

                if (Clients.AllServices.ContainsKey(id))
                {
                    DelService(Clients.AllServices[id]);
                }
            }

            Summ = 0;
        }

        public bool LoadServicesFrom_xls(string path)
        {
            var xls = new GetContractInfoFromXls(path + FileName, ModeGetData.OLEDB);

            if (xls.Dt == null) // Не удалось загрузить информацию
                return false;

            var gls = new GetListServicesFromDT(xls.Dt, this);

            return gls.GetListServices(); // если false - чтение списка услуг не удалось
        }

        // клонируем Contract(неполная копия. Список услуг - ссылка)
        public Contract Clone() => new Contract(Client, Id, Dt, Numb, Summ, Signed, FileName, Type) { services = this.services };

        public int CompareTo(Contract other)
        {
            int c = Dt.CompareTo(other.Dt);
            if (c != 0)
                return c;

            return Numb.CompareTo(other.Numb);
        }
    }

    //public class SortedContractDescending: IComparer<Contract>
    //{
    //    int IComparer<Contract>.Compare(Contract x, Contract y)
    //    {
    //        int c = x.Dt.CompareTo(y.Dt);
    //        if (c != 0)
    //            return c;

    //        return x.Numb.CompareTo(y.Numb);
    //    }
    //}
}
