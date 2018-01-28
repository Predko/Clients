using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Clients
{
    public partial class Clients : Form
    {
        private void InitTabControlClients()
        {
            tabControlClients.Selecting += TabControlClients_Selecting;
        }

        private void TabControlClients_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(e.TabPage.Name == "tabPageContractEdit") // если выбрана вкладка "Договор" 
            {
                if (CurrentClient == null)  // если текущий клиент не загружен/выбран, не разрешаем открытие вкладки
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
