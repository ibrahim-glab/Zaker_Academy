using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_Academy.Service.DTO_s
{
    public class ResetPasswordDto
    {
        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}