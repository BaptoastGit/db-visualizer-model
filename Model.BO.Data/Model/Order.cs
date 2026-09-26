using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BO.Data.Model
{
    public partial class Order
    {
        public string? OrderNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Currency { get; set; }
        public string? ShippingMethod { get; set; }
        public string? TrackingNumber { get; set; }
        public int ItemCount { get; set; }
        public DateTime OrderedOn { get; set; }
    }
}
