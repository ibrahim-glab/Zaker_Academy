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
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreationDto, applicationUser>()
               .ForMember(
               des => des.UserName,
               opt => opt.MapFrom(s => s.UserName)
                  ).ForMember(
               des => des.Email,
               opt => opt.MapFrom(s => s.Email)
               ).ForMember(
               des => des.PasswordHash,
               opt => opt.MapFrom(s => s.Password)
               ).ForMember(
               des => des.PhoneNumber,
               opt => opt.MapFrom(s => s.PhoneNumber)
               ).ForMember(
               des => des.FirstName,
               opt => opt.MapFrom(s => s.FirstName)
               ).ForMember(
               des => des.LastName,
               opt => opt.MapFrom(s => s.LastName)
               ).ForMember(
               des => des.DateOfBirth,
               opt => opt.MapFrom(s => s.DateOfBirth)
               ).ForMember(
               des => des.Gender,
               opt => opt.MapFrom(s => s.Gender)
               ).ForMember(
               des => des.imageURL,
               opt => opt.MapFrom(s => s.imageURL)
               );
        }
    }
}