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

        public applicationUser MapUser(applicationUser applicationUser, UserDto userDto)
        {

            applicationUser.PhoneNumber = userDto.PhoneNumber;
            applicationUser.FirstName = userDto.FirstName;
            applicationUser.LastName = userDto.LastName;
            applicationUser.DateOfBirth = userDto.DateOfBirth;
            applicationUser.Gender = userDto.Gender;
            applicationUser.imageURL = userDto.imageURL;
            return applicationUser;
        }
        public async Task<ServiceResult<UserDto>> UpdateProfile(UserDto updateProfileDto, string id)
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
            result.Data = _mapper.Map<UserDto>(user);
            return result;
        }

        // 1. Map User
        // 2. Check if User Exist
        // 3. Create User
        // 4. add User To Role (Student or Teacher)
        // 5. Send Email
        // 4. Save Changes
        /// <summary>
        /// This code snippet is a method named Register that takes a UserCreationDto object and a callback URL as parameters. It registers a new user by mapping the UserCreationDto to an applicationUser, checking if the user already exists, creating the user with a password using userManager, assigning a role, generating and sending email verification, and handling transaction commits and rollbacks. It returns a ServiceResult<string> indicating the success or failure of the registration process along with an appropriate message.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="callbackUrl"></param>
        /// <returns>Serivce</returns>
        public async Task<ServiceResult<string>> Register(UserCreationDto user, string callbackUrl)
        {
            var User = _mapper.Map<applicationUser>(user);
            var CheckResult = await CheckUserExistence(user.UserName, user.Email);
            if (!CheckResult.succeeded)
                return CheckResult;
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                // var result = await CreateUser(User, user.Password);
                IdentityResult res = await _userManager.CreateAsync(User, user.Password);
                if (!res.Succeeded)
                    throw new Exception(message: ConstructErrorInUserIdentity(res));
                res = await _userManager.AddToRoleAsync(User, user.Role);
                if (!res.Succeeded)
                    throw new Exception(message: ConstructErrorInUserIdentity(res));

                var result = await _authorizationService.CreateEmailTokenAsync(User);
                var Url = ConstructUrl(token: result.Data, callbackUrl);
                result = await _authorizationService.SendVerificationEmailAsync(User.Email!, Url);
                
                result.succeeded = true;
                result.Message = "Registration Succeeded! Please Verify Your Email";
                transaction.Commit();
                return new ServiceResult<string> { succeeded = true, Message = "Registration Succeeded! Please Verify Your Email" };
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return new ServiceResult<string> { succeeded = false, Message = "Registration Failed", Error = e.Message };
            }
        }
        private async Task<ServiceResult<string>> CheckUserExistence(string userName, string email)
        {
            if (await _userManager.FindByNameAsync(userName) != null)
                return new ServiceResult<string> { Message = $"{userName} already exist" };
            else if (await _userManager.FindByEmailAsync(email) != null)
                return new ServiceResult<string> { Message = $"{email} already exist" };

            return new ServiceResult<string> { succeeded = true };
        }


        private string ConstructUrl(string token, string callBackUrl)
        {

            string EncodedToken = Uri.EscapeDataString(token.ToString());
            return callBackUrl + $"&token={EncodedToken}";
            ;
        }
        private string ConstructErrorInUserIdentity(IdentityResult res)
        {
            string error = string.Empty;
            foreach (var err in res.Errors)
            {
                error += $" {err.Description} , ";
            }
            return error;
        }

        public async Task<ServiceResult<UserDto>> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;

            return new ServiceResult<UserDto> { succeeded = true, Data = _mapper.Map<UserDto>(user) };
        }



    }
}