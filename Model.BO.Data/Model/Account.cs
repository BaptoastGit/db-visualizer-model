using System;
using System.Collections.Generic;

namespace Model.BO.Data.Model;

public partial class Account
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
    public string? Tier { get; set; }
    public decimal Balance { get; set; }
    public bool IsVatExempt { get; set; }
    public DateTime CreatedOn { get; set; }


}
