using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;

namespace Clients
{
    public partial class Clients : Form
    {
        private DataTable dtFile_xls;



        // Инициализация меню
        private void InitToolStripMenu()
        {
            this.toolStripMenuItemSaveXml.Click += new System.EventHandler(this.ToolStripMenuItemSaveXml_Click);
            this.toolStripMenuItemLoad.Click += new System.EventHandler(this.ToolStripMenuItemLoad_Click);
            this.toolStripMenuItemLoadXmlAccess.Click += new System.EventHandler(this.ToolStripMenuItemLoadXmlAccess_Click);
            this.toolStripMenuItemReadXlsOLEDB.Click += new System.EventHandler(this.ToolStripMenuItemReadXlsOLEDB_Click);
            this.toolStripMenuItemRead_xls.Click += new System.EventHandler(this.ToolStripMenuItemRead_xls_Click);
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.ToolStripMenuItemExit_Click);
        }

        // Выход из программы
        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        // Сохранить данные в файле xml
        private void ToolStripMenuItemSaveXml_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 0,
                RestoreDirectory = true
            };

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                ClientsXml xml = new ClientsXml(clients);   // Создаём xml документ из списка клиентов

                xml.SaveXml(saveFile.FileName);             // Сохраняем его в файл saveFile.FileName
            }
        }

        // Загрузить данные из xml файла собственного формата
        private void ToolStripMenuItemLoad_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 0,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Открываем и загружаем файл данных со списком клиентов и контрактов(xml)
                var clientsXml = new ClientsXml(openFileDialog.FileName);

                if (clientsXml.Load_Ok)
                {
                    comboBoxClients.BeginUpdate();              // приостанавливаем изменение ComboBox, отображающего clients

                    // Заполняем список клиентов с одновременным заполнением ComboBox(через событие в ListClients)
                    // Заполнение происходит без нарушения сортировки
                    clientsXml.XmlToClientsAndContracts(clients);

                    SetComboBoxColumns();

                    // Выбираем первого клиента в списке и заполняем список договоров ListBoxContracts
                    comboBoxClients.SelectedIndex = 0;

                    comboBoxClients.EndUpdate();               // обновляем содержимое ComboBox, отображающего clients
                }
            }

            LoadAllServicesToolStripMenuItem.Enabled = true;
        }

        // Загрузить данные из файла xml полученного в результате импорта из базы данных MS Access
        private void ToolStripMenuItemLoadXmlAccess_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 0,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Открываем и загружаем файл данных со списком клиентов и контрактов(xml)
                var clientsXml = new ClientsXml(openFileDialog.FileName);

                if (clientsXml.Load_Ok)
                {
                    comboBoxClients.BeginUpdate();              // приостанавливаем изменение ComboBox, отображающего clients

                    // Заполняем список клиентов с одновременным заполнением ComboBox(через событие в ListClients),
                    // вызываемое при изменении текущего клиента
                    // Заполнение происходит без нарушения сортировки
                    clientsXml.AccessXmlToClients(clients);

                    comboBoxClients.SelectedIndex = 0;

                    comboBoxClients.EndUpdate();               // обновляем содержимое ComboBox, отображающего clients
                }
            }

            LoadAllServicesToolStripMenuItem.Enabled = true;
        }

        // Чтение данных о договоре из файла xls и вывод в новой форме в DataGridView с помощью Interop
        private void ToolStripMenuItemRead_xls_Click(object sender, EventArgs e)
        {
            dtFile_xls = GetDataTableFromFile_xls(ModeGetData.Interop); // Читаем xls с помощью Interop

            if (dtFile_xls != null)
                ShowDataTable();
        }

        // Чтение данных о договоре из файла xls и вывод в новой форме в DataGridView с помощью OLEDB
        private void ToolStripMenuItemReadXlsOLEDB_Click(object sender, EventArgs e)
        {
            dtFile_xls = GetDataTableFromFile_xls(ModeGetData.OLEDB); // читаем с помощью OLEDB

            if (dtFile_xls != null)
                ShowDataTable();
        }

        private void LoadAllServicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangedCurrentClient_EventHandler -= SetClientContracts;

            comboBoxClients.BeginUpdate();              // приостанавливаем изменение ComboBox, отображающего clients

            LoadAllServices();

            SetComboBoxColumns();

            ChangedCurrentClient_EventHandler += SetClientContracts;

            comboBoxClients.SelectedIndex = 0;  // После загрузки выбираем первый элемент

            comboBoxClients.EndUpdate();        // обновляем содержимое ComboBox, отображающего clients

            if(listBoxContracts.Items.Count == 0)
            {
                CurrentContract = null;
            }
            else
            {
                listBoxContracts.SelectedIndex = 0;
            }
        }

        // Вывод данных в форме FormShowDataTable в DataGridView
        private void ShowDataTable()
        {
            BindingSource bindingDataTable = new BindingSource();

            FormShowDataTable fs = new FormShowDataTable();

            fs.dataGridViewFile_xls.DataSource = bindingDataTable;

            fs.dataGridViewFile_xls.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader);

            bindingDataTable.DataSource = dtFile_xls;

            fs.Show();
        }

        // Выбираем файл и заполняем таблицу DataTable
        private DataTable GetDataTableFromFile_xls(ModeGetData mode)
        {
            var fd = new OpenFileDialog
            {
                Filter = "Excel files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*",
                FilterIndex = 0
            };

            if (fd.ShowDialog() == DialogResult.OK)
            {
                var xls = new GetContractInfoFromXls(fd.FileName, mode); // чтение файла и заполнение DataTable
                                                                                            // из области печати
                return xls.Dt;
            }

            return null;    // Ошибка - таблица не заполнена
        }
    }
}
