using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;

namespace Clients
{
    public static partial class ExtensionMethods
    {
        // Поиск строки из массива строк, в строке s, начиная с startindex
        public static bool Contains(this string s, string[] sa, int startindex = 0)
        {
            for (int i = startindex; i != s.Length; i++)
            {
                for (int j = 0; j != sa.Length; j++)
                {
                    bool isFound = true;
                    for (int k = 0; k != sa[j].Length; k++)
                    {
                        if (i + k != s.Length && s[i + k] == sa[j][k])
                            continue;

                        isFound = false;
                        break;
                    }

                    if (isFound)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    // Класс предназначен для извлечения списка услуг из таблицы DataTable
    // Таблица заполнена из существующих файлов договоров в формате xls
    public class GetListServicesFromDT
    {
        public readonly Contract contract;
        private readonly DataTable dt;
        private readonly int rowcount;

        private static readonly string[] nameDevices = 
            { "Hp", "hp", "HP", "Ca", "CA", "ca", "Br", "BR", "br", "Le", "LE", "le",
              "Ep", "EP", "ep", "Ri", "RI", "ri", "Xe", "XE", "xe", "Sa", "SA", "sa"};

        private static readonly string[] nameWorks = 
            { "артр", "прин", "апр", "апра", "закре", "пода", "бума", "ПК", "Пк", "пк", "ерсо", "комп", "Комп", "омьп", "емон", "ехнич", "кно",
              "аппа", "обсл", "рогр", "бесп", "рош", "астр", "бсорб", "осст", "шт.", "едукт", "ридж" };

        private static readonly string[] addInfoArray = 
            { "ф/", "Ф/", "Фот", "фот", "Доз", "доз", "чис", "Чис", "нож", "Ч/", "ч/",
              "Вал", "вал", "Маг", "маг", "Т/", "т/", "без", "б/з", "Б/з","ермоп", "амена", "велич", "объ"};

        public GetListServicesFromDT(DataTable dt, Contract contract)
        {
            this.dt = dt;
            rowcount = dt.Rows.Count;

            this.contract = contract;
        }

        public bool GetListServices()
        {
            int i;
            
            // Ищем и извлекаем тип договора и номер
            for (i = 0; i != rowcount; i++)
            {
                if(GetTypeContractAndEtc(dt.Rows[i]))
                    break;
            }

            int NameOfServiceCol = -1,
                SummCol = -1;

            // Определяем номера колонок списка услуг. Начинаем с последней проверенной строки
            for (++i; i < rowcount; i++)
            {
                if (GetNumbColContractServices(dt.Rows[i], out NameOfServiceCol, out SummCol))
                    break;
            }

            // Извлекаем и заполняем список услуг
            return GetContractListServices(++i, NameOfServiceCol, SummCol);
        }

        // Ищем тип договора и номер. Дату не извлекаем. Образцы возможных строк:
        // "Акт приёмки сдачи работ" 	"№"	"6"	"от"	"16 января 2018 г."
        // "Договор" "№"  "1"	"от"	"5 января 2018 г."
        //
        private bool GetTypeContractAndEtc(DataRow dr)
        {
            int count = dr.ItemArray.Length;

            // ищем тип: строки "Акт" или "Договор"
            var tc = TypeContract.None;
            string s;
            int i = -1;
            while (++i < count)
            {
                s = dr.ItemArray[i].ToString();

                if (s?.Length == 0)
                {
                    continue;
                }

                if (s.Contains("Акт"))
                {
                    tc = TypeContract.CWC;
                    break;
                }
                else
                if (s.Contains("Договор"))
                {
                    tc = TypeContract.Contract;
                    break;
                }
            }

            if (i >= count && tc == TypeContract.None)
                return false;       // в этой строке нет искомых подстрок

            // Ищем номер договора. Продолжаем со следующей колонки
            bool isNumb = false;
            while (i < count)
            {
                s = dr.ItemArray[i++].ToString();

                if (s?.Length == 0)
                {
                    continue;
                }

                if (isNumb)
                {
                    if (!int.TryParse(s, out int n))
                        return false;    //не удалось определить номер.   

                    contract.Numb = n;

                    contract.Type = tc;

                    return true;        // завершаем после получения номера.
                }
                else
                if (s.Contains("№"))
                {
                    isNumb = true;
                }
            }

            return false;   // Номер не найден
        }

        // Извлекаем номера колонок списка услуг из строк такого вида:
        // "№" "Наименование работ" "Сумма в бел.р."

        private bool GetNumbColContractServices(DataRow dr, out int NameOfServiceCol, out int SummCol)
        {
            int NumbCol = NameOfServiceCol = SummCol = -1;

            // Определяем номера колонок со списком оказанных услуг
            int count = dr.ItemArray.Length;

            NumbCol = GetNumberCol(dr, -1, count, "№");
            if (NumbCol == -1)
                return false;

            NameOfServiceCol = GetNumberCol(dr, NumbCol, count, "Наименование работ");
            if (NameOfServiceCol == -1)
                return false;

            SummCol = GetNumberCol(dr, NameOfServiceCol, count, "Сумма в бел.р.");
            if (SummCol == -1)
                return false;

            // Номера колонок найдены
            return true;
        }

        // Заполняем список услуг
        private bool GetContractListServices(int indexRow, int NameOfServiceCol, int SummCol)
        {
            Service sv;

            decimal OldSumm = contract.Summ;

            int countRows = dt.Rows.Count;

            // Функция, сбрасывающая результат чтения данных в случае ошибки.
            bool ErrorLoadRow()
            {
                contract.ClearServices();
                contract.Summ = OldSumm;    // Возвращаем старое значение суммы
                return false;
            }

            if (indexRow >= countRows)
            {
                return ErrorLoadRow();
            }

            contract.Summ = 0;

            while ((sv = GetContractService(contract.Client, dt.Rows[indexRow++], NameOfServiceCol, SummCol)) != null)
            {
                contract.AddService(sv); // добавляем услугу в список услуг договора

                if(indexRow >= countRows) // Проверяем, полностью ли прочитан файл
                {
                    return ErrorLoadRow();
                }
            }

            return true;
        }

        [Flags]
        private enum Flags : byte
        {
            None = 0,
            isNumber = 1,
            isSubdivision = 2,
            isAddInfo = 4
        }

        // Извлекаем список услуг из найденных колонок
        // Типичные строки:
        //  "Заправка картриджа Canon 737 (13824)"
        //  "Восстановление картриджа Canon 737 (2 к.) (13855)(доз.нож, без з.)"
        //  "Восстановление картриджа Canon 737 (13855)(доз.нож, без з.)"
        //  "Ремонт картриджа Canon 703 (13775) "
        //  "Ремонт картриджа Canon 703  "
        //  "Ремонт картриджа Canon 703 (фотовал) "
            // Извлекаем из строк:
            //  NameWork - название услуги(напр. "Заправка картриджа")
            //  Subdivision - название подразделения клиента(напр. "к.401")
            //  NameDevice - название устройства(напр. "Canon 725")
            //  Price   - стоимость услуги

        private Service GetContractService(Client cl, DataRow dr, int NameOfServiceCol, int SummCol)
        {
            string s = dr.ItemArray[NameOfServiceCol].ToString();

            string[] sa = { "Итог", "итог" };

            if (s.Length == 0 || s.Contains(sa))    // Итоговая строка.  Список услуг завершён
            {
                return null;
            }

            string[] res = s.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(rs => rs.Trim())
                            .Where(rs => rs != "")
                            .ToArray();

            // выделяем название устройства и название услуги из первой строки

            GetNameWorkAndDevice(res[0], out string namew, out string named);

            string subdiv = "";     //
                                    // для инициализации нельзя использовать null. Это приводит к удалению текущего id
            string addInfo = "";    //

            int numb = 0;

            // Извлекаем из строки номер(порядковый) услуги, название подразделения и дополнительную информацию о услуге
            // А также, возможно, - название устройства

            GetNumb_Subdiv_Addinfo(res, ref numb, ref subdiv, ref addInfo, ref named);

            var culture = new CultureInfo("ru-RU");

            int IdSubdiv = cl.AddSubdivision(subdiv);

            string summ = dr.ItemArray[SummCol]?.ToString().Trim();

            if (string.IsNullOrEmpty(summ))
            {
                summ = "0";
            }

            var value = decimal.Parse(summ, culture.NumberFormat);

            return new Service(namew, named, IdSubdiv, numb, value, -1, addInfo);
        }

        // Извлекаем из строки номер(порядковый) услуги, название подразделения и дополнительную информацию о услуге
        // А также, возможно, - название устройства
        private void GetNumb_Subdiv_Addinfo(string[] s, ref int numb, ref string subdiv, ref string addInfo, ref string named)
        {
            var flag = Flags.None;

            for (int idx = 1; idx < s.Length; idx++)
            {
                if (!flag.HasFlag(Flags.isNumber) && int.TryParse(s[idx], out numb))
                {
                    flag |= Flags.isNumber;  // это номер услуги
                }
                else
                if (s[idx].Contains(addInfoArray))
                {
                    addInfo = s[idx];   // Это дополнительная информация
                }
                else
                if (named.Length == 0 && s[idx].Contains(nameDevices))
                {
                    named = s[idx]; // Это название устройства
                }
                else
                if (!flag.HasFlag(Flags.isSubdivision))
                {
                    subdiv = s[idx]; // Это название подразделения
                    flag |= Flags.isSubdivision;
                }
            }
        }

        // Выделяет из строки название услуги и устройстрва
        private void GetNameWorkAndDevice(string s, out string namew, out string named)
        {
            int index,
                idxStartWord = s.Length;

            for (index = s.Length - 1; index != 0; index--)
            {
                if (s[index] == ' ')
                {
                    // Пропускаем все пробелы кроме одного
                    while (s[index - 1] == ' ')
                        index--;

                    // Проверяем выделенное слово на наличие ключевых подстрок, соответствующих названию услуги, например "картридж".
                    if (s.Contains(nameWorks, index))
                    {
                        // это не название устройства. Это название услуги
                        break;
                    }

                    idxStartWord = index;
                }
            }

            namew = s.Substring(0, idxStartWord).Trim(); // выполненная работа без названия устройства

            named = (idxStartWord == s.Length)  // Строка не содержит название устройства?
                                        ? ""
                                        : s.Substring(idxStartWord, s.Length - idxStartWord).Trim(); // название устройства
        }

        // Ищет колонку в DateRow dr, со строкой isS, начиная с колонки startcol, count колонок
        private int GetNumberCol(DataRow dr, int startcol, int count, string isS)
        {
            while (++startcol < count)
            {
                string s = dr.ItemArray[startcol].ToString();
                if (s == isS)
                {
                    return startcol;
                }
            }

            return -1;
        }
    }
}
