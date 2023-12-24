namespace TravelTask.Areas.Admin.Models
{
    public class PagedEntityModel<T>
    {
        public T Data { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }


        public bool HasNext { get => Page < PageCount; }
        public bool HasPrev { get => Page != 1; }

        public string Next { get; set; }
        public string Prev { get; set; }
    }
}
