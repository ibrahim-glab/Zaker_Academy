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
        Task<ServiceResult> CreateTokenAsync(string username);

        Task<ServiceResult> SendVerificationEmailAsync(string UserEmail, string callBackUrl);

        Task<ServiceResult> CreateEmailTokenAsync(string username);

        Task<ServiceResult> CreatePasswordTokenAsync(string Email);

        Task<ServiceResult> VerifyEmailAsync(string Email, string code);

        Task<ServiceResult> SendResetpasswordAsync(string UserEmail, string callBackUrl);
    }
}