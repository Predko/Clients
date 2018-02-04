using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

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

        private void ButtonLoadContractFrom_xls_Click(object sender, EventArgs e)
        {
            string filename = CurrentContract.FileName;

            while (!File.Exists(filename))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                DialogResult res = fbd.ShowDialog();
                switch (res)
                {
                    case DialogResult.Cancel:
                    case DialogResult.Abort:
                        return;

                    case DialogResult.OK:
                        filename = fbd.SelectedPath + CurrentContract.FileName;
                        break;
                }
            }

            GetContractInfoFromXls xls = new GetContractInfoFromXls(filename, ModeGetData.OLEDB);

            if (xls.Dt == null) // Не удалось загрузить информацию
                return;

            GetListServicesFromDT gls = new GetListServicesFromDT(xls.Dt, CurrentContract.Clone());

            if (!gls.GetListServices())
            {
                return; // чтение списка услуг не удалось
            }

            CurrentContract.services = gls.contract.services;
        }
    }
}
