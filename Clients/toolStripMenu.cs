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

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void ToolStripMenuItemLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Открываем и загружаем файл данных со списком клиентов и сонтрактов(xml)
                ClientsXml clientsXml = new ClientsXml(openFileDialog.FileName);

                if (clientsXml.Load_Ok)
                {
                    comboBoxClients.BeginUpdate();              // приостанавливаем изменение ComboBox, отображающего clients

                    clientsXml.XmlToClientsAndContracts(clients);
                    SetClientContracts(clients[0]);

                    comboBoxClients.SelectedIndex = 0;

                    comboBoxClients.EndUpdate();               // обновляем содержимое ComboBox, отображающего clients
                }
            }
        }

        // Чтение данных о договоре из файла xls
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
            }

            ShowDataTable();
        }

        private void ShowDataTable()
        {
            BindingSource bindingDataTable = new BindingSource();

            FormShowDataTable fs = new FormShowDataTable();

            fs.dataGridViewFile_xls.DataSource = bindingDataTable;

            fs.dataGridViewFile_xls.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader);

            bindingDataTable.DataSource = dtFile_xls;

            fs.Show();
        }



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


        private void ToolStripMenuItemReadXlsOLEDB_Click(object sender, EventArgs e)
        {

        }
    }
}
