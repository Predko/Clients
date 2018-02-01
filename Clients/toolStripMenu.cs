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
        DataTable dtFile_xls;

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
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 0,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Открываем и загружаем файл данных со списком клиентов и контрактов(xml)
                ClientsXml clientsXml = new ClientsXml(openFileDialog.FileName);

                if (clientsXml.Load_Ok)
                {
                    comboBoxClients.BeginUpdate();              // приостанавливаем изменение ComboBox, отображающего clients

                    // Заполняем список клиентов с одновременным заполнением ComboBox(через событие в ListClients)
                    // Заполнение происходит без нарушения сортировки
                    clientsXml.XmlToClientsAndContracts(clients);

                    // Выбираем первого клиента в списке и заполняем список договоров ListBoxContracts
                    SetClientContracts(clients[0]);
                    comboBoxClients.SelectedIndex = 0;

                    comboBoxClients.EndUpdate();               // обновляем содержимое ComboBox, отображающего clients
                }
            }
        }

        // Загрузить данные из файла xml полученного в результате импорта из базы данных MS Access
        private void LoadXmlAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 0,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Открываем и загружаем файл данных со списком клиентов и контрактов(xml)
                ClientsXml clientsXml = new ClientsXml(openFileDialog.FileName);

                if (clientsXml.Load_Ok)
                {
                    comboBoxClients.BeginUpdate();              // приостанавливаем изменение ComboBox, отображающего clients

                    clientsXml.AccessXmlToClients(clients);
                    SetClientContracts(clients[0]);

                    comboBoxClients.SelectedIndex = 0;

                    comboBoxClients.EndUpdate();               // обновляем содержимое ComboBox, отображающего clients
                }
            }
        }

        // Чтение данных о договоре из файла xls и вывод в новой форме в DataGridView с помощью Interop
        private void ToolStripMenuItemRead_xls_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Excel files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
            fd.FilterIndex = 0;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                dtFile_xls = GetDataFrom_xls(fd.FileName);
                if (dtFile_xls == null)     // Ошибка - таблица не заполнена
                    return;

                ShowDataTable();
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


        // Чтение данных из xls файла договора(акта приёмки/сдачи работ)
        public DataTable GetDataFrom_xls(string filename)
        {
            Excel.Application ExcelApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;

            ExcelApp = new Excel.Application();

            xlWorkBook = ExcelApp.Workbooks.Open(filename, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                              Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                              Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                              Type.Missing, Type.Missing);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets[1];

            string printArea = xlWorkSheet.PageSetup.PrintArea;

            Regex reg = new Regex(@"\b\w+\b");

            var pa = reg.Matches(printArea);

            if (pa.Count != 4)
            {
                MessageBox.Show("Ошибка распознавания области печати");

                return null;         // должно быть 4 значения: (x1,y1,x2,y2)
            }

            int x1 = GetIndexFromString(pa[0].Value);
            int y1 = int.Parse(pa[1].Value);

            int x2 = GetIndexFromString(pa[2].Value);
            int y2 = int.Parse(pa[3].Value);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets[1];
            ExcelApp.Visible = true;

            // Создаём таблицу для хранения записей из xls файла
            DataTable dt = new DataTable("dtContract");

            // добавляем колонки в таблицу
            for (int i = x1; i != x2 + 1; i++)
                dt.Columns.Add(i.ToString());

            // текущая строка таблицы
            DataRow dr;

            for (int j = y1; j <= y2; j++)
            {
                dr = dt.Rows.Add();

                for (int i = x1; i <= x2; i++)
                {
                    string s = ((Excel.Range)xlWorkSheet.Cells[j, i]).Value2?.ToString();

                    dr.SetField<string>(i - 1, s);
                }
            }


            xlWorkBook.Close();

            return dt;
        }


        // Преобразует буквенный индекс(из xls) в цифровой
        private int GetIndexFromString(string s)
        {
            int index = 0;

            int l = s.Length;
            for (int i = l - 1; i >=0; i--)
            {
                index += (s[i] - 'A' + 1) * (int)Math.Pow(26, l - i - 1);
            }

            return index;
        }

        // Чтение данных о договоре из файла xls и вывод в новой форме в DataGridView с помощью OLEDB
        private void ToolStripMenuItemReadXlsOLEDB_Click(object sender, EventArgs e)
        {

        }
    }
}
