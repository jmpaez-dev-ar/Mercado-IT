﻿namespace MercadoIT.Web.Models
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public string PageAction { get; set; }
    }
}
