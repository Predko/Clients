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
        Contract = 0,
        СWC = 1,         // Сompleted Works Certificate - акт выполненных работ
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

        public static ChangingListServicesEventArgs ChangingLSEventArgs = new ChangingListServicesEventArgs();

        private static int lastYear = DateTime.Now.Year;    // год в последнем договоре с номером lastNumb

        private static int lastNumb = 1;                    // номер последнего договора.

        public bool Signed;                                 // договор подписан и возвращён = true. Иначе - false
        public int Id { get; set; }                         // Идентификатор договора
        public DateTime Dt { get; set; }                    // дата договора(устанавливается на текущую)
        public int Numb { get; set; }                       // Номер договора
        public decimal Summ { get; set; }                   // Сумма по договору. 
                                                            // Вычисляется из сумм всех оказанных услуг

        public string FileName;                             // имя файла договора

        public TypeContract Type { get; set; }              // тип договора

        public List<int> services;                          // Список идентификаторов(ключей) оказанных услуг в данном договоре

        public Client Client { get; set; }                  // ссылка на владельца договора


        public Contract(int id = -1)            // Id получаем от родителя
        {
            this.Signed = false;
            this.Id = id;
            this.Dt = DateTime.Now;
            this.Numb = IncNumb();
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


        private void OnChangingListServices(ChangingListServicesEventArgs e)
        {
            ChangeServiceList?.Invoke(this, e);
        }

        // добавить услугу
        public void AddService(Service sv)
        {
            Summ += sv.Value;

            ChangingLSEventArgs.type = Change.Add;
            ChangingLSEventArgs.service = sv;

            sv.Add();   // добавляем услугу в общий список всех услуг

            OnChangingListServices(ChangingLSEventArgs);

            services.Add(sv.Id); // добавляем Id услуги в список услуг данного договора
        }

        // удалить услугу
        public void DelService(Service sv)
        {
            if (sv.Id != -1)        // если такая услуга есть в списке
            {
                Summ -= sv.Value;            // уменьшаем сумму договора на стоимость этой услуги

                int index = services.IndexOf(sv.Id);

                ChangingLSEventArgs.type = Change.Del;
                ChangingLSEventArgs.index = index;
                ChangingLSEventArgs.service = sv;

                OnChangingListServices(ChangingLSEventArgs);

                services.RemoveAt(index);                 // удаляем
            }
        }

        // клонируем Contract(неполная копия. Список услуг - ссылка)
        public Contract Clone()
        {
            return new Contract(this.Client, this.Id, this.Dt, this.Numb, this.Summ, this.Signed, this.FileName, this.Type)
            {
                services = this.services
            };
        }

        public int CompareTo(Contract other)
        {
            int c = Dt.CompareTo(other.Dt);
            if (c != 0)
                return c;

            return Numb.CompareTo(other.Numb);
        }
    }
}
