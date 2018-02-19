using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
                }
                else
                    if(CurrentContract == null) // если текущий договор отсутствует, не разрешаем открытие вкладки
                    {
                        e.Cancel = true;
                    }
            }
        }

        private void LoadAllServices()
        {
            string path = null;
            DateTime minDate = DateTime.Parse("01/01/2015");

            Contract.ChangeServiceList -= ChangeServiceList_Event;

            foreach (Client cl in clients)
            {
                CurrentClient = cl;

                foreach (Contract ct in cl.contracts)
                {
                    if (ct.Dt < minDate || ct.FileName.Length == 0)
                    {
                        continue;
                    }

                    while (path == null && !File.Exists(path + ct.FileName))
                    {
                        var fbd = new FolderBrowserDialog();  // Если файл не существует, предлагаем ввести путь к файлу(один раз)

                        switch (fbd.ShowDialog())
                        {
                            case DialogResult.Cancel:
                            case DialogResult.Abort:
                                return;

                            case DialogResult.OK:
                                path = fbd.SelectedPath;
                                break;
                        }
                    }

                    CurrentContract = ct;

                    LoadFromFile_xls(ct, path + ct.FileName);
                }
            }

            Contract.ChangeServiceList -= ChangeServiceList_Event;
        }
    }
}
