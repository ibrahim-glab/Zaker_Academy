using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;

namespace Zaker_Academy.Service.Interfaces
{
    public interface IAuthorizationService
    {
        Task<ServiceResult<string>> CreateTokenAsync(string username);

        Task<ServiceResult<string>> SendVerificationEmailAsync(string UserEmail, string callBackUrl);

        Task<ServiceResult<string>> CreateEmailTokenAsync(applicationUser applicationUser);


        Task<ServiceResult<string>> CreatePasswordTokenAsync(string Email);

        Task<ServiceResult<string>> VerifyEmailAsync(string Email, string code);

        Task<ServiceResult<string>> SendResetPasswordAsync(string UserEmail, string token);

        Task<ServiceResult<string>> ConfirmResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}