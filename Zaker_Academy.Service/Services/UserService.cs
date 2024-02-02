using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<applicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorizationService _authorizationService;

        public UserService(IMapper mapper, UserManager<applicationUser> userManager, IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _authorizationService = authorizationService;
        }

        public async Task<ServiceResult<string>> Login(UserLoginDto userDto)
        {
            ServiceResult<string> serviceResult = new ServiceResult<string>();

            if (userDto == null)
            {
                serviceResult.Message = "Login Failed";
                serviceResult.Error = "No Data was sent";
                return serviceResult;
            }

            var user = await _userManager.FindByNameAsync(userDto.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userDto.Password))
            {
                serviceResult.Message = "Login Failed";
                serviceResult.Error = "Invalid username or password";
                return serviceResult;
            }

            if (!user.EmailConfirmed)
            {
                serviceResult.Message = "Login Failed";
                serviceResult.Error = "Please Verify Your Email, Check your mail";
                return serviceResult;
            }

            serviceResult = await _authorizationService.CreateTokenAsync(userDto.UserName);
            user.LastLogin = DateTime.UtcNow;
            await _unitOfWork.SaveChanges();
            serviceResult.succeeded = true;
            serviceResult.Message = "Login Succeeded";
            return serviceResult;
        }

        public applicationUser MapUser(applicationUser applicationUser , UserDto userDto)
        {

            applicationUser.PhoneNumber = userDto.PhoneNumber;
            applicationUser.FirstName = userDto.FirstName;
            applicationUser.LastName = userDto.LastName;
            applicationUser.DateOfBirth = userDto.DateOfBirth;
            applicationUser.Gender = userDto.Gender;
            applicationUser.imageURL = userDto.imageURL;
            return applicationUser;
        }
        public async Task<ServiceResult<UserDto>> UpdateProfile(UserDto updateProfileDto , string id)
        {
            ServiceResult<UserDto> result = new ServiceResult<UserDto>();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                result.Message = "Retrive User Failed";
                result.Error = "User Not Found";
                return result;
            }

            user = MapUser(user, updateProfileDto);
            IdentityResult res = await _userManager.UpdateAsync(user);

            if (!res.Succeeded)
            {
                foreach (var err in res.Errors)
                {
                    result.Error += $" {err.Description} , ";
                }
                result.Message = "Failed to update profile";
                return result;
            }
            user.LastProfileUpdate = DateTime.UtcNow;
            await _unitOfWork.SaveChanges();
            result.succeeded = true;
            result.Message = "Profile updated successfully";
            result.Data = _mapper.Map<UserDto>(user) ;
            return result;
        }

        public async Task<ServiceResult<string>> Register(UserCreationDto user, string callbackUrl)
        {
            ServiceResult<string> serviceResult = new ServiceResult<string>();
            var User = _mapper.Map<applicationUser>(user);

            if (await _userManager.FindByEmailAsync(user.Email) is not null)
            {
                serviceResult.Message = "Registration Failed";
                serviceResult.Error = $"{user.Email} is already Register!";
                return serviceResult;
            }

            if (await _userManager.FindByNameAsync(user.UserName) is not null)
            {
                serviceResult.Message = "Registration Failed";
                serviceResult.Error = $"{user.UserName} is already Exist!";
                return serviceResult;
            }

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                IdentityResult res = await _userManager.CreateAsync(User, user.Password);

                if (!res.Succeeded)
                {
                    foreach (var err in res.Errors)
                    {
                        serviceResult.Error += $" {err.Description} , ";
                    }
                    throw new Exception(message: (string)serviceResult.Error!);
                }
                res  = await _userManager.AddToRoleAsync(User, user.Role);
                if (!res.Succeeded)
                    throw new Exception(message: (string)serviceResult.Error!);

                var result = await _authorizationService.CreateEmailTokenAsync(User.UserName!);

                if (!result.succeeded)
                    throw new Exception(message: "Somthing Happend");

                var token = Uri.EscapeDataString(result.Data.ToString());
                callbackUrl += $"&token={token}";

                result = await _authorizationService.SendVerificationEmailAsync(User.Email!, callbackUrl);

                if (!result.succeeded)
                    throw new Exception(message: "Somthing Happend");

                serviceResult.succeeded = true;
                serviceResult.Message = "Registration Succeeded";
                transaction.Commit();
                return serviceResult;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                serviceResult.Message = e.Message;
                serviceResult.Error = "Internal Server Error";
                return serviceResult;
            }
        }

        public async Task<ServiceResult<UserDto>> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;

            return new ServiceResult<UserDto> {succeeded = true , Data = _mapper.Map<UserDto>(user) };
        }



    }
}