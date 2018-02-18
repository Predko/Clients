using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Clients
{
    public enum ModeGetData
    {
        None,               // чтение не использовать
        Interop,            // с помощью Interop
        OLEDB               // с помощью OLEDB
    }

    public class GetContractInfoFromXls
    {
        public string FileName;
        public DataTable Dt;

        public GetContractInfoFromXls(string file, ModeGetData modeGet = ModeGetData.None)
        {
            FileName = file;

            switch (modeGet)
            {
                case ModeGetData.None:
                    break;
                case ModeGetData.Interop:
                    Dt = Interop_GetDataFrom_xls(file);
                    break;
                case ModeGetData.OLEDB:
                    Dt = OLEDB_GetDataFrom_xls(file);
                    break;
            }
        }

        // Чтение данных из xls файла договора(акта приёмки/сдачи работ)
        public DataTable OLEDB_GetDataFrom_xls(string file)
        {
            //Connection String

            //ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";

            string ConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;Data Source="
                                      + file
                                      + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";

            using (OleDbConnection conn = new OleDbConnection(ConnectionString))
            {
                conn.Open();

                // Get all Sheets in Excel File
                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                DataRow dr = dtSheet.Rows[0];

                string sheetName = dr["TABLE_NAME"].ToString().Trim('\'');

                if (!sheetName.EndsWith("$"))
                    return null;                // нет первого листа

                string sql = $"SELECT * FROM [{sheetName}]";

                var ada = new OleDbDataAdapter(sql, ConnectionString);

                var dt = new DataTable();
                var set = new DataSet();

                ada.Fill(0, 100, dt);

                set.Tables.Add(dt);
                return set.Tables[0];
            }
        }

        // Чтение данных из xls файла договора(акта приёмки/сдачи работ)
        public DataTable Interop_GetDataFrom_xls(string filename)
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
        public static int GetIndexFromString(string s)
        {
            int index = 0;

            int l = s.Length;
            for (int i = l - 1; i >= 0; i--)
            {
                index += (s[i] - 'A' + 1) * (int)Math.Pow(26, l - i - 1);
            }

            return index;
        }
    }
}
