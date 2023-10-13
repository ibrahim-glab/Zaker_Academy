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
    public class UserService : IUserService
    {
        private readonly IMapper _Mapper;
        private readonly UserManager<applicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IMapper mapper, UserManager<applicationUser> userManger, IUnitOfWork work)
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
            applicationUser User = _Mapper.Map<applicationUser>(user);
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

            try
            {
                IdentityResult res = await userManager.CreateAsync(User, user.Password);
                if (!res.Succeeded)
                {
                    serviceResult.Message = "Registration Filled";
                    foreach (var err in res.Errors)
                    {
                        serviceResult.Details += $"{err.Description} \n";
                    }
                    return serviceResult;
                }
                await userManager.AddToRoleAsync(User, user.Role);
                if (user.Role == "Instructor")
                {
                    await unitOfWork.InstructorRepository.Add((Instructor)User);
                }
                serviceResult.succeeded = true;
                return serviceResult;
            }
            catch (Exception e)
            {
                serviceResult.Message = "Registration Filled";
                serviceResult.Details = "Internal Server Error";
                return serviceResult;
            }
        }
    }
}