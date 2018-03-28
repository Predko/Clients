using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace Clients
{
    public static partial class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }

    //------------------------------------------------------------------------
    //                          dataGridViewContract
    //------------------------------------------------------------------------

    // список колонок:
    //  "ColumnNameWork"
    //  "ColumnNameDevice"
    //  "ColumnSubdivision"
    //  "ColumnServiceNumb"
    //  "ColumnServiceSumm"

    public partial class Clients : Form
    {
        // Переменные для редактирования ComboBoxCell
        private string cbCellValue;                 // Значение введённой строки в ComboBoxCell
        private bool cbNeedFill = false;            // Флаг, указывающий на необходимость зафиксировать введённое значение в ячейке

        // переменные для реализации ввода с перемещением по строке
        private bool NeedBackStyle = false;         // флаг, указывающий на необходимость возврата стиля
        private Color savedSelectionForeColor;      // сохранённые цвета
        private Color savedSelectionBackColor;      // сохранённые цвета

        private readonly Dictionary<int, int> SelectedRow = new Dictionary<int, int>();  // Id услуг в выбранных строках в DataGridView

        //--------- Методы ----------

        //------------------------------------------------------------------------------------
        // Инициализация.
        // Подключение событий DataGridViewContract
        //------------------------------------------------------------------------------------
        private void InitDataGridViewContract()
        {
            dataGridViewContract.CellParsing += DataGridViewContract_CellParsing;
            dataGridViewContract.CellEndEdit += DataGridViewContract_CellEndEdit;
            dataGridViewContract.CellValidating += DataGridViewContract_CellValidating;

            dataGridViewContract.DefaultValuesNeeded += DataGridViewContract_DefaultValuesNeeded;
            dataGridViewContract.RowsRemoved += DataGridViewContract_RowsRemoved;
            dataGridViewContract.RowsAdded += DataGridViewContract_RowsAdded;
            dataGridViewContract.RowEnter += DataGridViewContract_RowEnter;
            dataGridViewContract.RowValidating += DataGridViewContract_RowValidating;
            dataGridViewContract.SelectionChanged += DataGridViewContract_SelectionChanged;

            dataGridViewContract.EditingControlShowing += DataGridViewContract_EditingControlShowing;

            dataGridViewContract.KeyDown += DataGridViewContract_KeyDown;
            dataGridViewContract.PreviewKeyDown += DataGridViewContract_PreviewKeyDown;

            dataGridViewContract.Enter += DataGridViewContract_Enter;
            dataGridViewContract.Leave += DataGridViewContract_Leave;

            InitDebugInfoEvent();

            InitComboBoxColumns();

            dataGridViewContract.DoubleBuffered(true);
        }


        // Инициализация ячеек, содержащих элемент ComboBoxColumn
        private void InitComboBoxColumns()
        {
            ColumnNameWork.Items.Add("");
            dataGridViewContract["ColumnNameWork", 0].Value = "";

            ColumnNameDevice.Items.AddRange("");
            dataGridViewContract["ColumnNameDevice", 0].Value = "";

            ColumnSubdivision.Items.AddRange("");
            dataGridViewContract["ColumnSubdivision", 0].Value = "";

            ColumnAddInfo.Items.AddRange("");
            dataGridViewContract["ColumnAddInfo", 0].Value = "";

            dataGridViewContract["ColumnServiceNumb", 0].Value = 0;
            dataGridViewContract["ColumnServiceSumm", 0].Value = (decimal)0;     // стоимость

            dataGridViewContract["ColumnIdService", 0].Value = -1;

            ColumnSubdivision.DisplayIndex = 1;
            ColumnNameDevice.DisplayIndex = 2;
            ColumnAddInfo.DisplayIndex = 3;
            ColumnNameWork.DisplayIndex = 4;
            ColumnServiceNumb.DisplayIndex = 5;
            ColumnServiceSumm.DisplayIndex = 6;
        }

        // Устанавливаем значения списков ComboBoxColumn
        public void SetComboBoxColumns()
        {
            ColumnNameWork.Items.Clear();

            AddRangeComboBoxColumn(ColumnNameWork.Items, AllNameWorks.Values);
            dataGridViewContract["ColumnNameWork", 0].Value = ColumnNameWork.Items[0];

            ColumnNameDevice.Items.Clear();

            AddRangeComboBoxColumn(ColumnNameDevice.Items, AllNameDevices.Values);
            dataGridViewContract["ColumnNameDevice", 0].Value = ColumnNameDevice.Items[0];

            ColumnAddInfo.Items.Clear();

            AddRangeComboBoxColumn(ColumnAddInfo.Items, AllAddInfo.Values);
            dataGridViewContract["ColumnAddInfo", 0].Value = ColumnAddInfo.Items[0];
        }

        // Добавление в список пунктов в ComboBoxColumn
        private void AddRangeComboBoxColumn(DataGridViewComboBoxCell.ObjectCollection cbcell, IList<string> lstr)
        {
            foreach (string s in lstr)
            {
                cbcell.Add(s);
            }
        }

        private void ChangeServiceList_Event(object o, ChangingListServicesEventArgs e)
        {
            switch (e.type)
            {
                case Change.Add:

                    AddServiceToDGV(e.service);
                    break;

                case Change.Del:

                    RemoveServiceFromDGV(e.service.Id);
                    break;
                case Change.Clear:
                    break;
            }
        }

        public void AddServiceToDGV(Service sv)
        {
            DataGridViewRow row = dataGridViewContract.Rows[dataGridViewContract.Rows.Add()];

            // Если в ComboBoxCell нет этих пунктов, добавляем их
            if (!ColumnNameWork.Items.Contains(sv.Nw.Name))
                ColumnNameWork.Items.Add(sv.Nw.Name);
            row.Cells["ColumnNameWork"].Value = sv.Nw.Name;

            if (!ColumnNameDevice.Items.Contains(sv.Nd.Name))
                ColumnNameDevice.Items.Add(sv.Nd.Name);
            row.Cells["ColumnNameDevice"].Value = sv.Nd.Name;

            string namesd = CurrentClient.Subdivisions[sv.Sd];
            if (!ColumnSubdivision.Items.Contains(namesd))
                ColumnSubdivision.Items.Add(namesd);
            row.Cells["ColumnSubdivision"].Value = namesd;

            if (!ColumnAddInfo.Items.Contains(sv.Ai.Name))
                ColumnAddInfo.Items.Add(sv.Ai.Name);
            row.Cells["ColumnAddInfo"].Value = sv.Ai.Name;

            // Заполяем остальные ячейки
            row.Cells["ColumnServiceNumb"].Value = sv.Number;
            row.Cells["ColumnServiceSumm"].Value = sv.Value;     // стоимость
            row.Cells["ColumnIdService"].Value = sv.Id;
        }

        public void RemoveServiceFromDGV(int ServiceId)
        {
            DataGridViewRow row = null;
            foreach (DataGridViewRow r in dataGridViewContract.Rows)
            {
                if (r.IsNewRow)
                    continue;

                var id = (int)r.Cells["ColumnIdService"].Value;
                if (id == ServiceId)
                {
                    row = r;
                    break;
                }
            }

            if (row != null)
            {
                dataGridViewContract.Rows.Remove(row);
            }
        }

        public void ClearDataGridView()
        {
            dataGridViewContract.RowsRemoved -= DataGridViewContract_RowsRemoved;
            dataGridViewContract.RowValidating -= DataGridViewContract_RowValidating;
            dataGridViewContract.RowsAdded -= DataGridViewContract_RowsAdded;

            dataGridViewContract.Rows.Clear();

            dataGridViewContract.RowsAdded += DataGridViewContract_RowsAdded;
            dataGridViewContract.RowValidating += DataGridViewContract_RowValidating;
            dataGridViewContract.RowsRemoved += DataGridViewContract_RowsRemoved;
        }

        //------------------------------------------------------------------------------------
        // устанавливаем номера строк в заголовки строк, начиная с номера строки beginIndex
        //------------------------------------------------------------------------------------
        private void SetRowsNumber(int beginIndex)
        {
            int index = beginIndex;
            int count = dataGridViewContract.Rows.Count;

            if(index != 0)
            {
                index--;
            }

            while (index < count) // начинаем с заданного индекса
            {
                var row = dataGridViewContract.Rows[index];

                if(index != count - 1 && !row.IsNewRow)
                {
                    row.Cells["ColumnNumberRow"].Value = String.Format("{0,3}", index + 1);

                }

                index++;
            }
        }

        private void DataGridViewContract_Enter(object sender, EventArgs e)
        {
        }

        private void DataGridViewContract_Leave(object sender, EventArgs e)
        {
        }

        private void DataGridViewContract_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // Значения по умолчанию для новых ячеек, содержащих ComboBox
            e.Row.Cells["ColumnNameWork"].Value = ColumnNameWork.Items[0];
            e.Row.Cells["ColumnNameDevice"].Value = ColumnNameDevice.Items[0];
            e.Row.Cells["ColumnSubdivision"].Value = ColumnSubdivision.Items[0];
            e.Row.Cells["ColumnAddInfo"].Value = ColumnAddInfo.Items[0];

            // Значения по умолчанию для новых ячеек, содержащих TextBox
            e.Row.Cells["ColumnServiceNumb"].Value = 0;
            e.Row.Cells["ColumnServiceSumm"].Value = (decimal)0;
            e.Row.Cells["ColumnIdService"].Value = -1;
        }

        // сохраняем Id услуг в выбранных строках
        private void DataGridViewContract_SelectionChanged(object sender, EventArgs e)
        {
            SelectedRow.Clear();
            foreach (DataGridViewRow dr in dataGridViewContract.SelectedRows)
            {
                if(!dr.IsNewRow)
                {
                    SelectedRow[dr.Index] = (int)dr.Cells["ColumnIdService"].Value;
                }
            }

        }

        // при удалении строки, переписываем изначения заголовков строк, начиная со следующей, после удалённой
        private void DataGridViewContract_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SetRowsNumber(e.RowIndex);

            // Получаем id услуги, соответствующий удаляемой строке и вызываем метод удаления услуги, если он подключен
            for(int i = 0; i != e.RowCount; i++)
            {
                if(e.RowIndex == -1)
                {
                    continue;
                }

                if (SelectedRow.Count != 0 && SelectedRow.ContainsKey(e.RowIndex + i))
                {
                    // удаляем в выбранные ранее строки
                    OnRemovedService(SelectedRow[e.RowIndex + i]);
                }
                else
                if (dataGridViewContract["ColumnIdService", i].Value != null)
                {
                    // удаляем строки, без выбора(удалены программно)
                    OnRemovedService((int)dataGridViewContract["ColumnIdService", e.RowIndex + i].Value);
                }
            }

            labelInTotalValue.Text = $"{CurrentContract.Summ}";
        }

        private void OnRemovedService(int id)
        {
            RemovedDgvRows?.Invoke(id);
        }

        private void DataGridViewContract_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            SetRowsNumber(e.RowIndex);

            labelInTotalValue.Text = $"{CurrentContract.Summ}";
        }

        //------------------------------------------------------------------------------------
        //                              Обработчики событий 
        //------------------------------------------------------------------------------------

        private void DataGridViewContract_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
        }

        // проверка правильности введённых значений в строке
        private void DataGridViewContract_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            ChangeServiceRow(dataGridViewContract.Rows[e.RowIndex]);

            labelInTotalValue.Text = $"{CurrentContract.Summ}";
        }

        // Проверяет, изменились ли данные в строке и добавляет новую услугу или меняет существующую.
        // Если в строке недостаточно данных для услуги(новая строка), ничего не делает, предотвращая создание новой услуги из пустой строки
        private void ChangeServiceRow(DataGridViewRow row)
        {
            if (row.IsNewRow)
                return;

            var id = AllServices.IndexOfKey((int)row.Cells["ColumnIdService"].Value); // индекс в списке

            var summ = (decimal)row.Cells["ColumnServiceSumm"].Value;

            var nw = (string)row.Cells["ColumnNameWork"].Value ?? "";

            if (summ == 0 || nw?.Length == 0)
            {
                return; // если эти ячейки пусты, значит данных для добавления/изменения услуги недостаточно(false)
            }

            var nd = (string)row.Cells["ColumnNameDevice"].Value ?? "";
            var sd = CurrentClient.Subdivisions.IndexOfValue((string)row.Cells["ColumnSubdivision"].Value ?? "");
            var ai = (string)row.Cells["ColumnAddInfo"].Value ?? "";
            var numb = (int)row.Cells["ColumnServiceNumb"].Value;

            Service sv;

            if (id == -1)
            {
                sv = new Service(nw, nd, sd, numb, summ, -1, ai); // добавляем новую услугу

                CurrentContract.AddService(sv); // добавляем в список услуг текущего договора
                row.Cells["ColumnIdService"].Value = sv.Id;
            }
            else
            {
                sv = AllServices.Values[id];    // извлекаем существующую

                // Проверяем, изменились ли данные
                if (summ != sv.Value
                 || numb != sv.Number
                 || nw != sv.Nw.Name
                 || nd != sv.Nd.Name
                 || sd != sv.Sd
                 || ai != sv.Ai.Name)
                {
                    CurrentContract.SetService(nw, nd, sd, numb, summ, sv.Id, ai); // меняем значения
                }
            }
        }

        //----------------------------------------------------------------------------------------
        //            Переходит на следующую ячейку в строке с учётом DisplayIndex
        //----------------------------------------------------------------------------------------
        private void NextColumn()
        {
            Point currentCell = dataGridViewContract.CurrentCellAddress; // адрес редактируемой ячейки

            var currentColumn = dataGridViewContract.Columns[currentCell.X];

            if (dataGridViewContract.Columns.GetNextColumn(currentColumn, DataGridViewElementStates.Displayed,
                                                            DataGridViewElementStates.None) == null)
            {
                // это последняя колонка и переходим в первую в следующей строке

                // проверяем, есть ли следующая строка
                if (currentCell.Y + 1 == dataGridViewContract.RowCount && !dataGridViewContract.IsCurrentRowDirty)
                {
                    // нельзя покидать строку, если не создана новая
                    // и текущая не прошла проверку на правильность значений
                    return;
                }

                // сохраняем цвета выделенной ячейки
                savedSelectionForeColor = dataGridViewContract.DefaultCellStyle.SelectionForeColor;
                savedSelectionBackColor = dataGridViewContract.DefaultCellStyle.SelectionBackColor;

                // делаем цвет выделенной ячейки равным цвету невыделенной ячейки
                // Это сделает незаметным переход в следующую
                dataGridViewContract.DefaultCellStyle.SelectionBackColor
                                        = dataGridViewContract.DefaultCellStyle.BackColor;

                dataGridViewContract.DefaultCellStyle.SelectionForeColor
                                        = dataGridViewContract.DefaultCellStyle.ForeColor;

                SendKeys.Send("{DOWN}");    // эмулируем нажатие клавиши "Down" - уходим из этой строки в следующую
                SendKeys.Send("{HOME}");    // эмулируем нажатие клавиши "Home".

                NeedBackStyle = true;       // Сообщаем, что нужно вернуть сохранённые цвета выделенной ячейки
            }
            else
            {   // иначе, переходим в следующую ячейку справа(в соответствии с DisplayIndex)
                SendKeys.Send("{RIGHT}");   // эмулируем нажатие клавиши "Right" - уходим из этой ячейки в следующую
            }

            #region if DEBUG
#if DEBUG
            Console.WriteLine("NextColumn " + currentCell.X + "," + currentCell.Y);
#endif
            #endregion
        }

        private void DataGridViewContract_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e?.Value != null)
            {
                string nameCol = dataGridViewContract.Columns[e.ColumnIndex].Name;
                if (nameCol == "ColumnServiceSumm")
                {
                    try
                    {
                        e.Value = decimal.Parse(e.Value.ToString());
                        // Set the ParsingApplied property to 
                        // Show the event is handled.
                        e.ParsingApplied = true;

                    }
                    catch (FormatException)
                    {
                        // Set to false in case another CellParsing handler
                        // wants to try to parse this DataGridViewCellParsingEventArgs instance.
                        e.ParsingApplied = false;
                    }
                }
                else
                if (nameCol == "ColumnServiceNumb")
                {
                    try
                    {
                        e.Value = int.Parse(e.Value.ToString());
                        // Set the ParsingApplied property to 
                        // Show the event is handled.
                        e.ParsingApplied = true;

                    }
                    catch (FormatException)
                    {
                        // Set to false in case another CellParsing handler
                        // wants to try to parse this DataGridViewCellParsingEventArgs instance.
                        e.ParsingApplied = false;
                    }
                }
            }
        }

        private void DataGridViewContract_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Завершаем редактирование ComboBoxCell с вводом нового значения
            if (cbNeedFill)
            {
                // заносим новое значение в редактируемую ячейку
                dataGridViewContract[e.ColumnIndex, e.RowIndex].Value = cbCellValue;

                cbNeedFill = false; // сбрасываем флаг
            }

            #region if DEBUG
