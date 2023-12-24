namespace TravelTask.Areas.Admin.Models.Destinations
{
    public class DestinationsItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
        public string CoverImageUrl { get; set; }
        public bool IsDeleted { get; set; }
    }
}
