using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clients
{
    // Стоимость услуги с датой изменения цены
    #region struct Price
    public struct Price
    {
        private static int lastId = 1;      // последний не использованный идентификатор.
                                                // инкрементируется при создании объекта
        public int Id { get; set;}          // идентификатор цены
        public decimal Summ { get; set;}    // стоимость работы
        public DateTime Date { get; set;}   // дата изменения цены

        public Price(decimal price, DateTime date)
        {
            this.Id = lastId++;
            this.Summ = price;
            this.Date = date;
        }
    }

    #endregion

    // услуга(работа)
    #region struct NameWork
    public struct NameWork
    {
        private static int lastId = 1;          // последний не использованный идентификатор.
                                                    // инкрементируется при создании объекта
        public int Id { get; set; }             // идентификатор работы

        public string Name { get; set; }        // название выполненной работы, например: "Заправка картриджа"

        public NameWork(string name)
        {
            this.Id = lastId++;
            this.Name = name;
        }
    }
    #endregion

    // подразделение, для которого выполнена работа
    #region struct Subdivision
    public struct Subdivision
    {
        private static int lastId = 1;      // последний не использованный идентификатор.
                                                // инкрементируется при создании объекта
        public int Id { get; set; }         // идентификатор подразделения
        public string Name { get; set; }    // название подразделения, например "к. 410"
                                            // (сокращение от кабинет 410)
        public Subdivision(string name)
        {
            this.Id = lastId++;
            this.Name = name;
        }
    }
    #endregion

    // Название устройства
    #region struct NameDevice
    public struct NameDevice
    {
        private static int lastId = 1;      // последний не использованный идентификатор.
                                                // инкрементируется при создании объекта
        public int Id { get; set; }         // идентификатор устройства
        public string Name { get; set; }    // название (например, "Canon 725")

        public NameDevice(int id, string name)
        {
            this.Id = lastId++;
            this.Name = name;
        }
    }
    #endregion


    // Оказанная услуга/выполненная работа
    #region class Service
    public class Service
    {
        private static int lastId = 0;      // последний не использованный идентификатор.
                                                // инкрементируется при создании объекта
        public int Id { get; set; }         // идентификатор услуги
        public int Number { get; set; }     // порядковый номер выполненной работы, например - 13791
        public NameWork Nw { get; set; }    // услуга/работа - "Заправка картриджа"
        public NameDevice Nd { get; set; }  // устройство    - "Canon 725"
        public Subdivision Sd { get; set; } // подразделение - "(к. 410)"
        public Price Value { get; set; }      // стоимость услуги

        public Service(NameWork nw, NameDevice nd, Subdivision sd, int number, Price value)
        {
            Id = lastId++;

            this.Nw = nw;
            this.Nd = nd;
            this.Sd = sd;
            this.Number = number;
            Value = value;
        }

        // клонируем Service
        public Service Clone() => new Service(Nw, Nd, Sd, Number, Value);
    }
    #endregion





}
