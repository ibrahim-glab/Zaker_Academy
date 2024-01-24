namespace Zaker_Academy.infrastructure.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int rating { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string review { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTime createdIt { get; set; }
    }
}