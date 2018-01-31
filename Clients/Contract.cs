using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace Clients
{
    public enum TypeContract
    {
        Contract,
        СWC         // Сompleted Works Certificate - акт выполненных работ
    }
    
    
    // Договор с клиентом.
    // Включает идентификатор, дату, номер, сумму
    // и список оказанных по данному договору услуг
    // а также ссылку на владельца договора
    // Номер договора устанавливается при создании экземпляра
    // Нумерация начинается с 1 с начала года
    public class Contract
    {
        private static int lastYear = DateTime.Now.Year;    // год в последнем договоре с номером lastNumb
        private static int lastId = 1;                      // последний не использованный идентификатор.
                                                            // инкрементируется при создании объекта
        private static int lastNumb = 1;                    // номер последнего договора.

        public bool Signed;                                 // договор подписан и возвращён = true. Иначе - false
        public int Id { get; set; }                         // Идентификатор договора
        public DateTime Dt { get; set; }                    // дата договора(устанавливается на текущую)
        public int Numb { get; set; }                       // Номер договора
        public decimal Summ { get; set; }                   // Сумма по договору. 
                                                            // Вычисляется из сумм всех оказанных услуг

        public string FileName;                             // имя файла договора

        public TypeContract Type { get; set; }              // тип договора

        public readonly List<Service> services;             // Список оказанных услуг

        public Client Client { get; set; }                  // ссылка на владельца договора


        public Contract()
        {
            this.Signed = false;
            this.Id = lastId++;
            this.Dt = DateTime.Now;
            this.Numb = IncNumb();
            this.Summ = 0;
            this.FileName = String.Empty;
            Type = TypeContract.Contract;
            services = new List<Service>();
        }

        // этот конструктор нужен для совместимости со старой базой данных,
        // в которой нет списка услуг и есть только общая сумма
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
            Summ += sv.Value.Summ;
            services.Add(sv);
        }

        // удалить услугу
        public void DelService(int id)
        {
            int i = services.FindIndex(s => s.Id == id);    // находим индекс данной услуги

            if (i != -1)                                    // если найдена
            {
                Summ -= services[i].Value.Summ;            // уменьшаем сумму договора на стоимость этой услуги
                services.RemoveAt(i);                       // удаляем
            }
        }
        
        // клонируем Contract
        public Contract Clone()
        {
            Contract contract = new Contract(this.Client, this.Id, this.Dt, this.Numb, this.Summ, this.Signed, this.FileName, this.Type);

            foreach(Service service in this.services)
                contract.services.Add(service.Clone());

            return contract;
        }
    }
}
