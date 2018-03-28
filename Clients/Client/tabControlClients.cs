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

            foreach (Client cl in clients)
            {
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

                    LoadFromFile_xls(ct, path + ct.FileName);
                }
            }
        }

        #region События, связанные с клиентами
        // Выводит диалаговое окно для редактирования, создания нового и удаления клиента
        private Client DialogEditClient(Client client)
        {
            var dialogClient = new DialogBoxClients(client);

            dialogClient.ShowDialog();

            if(clients.Count == 0)
            {
                comboBoxClients.Text = "";
            }

            return dialogClient.CurrentClient;
        }

        // Редактирование(и др.) клиента
        private void ButtonEditClient_Click(object sender, EventArgs e)
        {
            if (CurrentClient == null)
            {
                return;
            }

            CurrentClient = DialogEditClient(CurrentClient);
        }

        // Создание нового и редактирование(и др.) клиента
        private void ButtonNewClient_Click(object sender, EventArgs e)
        {
            var client = new Client();

            CurrentClient = DialogEditClient(client);
        }
        #endregion

        #region События, связанные с договорами
        // Создание нового договора
        private void ButtonNewContract_Click(object sender, EventArgs e)
        {
            if(CurrentClient != null)
            {
                CurrentContract = new Contract();

                listBoxContracts.Items.Add(CurrentContract);
                listBoxContracts.SelectedItem = CurrentContract;

                CurrentClient.contracts.Add(CurrentContract, true);

                tabControlClients.SelectedTab = tabPageContractEdit;
            }
        }

        // Редактирование выбранного договора
        private void ButtonEditContract_Click(object sender, EventArgs e)
        {
            if (CurrentClient != null && CurrentContract != null)
            {
                tabControlClients.SelectedTab = tabPageContractEdit;
            }
        }

        // Удаление текущего договора
        private void ButtonDeleteContract_Click(object sender, EventArgs e)
        {
            if(CurrentClient != null && CurrentContract != null)
            {
                var typeContract = (CurrentContract.Type == TypeContract.CWC) ? "Акт"
                                                                              : "Договор";

                var res = MessageBox.Show($"Удалить {typeContract} №{CurrentContract.Numb}?\nВся информация будет потеряна!", "Удаление", MessageBoxButtons.YesNo);

                if(res != DialogResult.Yes)
                {
                    return;
                }

                int index = listBoxContracts.Items.IndexOf(CurrentContract); // индекс в списке удаляемого договора 
                int count = listBoxContracts.Items.Count;

                listBoxContracts.Items.Remove(CurrentContract); // Удаляем договор из списка ListBox

                CurrentClient.contracts.Remove(CurrentContract); // Удаляем договор из списка договоров выбранного клиента

                if (count == 1) // Это последний договор клиента и список пуст?
                {
                    CurrentContract = null;

                    listBoxContracts.SelectedItem = null;
                }
                else
                if(index == count - 1) // Удалённый договор был последним в списке?
                {
                    listBoxContracts.SelectedIndex = index - 1; // Выбираем ближайший договор в списке
                }
            }
        }
        #endregion
    }
}