#if DEBUG
            PrintDebugInfo("CellEndEdit", e.ColumnIndex, e.RowIndex);
#endif
            #endregion
        }

        // 
        private void DataGridViewContract_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Если эта ячейка DataGridViewComboBoxColumn, добавляем новое значение в список
            if (dataGridViewContract.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn ComboBoxColumn    // это ComboBox
                && !ComboBoxColumn.Items.Contains(e.FormattedValue)                                         // этой строки ещё нет
                && e.FormattedValue.ToString().Trim() != "")                                                // строка не пустая
            {
                cbCellValue = e.FormattedValue.ToString();  // Новое значение в ComboBoxCell
                cbNeedFill = true;                          // необходимо добавить пункт в ComboBoxCell
                ComboBoxColumn.Items.Add(cbCellValue);      // Добавляем новое значение в список ComboBoxColum
            }
            #region if DEBUG
#if DEBUG
            PrintDebugInfo("CellValidating", e.ColumnIndex, e.RowIndex, $"\n{e.FormattedValue}");
#endif
            #endregion
        }

        //------------------------------------------------------------------------------------------------------- 
        // События с клавиатуры
        // Используются для организации перехода по строке при нажатии Enter
        // Если ячейка находится в режиме редактирования - не вызывается
        //-------------------------------------------------------------------------------------------------------
        #region События клавиатуры: PreviewKeyDown, KeyDown

        private void DataGridViewContract_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Tab:

                    e.IsInputKey = false;        // разрешаем вызов KeyDown
                    break;

                case Keys.Home:
                    if (NeedBackStyle)
                        e.IsInputKey = true;    // разрешаем вызов KeyDown для возврата цвета ячеек к исходным
                    break;
            }

            #region if DEBUG
