using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.Service.DTO_s
{
    public class UserDto
    {
        [Required]

        public string PhoneNumber { get; set; }

        [Required]

        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.DateTime) ]
        public DateTimeOffset DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }
        [Required]

        public string imageURL { get; set; }

    }
}
