using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace Clients
{
    // Договор с клиентом.
    // Включает идентификатор, дату, номер, сумму
    // и список оказанных по данному договору услуг
    // Номер договора устанавливается при создании экземпляра
    // Нумерация начинается с 1 с начала года
    public class Contract
    {
        private static int lastYear = DateTime.Now.Year;    // год в последнем договоре с номером lastNumb
        private static int lastId = 1;                      // последний не использованный идентификатор.
                                                                // инкрементируется при создании объекта
        private static int lastNumb = 1;                    // номер последнего договора.
        public int Id { get; set; }                         // Идентификатор договора
        public DateTime Dt { get; set; }                    // дата договора(устанавливается на текущую)
        public int Numb { get; set; }                       // Номер договора
        public decimal Summ { get; set; }                   // Сумма по договору. 
                                                            // Вычисляется из сумм всех оказанных услуг

        private List<Service> services;                     // Список оказанных услуг

        public int IdClient;                                // Для совместимости со старой базой данных.


        public Contract()
        {
            this.Id = lastId++;
            this.Dt = DateTime.Now;
            this.Numb = IncNumb();
            this.Summ = 0;
            services = new List<Service>();
        }

        // этот конструктор нужен для совместимости со старой базой данных,
        // в которой нет списка услуг и есть только общая сумма
        public Contract(int idclient, DateTime dt, int numb, decimal summ)
        {
            this.IdClient = idclient;

            this.Id = lastId++;
            this.Dt = dt;
            this.Numb = numb;
            this.Summ = summ;
            services = new List<Service>();
        }



        // увеличивает номер договора на 1.
        // если это первый договор в году, сбрасывает номер договора на 1
        private int IncNumb()
        {
            int Year = DateTime.Now.Year;
            if (Year != lastYear)
            {
                lastYear = Year;
                lastNumb = 1;
            }

            return lastNumb++;
        }

        public override string ToString()
        {
            return String.Format($"{Dt,-12:d} {Numb,5} {Summ,20:c}");
        }

        // добавить услугу
        public void AddService(Service sv)
        {
            Summ += sv.Nw.Val.Summ;
            services.Add(sv);
        }

        // удалить услугу
        public void DelService(int id)
        {
            int i = services.FindIndex(s => s.Id == id);    // находим индекс данной услуги

            if (i != -1)                                    // если найдена
            {
                Summ -= services[i].Nw.Val.Summ;            // уменьшаем сумму договора на стоимость этой услуги
                services.RemoveAt(i);                       // удаляем
            }
        }
    }
}
