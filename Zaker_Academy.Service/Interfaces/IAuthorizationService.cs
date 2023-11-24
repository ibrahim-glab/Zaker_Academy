using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.ErrorHandling;

namespace Zaker_Academy.Service.Interfaces
{
    public interface IAuthorizationService
    {
        Task<ServiceResult> CreateToken(string userName);
    }
}