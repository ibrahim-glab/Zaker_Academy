using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.infrastructure.Entities;

namespace Zaker_Academy.core.Entities
{
    public class SubCategory
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Category Category{ get; set; }

        public int CategoryId { get; set; }
    }
}
