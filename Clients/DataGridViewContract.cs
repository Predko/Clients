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

            // dataGridViewContract.RowTemplate = dataGridViewContract.Rows[0]; // шаблонная строка
        }

        //----------------------------------------------------------------------------------------
        // Переходит на следующую ячейку в строке.
        // Если строка закончилась, переходит на следующую, добавляя её, если нет
        //----------------------------------------------------------------------------------------
        private void NextColumn()
        {
            Point TargetCell = EditingCell;

            // проверяем, последняя ли колонка
            if (TargetCell.X >= dataGridViewContract.ColumnCount - 1)
            {
                TargetCell.X = 1;      // если да, переходим в первую колонку(колонка 0 содержит порядковый номер
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
            e.Row.Cells[0].Value = e.Row.Index + 1;
            
            e.Row.Cells[1].Value = ColumnNameService.Items[0];
            e.Row.Cells[2].Value = ColumnNameDevice.Items[0];
            e.Row.Cells[3].Value = ColumnSubdivision.Items[0];

            e.Row.Cells[4].Value = 14;
            e.Row.Cells[5].Value = 12;

        }

        // проверка правильности введённых значений в строке
        private void dataGridViewContract_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!dataGridViewContract.IsCurrentRowDirty)
                e.Cancel = true;
        }

        private void dataGridViewContract_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;

            if (KeyEnterPressed)
            {
                NextColumn();
                KeyEnterPressed = false;
            }

            string s = (ri >= 0 && ci >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellEndEdit:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }

        private void dataGridViewContract_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.Cell.State == DataGridViewElementStates.Selected)     // выбор новой ячейки
            {
                EditingCell.X = e.Cell.ColumnIndex;
                EditingCell.Y = e.Cell.RowIndex;
            }

            Point cc = dataGridViewContract.CurrentCellAddress;

            richTextBoxDebug.AppendText($"\nCellStateChanged: C={e.Cell.ColumnIndex,3}, R={e.Cell.RowIndex,3}\tValue = \"{e.Cell.Value}\""
                + $"\n{cc.ToString()}"
                + $"\n{e.Cell.State}");
        }

        //------------------------------------------------------------------------------------------------------- 
        // События с клавиатуры
        // Используются для организации перехода по строке при нажатии Enter 
        //-------------------------------------------------------------------------------------------------------
        #region PreviewKeyDown, KeyDown

        private void DataGridViewContract_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Tab:
                    e.IsInputKey = true;
                    break;
            }

#if DEBUG
            Point cc = dataGridViewContract.CurrentCellAddress;

            string s = (cc.Y >= 0 && cc.X >= 0) ? dataGridViewContract[cc.X, cc.Y]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nPreviewKeyDown:   Column = {cc.X,3}, Row = {cc.Y,3}\tValue = \"{s}\""
                + $"\n{e.KeyValue}");
#endif
        }

        private void dataGridViewContract_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                KeyEnterPressed = true;

                NextColumn();

                e.Handled = true;   // сообщаем, что нажатие Enter обработано
            }

#if DEBUG
            Point cc = dataGridViewContract.CurrentCellAddress;

            string s = (cc.X >= 0 && cc.Y >= 0) ? dataGridViewContract[cc.X, cc.Y]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nKeyDown:   Column = {cc.X,3}, Row = {cc.Y,3}\tValue = \"{s}\"");
#endif
        }
        #endregion

        #region DEBUG // События, используемые для отладки

#if DEBUG // События, используемые для отладки
        private void DataGridViewContract_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;

            string s = (ri >= 0 && ci >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellClick:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }

        private void DataGridViewContract_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;

            string s = (ri >= 0 && ci >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellEnter:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");

        }

        private void DataGridViewContract_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            EditingCell.X = e.ColumnIndex;  // Фиксируем координаты редактируемой ячейки
            EditingCell.Y = e.RowIndex;     //

            string s = (EditingCell.Y >= 0 && EditingCell.X >= 0) ? dataGridViewContract[EditingCell.X, EditingCell.Y]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellBeginEdit:   Column = {EditingCell.X,3}, Row = {EditingCell.Y,3}\tValue = \"{s}\"");
        }

        private void dataGridViewContract_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;

            string s = (ri >= 0 && ci >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellLeave:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }

        private void dataGridViewContract_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;
            string s = (ri >= 0 && ci >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellValueChanged:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }

        private void dataGridViewContract_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;

            string s = (ri >= 0 && ci >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellValidated:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }

        private void dataGridViewContract_KeyPress(object sender, KeyPressEventArgs e)
        {
            Point cc = dataGridViewContract.CurrentCellAddress;

            string s = (cc.Y >= 0 && cc.X >= 0) ? dataGridViewContract[cc.X, cc.Y]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nKeyPress:   Column = {cc.X,3}, Row = {cc.Y,3}\tValue = \"{s}\""
                + $"\n{e.ToString()}");
        }
#endif
        #endregion

        //------------------------------------------------------------------------------------------------------- 
        // События с клавиатуры для элементов, встроенных в ячейку, находящихся в режиме редактирования
        // Используются для организации перехода по строке при нажатии Enter 
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
                dtb.PreviewKeyDown += new PreviewKeyDownEventHandler(DataGridViewEditingControl_PreviewKeyDown);

                // Проблема!
                // с данным элементом, событие KeyDown не вызывается!...
                // ... усли в EditingControl_PreviewKeyDown не добавить dataGridViewContract.EndEdit()...
                dtb.KeyDown += new KeyEventHandler(DataGridViewEditingControl_KeyDown);

                return;
            }

            var dcb = e.Control as DataGridViewComboBoxEditingControl;

            if (dcb != null)
            {
                // добавляем события PreviewKeyDown - включаем вызов события KeyDown
                // KyeDown - указываем что событие обработано(нажатие Enter
                dcb.PreviewKeyDown += new PreviewKeyDownEventHandler(DataGridViewEditingControl_PreviewKeyDown);

                dcb.KeyDown += new KeyEventHandler(DataGridViewEditingControl_KeyDown);
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
                dataGridViewContract.EndEdit();
            }
        }

        private void DataGridViewEditingControl_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    var dcb = sender as DataGridViewComboBoxEditingControl;
            //    if (dcb != null)
            //    {
            //        KeyEnterPressed = true;
            //        e.Handled = true;           // Сообщаем, что нажатие клавиши Enter обработано
            //        e.SuppressKeyPress = true;
            //    }

            //    var dtb = sender as DataGridViewTextBoxEditingControl;
            //    if (dtb != null)
            //    {
            //        KeyEnterPressed = true;
            //        e.SuppressKeyPress = true;
            //        e.Handled = true;           // Сообщаем, что нажатие клавиши Enter обработано
            //    }
            //}
        }
        #endregion
    }
}
