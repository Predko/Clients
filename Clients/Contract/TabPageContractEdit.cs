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
        private int tpNumbCurrentContract = 0;  // Номер договора, показанного на вкладке

        private DateTime tpDate = DateTime.MinValue; // и дата

        //---------------------------------------------
        // инициализация вкладки "Договор"
        // 
        private void InitTabPageContractEdit()
        {
            tabPageContractEdit.Enter += TabPageContractEdit_Enter;
            tabPageContractEdit.Leave += TabPageContractEdit_Leave;
        }

        //---------------------------------------------
        // Заполняем вкладку информацией о выбранном договоре
        //
        private void TabPageContractEdit_Enter(object sender, EventArgs e)
        {
            ToolStripMenuChange(false); // Отключаем пукты меню, которые не должны использоваться на вкладке

            SetInfoTabPageContractEdit();

            dataGridViewContract.Select();
        }

        private void TabPageContractEdit_Leave(object sender, EventArgs e)
        {
            ToolStripMenuChange(true);

            // Обновляем текущий элемент в ListBox
            listBoxContracts.Items[listBoxContracts.SelectedIndex] = CurrentContract;
        }

        private void ToolStripMenuChange(bool change)
        {
            toolStripMenuItemLoadXmlAccess.Enabled = change;

            toolStripMenuItemLoad.Enabled = change;
        }

        private void SetInfoTabPageContractEdit()
        {
            if (tpNumbCurrentContract == CurrentContract.Numb
                && tpDate == CurrentContract.Dt)
            {
                return;
            }

            tpNumbCurrentContract = CurrentContract.Numb;
            tpDate = CurrentContract.Dt;

            ClearDataGridView();

            labelClientName.Text = CurrentClient.Name;  // Название клиента

            // заполняем список подразделений текущего клиента
            ColumnSubdivision.Items.Clear();

            List<string> list = CurrentClient.Subdivisions.Values.ToList();

            list.Sort();

            foreach (string s in list)
            {
                ColumnSubdivision.Items.Add(s);
            }

            dataGridViewContract["ColumnSubdivision", 0].Value = CurrentClient.Subdivisions.Values[0];

            // Информация о текущем договоре
            if (CurrentContract != null)
            {
                comboBoxTypeContract.Items[0] = String.Format($"Договор № {CurrentContract.Numb} от {CurrentContract.Dt:d}");

                comboBoxTypeContract.Items[1] = String.Format($"Акт приёмки сдачи работ № {CurrentContract.Numb} от {CurrentContract.Dt:d}");

                comboBoxTypeContract.SelectedIndex = (int)CurrentContract.Type;
            }

            foreach(int id in CurrentContract.services)
            {
                AddServiceToDGV(AllServices[id]);
            }

            labelInTotalValue.Text = $"{CurrentContract.Summ}";
        }

        // Загружаем информацию об услугах из файла договора .xls
        private void ButtonLoadContractFrom_xls_Click(object sender, EventArgs e)
        {
            string filename = @"H:\Документы" + CurrentContract.FileName;

            while (!File.Exists(filename))  // Если файл не существует, предлагаем ввести путь к файлу
            {
                var fbd = new FolderBrowserDialog();

                switch (fbd.ShowDialog())
                {
                    case DialogResult.Cancel:
                    case DialogResult.Abort:
                        return;

                    case DialogResult.OK:
                        filename = fbd.SelectedPath + CurrentContract.FileName;
                        break;
                }
            }

            ClearDataGridView();

            // добавляем событие вызываемое при изменении списка услуг в текущем контракте
            Contract.ChangeServiceList -= ChangeServiceList_Event;
            Contract.ChangeServiceList += ChangeServiceList_Event;

            LoadFromFile_xls(CurrentContract, filename);

            // Удаляем событие вызываемое при изменении списка услуг в текущем контракте
            Contract.ChangeServiceList -= ChangeServiceList_Event;

            labelInTotalValue.Text = $"{CurrentContract.Summ}";
        }


        private bool LoadFromFile_xls(Contract contract, string filename)
        {
            var xls = new GetContractInfoFromXls(filename, ModeGetData.OLEDB);

            if (xls.Dt == null) // Не удалось загрузить информацию
                return false;

            var gls = new GetListServicesFromDT(xls.Dt, contract);

            if (!gls.GetListServices())
            {
                return false; // чтение списка услуг не удалось
            }

            return true;
        }
    }
}

