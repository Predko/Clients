using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace PrintGraphics
{
    public partial class PrintGraphics : Form
    {
        private readonly List<string> ls = new List<string>();     // загружаемые строки для печати/отрисовки

        private string unit;

        private Font CurrentFont;

        private RectangleF RectF;       // Размер области печати принтера по умолчанию

        private bool IsGetingPrintArea; // Данные области печати получены/нет

        public PrintGraphics()
        {
            InitializeComponent();

            comboBox1.DataSource = new BindingSource
            {
                DataSource = FontFamily.Families
            };

            comboBox1.DisplayMember = "Name";

            ResizeRedraw = true;

            AutoScroll = true;

            CurrentFont = this.Font;

            comboBox1.SelectedValueChanged += new EventHandler(ComboBox1_SelectedValueChanged);

            Load_strings();
        }

        private void ComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            CurrentFont = new Font((FontFamily)comboBox1.SelectedItem, 10);

            Invalidate();
        }

        private void Load_strings()
        {
            const string filename = "Info.txt";

            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    ls.Add(s);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            SizeF sz = g.MeasureString(" ", Font);

            float y = g.VisibleClipBounds.Y + 50f;

            foreach (string s in ls)
            {
                g.DrawString(s, CurrentFont, Brushes.Black, g.VisibleClipBounds.Width / 2 + g.VisibleClipBounds.X, y, sf);
                y += sz.Height;
            }

            base.OnPaint(e);
        }

        private void ChangePageScaleAndTranslateTransform(object sender, PaintEventArgs e)
        {

            // Create a rectangle.
            Rectangle rectangle1 = new Rectangle(20, 20, 50, 100);

            // Draw its outline.
            e.Graphics.DrawRectangle(Pens.SlateBlue, rectangle1);

            // Change the page scale.  
            e.Graphics.PageScale = 2.0F;

            // Call TranslateTransform to change the origin of the
            //  Graphics object.
            e.Graphics.TranslateTransform(10.0F, 10.0F);

            // Draw the rectangle again.
            e.Graphics.DrawRectangle(Pens.Tomato, rectangle1);

            // Set the page scale and origin back to their original values.
            e.Graphics.PageScale = 1.0F;
            e.Graphics.ResetTransform();

            SolidBrush transparentBrush = new SolidBrush(Color.FromArgb(50,
                Color.Yellow));

            // Create a new rectangle with the coordinates you expect
            // after setting PageScale and calling TranslateTransform:
            // x = (10 + 20) * 2
            // y = (10 + 20) * 2
            // Width = 50 * 2
            // Length = 100 * 2
            Rectangle newRectangle = new Rectangle(60, 60, 100, 200);

            // Fill in the rectangle with a semi-transparent color.
            e.Graphics.FillRectangle(transparentBrush, newRectangle);
        }

        private void GetPrintData_Click(object sender, EventArgs e)
        {
            Graphics g = CreateGraphics();

            Paint_Info(g);

            g.Dispose();
            //PrintDocument pd = new PrintDocument();

            //pd.PrintPage += new PrintPageEventHandler(GetPrintInfo);

            //pd.Print();

            //Paint -= new PaintEventHandler(ChangePageScaleAndTranslateTransform);

            //Paint += new PaintEventHandler(PrintGraphics_Paint);
        }

        private void GetPrintInfo(object sender, PrintPageEventArgs e)
        {
            RectF = e.PageSettings.PrintableArea;

            unit = nameof(e.Graphics.PageUnit);

            IsGetingPrintArea = true;
        }

        private void PrintGraphics_Paint(object sender, PaintEventArgs e)
        {
            if (!IsGetingPrintArea)
                return;

            Paint_Info(e.Graphics);
        }

        private void Paint_Info(Graphics g)
        {
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            SizeF sz = g.MeasureString(" ", Font);

            float y = g.VisibleClipBounds.Y + 50f;

            foreach (string s in ls)
            {
                g.DrawString(s, CurrentFont, Brushes.Black, g.VisibleClipBounds.Width / 2 + g.VisibleClipBounds.X, y, sf);
                y += sz.Height;
            }
        }

        private void ButtonInfo1_Click(object sender, EventArgs e)
        {
            Paint -= new PaintEventHandler(PrintGraphics_Paint);

            Paint += new PaintEventHandler(ChangePageScaleAndTranslateTransform);
        }
    }
}
