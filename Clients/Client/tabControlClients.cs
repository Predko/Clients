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
            string pathfile = DefaultPathFile;
            DateTime minDate = DateTime.Parse("01/01/2015");

            foreach (Client cl in clients)
            {
                foreach (Contract ct in cl.contracts)
                {
                    if (ct.Dt < minDate || ct.FileName.Length == 0)
                    {
                        continue;
                    }

                    pathfile = GetPathFile(ct.FileName, pathfile);

                    if(pathfile == null)
                    {
                        return; // Отсутствует путь к файлу
                    }

                    ct.ClearServices();

                    ct.LoadServicesFrom_xls(pathfile);
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

            return dialogClient.DBC_CurrentClient;
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

                // Вставляем CurrentContract первым элементом, т.к. список отсортирован в обратном порядке
                listBoxContracts.Items.Insert(0, CurrentContract);
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
