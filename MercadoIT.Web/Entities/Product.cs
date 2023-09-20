﻿using System;
using System.Collections.Generic;

namespace MercadoIT.Web.Entities;

public partial class Product
{
	public int ProductID { get; set; }
	
	public string ProductName { get; set; } = null!;

    public int? SupplierID { get; set; }

    public int? CategoryID { get; set; }

    public string? QuantityPerUnit { get; set; }

    public decimal? UnitPrice { get; set; }

    public short? UnitsInStock { get; set; }

    public short? UnitsOnOrder { get; set; }

    public short? ReorderLevel { get; set; }

    public bool Discontinued { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> Order_Details { get; set; } = new List<OrderDetail>();

    public virtual Supplier? Supplier { get; set; }
}
