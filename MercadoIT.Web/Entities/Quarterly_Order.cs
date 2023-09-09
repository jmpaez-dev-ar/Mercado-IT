using System;
using System.Collections.Generic;

namespace MercadoIT.Web.Entities;

public partial class Quarterly_Order
{
    public string? CustomerID { get; set; }

    public string? CompanyName { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }
}
