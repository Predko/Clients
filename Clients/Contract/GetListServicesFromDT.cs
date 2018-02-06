using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;

namespace Clients
{
    public class GetListServicesFromDT
    {
        public readonly Contract contract;
        private readonly DataTable dt;
        private int rowcount;

        public GetListServicesFromDT(DataTable dt, Contract contract)
        {
            this.dt = dt;
            rowcount = dt.Rows.Count;

            this.contract = contract;
        }

        public bool GetListServices()
        {
            int i;

            for (i = 0; i != rowcount; i++)
            {
                if(GetTypeContractAndEtc(dt.Rows[i]))   // Ищем и извлекаем тип договора и номер
                    break;
            }

            int NameOfServiceCol = -1,
                SummCol = -1;

            for (++i; i < rowcount; i++)
            {
                // Извлекаем номера колонок списка услуг. Начинаем с последней проверенной строки
                if (GetNumbColContractServices(dt.Rows[i], out NameOfServiceCol, out SummCol))
                    break;
            }

            for (++i; i < rowcount; i++)
            {
                // Извлекаем список услуг
                List<Service> ls = GetContractListServices(i, NameOfServiceCol, SummCol);

                if (ls != null)
                {
                    contract.services = ls;
                    return true;
                }
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

            if (i == count && tc == TypeContract.None)
                return false;       // в этой строке нет искомых подстрок

            contract.Type = tc;

            // Ищем номер договора. Продолжаем со следующей колонки
            bool isNumb = false;
            while (++i < count)
            {
                s = dr.ItemArray[i].ToString();

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

 
        private List<Service> GetContractListServices(int indexRow, int NameOfServiceCol, int SummCol)
        {
            // Заполняем список услуг

            List<Service> services = new List<Service>();

            Service sr;

            while ((sr = GetContractServices(dt.Rows[indexRow++], NameOfServiceCol, SummCol)) != null)
            {
                services.Add(sr);
            }

            return services;
        }

        // Извлекаем список услуг из найденных колонок
        // Типичные строки:
        //  "Заправка картриджа Canon 737 (13824)"
        //  "Восстановление картриджа Canon 737 (13855)(доз.нож, без з.)"
        //  "Ремонт картриджа Canon 703 (13775) (2 к.)"

        private Service GetContractServices(DataRow dr, int NameOfServiceCol, int SummCol)
        {
            // Извлекаем из строк:
            //  NameWork - название услуги(напр. "Заправка картриджа")
            //  Subdivision - название подразделения клиента(напр. "к.401")
            //  NameDevice - название устройства(напр. "Canon 725")
            //  Price   - стоимость услуги

            string s = dr.ItemArray[NameOfServiceCol].ToString();

            if (s?.Length == 0)    // Пустая строка.  Список услуг завершён
                return null;

            string[] res = s.Split(new char[]{ '(', ')'}, StringSplitOptions.RemoveEmptyEntries);

            // выделяем два последних слова из первой строки
            // Это название устройства

            s = res[0].Trim();

            int numbWS = 2; // количество пробелов которое необходимо найти
            int index;

            for (index = s.Length - 1; index != 0 && numbWS != 0; index--)
            {
                if (s[index] == ' ' && --numbWS == 0)
                    break;
            }


            string namew = s.Substring(0, index); // выполненная работа без названия устройства

            string named = s.Substring(index, s.Length - index).Trim(); // название устройства

            string subdiv;

            int numb = -1;

            if (res.Length >= 2)
            {
                subdiv = res[1].Trim();

                if (!int.TryParse(subdiv, out numb)) // возможно это номер заправки?
                {
                    // Нет. Считаем, что это название подразделения
                    // следующий есть и является номером?
                    if (!(res.Length == 3 && int.TryParse(res[2], out numb)))
                        numb = -1; // не номер или его нет
                }
                else
                if (res.Length == 3)
                {
                    subdiv = res[2].Trim();
                }
                else
                    subdiv = null;
            }
            else
            {
                subdiv = null;
            }

            CultureInfo culture = new CultureInfo("ru-RU");

            return new Service(namew, named, subdiv, numb, decimal.Parse(dr.ItemArray[SummCol].ToString().Trim(), culture.NumberFormat));
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
