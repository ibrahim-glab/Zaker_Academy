using Zaker_Academy.core.Entities;

namespace Zaker_Academy.infrastructure.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<SubCategory>? SubCategories { get; set; }
    }
}