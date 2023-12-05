using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Repository;
using Zaker_Academy.Service.Interfaces;
using Zaker_Academy.Service.Services;

namespace Zaker_Academy.Service
{
    public static class ServicesInject
    {
        public static IServiceCollection Services(this IServiceCollection services)
        {
            services.AddTransient(typeof(IStudentService), typeof(StudentService));
            services.AddTransient(typeof(IInstructorService), typeof(InstructorService));
            services.AddTransient(typeof(IAuthorizationService), typeof(AuthorizationService));
            services.AddTransient(typeof(IUserService), typeof(UserService));
            services.AddTransient(typeof(IEmailService), typeof(EmailService));
            return services;
        }
    }
}