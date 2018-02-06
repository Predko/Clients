using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clients
{
    //// Стоимость услуги с датой изменения цены
    //#region struct Price
    //public struct Price
    //{
    //    private static int lastId = 1;      // последний не использованный идентификатор.
    //                                            // инкрементируется при создании объекта
    //    public int Id { get; set;}          // идентификатор цены
    //    public decimal Summ { get; set;}    // стоимость работы
    //    public DateTime Date { get; set;}   // дата изменения цены

    //    public Price(decimal price, DateTime date)
    //    {
    //        this.Id = lastId++;
    //        this.Summ = price;
    //        this.Date = date;
    //    }
    //}

    //#endregion

    // услуга(работа)
    #region class NameWork
    public class NameWork
    {
        public static List<string> NWlist = new List<string>();      // список всех видов услуг

        public int Id { get; set; }             // идентификатор услуги

        public string Name                      // название выполненной работы, например: "Заправка картриджа"
        {
            get =>  NWlist[Id];
            set
            {
                int id;
                if ((id = NWlist.IndexOf(value)) != -1)
                {
                    Id = id;
                }
                else
                {
                    Id = NWlist.Count;
                    NWlist.Add(value);
                }
            }
        }

        public NameWork(string name)
        {
            Name = name;
        }
    }
    #endregion

    // подразделение, для которого выполнена работа
    #region class Subdivision
    public class Subdivision
    {
        public static List<string> Sdlist = new List<string>();      // список всех подразделений

        public int Id { get; set; }         // идентификатор подразделения

        public string Name                      // название подразделения, например "к. 410"
        {                                       // (сокращение от кабинет 410)
            get => Sdlist[Id];
            set
            {
                int id;
                if ((id = Sdlist.IndexOf(value)) != -1)
                {
                    Id = id;
                }
                else
                {
                    Id = Sdlist.Count;
                    Sdlist.Add(value);
                }
            }
        }

        public Subdivision(string name)
        {
            Name = name;
        }
    }
    #endregion

    // Название устройства
    #region class NameDevice
    public class NameDevice
    {
        public static List<string> NDlist = new List<string>();     // Список всех обслуживаемых устройств

        public int Id { get; set; }         // идентификатор устройства

        public string Name                      // название (например, "Canon 725")
        {
            get => NDlist[Id];
            set
            {
                int id;
                if ((id = NDlist.IndexOf(value)) != -1)
                {
                    Id = id;
                }
                else
                {
                    Id = NDlist.Count;
                    NDlist.Add(value);
                }
            }
        }

        public NameDevice(string name)
        {
            Name = name;
        }
    }
    #endregion


    // Оказанная услуга/выполненная работа
    #region class Service
    public class Service: IComparable<Service>
    {
        private static int lastId = 0;      // последний не использованный идентификатор.
                                                // инкрементируется при создании объекта
        public int Id { get; set; }         // идентификатор услуги
        public int Number { get; set; }     // порядковый номер выполненной работы, например - 13791
        public NameWork Nw { get; set; }    // услуга/работа - "Заправка картриджа"
        public NameDevice Nd { get; set; }  // устройство    - "Canon 725"
        public Subdivision Sd { get; set; } // подразделение - "(к. 410)"
        public decimal Value { get; set; }      // стоимость услуги

        public Service(NameWork nw, NameDevice nd, Subdivision sd, int number, decimal value)
        {
            Id = lastId++;

            Nw = nw;
            Nd = nd;
            Sd = sd;
            Number = number;
            Value = value;
        }

        public Service(string nw, string nd, string sd, int number, decimal value)
        {
            Id = lastId++;

            Nw = new NameWork(nw);
            Nd = new NameDevice(nd);
            Sd = new Subdivision(sd);
            Number = number;
            Value = value;
        }

        // клонируем Service
        public Service Clone() => new Service(Nw, Nd, Sd, Number, Value);

        public int CompareTo(Service other)
        {
            return Id.CompareTo(other.Id);
        }
    }
    #endregion





}
