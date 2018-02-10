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

        public static SortedList<int, string> AllNameWorks = new SortedList<int, string>();     // список всех видов услуг

        public static SortedList<int, string> AllSubdivisions = new SortedList<int, string>();  // список всех подразделений

        public static SortedList<int, string> AllNameDevices = new SortedList<int, string>();   // Список всех обслуживаемых устройств

        public static SortedList<int, string> AllAddInfo = new SortedList<int, string>();   // Список всех видов дополнительной информации о услуге

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

    }

    // услуга(работа)
    // При создании экземпляра классов - NameWork,Subdivision,NameDevice
    // свойство Name, добавляет имя к списку имён данных классов(наименование работ, подразделения, названия устройств),
    // если его ещё нет в списке, и присваевает Id экземпляра, соответствующему ключу списка
    // при попытке присвоить экземпляру имя = null, Имя удаляется из списка.
    #region class NameWork
    public class NameWork : IComparable<NameWork>
    {
        public static SortedList<int, string> NWlist => Clients.AllNameWorks;      // список всех видов услуг

        public int Id { get; set; }             // идентификатор услуги

        public string Name                      // название выполненной работы, например: "Заправка картриджа"
        {
            get => NWlist[Id];
            set
            {
                if (value == null)
                {
                    if (Id != 0)
                    {
                        NWlist.Remove(Id); // удаляем имя из списка
                        return;
                    }

                    value = ""; // обрабатываем как пустую строку.
                }

                int i;
                if ((i = NWlist.IndexOfValue(value)) != -1) // если такое значение есть
                {
                    Id = NWlist.Keys[i]; // устанавливаем Id на Id найденного значения
                    return;
                }
                else
                { // если такого значения нет
                    Id = 1; // ищем все свободные ключи
                    while (NWlist.Keys.Contains(Id))    // если такой ключ уже есть - увеличиваем и проверяем опять
                        Id++;

                    NWlist.Add(Id, value);  // Добавляем новое значение в список
                }
            }
        }

        public NameWork(string name)
        {
            Name = name;
        }

        public NameWork(int id) => Id = id;     // Имя уже должно быть в списке!

        public int CompareTo(NameWork other) => Clients.Compare(Id, other.Id);
    }
    #endregion

    // подразделение, для которого выполнена работа
    #region class Subdivision
    public class Subdivision : IComparable<Subdivision>
    {
        public static SortedList<int, string> Sdlist => Clients.AllSubdivisions;      // список всех подразделений

        public int Id { get; set; }             // идентификатор подразделения

        public string Name                      // название подразделения, например "к. 410"
        {                                       // (сокращение от кабинет 410)
            get => Sdlist[Id];
            set
            {
                if (value == null)
                {
                    if (Id != 0)
                    {
                        Sdlist.Remove(Id); // удаляем имя из списка
                        return;
                    }

                    value = ""; // обрабатываем как пустую строку.
                }

                int i;
                if ((i = Sdlist.IndexOfValue(value)) != -1) // если такое значение есть
                {
                    Id = Sdlist.Keys[i]; // устанавливаем Id на Id найденного значения
                    return;
                }
                else
                { // если такого значения нет
                    Id = 1; // ищем все свободные ключи
                    while (Sdlist.Keys.Contains(Id))    // если такой ключ уже есть - увеличиваем и проверяем опять
                        Id++;

                    Sdlist.Add(Id, value);  // Добавляем новое значение в список
                }
            }
        }

        public Subdivision(string name)
        {
            Name = name;
        }

        public Subdivision(int id) => Id = id;

        public int CompareTo(Subdivision other) => Clients.Compare(Id, other.Id);
    }
    #endregion

    // Название устройства
    #region class NameDevice
    public class NameDevice : IComparable<NameDevice>
    {
        public static SortedList<int, string> NDlist => Clients.AllNameDevices;     // Список всех обслуживаемых устройств

        public int Id { get; set; }         // идентификатор устройства

        public string Name                      // название (например, "Canon 725")
        {
            get => NDlist[Id];
            set
            {
                if (value == null)
                {
                    if (Id != 0)
                    {
                        NDlist.Remove(Id); // удаляем имя из списка
                        return;
                    }

                    value = ""; // обрабатываем как пустую строку.
                }

                int i;
                if ((i = NDlist.IndexOfValue(value)) != -1) // если такое значение есть
                {
                    Id = NDlist.Keys[i]; // устанавливаем Id на Id найденного значения
                    return;
                }
                else
                { // если такого значения нет
                    Id = 1; // ищем все свободные ключи
                    while (NDlist.Keys.Contains(Id))    // если такой ключ уже есть - увеличиваем и проверяем опять
                        Id++;

                    NDlist.Add(Id, value);  // Добавляем новое значение в список
                }
            }
        }

        public NameDevice(string name)
        {
            Name = name;
        }

        public NameDevice(int id) => Id = id;

        public int CompareTo(NameDevice other) => Clients.Compare(Id, other.Id);
    }
    #endregion


    // Дополнительная информация о услуге
    #region class AddInfo
    public class AddInfo : IComparable<AddInfo>
    {
        public static SortedList<int, string> AIlist => Clients.AllAddInfo;     // Список всех видов дополнительной информации о услуге

        public int Id { get; set; }                     // идентификатор информации

        public string InfoString                        // значение, например: "фотовал, доз. нож, без запр."
        {
            get => AIlist[Id];
            set
            {
                if (value == null)
                {
                    if(Id != 0)
                    {
                        AIlist.Remove(Id); // удаляем имя из списка
                        return;
                    }

                    value = ""; // обрабатываем как пустую строку.
                }

                int i;
                if ((i = AIlist.IndexOfValue(value)) != -1) // если такое значение есть
                {
                    Id = AIlist.Keys[i]; // устанавливаем Id на Id найденного значения
                    return;
                }
                else
                { // если такого значения нет
                    Id = 1; // ищем все свободные ключи
                    while (AIlist.Keys.Contains(Id))    // если такой ключ уже есть - увеличиваем и проверяем опять
                        Id++;

                    AIlist.Add(Id, value);  // Добавляем новое значение в список
                }
            }
        }

        public AddInfo(string info)
        {
            InfoString = info;
        }

        public AddInfo(int id) => Id = id;

        public int CompareTo(AddInfo other) => Clients.Compare(Id, other.Id);
    }
    #endregion

    // Класс для хранения освобождённых в результате удаления, идентификаторов
    // а также для получения свободного id
    // при удалении услуги, удаляется id и его присваиваем свойству Id этого класса
    // Для получения уникального id, достаточно извлечь его из свойства Id этого класса
    public class FreeServiceID
    {
        private readonly List<int> freeId = new List<int>();

        private int LastId = 0;

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
    public class Service : IComparable<Service>
    {
        public static SortedList<int, Service> Svlist => Clients.AllServices; // Спискок всех услуг, всех клиентов

        public static FreeServiceID LastfreeId = new FreeServiceID(); // Последний свободный id. 
                                                                      // Используется в качестве ключа при добавлении новой услуги

        public int Id { get; set; }         // идентификатор услуги
        public int Number { get; set; }     // порядковый номер выполненной работы, например - 13791
        public NameWork Nw { get; set; }    // услуга/работа - "Заправка картриджа"
        public NameDevice Nd { get; set; }  // устройство    - "Canon 725"
        public Subdivision Sd { get; set; } // подразделение - "(к. 410)"
        public AddInfo Ai { get; set; }     // дополнительная информации о услуге
        public decimal Value { get; set; }  // стоимость услуги

        public Service(NameWork nw, NameDevice nd, Subdivision sd, int number, decimal value, int id = -1, AddInfo ai = null)
                            : this(nw.Name, nd.Name, sd.Name, number, value, id, ai?.InfoString)
        {
        }

        // При создании нового экземпляра, устанавливаем id = -1
        // Id устанавливает операция Service.Add() - добавления в список услуг

        public Service(string nw, string nd, string sd, int number, decimal value, int id = -1, string ai = "")
        {
            Nw = new NameWork(nw);
            Nd = new NameDevice(nd);
            Sd = new Subdivision(sd);
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
                Id = LastfreeId.Id;  // Добавляем новую услугу в список со свободным id.

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

        // Сравниваем по номеру услуги
        public int CompareTo(Service other)
        {
            return Number.CompareTo(other.Number);
        }
    }
    #endregion
}

