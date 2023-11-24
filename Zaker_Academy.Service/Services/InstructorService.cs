using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Service.Services
{
    public class InstructorService : UserService<Instructor>, IInstructorService

    {
        public InstructorService(IMapper mapper, UserManager<Instructor> userManager, UserManager<applicationUser> appusermanager, IUnitOfWork work, IAuthorizationService authorizationService) : base(mapper, userManager, appusermanager, work, authorizationService)
        {
        }
    }
}