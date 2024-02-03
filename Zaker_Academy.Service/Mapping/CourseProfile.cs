using AutoMapper;
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
                opt => opt.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue))
                ).ForMember(
                dest => dest.endDate,
                opt => opt.MapFrom(src => src.EndDate.ToDateTime(TimeOnly.MinValue))
                ).ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => DateTime.Now)
                ).ForMember(
                dest => dest.CategoryId,
                opt => opt.MapFrom(src => src.CategoryId)
                ).ForMember(
                dest => dest.SubCategoryId,
                opt => opt.MapFrom(src => src.SubCategoryId)
                );
            CreateMap<Course, CourseDto>().ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src => src.price)
                ).ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title)
                ).ForMember(
                dest => dest.Description,
                opt => opt.MapFrom(src => src.Description)
                ).ForMember(
                dest => dest.EnrollmentCapacity,
                opt => opt.MapFrom(src => src.enrollmentCapacity)
                ).ForMember(
                dest => dest.CourseStatus,
                opt => opt.MapFrom(src => src.courseStatus)
                ).ForMember(
                dest => dest.EnrollmentCapacity,
                opt => opt.MapFrom(src => src.enrollmentCapacity)
                ).ForMember(
                dest => dest.CourseDurationInHours,
                opt => opt.MapFrom(src => src.courseDurationInHours)
                ).ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.imageUrl)
                ).ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src => src.price)
                ).ForMember(
                dest => dest.Discount,
                opt => opt.MapFrom(src => src.discount)
                ).ForMember(
                dest =>  dest.StartDate,
                opt => opt.MapFrom(src => src.startDate)
                ).ForMember(
                dest => dest.EndDate,
                opt => opt.MapFrom(src => src.endDate)
                ).ForMember(
                dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => src.UpdatedAt)
                ).ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.CourseId)
                );

            CreateMap<CourseBasicUpdateDto, Course>();
        }
    }
}