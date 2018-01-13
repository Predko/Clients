using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clients
{
    // Стоимость услуги с датой изменения цены
    #region class Price
    public class Price
    {
        public static int lastId = 1;       // последний не использованный идентификатор.
                                         // инкрементируется при создании объекта
        public int id { get; set;}          // идентификатор цены
        public decimal price { get; set;}   // стоимость работы
        public DateTime date { get; set;}   // дата изменения цены

        public Price(decimal price, DateTime date)
        {
            this.id = lastId++;
            this.price = price;
            this.date = date;
        }
    }

    #endregion

    // услуга(работа)
    #region class NameWork
    public class NameWork
    {
        public static int lastId = 1;           // последний не использованный идентификатор.
                                             // инкрементируется при создании объекта
        public int id { get; set; }             // идентификатор работы

        public string name { get; set; }        // название выполненной работы, например: "Заправка картриджа"

        public Price price { get; set; }        // стоимость услуги

        public NameWork(string name, Price price)
        {
            this.id = lastId++;
            this.name = name;
            this.price = price;
        }
    }
    #endregion

    // подразделение, для которого выполнена работа
    #region class Subdivision
    public class Subdivision
    {
        public static int lastId = 1;       // последний не использованный идентификатор.
                                         // инкрементируется при создании объекта
        public int id { get; set; }         // идентификатор подразделения
        public string name { get; set; }    // название подразделения, например "к. 410"
                                            // (сокращение от кабинет 410)
        public Subdivision(string name)
        {
            this.id = id;
            this.name = name;
        }
    }
    #endregion

    // Название устройства
    #region class NameDevice
    public class NameDevice
    {
        public static int lastId = 1;       // последний не использованный идентификатор.
                                         // инкрементируется при создании объекта
        public int id { get; set; }         // идентификатор устройства
        public string name { get; set; }    // название (например, "Canon 725")

        public NameDevice(int id, string name)
        {
            this.id = lastId++;
            this.name = name;
        }
    }
    #endregion


    // Оказанная услуга/выполненная работа
    #region class Service
    public class Service
    {
        public static int lastId = 1;       // последний не использованный идентификатор.
                                         // инкрементируется при создании объекта
        public int id { get; set; }         // идентификатор услуги
        public NameWork nw { get; set; }    // услуга/работа - "Заправка картриджа"
        public NameDevice nd { get; set; }  // устройство    - "Canon 725"
        public Subdivision sb { get; set; } // подразделение - "(к. 410)"
        public int number { get; set; }     // порядковый номер выполненной работы, например - 13791

        public Service(NameWork nw, NameDevice nd, Subdivision sd, int number)
        {
            id = lastId++;

            this.nw = nw;
            this.nd = nd;
            this.sb = sb;
            this.number = number;
        }
    }
    #endregion





}
