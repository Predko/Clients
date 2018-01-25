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

        private string cbCellValue;                 // Значение введённой строки в ComboBoxCell
        private Point cbCellPoint;                  // Координаты редактируемой ячейки ComboBoxCell
        private bool cbNeedFill = false;            // Флаг, указывающий на необходимость зафиксировать введённое значение в ячейке
        private bool isComboBox = false;
        private object formattedValue;

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
            dataGridViewContract.RowValidating += DataGridViewContract_RowValidating;

            dataGridViewContract.EditingControlShowing += DataGridViewContract_EditingControlShowing;

            dataGridViewContract.KeyDown += DataGridViewContract_KeyDown;
            dataGridViewContract.PreviewKeyDown += DataGridViewContract_PreviewKeyDown;

            InitDebugInfoEvent();

            InitComboBoxColumns();
        }


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


        //----------------------------------------------------------------------------------------
        //                      Переходит на следующую ячейку в строке.
        //----------------------------------------------------------------------------------------
        private void NextColumn()
        {
            Point currentCell = dataGridViewContract.CurrentCellAddress; // адрес редактируемой ячейки

            // проверяем, последняя ли колонка
            if (currentCell.X >= dataGridViewContract.ColumnCount - 1)
            {
                currentCell.X = 0;      // если да, переходим в первую колонку
                currentCell.Y++;        // следующей строки

                if (currentCell.Y == dataGridViewContract.RowCount && !dataGridViewContract.IsCurrentRowDirty)
                {
                    // нельзя покидать строку, если не создана новая
                    // и текущая прошла проверку на правильность значений
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
            {
                currentCell.X++;             // иначе, переходим в следующую колонку
                SendKeys.Send("{RIGHT}");   // эмулируем нажатие клавиши "Right" - уходим из этой ячейки в следующую
                                            // Важно! При изменении клавиши перехода, изменить обработку в PreviewKeyDown и KeyDown
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
            return;
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

        private void EditComboBoxCell(Object f)
        {
            var FormattedValue = f;

            Point p = dataGridViewContract.CurrentCellAddress;

            var cbCell = dataGridViewContract[p.X, p.Y] as DataGridViewComboBoxCell;

            if (cbCell?.Items.Contains(FormattedValue) == false)
            {
                cbCell.Items.Add(FormattedValue);        // Добавляем новый пункт
                cbCellValue = FormattedValue.ToString(); // Новое значение в ComboBoxCell
                cbNeedFill = true;           // необходимо добавить пункт в ComboBoxCell
            }
        }

        // 
        private void dataGridViewContract_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            return;
            if (formattedValue != null)
                EditComboBoxCell(formattedValue);
            else
                EditComboBoxCell(e.FormattedValue);



            //if (e.ColumnIndex == dataGridViewContract.Columns["ColumnNameService"].Index
            //    || e.ColumnIndex == dataGridViewContract.Columns["ColumnNameDevice"].Index
            //    || e.ColumnIndex == dataGridViewContract.Columns["ColumnSubdivision"].Index)
            //{
            //    var cbCell = dataGridViewContract[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;

            //    if (cbCell?.Items.Contains(e.FormattedValue) == false)
            //    {
            //        cbCell.Items.Add(e.FormattedValue);        // ???
            //        cbCellValue = e.FormattedValue.ToString(); // Новое значение в ComboBoxCell
            //        cbNeedFill = true;           // необходимо добавить пункт в ComboBoxCell
            //    }
            //}
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

            var dtb = e.Control as DataGridViewTextBoxEditingControl;

            if (dtb != null)
            {
                // добавляем события PreviewKeyDown - включаем вызов события KeyDown
                // KyeDown - указываем что событие обработано (нажатие Enter)
                dtb.PreviewKeyDown -= DataGridViewTextBoxEditingControl_PreviewKeyDown; // удаляем, если есть
                dtb.PreviewKeyDown += DataGridViewTextBoxEditingControl_PreviewKeyDown; // добавляем

                isComboBox = false;

                // с данным элементом, событие KeyDown вызывается только после вызова EndEdit()
                dtb.KeyDown -= DataGridViewEditingControl_KeyDown; // удаляем, если есть
                dtb.KeyDown += DataGridViewEditingControl_KeyDown; // добавляем

                return;
            }

            var dcb = e.Control as DataGridViewComboBoxEditingControl;

            if (dcb != null)
            {
                // добавляем события PreviewKeyDown - включаем вызов события KeyDown
                // KyeDown - указываем что событие обработано(нажатие Enter)
                dcb.PreviewKeyDown -= DataGridViewComboBoxEditingControl_PreviewKeyDown; // удаляем, если есть
                dcb.PreviewKeyDown += DataGridViewComboBoxEditingControl_PreviewKeyDown; // добавляем

                dcb.KeyDown -= DataGridViewEditingControl_KeyDown; // удаляем, если есть
                dcb.KeyDown += DataGridViewEditingControl_KeyDown; // добавляем

                // включаем для ComboBoxCell стиль с редактированием текста и добавлением нового пункта
                //dcb.DropDownStyle = ComboBoxStyle.DropDown;
                //isComboBox = true;

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
        
            return;
            formattedValue = dataGridViewContract.CurrentCell.EditedFormattedValue;

            dataGridViewContract.EndEdit();
        }

        //------------------------ для TextBox --------------------------
        private void DataGridViewTextBoxEditingControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;        // разрешаем событие KeyDown для управляющих клавиш
                                            // из ControlTextBox? событие KeyDown вызывается только после вызова EndEdit()

                dataGridViewContract.EndEdit(); // Завершаем редактирование. После этого произойдёт вызов KeyDown

                return;
                // если ячейка этого типа завершаем редактирование ячейки
                // в EndEdit() происходит вызов NextColumn()
                // ячейка DataGridViewComboBoxCell обрабатывается в KeyDown

                if (isComboBox)
                    formattedValue = dataGridViewContract.CurrentCell.EditedFormattedValue;
                else
                    formattedValue = null;

                if (sender is DataGridViewComboBoxCell)
                {
                    Object f = dataGridViewContract.CurrentCell.EditedFormattedValue;
                }

                if (sender is DataGridViewComboBoxEditingControl)
                {
                    Object f = dataGridViewContract.CurrentCell.EditedFormattedValue;
                }



            }
        }

        private void DataGridViewEditingControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.Handled = true;

                NextColumn();

                //PrintDebugInfo("EditingControl_KeyDown", currentCell.X, currentCell.Y, $"\n{e.KeyCode.ToString()}");

                return;
                formattedValue = dataGridViewContract.CurrentCell.FormattedValue;

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
