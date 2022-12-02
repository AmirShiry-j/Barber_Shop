using Application.ProfileService.Command;
using Application.ProfileService.Query;
using Application.SalonsService.Command;
using AutoMapper;
using Domain.Salons;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MappingProfile
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, ProfileDto>()
                .ForMember(profile => profile.UserId, user => user.MapFrom(p => p.Id)).ReverseMap();

            //CreateMap<,>().ReverseMap();

        }
    }
}
