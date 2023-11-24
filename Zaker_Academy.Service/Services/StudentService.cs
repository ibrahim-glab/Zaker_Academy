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
    public class StudentService : UserService<Student>, IStudentService
    {
        public StudentService(IMapper mapper, UserManager<Student> userManager, UserManager<applicationUser> appusermanager, IUnitOfWork work, IAuthorizationService authorizationService) : base(mapper, userManager, appusermanager, work, authorizationService)
        {
        }
    }
}