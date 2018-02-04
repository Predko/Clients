using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

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
            DataRow dr = dt.Rows[0];

            int i;

            for (i = 0; i != rowcount; i++)
            {
                if(GetTypeContractAndEtc(dt.Rows[i]))   // Ищем и извлекаем тип договора и номер
                    break;
            }

            int[] numb_col = null;

            for (++i; i < rowcount; i++)
            {
                // Извлекаем номера колонок списка услуг. Начинаем с последней проверенной строки
                if ((numb_col = GetNumbColContractServices(dt.Rows[i])) != null)
                    break;
            }

            for (++i; i < rowcount; i++)
            {
                // Извлекаем список услуг
                List<Service> ls = GetContractServices(dt.Rows[i], numb_col[1], numb_col[2]);

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

        private int[] GetNumbColContractServices(DataRow dr)
        {
            int NumbCol;
            int NameOfServiceCol;
            int SummCol;

            // Определяем номера колонок со списком оказанных услуг
            int count = dr.ItemArray.Length;
            int currcol = -1;
            NumbCol = GetNumberCol(dr, currcol, count, "№");
            if (NumbCol == -1)
                return null;

            NameOfServiceCol = GetNumberCol(dr, currcol, count, "Наименование работ");
            if (NameOfServiceCol == -1)
                return null;

            SummCol = GetNumberCol(dr, currcol, count, "Сумма в бел.р.");
            if (SummCol == -1)
                return null;

            // Номера колонок найдены

            return new int[] { NumbCol, NameOfServiceCol, SummCol };
        }

        // Извлекаем список услуг из найденных колонок
        // 

        private List<Service> GetContractServices(DataRow dr, int NameOfServiceCol, int SummCo)
        {
            // Заполняем список услуг

            // Извлекаем:
            //  NameWork - название услуги(напр. "Заправка картриджа")
            //  Subdivision - название подразделения клиента(напр. "к.401")
            //  NameDevice - название устройства(напр. "Canon 725")
            //  Price   - стоимость услуги

            string s = dr.ItemArray[NameOfServiceCol].ToString();
            string[] res = s.Split('(', ')');

            //Service sr = new Service()

            return new List<Service>();
        }

        // Ищет колонку в DateRow dr, со строкой isS, начиная с колонки startcol, до колонки (count - 1)
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
