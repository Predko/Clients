using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace Clients
{
    public class Contract
    {
        public int id { get; set; }
        public DateTime dt { get; set; }
        public double numb { get; set; }
        public double sum { get; set; }




        public Contract(int id, DateTime dt, double numb, double sum)
        {
            this.id = id;
            this.dt = dt;
            this.numb = numb;
            this.sum = sum;
        }

        public Contract()
        {
            id = 0; dt = DateTime.Now; numb = 0; sum = 0;
        }

        public override string ToString()
        {
            return String.Format($"{dt,-12:d} {numb,5} {sum,20:c}");
        }
    }


}