#if DEBUG
            Point cc = dataGridViewContract.CurrentCellAddress;

            PrintDebugInfo("PreviewKeyDown", cc.X, cc.Y, $"\n{e.KeyData}");
#endif
        #endregion
        }

        private void DataGridViewContract_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Tab:
                    e.Handled = true;   // сообщаем, что нажатие Enter обработано

                    NextColumn();       // переходим в колонку справа
                    break;

                case Keys.Right:
                case Keys.Down:
                    break;

                case Keys.Home:
                    if (NeedBackStyle)  // необходимо вернуть цвет ячеек?
                    {
                        dataGridViewContract.DefaultCellStyle.SelectionBackColor = savedSelectionBackColor;
                        dataGridViewContract.DefaultCellStyle.SelectionForeColor = savedSelectionForeColor;
                        NeedBackStyle = false;
                    }
                    break;


                case Keys.Delete:
                    break;

                default:
                    break;
            }

            #region if DEBUG
#if DEBUG
            Point cc = dataGridViewContract.CurrentCellAddress;

            PrintDebugInfo("KeyDown", cc.X, cc.Y, $"\n{e.KeyCode.ToString()}");
#endif
        #endregion
        }

        #endregion

        //------------------------------------------------------------------------------------------------------- 
        // События с клавиатуры для элементов, встроенных в ячейку, находящихся в режиме редактирования
        // Используются для организации перехода по строке при нажатии Enter
        // 1. Устанавливаем EditingControlShowing - в нём устанавливаем событие EditingControl_PreviewKeyDown
        // 2. В EditingControl_PreviewKeyDown. Для TextBoxEditingControl, при нажатии Enter, вызываем EndEdit()
        // 3. Устанавливаем EditingControl_KeyDown, для обработки нажатия Enter - переход в строку справа
        //    с помощью NextColumn()
        //-------------------------------------------------------------------------------------------------------
        #region События клавиатуры в режиме редактирования: EditingControlShowing, EditingControl_PreviewKeyDown, EditingControl_KeyDown 

        private void DataGridViewContract_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // определяем тип редактируемого столбца и перехватыаем нажатие Enter


            if (e.Control is DataGridViewTextBoxEditingControl dtb)
            {
                // добавляем события PreviewKeyDown - включаем вызов события KeyDown
                // KyeDown - указываем что событие обработано (нажатие Enter)
                dtb.PreviewKeyDown -= DataGridViewTextBoxEditingControl_PreviewKeyDown; // удаляем, если есть
                dtb.PreviewKeyDown += DataGridViewTextBoxEditingControl_PreviewKeyDown; // добавляем

                // с данным элементом, событие KeyDown вызывается только после вызова EndEdit()
                dtb.KeyDown -= DataGridViewEditingControl_KeyDown; // удаляем, если есть
                dtb.KeyDown += DataGridViewEditingControl_KeyDown; // добавляем

                return;
            }


            if (e.Control is DataGridViewComboBoxEditingControl dcb)
            {
                // добавляем события PreviewKeyDown - включаем вызов события KeyDown
                // KyeDown - указываем что событие обработано(нажатие Enter)
                dcb.PreviewKeyDown -= DataGridViewComboBoxEditingControl_PreviewKeyDown; // удаляем, если есть
                dcb.PreviewKeyDown += DataGridViewComboBoxEditingControl_PreviewKeyDown; // добавляем

                dcb.KeyDown -= DataGridViewEditingControl_KeyDown; // удаляем, если есть
                dcb.KeyDown += DataGridViewEditingControl_KeyDown; // добавляем

                // включаем для ComboBoxCell стиль с редактированием текста и добавлением нового пункта
                dcb.DropDownStyle = ComboBoxStyle.DropDown;

                return;
            }
        }

        //--------------------- для ComboBox -------------------------
        private void DataGridViewComboBoxEditingControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Tab:
                case Keys.Right:

                    e.IsInputKey = true;        // разрешаем событие KeyDown для управляющих клавиш
                    break;
            }
        }

        //------------------------ для TextBox --------------------------
        private void DataGridViewTextBoxEditingControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;        // разрешаем событие KeyDown для управляющих клавиш
                                            // из ControlTextBox? событие KeyDown вызывается только после вызова EndEdit()

                dataGridViewContract.EndEdit(); // Завершаем редактирование. После этого произойдёт вызов KeyDown
            }
        }

        private void DataGridViewEditingControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.Handled = true;

                NextColumn();
            }

            #region if DEBUG
#if DEBUG
            Point cc = dataGridViewContract.CurrentCellAddress;

            PrintDebugInfo("EditingControl_KeyDown", cc.X, cc.Y, $"\n{e.KeyData}");
#endif
#endregion
        }
        #endregion
    }
}
