using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;

namespace Clients
{
    public class PrintContract
    {
        public PrintDocument pd_Contract;

        private readonly Contract contract;

        private readonly TypeContract typeContract;

        public PrintContract(Contract contract, TypeContract tc)
        {
            pd_Contract = new PrintDocument();

            this.contract = contract;

            typeContract = tc;

            // Устанавливаем принтер по умолчанию(если есть)
            pd_Contract.PrinterSettings.PrinterName =
                                     PrinterSettings.InstalledPrinters
                                                    .Cast<string>()
                                                    .FirstOrDefault(s => (new PrinterSettings() { PrinterName = s }).IsDefaultPrinter);

            if (pd_Contract.PrinterSettings.PrinterName != null)
            {
                pd_Contract.PrintPage += PrintPage; // подключаем событие печати документа на принтер по умолчанию
            }
            else
            {
                pd_Contract.PrintPage += PrintPageToJPG;    // печатаем(сохраняем) в файл jpg 
            }
        }

        // Печать договора
        public void Print()
        {
            try
            {
                pd_Contract.Print();
            }
            catch(Exception ex)
            {

            }
        }

        // Печать документа на принтер по умолчанию
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            PrintGraphics(ev.Graphics);
        }

        // Печать в jpg файл
        private void PrintPageToJPG(object sender, PrintPageEventArgs ev)
        {
            var bitmap = new Bitmap(ev.MarginBounds.Width, ev.MarginBounds.Height, ev.Graphics);

            Graphics g = Graphics.FromImage(bitmap);





        }

        // 
        private void PrintGraphics(Graphics graphics)
        {

        }

    }
}
