using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace Clients
{
    public partial class Clients : Form
    {
        public static SortedList<int, Service> AllServices = new SortedList<int, Service>();    // Спискок всех услуг, всех клиентов

        public static SortedList<int, NameAndCount> AllNameWorks = new SortedList<int, NameAndCount>() { { 0, new NameAndCount("", 0) } };     // список всех видов услуг

        public static SortedList<int, NameAndCount> AllNameDevices = new SortedList<int, NameAndCount>() { { 0, new NameAndCount("", 0) } };   // Список всех обслуживаемых устройств

        public static SortedList<int, NameAndCount> AllAddInfo = new SortedList<int, NameAndCount>() { { 0, new NameAndCount("", 0) } };   // Список всех видов дополнительной информации о услуге

        public static Action<int> RemovedDgvRows;     // Метод, вызываемый при удалении строк в dataGridViewContract.

        public static Action ChangedServiceList;     // Метод, вызываемый при изменении списков видов услуг, устройств, подразделений, доп. информации.

        public static int Compare(int t1, int t2)
        {
            int r = t1 - t2;
            if (r < 0)
                return -1;
            else
            if (r > 0)
                return 1;

            return 0;
        }

        public void RemoveService(int id)
        {
            if (id == -1)    // удаление строки, не занесённой в список услуг(во время редактирования)
                return;

            Service sv = AllServices[id];

            if (sv != null)
                CurrentContract.DelService(sv); // Если найдена, удаляем из списка услуг
        }
    }

    #region class NameService

    public class NameAndCount : IEquatable<NameAndCount>
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public NameAndCount(string name, int count = 0)
        {
            Name = name;
            Count = count;
        }

        public bool Equals(NameAndCount other)
        {
            return Name.Equals(other.Name);
        }
    }


    public class IncrCountNameServiceEventArgs : EventArgs
    {
        public int Id;

        public IncrCountNameServiceEventArgs(int id)
        {
            Id = id;
        }
    }

    public abstract class NameService : IComparable<NameService>, IEquatable<NameService>
    {
        public abstract SortedList<int, NameAndCount> List { get; }     // список наименований услуг

        // Вызывается при увеличения счётчика использования имени услуги/устройства/доп информации
        public static EventHandler<IncrCountNameServiceEventArgs> IncrCountNameService;

        public int Id { get; set; }             // идентификатор услуги

        public string Name                      // название услуги, например: "Заправка картриджа"
        {
            get => List[Id].Name;
            set
            {
                if (value == null)
                {
                    if (Id != 0)
                    {
                        List.Remove(Id); // удаляем имя из списка
                        return;
                    }

                    value = ""; // обрабатываем как пустую строку.
                }

                int i;
                NameAndCount nc = new NameAndCount(value);
                if ((i = List.IndexOfValue(nc)) != -1) // если такое значение есть
                {
                    Id = List.Keys[i];  // устанавливаем Id на Id найденного значения
                    List[Id].Count++;    // увеличиваем счётчик использования данного имени
                    OnIncCountNameService(Id);
                }
                else
                { // если такого значения нет
                    Id = 1; // ищем все свободные ключи
                    while (List.Keys.Contains(Id))    // если такой ключ уже есть - увеличиваем и проверяем опять
                        Id++;

                    nc.Count = 1;
                    List.Add(Id, nc);  // Добавляем новое значение в список
                }
            }
        }

        // Вызов события увеличения счётчика NameService
        private void OnIncCountNameService(int id)
        {
            IncrCountNameService?.Invoke(this, new IncrCountNameServiceEventArgs(id));
        }

        public int CompareTo(NameService other) => Id.CompareTo(other.Id);

        public static bool operator ==(NameService a, NameService b)
        {
            return a.Id == b.Id;
        }

        public static bool operator !=(NameService a, NameService b)
        {
            return a.Id != b.Id;
        }

        public bool Equals(NameService other)
        {
            return Id == other.Id;
        }

        public override bool Equals(Object obj)
        {
            return Equals((NameService)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    #endregion

// услуга(работа)
// При создании экземпляра классов - NameWork,Subdivision,NameDevice
// свойство Name, добавляет имя к списку имён данных классов(наименование работ, подразделения, названия устройств),
// если его ещё нет в списке, и присваевает Id экземпляра, соответствующему ключу списка
// при попытке присвоить экземпляру имя = null, Имя удаляется из списка.
    #region class NameWork
	public class NameWork: NameService
	{
        public override SortedList<int, NameAndCount> List => Clients.AllNameWorks;      // список всех видов услуг

        public NameWork(string name)
        {
            Name = name;
        }

        public NameWork(int id) => Id = id;     // Имя уже должно быть в списке!
    }

    #endregion

    // Название устройства
    #region class NameDevice
    public class NameDevice : NameService
    {
        public override SortedList<int, NameAndCount> List => Clients.AllNameDevices;      // список всех видов услуг

        public NameDevice(string name)
        {
            Name = name;
        }

        public NameDevice(int id) => Id = id;     // Имя уже должно быть в списке!
    }
    #endregion


    // Дополнительная информация о услуге
    #region class AddInfo
    public class AddInfo : NameService
    {
        public override SortedList<int, NameAndCount> List => Clients.AllAddInfo;      // список всех видов услуг

        public AddInfo(string info)
        {
            Name = info;
        }

        public AddInfo(int id) => Id = id;
    }
    #endregion

    // Класс для хранения освобождённых в результате удаления, идентификаторов
    // а также для получения свободного id
    // при удалении услуги, удаляется id и его присваиваем свойству Id этого класса
    // Для получения уникального id, достаточно извлечь его из свойства Id этого класса
    public class FreeID
    {
        private readonly List<int> freeId = new List<int>();

        private int LastId = 0;         // последний использованный идентификатор

        public const int ClearListId = -1,  // выполнить очистку списка?
                         IndexNotFound = -1;

        public void SetLastId(int id)   // устанавливаем последний неиспользованный id. В -1 - при очистке списка, или в максимальное значение
        {                               // Используем это при загрузке списка с уже установленными id
            if (id == ClearListId)
            {
                freeId.Clear(); // при установке идентификатора в 0 - очищаем список свободных Id
            }

            if(id <= LastId)
            {
                return;
            }

            LastId = id;
        }

        public int Id
        {
            get
            {
                int count = freeId.Count;
                if (count != 0) // если список свободных id не пуст, берём оттуда id и удаляем его из списка
                {
                    int id = freeId[count - 1];
                    freeId.RemoveAt(count - 1);
                    return id;
                }

                return ++LastId; // Иначе, возвращаем последний свободный id
            }

            set // присваиваем этому свойству освободившийся id
            {
                if (!freeId.Contains(value)) // если такого id в списке нет,
                {
                    freeId.Add(value);  // добавляем
                }
            }
        }
    }

    // Оказанная услуга/выполненная работа
    #region class Service
    public class Service : IComparable<Service>, IEquatable<Service>
    {
        public static SortedList<int, Service> Svlist => Clients.AllServices; // Спискок всех услуг, всех клиентов

        public static FreeID LastfreeId = new FreeID(); // Последний свободный id. 
                                                                      // Используется в качестве ключа при добавлении новой услуги

        public int Id { get; set; }         // идентификатор услуги
        public int Number { get; set; }     // порядковый номер выполненной работы, например - 13791
        public NameWork Nw { get; set; }    // услуга/работа - "Заправка картриджа"
        public NameDevice Nd { get; set; }  // устройство    - "Canon 725"
        public int Sd { get; set; }         // id подразделения
        public AddInfo Ai { get; set; }     // дополнительная информации о услуге
        public decimal Value { get; set; }  // стоимость услуги

        public Service(NameWork nw, NameDevice nd, int sd, int number, decimal value, int id = -1, AddInfo ai = null)
                            : this(nw.Name, nd.Name, sd, number, value, id, ai?.Name)
        {
        }

        // При создании нового экземпляра, устанавливаем id = -1
        // Id устанавливает операция Service.Add() - добавления в список услуг

        public Service(string nw, string nd, int sd, int number, decimal value, int id = -1, string ai = "")
        {
            Nw = new NameWork(nw);
            Nd = new NameDevice(nd);
            Sd = sd;
            Ai = new AddInfo(ai);
            Number = number;
            Value = value;
            Id = id;
        }

        public Service(int id)
        {
            // Если в списке услуг есть услуга с таким id, копируем её в создаваемый объект
            if (Svlist.Keys.Contains(id))
            {
                Service sv = Svlist[id];
                Id = id;

                Nw = sv.Nw;
                Nd = sv.Nd;
                Sd = sv.Sd;
                Ai = sv.Ai;
                Number = sv.Number;
                Value = sv.Value;
            }
            else   // если нет, экземпляр не инициализируется, т.к. неизвестны значения полей
            {

                Id = -1; // неинициализованный экземпляр
            }
        }

        // клонируем Service
        public Service Clone()
        {
            return new Service(Id);
        }

        public void Add()
        {
            // Добавляем услугу
            if (Id == -1)
            {
                int res = Svlist.IndexOfValue(this);    // такая услуга есть?
                if (res == -1)
                {
                    Id = LastfreeId.Id;  // Нет. Добавляем новую услугу в список со свободным id.
                }
                else
                {
                    Id = Svlist.Keys[res];  // такая услуга есть, меняем this.Id на id найденной услуги
                    return;
                }
            }
            else
            {
                LastfreeId.SetLastId(Id);   // меняем последний использованный на Id
            }

            Svlist.Add(Id, this);   // иначе, добавляем с Id, присвоенным данному экземпляру
        }

        // Удаляем услугу
        public void Remove()
        {
            if (Id != -1 && Svlist.Keys.Contains(Id))
            {
                LastfreeId.Id = Id; // При удалении, освободившийся идентификатор помещаем в свободные идентификаторы

                Svlist.Remove(Id);  // Удаляем услугу из списка.
            }
        }

        // Меняет значения свойств Service, на переданные. 
        public void SetService(string nw, string nd, int sd, int number, decimal value, string ai)
        {
            Nw.Name = nw;
            Nd.Name = nd;
            Sd = sd;
            Number = number;
            Value = value;
            Ai.Name = ai;
        }

        // Сравниваем по номеру услуги
        public int CompareTo(Service other)
        {
            int res = (Number.CompareTo(other.Number) * 10000)
                    + (Sd.CompareTo(other.Sd) * 1000)
                    + (Nw.CompareTo(other.Nw) * 100)
                    + (Nd.CompareTo(other.Nd) * 10)
                    + Value.CompareTo(other.Value);

            if (res > 0)
                return 1;
            else
            if (res < 0)
                return -1;

            return 0;
        }

        public bool Equals(Service other)
        {
            return  Number == other.Number
                    && Sd == other.Sd
                    && Nw == other.Nw
                    && Nd == other.Nd
                    && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals((Service)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    #endregion
}

