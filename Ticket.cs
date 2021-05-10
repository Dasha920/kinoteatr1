using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appliances_store
{
    public class Ticket : ICinema
    {
        public int id { get; set; }
        public string nameMovie { get; set; }
        public DateTime time { get; set; }
        //Конструктор для мінімальної ціни квитка на кіно
        int minval;
        public int price
        {
            get
            {
                return minval;
            }
            set
            {
                if (price > 40) minval = price;

                else minval = 60;
            }
        }
        public bool bought { get; set; }
    }
}
