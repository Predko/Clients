using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace Clients
{
    //------------------------------------------------------------------------
    //                          dataGridViewContract
    //------------------------------------------------------------------------



    public partial class Clients : Form
    {
        private bool NeedNextColumns = false;       // перейти в следующую колонку


        //--------- Методы ----------

        // Инициализация ячеек, содержащих элемент ComboBoxColumn
        private void InitComboBoxColumns()
        {
            string[] cns = { "Заправка картриджа", "Восстановление картриджа", "Прошивка чипа картриджа", "Ремонт узла закрепления принтера" };

            ColumnNameService.Items.AddRange(cns);
            dataGridViewContract["ColumnNameService", 0].Value = cns[0];

            string[] cnd = { "Canon 725", "Canon 728", "Canon 737", "Hp 35a", "Hp 85a", "Hp 12a", "Hp 49a", "Hp 53a" };

            ColumnNameDevice.Items.AddRange(cnd);
            dataGridViewContract["ColumnNameDevice", 0].Value = cnd[0];

            string[] csd = { "к.401", "к.402", "к.403", "к.405", "к.406", "к.407" };

            ColumnSubdivision.Items.AddRange(csd);
            dataGridViewContract["ColumnSubdivision", 0].Value = csd[0];

            dataGridViewContract.TopLeftHeaderCell.Value = "№";
        }

        //----------------------------------------------------------------------------------------
        // Переходит на следующую ячейку в строке.
        // Если строка закончилась, переходит на следующую, добавляя её, если нет
        //----------------------------------------------------------------------------------------
        private void NextColumn()
        {
            Point TargetCell = dataGridViewContract.CurrentCellAddress;

            // проверяем, последняя ли колонка
            if (TargetCell.X >= dataGridViewContract.ColumnCount - 1)
            {
                TargetCell.X = 0;      // если да, переходим в первую колонку
                TargetCell.Y++;        // следующей строки

                if (TargetCell.Y == dataGridViewContract.RowCount && !dataGridViewContract.IsCurrentRowDirty)
                                // нельзя покидать строку, не прошедшую проверку
                        return; // на правильность значений
            }
            else
                TargetCell.X++;        // иначе, переходим в следующую колонку

            // делаем следующую колонку текущей
            // если её строка создана.
            // новая строка создаётся автоматически, если последняя редактировалась.
            if (TargetCell.Y != dataGridViewContract.RowCount)
                dataGridViewContract.CurrentCell = dataGridViewContract[TargetCell.X, TargetCell.Y];
        }

        // устанавливаем номера строк в заголовки строк, начиная с номера строки beginIndex
        private void SetRowsHeaderValue(int beginIndex)
        {
            while(beginIndex != dataGridViewContract.Rows.Count) // начинаем с заданного индекса
            {
                dataGridViewContract.Rows[beginIndex].HeaderCell.Value = String.Format("{0,3}", ++beginIndex);
            }

            // пересчитываем размер заголовков строк
            dataGridViewContract.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        //------------------- События ------------------------

        private void DataGridViewContract_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // Заголовок строки содержит нумерацию строк
            SetRowsHeaderValue(0);

            // Значения по умолчанию для новых ячеек, содержащих ComboBox
            e.Row.Cells["ColumnNameService"].Value = ColumnNameService.Items[0];
            e.Row.Cells["ColumnNameDevice"].Value = ColumnNameDevice.Items[0];
            e.Row.Cells["ColumnSubdivision"].Value = ColumnSubdivision.Items[0];

            // Значения по умолчанию для новых ячеек, содержащих TextBox
            e.Row.Cells["ColumnServiceNumb"].Value = "";
            e.Row.Cells["ColumnServiceSumm"].Value = "";
        }

        // при удалении строки, переписываем изначения заголовков строк, начиная со следующей, после удалённой
        private void DataGridViewContract_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SetRowsHeaderValue(e.RowIndex);
        }

        // проверка правильности введённых значений в строке (не работает)
        private void DataGridViewContract_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if ((string)dataGridViewContract.Rows[e.RowIndex].Cells["ColumnServiceNumb"].Value == ""
            //    || (string)dataGridViewContract.Rows[e.RowIndex].Cells["ColumnServiceNumb"].Value == "")
            //    e.Cancel = true;
        }

        private void DataGridViewContract_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (NeedNextColumns)
            {
                NextColumn();               // переходим на следующую ячейку справа
                NeedNextColumns = false;
            }

            #region if DEBUG
#if DEBUG
            PrintDebugInfo("CellEndEdit", e.ColumnIndex, e.RowIndex, "\n{e.Cell.Value}\t{e.Cell.State}");
#endif
            #endregion
        }

        //------------------------------------------------------------------------------------------------------- 
        // События с клавиатуры
        // Используются для организации перехода по строке при нажатии Enter
        // Если ячейка находится в режиме редактирования - не вызывается
        //-------------------------------------------------------------------------------------------------------
        #region PreviewKeyDown, KeyDown

        private void DataGridViewContract_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Tab:
                    e.IsInputKey = true;        // разрешаем вызов KeyDown
                    break;
            }

        #region if DEBUG
#if DEBUG
            Point cc = dataGridViewContract.CurrentCellAddress;

            PrintDebugInfo("PreviewKeyDown", cc.X, cc.Y, "\n{e.KeyData}");
#endif
        #endregion
        }

        private void DataGridViewContract_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Tab:
                    NextColumn();       // делаем активной колонку справа

                    e.Handled = true;   // сообщаем, что нажатие Enter обработано
                    break;

                case Keys.Delete:
                    break;
            }

            #region if DEBUG
