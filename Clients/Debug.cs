using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Clients
{
    public partial class Clients : Form
    {
#if DEBUG // События, используемые для отладки

        private void InitDebugInfoEvent()
        {
            dataGridViewContract.CellBeginEdit += DataGridViewContract_CellBeginEdit;
            dataGridViewContract.CellClick += DataGridViewContract_CellClick;
            dataGridViewContract.CellEnter += DataGridViewContract_CellEnter;
            dataGridViewContract.CellLeave += DataGridViewContract_CellLeave;
            dataGridViewContract.CellStateChanged += DataGridViewContract_CellStateChanged;
            dataGridViewContract.CellValidated += DataGridViewContract_CellValidated;
            dataGridViewContract.CellValueChanged += DataGridViewContract_CellValueChanged;

        }

        private void PrintDebugInfo(string name, int col, int row, string s = null)
        {
            string sv = (col >= 0 && row >= 0) ? dataGridViewContract[col, row]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\n{name}:   Column = {col,3}, Row = {row,3}\tValue = \"{sv}\"{s}");
        }



        // используется для определения текущей ячейки
        private void DataGridViewContract_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            Point cc = dataGridViewContract.CurrentCellAddress;

            PrintDebugInfo("CellStateChanged", cc.X, cc.Y, $"\n{e.Cell.Value}\t{e.Cell.State}");
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

#endif
    }
}
