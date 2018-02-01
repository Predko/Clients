using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Clients
{
    public partial class Clients: Form
    {
        //
        //      Выбор элемента listBoxContracts
        //
        private void ListBoxContracts_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBoxContracts.SelectedIndex;

            if (i == -1)
                return;

            CurrentContract = (Contract)listBoxContracts.SelectedItem;

            WriteLabelContractInfo();
        }

        // Заполняет listBoxContracts списком договоров
        // при выборе другого клиента
        private void SetClientContracts(Client cl)
        {
            if (cl == null)
                return;

            decimal Summ = 0;

            DateTime d = DateTime.Parse(@"07/01/2016", CultureInfo.CreateSpecificCulture("en-US")); // дата деноминации
 
            // заполняем listBoxContracts списком договоров и подсчитываем общую сумму
            listBoxContracts.Items.Clear();

            foreach (Contract c in cl.contracts)
            {
                SortedInsertItem(listBoxContracts, c);  // Добавляем без нарушения сортировки списка

                Summ += (c.Dt >= d) ? c.Summ
                                    : c.Summ / 10000; // denomination after 07/01/2016
            }

            //
            labelListContractsTotals.Text = String.Format($"Договоров: {cl.contracts.Count,-5}  на сумму: {Summ:C}");

            if (cl.contracts.Count != 0)
            {
                listBoxContracts.SelectedIndex = 0;

                CurrentContract = (Contract)listBoxContracts.SelectedItem;
            }
            else
                CurrentContract = null;

            WriteLabelContractInfo();

        }

        // Вставка нового элемента без нарушения сортировки списка
        private void SortedInsertItem(ListBox lb, Contract ct)
        {
            if (lb.Items.Count != 0)
            {
                for (int i = 0; i < lb.Items.Count; i++)
                {
                    Contract c = (Contract)lb.Items[i];

                    int res = c.CompareTo(ct);

                    if (res == 0)
                        return;     // такой элемент есть. Ничего не делаем

                    if (res > 0)
                    {
                        lb.Items.Insert(i, ct); // найден элемент с большим весом, вставляем новый до него
                        return;
                    }
                }
            }

            lb.Items.Add(ct);   // добавляем, если первый или если не найден больший чем данный
        }

        private void WriteLabelContractInfo()
        {
            labelFileName.Text = CurrentContract?.FileName ?? "Файл отсутствует";

            String numb = CurrentContract?.Numb.ToString() ?? "";

            labelContract.Text = String.Format($"Договор № {numb}");
        }

        public void ChangeContracts(Object sender, ChangedContractsEventArgs e)
        {
            switch (e.change)
            {
                case Change.Add:                            // добавляем элемент в список
                    listBoxContracts.Items.Add(e.contract);
                    break;

                case Change.Clear:                          // очищаем список
                    listBoxContracts.Items.Clear();
                    break;

                case Change.Set:                            // устанавливаем элемент с данным индексом
                    listBoxContracts.Items[e.index] = e.contract;
                    break;
            }
        }

    }
}
