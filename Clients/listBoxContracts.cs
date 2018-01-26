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

            Contract c = (Contract)listBoxContracts.SelectedItem;

            labelContract.Text = String.Format($"Договор № {c.Numb}");
        }

        // Заполняет listBoxContracts списком договоров
        // при выборе другого клиента
        private void SetClientContracts(Client cl)
        {
            if (cl == null)
                return;

            decimal Summ = 0;

            DateTime d = DateTime.Parse(@"07/01/2016", CultureInfo.CreateSpecificCulture("en-US"));
 
            // заполняем listBoxContracts списком договоров и подсчитываем общую сумму
            listBoxContracts.Items.Clear();

            foreach (Contract c in cl.contracts)
            {
                listBoxContracts.Items.Add(c);
                Summ += (c.Dt >= d) ? c.Summ
                                    : c.Summ / 10000; // denomination after 07/01/2016
            }

            //
            labelListContractsTotals.Text = String.Format($"Договоров: {cl.contracts.Count,-5}  на сумму: {Summ:C}");

            String numb = (cl.contracts.Count != 0) ? cl.contracts[0].Numb.ToString() : "";

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
