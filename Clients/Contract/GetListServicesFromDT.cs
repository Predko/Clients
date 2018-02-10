using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;

namespace Clients
{
    // Класс предназначен для извлечения списка услуг из таблицы DataTable
    // Таблица заполнена из существующих файлов договоров в формате xls
    public class GetListServicesFromDT
    {
        public readonly Contract contract;
        private readonly DataTable dt;
        private readonly int rowcount;
        private bool EndListService = false;    // Список услуг прочитан полностью

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
            for (++i; i < rowcount; i++)
            {
                GetContractListServices(i, NameOfServiceCol, SummCol);
                if (EndListService)
                    break;
            }

            return true;
        }

        // Ищем тип договора и номер. Дату не извлекаем. Образцы возможных строк:
        // "Акт приёмки сдачи работ" 	"№"	"6"	"от"	"16 января 2018 г."
        // "Договор" "№"  "1"	"от"	"5 января 2018 г."
        //
        private bool GetTypeContractAndEtc(DataRow dr)
        {
            int count = dr.ItemArray.Length;

            // ищем тип: строки "Акт" или "Договор"
            TypeContract tc = TypeContract.None;
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
                    tc = TypeContract.СWC;
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

            contract.Type = tc;

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
            int currcol = -1;
            NumbCol = GetNumberCol(dr, currcol, count, "№");
            if (NumbCol == -1)
                return false;

            NameOfServiceCol = GetNumberCol(dr, currcol, count, "Наименование работ");
            if (NameOfServiceCol == -1)
                return false;

            SummCol = GetNumberCol(dr, currcol, count, "Сумма в бел.р.");
            if (SummCol == -1)
                return false;

            // Номера колонок найдены

            return true;
        }

        // Заполняем список услуг
        private void GetContractListServices(int indexRow, int NameOfServiceCol, int SummCol)
        {
            Service sr;

            contract.Summ = 0; // обнуляем общую сумму услуг договора

            while ((sr = GetContractServices(dt.Rows[indexRow++], NameOfServiceCol, SummCol)) != null)
            {
                contract.AddService(sr); // добавляем услугу в список услуг договора
            }
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

        private Service GetContractServices(DataRow dr, int NameOfServiceCol, int SummCol)
        {
            // Извлекаем из строк:
            //  NameWork - название услуги(напр. "Заправка картриджа")
            //  Subdivision - название подразделения клиента(напр. "к.401")
            //  NameDevice - название устройства(напр. "Canon 725")
            //  Price   - стоимость услуги

            string s = dr.ItemArray[NameOfServiceCol].ToString();

            if (s.Length == 0
                || s.Contains("Итог")
                || s.Contains("итог"))    // Итоговая строка.  Список услуг завершён
            {
                EndListService = true;
                return null;
            }

            string[] res = s.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(rs => rs.Trim())
                            .Where(rs => rs != "")
                            .ToArray();

            // выделяем два последних слова из первой строки
            // Это название устройства

            s = res[0];

            int numbWS = 2; // количество пробелов которое необходимо найти
            int index;

            for (index = s.Length - 1; index != 0 && numbWS != 0; index--)
            {
                if (s[index] == ' ' && --numbWS == 0)
                    break;
            }


            string namew = s.Substring(0, index); // выполненная работа без названия устройства

            string named = s.Substring(index, s.Length - index).Trim(); // название устройства

            string subdiv = "";     //
                                    // для инициализации нельзя использовать null. Это приводит к удалению текущего id
            string addInfo = "";    //

            int numb = 0;

            Flags flag = Flags.None;

            // Извлекаем из строки номер(порядковый) услуги, название подразделения и дополнительную информацию о услуге
            for (int idx = 1; idx < res.Length; idx++)
            {
                if(!(flag.HasFlag(Flags.isNumber)) && int.TryParse(res[idx], out numb))
                {
                    flag |= Flags.isNumber;  // это номер услуги
                }
                else
                if (!(flag.HasFlag(Flags.isAddInfo)) && IsAddInfo(res[idx]))
                {
                    addInfo = res[idx];
                    flag |= Flags.isAddInfo; // Это дополнительная информация о услуге
                }
                else
                if(!flag.HasFlag(Flags.isSubdivision))
                {
                    subdiv = res[idx]; // Это название подразделения
                    flag |= Flags.isSubdivision;
                }
            }

            CultureInfo culture = new CultureInfo("ru-RU");

            return new Service(namew, named, subdiv, numb, decimal.Parse(dr.ItemArray[SummCol].ToString().Trim(), culture.NumberFormat), -1, addInfo);
        }

        private bool IsAddInfo(string s)
        {
            // подстроки, присутвующие в доп. информации
            string[] sa = { "ф/", "Ф/", "фот", "Доз", "доз", "чис", "Чис", "нож", "Ч/", "ч/", "Вал", "вал", "Маг", "маг", "Т/", "т/", "без","Терм", "терм"};

            foreach(string str in sa)
            {
                if (s.Contains(str))
                    return true;
            }

            return false;
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
