using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;

namespace Zaker_Academy.Service.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<string>> Register(UserCreationDto user, string CallbackUrl);

        Task<ServiceResult<string>> Login(UserLoginDto userDto);
        Task<ServiceResult<UserDto>> UpdateProfile(UserDto updateProfileDto , string id );
        Task<ServiceResult<UserDto>> GetUser(string id);
    }
}