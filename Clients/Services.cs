using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clients
{
    public class Price
    {
        public int IdPrice { get; set;}   // идентификатор цены
        public decimal price { get; set;} // стоимость работы
        public DateTime date { get; set;} // дата изменения цены

        public static List<Price> prices = new List<Price>(); // список всех цен
    }

    public class NameWork
    {
        public int IdNameWork { get; set; }     // идентификатор работы

        public string name { get; set;}         // название выполненной работы

        public int IdPrice { get; set; }        // Идентификатор цены

        public decimal summ { get => Price.prices[IdPrice].price; }



        public static List<NameWork> works = new List<NameWork>();     // список всех видов работ
    }


    public class Subdivision
    {
        public int IdSubdivision { get; set; }
        public string name { get; set; }

        public static List<Subdivision> Subdivs = new List<Subdivision>();


    }



    public class Services
    {





    }





}
