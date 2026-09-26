using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BO.Data.Model
{
    public partial class Message
    {
        public int RetryCount { get; set; }
        public string? ErrorMessage { get; set; }
        public long? PayloadSize { get; set; }
        public string? Priority { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string? CorrelationId { get; set; }
    }
}
