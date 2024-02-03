using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;

namespace Zaker_Academy.Service.Mapping
{
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            CreateMap<LessonCreationDto, Lesson>().ReverseMap();
            CreateMap<LessonDto, Lesson>();
            CreateMap<Lesson, LessonDto>();
        }
    }
}
