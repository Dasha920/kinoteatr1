using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appliances_store
{
    public class Movie : ICinema
    {
        public int id { get ; set; }
        public int price { get; set; }
        public string nameMovie { get; set; }
        public DateTime time { get; set; }

        public string description { get; set; }
        public string genre { get; set; }

        //Конструктор стандартний
        public Movie()
        {
            price = 0;
            nameMovie = "Помилка!";
            description = "Описану немає.";
            genre = "Описану немає.";
        }

        public Movie(int id, int price, string nameMovie, string description, string genre)
        {
            this.id = id;
            this.price = price;
            this.nameMovie = nameMovie;
            this.description = description;
            this.genre = genre;
        }

    }
}
