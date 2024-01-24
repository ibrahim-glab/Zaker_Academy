﻿using AutoMapper;
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
        private readonly IMapper _Mapper;

        private readonly UserManager<applicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthorizationService authorizationService;

        public UserService(IMapper mapper, UserManager<applicationUser> userManager, IUnitOfWork work, IAuthorizationService authorizationService)
        {
            _Mapper = mapper;
            this.userManager = userManager;
            unitOfWork = work;
            this.authorizationService = authorizationService;
        }

        public async Task<ServiceResult> Login(UserLoginDto userDto)
        {
            ServiceResult serviceResult = new ServiceResult();

            if (userDto is null)
            {
                serviceResult.Message = "Login Failed";
                serviceResult.Details = "No Data was sent";
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var user = await userManager.FindByNameAsync(userDto.UserName);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (user is null)
            {
                serviceResult.Message = "Login Failed";
                serviceResult.Details = "Invalid username or password";
            }
            else
            {
                var res = await userManager.CheckPasswordAsync(user, userDto.Password);
                if (res)
                {
                    if (!user.EmailConfirmed)
                    {
                        serviceResult.Message = "Login Failed";
                        serviceResult.Details = "Please Verify Your Email, Check your mail";
                        return serviceResult;
                    }
                    serviceResult = await authorizationService.CreateTokenAsync(userDto.UserName);
                    serviceResult.succeeded = true;
                    serviceResult.Message = "Login Succeeded";
                    return serviceResult;
                }
                serviceResult.Message = "Login Failed";
                serviceResult.Details = "Invalid username or password";
            }
            return serviceResult;
        }

        public async Task<ServiceResult> Register(UserCreationDto user, string CallbackUrl)
        {
            ServiceResult serviceResult = new ServiceResult();
            var User = _Mapper.Map<applicationUser>(user);
            if (await userManager.FindByEmailAsync(user.Email) is not null)
            {
                serviceResult.Message = "Registration Filled";
                serviceResult.Details = $"{user.Email} is already Register!";
                return serviceResult;
            }
            if (await userManager.FindByNameAsync(user.UserName) is not null)
            {
                serviceResult.Message = "Registration Filled";
                serviceResult.Details = $"{user.UserName} is already Exist!";
                return serviceResult;
            }
            using var transaction = unitOfWork.BeginTransaction();
            try
            {
                IdentityResult res = await userManager.CreateAsync(User, user.Password);
                if (!res.Succeeded)
                {
                    foreach (var err in res.Errors)
                    {
                        serviceResult.Details += $" {err.Description} , ";
                    }
                    throw new Exception(message: (string)serviceResult.Details!);
                }
                var result = await authorizationService.CreateEmailTokenAsync(user.UserName);
                if (!result.succeeded)
                    throw new Exception(message: "Somthing Happend");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                var Token = Uri.EscapeDataString(result.Details.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                CallbackUrl = CallbackUrl + "&" + "token=" + Token;

                result = await authorizationService.SendVerificationEmailAsync(user.Email, CallbackUrl);
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
                serviceResult.Details = "Internal Server Error";
                return serviceResult;
            }
        }
    }
}