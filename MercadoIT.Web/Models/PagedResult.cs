namespace MercadoIT.Web.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalRecords { get; set; }
    }

}
