using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appliances_store
{
    interface ICinema
    {
        int id { get; set; }
        int price { get; set; }
        string nameMovie { get; set; }
        DateTime time { get; set; }
    }
}
