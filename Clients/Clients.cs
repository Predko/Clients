using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using System.IO;


using static System.Console;

namespace Clients
{
    public partial class Clients : Form
    {
        public readonly ListClients clients = new ListClients();

        static Point cc;

        

        public Clients()
        {
            InitializeComponent();

            InitComboBoxClients();

            //ColumnNumb.T

            string[] cns = { "Заправка картриджа", "Восстановление картриджа", "Прошивка чипа картриджа", "Ремонт узла закрепления принтера" };

            ColumnNameService.Items.AddRange(cns);
            dataGridViewContract["ColumnNameService", 0].Value = cns[0];

            string[] cnd = { "Canon 725", "Canon 728", "Canon 737", "Hp 35a", "Hp 85a", "Hp 12a", "Hp 49a", "Hp 53a" };

            ColumnNameDevice.Items.AddRange(cnd);
            dataGridViewContract["ColumnNameDevice", 0].Value = cnd[0];

            string[] csd = { "к.401", "к.402", "к.403", "к.405", "к.406", "к.407" };

            ColumnSubdivision.Items.AddRange(csd);
            dataGridViewContract["ColumnSubdivision", 0].Value = csd[0];


        }

        private void DataGridViewContract_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;

            string s = (ri >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString(): null;

            richTextBoxDebug.AppendText($"\nCellClick:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }

        private void DataGridViewContract_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;

            string s = (ri >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellBeginEdit:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }

        private void dataGridViewContract_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;

            string s = (ri >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellEndEdit:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }

        private void dataGridViewContract_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;
            string s = (ri >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellValueChanged:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }

        private void dataGridViewContract_KeyDown(object sender, KeyEventArgs e)
        {
            int ci = cc.X;
            int ri = cc.Y;

            string s = (ri >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nKeyDown:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }

        // реализация перемещения фокуса ввода при нажатии Enter
        private void dataGridViewContract_KeyUp_1(object sender, KeyEventArgs e)
        {
            int countC = dataGridViewContract.ColumnCount;
            int countR = dataGridViewContract.RowCount;
            int ci = cc.X;
            int ri = cc.Y;

            DataGridViewCell cell = dataGridViewContract[ci, ri]; // текущая ячейка

            DataGridViewColumn currentColumn = dataGridViewContract.Columns[ci];

            if (ci >= countC - 1)
            {
                ci = 0;
                ri++;
                if (ri == countR)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridViewContract.CurrentRow.Clone();
                    dataGridViewContract.Rows.Add(row);
                }
            }
            else
                ci++;

            dataGridViewContract.CurrentCell = dataGridViewContract[ci, ri];


            string s = (cc.Y >= 0) ? dataGridViewContract[cc.X, cc.Y]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nKeyUp_1:   Column = {cc.X,3}, Row = {cc.Y,3}\tValue = \"{s}\"");
        }

        private void dataGridViewContract_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;

            string s = (ri >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellEnter:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");

        }

        private void dataGridViewContract_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;

            string s = (ri >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellLeave:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }

        private void dataGridViewContract_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;

            cc = dataGridViewContract.CurrentCellAddress;

            string s = (ri >= 0) ? dataGridViewContract[ci, ri]?.Value?.ToString() : null;

            richTextBoxDebug.AppendText($"\nCellLeave:   Column = {ci,3}, Row = {ri,3}\tValue = \"{s}\"");
        }
    }
}
