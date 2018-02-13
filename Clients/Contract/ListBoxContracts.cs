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
        public event EventHandler ChangeCurrentContract;    // событие, вызываемое при изменении текущего контракта

        public Contract CurrentContract // Текущий договор
        {
            get => _currentContract;
            set
            {
                _currentContract = value;
                OnChangeCurrentContractInfo(EventArgs.Empty); // обрабатываем подключённые события после изменения текущего договора
            }
        }

//
//------------------------------------  Методы -----------------------------------------
//
        private void InitListBoxContracts()
        {
            this.listBoxContracts.SelectedIndexChanged += new System.EventHandler(this.ListBoxContracts_SelectedIndexChanged);

            // при изменении текущего договора выводить информацию о нём
            ChangeCurrentContract += (o, e) => WriteLabelContractInfo();
        }

        // обрабатываем подключённые события после изменения текущего договора
        private void OnChangeCurrentContractInfo(EventArgs eventArgs)
        {
            ChangeCurrentContract?.Invoke(this, eventArgs);
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

            // Выводим информацию о количестве и общей сумме договоров
            labelListContractsTotals.Text = String.Format($"Договоров: {cl.contracts.Count,-5}  на сумму: {Summ:C}");

            // если список договоров пуст, устанавливаем текущий в null
            if (cl.contracts.Count == 0)
                CurrentContract = null;
            else // иначе выбираем первый в списке listBoxContracts
                listBoxContracts.SelectedIndex = 0;
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

        // Выводит информацию о договоре на основную форму и заголовок tabPageContractEdit
        private void WriteLabelContractInfo()
        {
            labelFileName.Text = CurrentContract?.FileName ?? "Файл отсутствует";

            String numb = CurrentContract?.Numb.ToString() ?? "";

            labelContract.Text = tabPageContractEdit.Text = $"Договор № {numb}";

            ClearDataGridView();
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
