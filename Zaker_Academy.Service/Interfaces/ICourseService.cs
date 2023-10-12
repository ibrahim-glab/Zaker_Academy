using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.Service.DTO_s;

namespace Zaker_Academy.Service.Interfaces
{
    public interface ICourseService
    {
        Task<bool> Create(CourseCreationDTO creationDTO);
    }
}