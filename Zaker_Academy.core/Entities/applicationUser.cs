using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Zaker_Academy.infrastructure.Entities
{
    public class applicationUser : IdentityUser
    {
        [MaxLength(50)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string FirstName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [MaxLength(50)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string LastName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public DateTime DateOfBirth { get; set; }

        [MaxLength(10)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Gender { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [MaxLength(255)]
        public string? imageURL { get; set; }

        public DateTime LastLogin { get; set; }

        [MaxLength(20)]
        public DateTime LastProfileUpdate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}