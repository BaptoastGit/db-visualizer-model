using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BO.Data.Model
{
    public partial class Product
    {
        public string? Sku { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public int StockQuantity { get; set; }
        public bool? IsPublished { get; set; }
        public double? WeightKg { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
