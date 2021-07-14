using System;
using System.Collections.Generic;

#nullable disable

namespace crawler.Models
{
    public partial class SportCar
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string Location { get; set; }
        public string Year { get; set; }
    }
}
