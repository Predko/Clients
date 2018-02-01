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
            labelClientName.Text = CurrentClient.Name;  // Название клиента

            if (CurrentContract != null)
            {
                comboBoxTypeContract.Items[0] = String.Format($"Договор № {CurrentContract.Numb} от {CurrentContract.Dt:d}");

                comboBoxTypeContract.Items[1] = String.Format($"Акт приёмки сдачи работ № {CurrentContract.Numb} от {CurrentContract.Dt:d}");

                comboBoxTypeContract.SelectedIndex = (int)CurrentContract.Type;
            }
        }
    }
}
