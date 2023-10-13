using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;

namespace Zaker_Academy.Service.Mapping
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseCreationDTO, Course>()
                .ForMember(
                dest => dest.price,
                opt => opt.MapFrom(src => src.Price)
                ).ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title)
                ).ForMember(
                dest => dest.Description,
                opt => opt.MapFrom(src => src.Description)
                ).ForMember(
                dest => dest.enrollmentCapacity,
                opt => opt.MapFrom(src => src.EnrollmentCapacity)
                ).ForMember(
                dest => dest.courseStatus,
                opt => opt.MapFrom(src => src.CourseStatus)
                ).ForMember(
                dest => dest.enrollmentCapacity,
                opt => opt.MapFrom(src => src.EnrollmentCapacity)
                ).ForMember(
                dest => dest.courseDurationInHours,
                opt => opt.MapFrom(src => src.CourseDurationInHours)
                ).ForMember(
                dest => dest.imageUrl,
                opt => opt.MapFrom(src => src.ImageUrl)
                ).ForMember(
                dest => dest.price,
                opt => opt.MapFrom(src => src.Price)
                ).ForMember(
                dest => dest.discount,
                opt => opt.MapFrom(src => src.Discount)
                ).ForMember(
                dest => dest.startDate,
                opt => opt.MapFrom(src => src.StartDate)
                ).ForMember(
                dest => dest.endDate,
                opt => opt.MapFrom(src => src.EndDate)
                ).ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => DateTime.Now)
                );
        }
    }
}