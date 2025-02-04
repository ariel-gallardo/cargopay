using Infraestructure;

namespace Domain
{
    public class Pagination<T>
    {
        public int Quantity { get; set; }
        public int? Page { get; set; }
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public dynamic Content { get; set; }

        public Pagination(IEnumerable<T> data, int total, int page)
        {
            Total = total;
            Quantity = total > 0 ? data.Count() : 0;
            if (total > 0)
            {
                Page = page;
                TotalPages = (int)Math.Ceiling((double)total / AppSettings.Config.Take);
                PreviousPage = Page > 1 ? Page - 1 : null;
                NextPage = Page < TotalPages ? Page + 1 : null;
                Content = data;
            }
        }

        public Pagination(IList<T> data, int total, int page)
        {
            Total = total;
            Quantity = total > 0 ? data.Count() : 0;
            if (total > 0)
            {
                Page = page;
                TotalPages = (int)Math.Ceiling((double)total / AppSettings.Config.Take);
                PreviousPage = Page > 1 ? Page - 1 : null;
                NextPage = Page < TotalPages ? Page + 1 : null;
                Content = data;
            }
        }

        public static Pagination<T> Crear<T>(IEnumerable<T> data, int total, int page)
        => new Pagination<T>(data, total, page);

        public static Pagination<T> Crear<T>(IList<T> data, int total, int page)
        => new Pagination<T>(data, total, page);
    }
}
