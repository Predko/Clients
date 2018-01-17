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
        private void listBoxContracts_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBoxContracts.SelectedIndex;

            if (i == -1)
                return;

            Contract c = (Contract)listBoxContracts.SelectedItem;

            labelContract.Text = String.Format($"Договор № {c.Numb}");
        }

       private void SetClientContracts(Client cl)
        {
            if (cl == null)
                return;

            decimal Summ = 0;

            DateTime d = DateTime.Parse(@"07/01/2016", CultureInfo.CreateSpecificCulture("en-US"));
            foreach (Contract c in cl.contracts)
                Summ += (c.Dt >= d) ? c.Summ
                                    : c.Summ / 10000; // denomination after 07/01/2016

            //
            labelListContractsTotals.Text = String.Format($"Договоров: {cl.contracts.Count,-5}  на сумму: {Summ:C}");

            String numb = (cl.contracts.Count != 0) ? cl.contracts[0].Numb.ToString() : "";

            labelContract.Text = String.Format($"Договор № {numb}");

            listBoxContracts.DataSource = cl.contracts;  // binding the contracts to listBoxContracts
        }
    }
}