#if DEBUG
            Point cc = dataGridViewContract.CurrentCellAddress;

            PrintDebugInfo("KeyDown", cc.X, cc.Y, "\n{e.KeyData}");
#endif
        #endregion
        }
#endregion

        #region DEBUG // События, используемые для отладки

#if DEBUG // События, используемые для отладки

        private void PrintDebugInfo(string name, int col, int row, string s = null)
        {
            string sv = (col >= 0 && row >= 0) ? dataGridViewContract[col, row]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\n{name}:   Column = {col,3}, Row = {row,3}\tValue = \"{sv}\"{s}");
        }


        // используется для определения текущей ячейки
        private void DataGridViewContract_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            #region if DEBUG
#if DEBUG
            Point cc = dataGridViewContract.CurrentCellAddress;

            PrintDebugInfo("CellStateChanged", cc.X, cc.Y, "\n{e.Cell.Value}\n{cc.ToString()}\t{e.Cell.State}");
#endif
            #endregion
        }

        private void DataGridViewContract_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            PrintDebugInfo("CellClick", e.ColumnIndex, e.RowIndex);
        }

        private void DataGridViewContract_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            PrintDebugInfo("CellEnter", e.ColumnIndex, e.RowIndex);
        }

        private void DataGridViewContract_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            PrintDebugInfo("CellBeginEdit", e.ColumnIndex, e.RowIndex);
        }

        private void DataGridViewContract_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            PrintDebugInfo("CellLeave", e.ColumnIndex, e.RowIndex);
        }

        private void DataGridViewContract_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            PrintDebugInfo("CellValueChanged", e.ColumnIndex, e.RowIndex);
        }

        private void DataGridViewContract_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            PrintDebugInfo("CellValidated", e.ColumnIndex, e.RowIndex);
        }

        private void DataGridViewContract_KeyPress(object sender, KeyPressEventArgs e)
        {
            Point cc = dataGridViewContract.CurrentCellAddress;

            PrintDebugInfo("KeyPress", cc.X, cc.Y, "\n{e.ToString()}");
        }
#endif
        #endregion

        //------------------------------------------------------------------------------------------------------- 
        // События с клавиатуры для элементов, встроенных в ячейку, находящихся в режиме редактирования
        // Используются для организации перехода по строке при нажатии Enter
        // 1. Устанавливаес обработчик события CellEndEdit.
        //    В нём  производим переход в следующую ячейку справа, если была нажата Enter
        // 2. EditingControlShowing - устанавливаем событие EditingControl_PreviewKeyDown
        // 3. В EditingControl_PreviewKeyDown, при нажатии Enter, вызываем DataGridView_***.EndEdit() 
        //-------------------------------------------------------------------------------------------------------
        #region EditingControlShowing, EditingControl_PreviewKeyDown, EditingControl_KeyDown 

        private void DataGridViewContract_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            // определяем тип редактируемого столбца и перехватыаем нажатие Enter

            var dtb = e.Control as DataGridViewTextBoxEditingControl;

            if (dtb != null)
            {
                // добавляем события PreviewKeyDown - включаем вызов события KeyDown
                // KyeDown - указываем что событие обработано - нажатие Enter
                dtb.PreviewKeyDown -= DataGridViewEditingControl_PreviewKeyDown; // удаляем, если есть
                dtb.PreviewKeyDown += DataGridViewEditingControl_PreviewKeyDown; // добавляем

                #region if DEBUG
#if DEBUG
                // Проблема!
                // с данным элементом, событие KeyDown не вызывается!...
                // поэтому событие EditingControl_KeyDown не нужно
                // используем только для отладки
                dtb.KeyDown -= DataGridViewEditingControl_KeyDown; // удаляем, если есть
                dtb.KeyDown += DataGridViewEditingControl_KeyDown; // добавляем
#endif
                #endregion

                return;
            }

            var dcb = e.Control as DataGridViewComboBoxEditingControl;

            if (dcb != null)
            {
                // добавляем события PreviewKeyDown - включаем вызов события KeyDown
                // KyeDown - указываем что событие обработано(нажатие Enter
                dcb.PreviewKeyDown -= new PreviewKeyDownEventHandler(DataGridViewEditingControl_PreviewKeyDown); // удаляем, если есть
                dcb.PreviewKeyDown += new PreviewKeyDownEventHandler(DataGridViewEditingControl_PreviewKeyDown); // добавляем

                #region if DEBUG
#if DEBUG
                // используем только для отладки
                dcb.KeyDown -= DataGridViewEditingControl_KeyDown; // удаляем, если есть
                dcb.KeyDown += DataGridViewEditingControl_KeyDown; // добавляем
#endif
                #endregion

                return;
            }
        }

        private void DataGridViewEditingControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;        // разрешаем событие KeyDown для управляющих клавиш
                                            // из ControlTextBox вызов KeyDown при (KeyCode == Keys.Enter) не происходит

                NeedNextColumns = true;     // необходимо перейти в следующую колонку

                // завершаем редактирование ячейки
                // в EndEdit() происходит вызов NextColumn()
                dataGridViewContract.EndEdit();
            }
        }

#if DEBUG
        private void DataGridViewEditingControl_KeyDown(object sender, KeyEventArgs e)
        {
            Point cc = dataGridViewContract.CurrentCellAddress;

            PrintDebugInfo("EditingControl_KeyDown", cc.X, cc.Y, "\n{e.KeyData}");
        }
#endif
        #endregion
    }
}
