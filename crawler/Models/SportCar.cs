using System;
using System.Collections.Generic;

#nullable disable

namespace crawler.Models
{
    public partial class SportCar
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal? Price { get; set; }
        public string Location { get; set; }
        public int? Year { get; set; }
    }
}
