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


        //--------- Методы ----------

        //------------------------------------------------------------------------------------
        // Инициализация.
        // Подключение событий DataGridViewContract
        //------------------------------------------------------------------------------------
        private void InitDataGridViewContract()
        {
            dataGridViewContract.CellEndEdit += DataGridViewContract_CellEndEdit;
            dataGridViewContract.CellValidating += dataGridViewContract_CellValidating;

            dataGridViewContract.DefaultValuesNeeded += DataGridViewContract_DefaultValuesNeeded;
            dataGridViewContract.RowsRemoved += DataGridViewContract_RowsRemoved;
            dataGridViewContract.RowsAdded += DataGridViewContract_RowsAdded;
            dataGridViewContract.RowValidating += DataGridViewContract_RowValidating;

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
            string[] cns = { "", "Заправка картриджа", "Восстановление картриджа", "Прошивка чипа картриджа", "Ремонт узла закрепления принтера" };

            ColumnNameWork.Items.AddRange(cns);
            dataGridViewContract["ColumnNameWork", 0].Value = cns[0];

            string[] cnd = { "", "Canon 725", "Canon 728", "Canon 737", "Hp 35a", "Hp 85a", "Hp 12a", "Hp 49a", "Hp 53a", "HP 05a" };

            ColumnNameDevice.Items.AddRange(cnd);
            dataGridViewContract["ColumnNameDevice", 0].Value = cnd[0];

            string[] csd = { "", "к.401", "к.402", "к.403", "к.405", "к.406", "к.407" };

            ColumnSubdivision.Items.AddRange(csd);
            dataGridViewContract["ColumnSubdivision", 0].Value = csd[0];

            string[] cai = { "", "фотовал", "доз.нож", "чист.нож", "без.запр", "вал зар.", "магн.вал" };

            ColumnAddInfo.Items.AddRange(cai);
            dataGridViewContract["ColumnAddInfo", 0].Value = cai[0];

            dataGridViewContract["ColumnServiceNumb",0].Value = 0;
            dataGridViewContract["ColumnServiceSumm",0].Value = (decimal)0;     // стоимость

            dataGridViewContract["ColumnIdService", 0].Value = -1;

            dataGridViewContract.TopLeftHeaderCell.Value = "№";

            ColumnSubdivision.DisplayIndex = 0;
            ColumnNameDevice.DisplayIndex = 1;
            ColumnAddInfo.DisplayIndex = 2;
            ColumnNameWork.DisplayIndex = 3;
            ColumnServiceNumb.DisplayIndex = 4;
            ColumnServiceSumm.DisplayIndex = 5;
        }

        // Устанавливаем значения списков ComboBoxColumn
        public void SetComboBoxColumns()
        {
            ColumnNameWork.Items.Clear();
            ColumnNameWork.Items.Add("");
            foreach (string s in AllNameWorks.Values)
            {
                ColumnNameWork.Items.Add(s);
            }
            dataGridViewContract["ColumnNameWork", 0].Value = AllNameWorks.Values[0];

            ColumnNameDevice.Items.Clear();
            ColumnNameDevice.Items.Add("");
            foreach (string s in AllNameDevices.Values)
            {
                ColumnNameDevice.Items.Add(s);
            }
            dataGridViewContract["ColumnNameDevice", 0].Value = AllNameDevices.Values[0];

            ColumnSubdivision.Items.Clear();
            foreach (string s in AllSubdivisions.Values)
            {
                ColumnSubdivision.Items.Add(s);
            }
            dataGridViewContract["ColumnSubdivision", 0].Value = AllSubdivisions.Values[0];

            ColumnAddInfo.Items.Clear();
            foreach (string s in AllAddInfo.Values)
            {
                ColumnAddInfo.Items.Add(s);
            }
            dataGridViewContract["ColumnAddInfo", 0].Value = AllAddInfo.Values[0];
        }

        private void ChangeServiceList_Event(object o, ChangingListServicesEventArgs e)
        {
            switch (e.type)
            {
                case Change.Add:

                    DataGridViewRow row = dataGridViewContract.Rows[dataGridViewContract.Rows.Add()];

                    // Если в ComboBoxCell нет этих пунктов, добавляем их
                    if (!ColumnNameWork.Items.Contains(e.service.Nw.Name))
                        ColumnNameWork.Items.Add(e.service.Nw.Name);
                    row.Cells["ColumnNameWork"].Value = e.service.Nw.Name;

                    if (!ColumnNameDevice.Items.Contains(e.service.Nd.Name))
                        ColumnNameDevice.Items.Add(e.service.Nd.Name);
                    row.Cells["ColumnNameDevice"].Value = e.service.Nd.Name;

                    if (!ColumnSubdivision.Items.Contains(e.service.Sd.Name))
                        ColumnSubdivision.Items.Add(e.service.Sd.Name);
                    row.Cells["ColumnSubdivision"].Value = e.service.Sd.Name;

                    if (!ColumnAddInfo.Items.Contains(e.service.Ai.InfoString))
                        ColumnAddInfo.Items.Add(e.service.Ai.InfoString);
                    row.Cells["ColumnAddInfo"].Value = e.service.Ai.InfoString;

                    // Заполяем остальные ячейки
                    row.Cells["ColumnServiceNumb"].Value = e.service.Number;
                    row.Cells["ColumnServiceSumm"].Value = e.service.Value;     // стоимость
                    row.Cells["ColumnIdService"].Value = e.service.Id;

                    break;

                case Change.Del:

                    int id;

                    row = null;
                    foreach (DataGridViewRow r in dataGridViewContract.Rows)
                    {
                        id = (int)r.Cells["ColumnIdService"].Value;
//                        int.TryParse(r.Cells["ColumnIdService"].Value.ToString(), out id);
                        if (id == e.service.Id)
                        {
                            row = r;
                            break;
                        }
                    }

                    if(row != null)
                        dataGridViewContract.Rows.Remove(row);

                    break;
                case Change.Clear:
                    break;
            }
        }

        public void ClearDataGridView()
        {
            int i = dataGridViewContract.Rows.Count;
            while (i-- > 1)   // Удаляем все, кроме последней, пустой ячейки
            {
                dataGridViewContract.Rows.RemoveAt(0); // удаляем каждый раз первую строку
            }
        }

        //------------------------------------------------------------------------------------
        // устанавливаем номера строк в заголовки строк, начиная с номера строки beginIndex
        //------------------------------------------------------------------------------------
        private void SetRowsHeaderValue(int beginIndex)
        {
            while (beginIndex != dataGridViewContract.Rows.Count) // начинаем с заданного индекса
            {
                dataGridViewContract.Rows[beginIndex].HeaderCell.Value = String.Format("{0,3}", ++beginIndex);
            }

            // пересчитываем размер заголовков строк
            dataGridViewContract.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        private void DataGridViewContract_Enter(object sender, EventArgs e)
        {
            Contract.ChangeServiceList -= ChangeServiceList_Event; // удаляем событие(на всякий случай.))

            // добавляем событие вызываемое при изменении списка услуг в текущем контракте
            Contract.ChangeServiceList += ChangeServiceList_Event;
            RemovedDgvRows = RemoveService;  // при удалении строк в DataGridView, будут удалятся услуги из договора
        }

        private void DataGridViewContract_Leave(object sender, EventArgs e)
        {
            Contract.ChangeServiceList -= ChangeServiceList_Event;       // удаляем событие
            RemovedDgvRows = null;  // при удалении строк в DataGridView, услуги в договоре удаляться не будут
        }

        private void DataGridViewContract_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // Заголовок строки содержит нумерацию строк
            SetRowsHeaderValue(0);

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

        // при удалении строки, переписываем изначения заголовков строк, начиная со следующей, после удалённой
        private void DataGridViewContract_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SetRowsHeaderValue(e.RowIndex);
            for (int i = 0; i != e.RowCount; i++)
            {
                // находим id услуги, соответствующий удаляемой строке и вызываем метод удаления услуги, если он подключен
                OnRemovedService((int)dataGridViewContract.Rows[e.RowIndex + i].Cells["ColumnIdService"].Value);
            }
        }

        private void OnRemovedService(int id)
        {
            RemovedDgvRows?.Invoke(id);
        }

        private void DataGridViewContract_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            SetRowsHeaderValue(e.RowIndex);

            Contract.ChangeServiceList -= ChangeServiceList_Event;       // удаляем событие для предотвращения рекурсивного вызова

            for (int i = 0; i != e.RowCount; i++)
            {
                DataGridViewRow dr = dataGridViewContract.Rows[e.RowIndex + i];

                if(dr.Cells["ColumnIdService"].Value != null && int.Parse(dr.Cells["ColumnIdService"].Value.ToString()) != -1)
                {
                    var sv = new Service(dr.Cells["ColumnNameWork"].Value.ToString(),
                                             dr.Cells["ColumnNameDevice"].Value.ToString(),
                                             dr.Cells["ColumnSubdivision"].Value.ToString(),
                                             int.Parse(dr.Cells["ColumnServiceNumb"].Value.ToString()),
                                             decimal.Parse(dr.Cells["ColumnServiceSumm"].Value.ToString()),
                                             int.Parse(dr.Cells["ColumnIdService"].Value.ToString()),
                                             dr.Cells["ColumnAddInfo"].Value.ToString());

                    CurrentContract.AddService(sv);
                }
            }

            Contract.ChangeServiceList += ChangeServiceList_Event;
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

        //------------------------------------------------------------------------------------
        //                              Обработчики событий 
        //------------------------------------------------------------------------------------

        // проверка правильности введённых значений в строке (не работает)
        private void DataGridViewContract_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if ((string)dataGridViewContract.Rows[e.RowIndex].Cells["ColumnServiceNumb"].Value == ""
            //    || (string)dataGridViewContract.Rows[e.RowIndex].Cells["ColumnServiceNumb"].Value == "")
            //    e.Cancel = true;
        }

        private void DataGridViewContract_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Завершаем редактирование ComboBoxCell с вводом нового значения
            if (cbNeedFill)
            {
                dataGridViewContract[e.ColumnIndex, e.RowIndex].Value = cbCellValue; // заносим новое значение в редактируемую ячейку
                cbNeedFill = false; // сбрасываем флаг
            }

            #region if DEBUG
#if DEBUG
            PrintDebugInfo("CellEndEdit", e.ColumnIndex, e.RowIndex);
#endif
            #endregion
        }

        // 
        private void dataGridViewContract_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Если эта ячейка DataGridViewComboBoxColumn, добавляем новое значение в список

            if (dataGridViewContract.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn ComboBoxColumn    // это ComboBox
                && !ComboBoxColumn.Items.Contains(e.FormattedValue)                                         // этой строки ещё нет
                && e.FormattedValue.ToString().Trim() != "")                                                // строка не пустая
            {
                cbCellValue = e.FormattedValue.ToString();  // Новое значение в ComboBoxCell
                cbNeedFill = true;                          // необходимо добавить пункт в ComboBoxCell
                ComboBoxColumn.Items.Add(cbCellValue);      // Добавляем новое значение в список ComboBoxColum
//                dataGridViewContract[e.ColumnIndex, e.RowIndex].Value = cbCellValue;
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
