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
        private readonly IUnitOfWork unitOfWork;

        public UserService(IMapper mapper, UserManager<T> userManger, IUnitOfWork work)
        {
            _Mapper = mapper;
            this.userManager = userManger;
            unitOfWork = work;
        }

        public Task<IdentityResult> Login()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> Register(UserCreationDto user)
        {
            ServiceResult serviceResult = new ServiceResult();
            T User = _Mapper.Map<T>(user);
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
                    throw new Exception(message: serviceResult.Details);
                }

                if (!(string.Equals(user.Role, "instructor", StringComparison.CurrentCultureIgnoreCase) || string.Equals(user.Role, "student", StringComparison.CurrentCultureIgnoreCase)))
                {
                    throw new Exception(message: "Invalid Role ");
                }
                transaction.Commit();
                serviceResult.succeeded = true;
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