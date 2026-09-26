using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BO.Data.Model
{
    public partial class Price
    {
        public string? Sku { get; set; }
        public string? PriceListCode { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
