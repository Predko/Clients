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
    public partial class Clients : Form
    {
        private bool KeyEnterPressed = false;       // флаг нажатия клавиши Enter
        private Point EditingCell;                  // координаты редактируемой ячейки


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
            Point TargetCell = EditingCell;
            Point cc = dataGridViewContract.CurrentCellAddress;

            if(cc != EditingCell) // можно использовать координаты текущей ячейки?
            {
                PrintDebugInfo("NextColumn currentCell:", cc.X, cc.Y);
                PrintDebugInfo("NextColumn EditingCell:", EditingCell.X, EditingCell.Y);
            }

            // проверяем, последняя ли колонка
            if (TargetCell.X >= dataGridViewContract.ColumnCount - 1)
            {
                TargetCell.X = 0;      // если да, переходим в первую колонку
                TargetCell.Y++;        // следующей строки

                if (TargetCell.Y == dataGridViewContract.RowCount)
                {
                    if (!dataGridViewContract.IsCurrentRowDirty) // нельзя покидать строку, не прошедшую проверку
                        return;                                  // на правильность значений

                    // добавляем шаблонную строку
                    dataGridViewContract.Rows.Add();
                }
            }
            else
                TargetCell.X++;        // иначе, переходим в следующую колонку

            

            // делаем следующую колонку текущей
            dataGridViewContract.CurrentCell = dataGridViewContract[TargetCell.X, TargetCell.Y];
        }

        private void DataGridViewContract_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.HeaderCell.Value = (e.Row.Index + 1).ToString();
            dataGridViewContract.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            e.Row.Cells["ColumnNameService"].Value = ColumnNameService.Items[0];
            e.Row.Cells["ColumnNameDevice"].Value = ColumnNameDevice.Items[0];
            e.Row.Cells["ColumnSubdivision"].Value = ColumnSubdivision.Items[0];

            e.Row.Cells["ColumnServiceNumb"].Value = 14;
            e.Row.Cells["ColumnServiceSumm"].Value = 12;

        }

        // проверка правильности введённых значений в строке
        private void dataGridViewContract_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!dataGridViewContract.IsCurrentRowDirty)
                e.Cancel = true;
        }

        private void dataGridViewContract_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (KeyEnterPressed)
            {
                NextColumn();               // переходим на следующую ячейку справа
                KeyEnterPressed = false;
            }

            #region if DEBUG
#if DEBUG
            PrintDebugInfo("CellEndEdit", e.ColumnIndex, e.RowIndex, "\n{e.Cell.Value}\t{e.Cell.State}");
#endif
            #endregion
        }

        // используется для определения текущей ячейки
        private void dataGridViewContract_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.Cell.State == DataGridViewElementStates.Selected)     // выбор новой ячейки
            {
                EditingCell.X = e.Cell.ColumnIndex; // Фиксируем координаты редактируемой ячейки
                EditingCell.Y = e.Cell.RowIndex;    //
            }

        #region if DEBUG
#if DEBUG
            Point cc = dataGridViewContract.CurrentCellAddress;

            PrintDebugInfo("CellStateChanged", cc.X, cc.Y, "\n{e.Cell.Value}\n{cc.ToString()}\t{e.Cell.State}");
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

        private void dataGridViewContract_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                KeyEnterPressed = true;  // сообщаем NextColumn(), что нажата Enter

                NextColumn();           // делаем активной колонку справа

                e.Handled = true;   // сообщаем, что нажатие Enter обработано
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

        private void dataGridViewContract_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            PrintDebugInfo("CellLeave", e.ColumnIndex, e.RowIndex);
        }

        private void dataGridViewContract_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            PrintDebugInfo("CellValueChanged", e.ColumnIndex, e.RowIndex);
        }

        private void dataGridViewContract_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            PrintDebugInfo("CellValidated", e.ColumnIndex, e.RowIndex);
        }

        private void dataGridViewContract_KeyPress(object sender, KeyPressEventArgs e)
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

        private void dataGridViewContract_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
            if (e.KeyCode == Keys.Enter)
            {
                e.IsInputKey = true;        // разрешаем событие KeyDown для управляющих клавиш
                KeyEnterPressed = true;

                // завершаем редактирование ячейки
                // (Без этого не работает вызов события DataGridViewEditingControl_KeyDown
                // там происходит вызов NextColumn()
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
