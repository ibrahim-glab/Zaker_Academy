using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.Service.DTO_s
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubCategoryCreationDto>? subCategories { get; set; }
    }
}
