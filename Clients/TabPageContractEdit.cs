using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace Clients
{
    public partial class Clients : Form
    {



        //---------------------------------------------
        // инициализация вкладки "Договор"
        // 
        private void InitTabPageContractEdit()
        {
            tabPageContractEdit.Enter += TabPageContractEdit_Enter;
        }

        //---------------------------------------------
        // Заполняем вкладку информацией о выбранном договоре
        //
        private void TabPageContractEdit_Enter(object sender, EventArgs e)
        {
            labelClientName.Text = CurrentClient.name;  // Название клиента

            Contract contract = (Contract)listBoxContracts.SelectedItem;

            comboBoxTypeContract.Items[0] = String.Format($"Договор № {contract.Numb} от {contract.Dt:d}");

            comboBoxTypeContract.Items[1] = String.Format($"Акт приёмки сдачи работ № {contract.Numb} от {contract.Dt:d}");

            comboBoxTypeContract.SelectedIndex = (int)contract.Type;
        }
    }
}
