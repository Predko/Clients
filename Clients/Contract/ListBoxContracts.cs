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
//------------------------------------  Методы -----------------------------------------
//
        private void InitListBoxContracts()
        {
            listBoxContracts.SelectedIndexChanged += ListBoxContracts_SelectedIndexChanged;

            // при изменении текущего договора выводить информацию о нём
            ChangeCurrentContract_EventHandler += (o, e) => WriteLabelContractInfo();

            // при изменении текущего клиента, вызываем это событие и отображаем список договоров для него
            ChangedCurrentClient_EventHandler += SetClientContracts;
        }

        // обрабатываем подключённые события после изменения текущего договора
        private void OnChangeCurrentContractInfo(EventArgs eventArgs)
        {
            var temp = ChangeCurrentContract_EventHandler;

            temp?.Invoke(this, eventArgs);
        }

        //
        //      Выбор элемента listBoxContracts
        //
        private void ListBoxContracts_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBoxContracts.SelectedIndex;

            if (i == -1)
                return;

            CurrentContract = (Contract)listBoxContracts.SelectedItem;
        }

        // Заполняет listBoxContracts списком договоров
        // при выборе другого клиента
        private void SetClientContracts(Object sender, EventArgs e)
        {
            if (CurrentClient == null)
            {
                // Выводим информацию о количестве и общей сумме договоров
                labelListContractsTotals.Text = String.Format($"Договоров: {0,-5}  на сумму: {0:C}");
                return;
            }

            listBoxContracts.BeginUpdate(); // Приостанавливаем обновление в listBox

            decimal Summ = 0;

            DateTime d = DateTime.Parse(@"07/01/2016", CultureInfo.CreateSpecificCulture("en-US")); // дата деноминации
 
            // заполняем listBoxContracts списком договоров и подсчитываем общую сумму
            listBoxContracts.Items.Clear();

            foreach (Contract c in CurrentClient.contracts)
            {
                SortedInsertItem(listBoxContracts, c);  // Добавляем без нарушения сортировки списка

                Summ += (c.Dt >= d) ? c.Summ
                                    : c.Summ / 10000; // denomination after 07/01/2016
            }

            // Выводим информацию о количестве и общей сумме договоров
            labelListContractsTotals.Text = String.Format($"Договоров: {CurrentClient.contracts.Count,-5}  на сумму: {Summ:C}");

            // если список договоров пуст, устанавливаем текущий в null
            if (CurrentClient.contracts.Count == 0)
                CurrentContract = null;
            else // иначе выбираем первый в списке listBoxContracts
                listBoxContracts.SelectedIndex = 0;

            listBoxContracts.EndUpdate();   // обновляем список договоров в listBox
        }

        // Вставка нового элемента без нарушения сортировки списка
        private void SortedInsertItem(ListBox lb, Contract ct)
        {
            if (lb.Items.Count != 0)
            {
                for (int i = 0; i < lb.Items.Count; i++)
                {
                    var c = (Contract)lb.Items[i];

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

        // Выводит информацию о договоре на основную форму и заголовок tabPageContractEdit
        private void WriteLabelContractInfo()
        {
            labelFileName.Text = CurrentContract?.FileName ?? "Файл отсутствует";

            string numb = CurrentContract?.Numb.ToString() ?? "";

            string typeContract;
            if (CurrentContract != null)
            {
                typeContract = (CurrentContract.Type == TypeContract.CWC) ? "Акт"
                                                                          : "Договор";
            }
            else
            {
                typeContract = "Договор";
            }

            labelContract.Text = tabPageContractEdit.Text = $"{typeContract} № {numb}";
        }

        public void LbChangeContracts(Object sender, ChangedContractsEventArgs e)
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
