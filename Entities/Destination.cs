namespace TravelTask.Entities
{
    public class Destination : BaseEntity
    {
        public string Name { get; set; }
        public int Rating { get; set; } = 0;
        public decimal Price { get; set; }
        public string? CoverImageUrl { get; set; }

        public void Delete()
        {
            IsDeleted = true;
        }

        public void RevokeDelete()
        {
            IsDeleted = false;
        }

    }
}
