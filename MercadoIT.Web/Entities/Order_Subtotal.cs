using System;
using System.Collections.Generic;

namespace MercadoIT.Web.Entities;

public partial class Order_Subtotal
{
    public int OrderID { get; set; }

    public decimal? Subtotal { get; set; }
}
