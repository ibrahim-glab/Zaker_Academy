using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.infrastructure.Entities
{
    public class applicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [MaxLength(255)]
        public string imageURL { get; set; }

        [MaxLength(1000)]
        public string AboutMe { get; set; }

        public DateTime LastLogin { get; set; }

        [MaxLength(20)]
        public string AccountStatus { get; set; }

        public DateTime LastProfileUpdate { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation property for related entities (e.g., courses, messages, etc.)
        //  public ICollection<Course> Courses { get; set; }

        //public ICollection<Message> Messages { get; set; }
    }
}