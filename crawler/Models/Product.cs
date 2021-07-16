using System;
using System.Collections.Generic;

#nullable disable

namespace crawler.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string Brand { get; set; }
        public string Price { get; set; }
    }
}
