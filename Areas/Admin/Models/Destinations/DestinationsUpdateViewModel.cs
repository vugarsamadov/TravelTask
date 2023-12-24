using System.ComponentModel.DataAnnotations;

namespace TravelTask.Areas.Admin.Models.Destinations
{
    public class DestinationsUpdateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, Range(0, 5)]
        public int Rating { get; set; }

        //TODO custom attribute
        [Required, Range(0, (double)decimal.MaxValue)]
        public decimal Price { get; set; }

        public IFormFile? CoverImage { get; set; }

        public string? CoverImageUrl { get; set; }
    }
}
