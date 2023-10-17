using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;

namespace Zaker_Academy.Service.Interfaces
{
    public interface IUserService<T> where T : class
    {
        Task<ServiceResult> Register(UserCreationDto user);

        Task<IdentityResult> Login();
    }
}