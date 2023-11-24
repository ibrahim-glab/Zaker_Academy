using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Service.Services
{
    public class UserService<T> : IUserService<T> where T : class
    {
        private readonly IMapper _Mapper;

        private readonly UserManager<T> userManager;
        private readonly UserManager<applicationUser> appusermanager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthorizationService authorizationService;

        public UserService(IMapper mapper, UserManager<T> userManager, UserManager<applicationUser> appusermanager, IUnitOfWork work, IAuthorizationService authorizationService)
        {
            _Mapper = mapper;
            this.appusermanager = appusermanager;
            this.userManager = userManager;
            unitOfWork = work;
            this.authorizationService = authorizationService;
        }

        public Task<IdentityResult> Login()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> Register(UserCreationDto user)
        {
            ServiceResult serviceResult = new ServiceResult();
            var User = _Mapper.Map<T>(user);
            if (await appusermanager.FindByEmailAsync(user.Email) is not null)
            {
                serviceResult.Message = "Registration Filled";
                serviceResult.Details = $"{user.Email} is already Register!";
                return serviceResult;
            }
            if (await appusermanager.FindByNameAsync(user.UserName) is not null)
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
                    throw new Exception(message: serviceResult.Details);
                }

                await userManager.AddToRoleAsync(User, user.Role);
                serviceResult = await authorizationService.CreateToken(user.UserName);
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