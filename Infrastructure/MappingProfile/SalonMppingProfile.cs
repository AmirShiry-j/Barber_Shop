using Application.SalonsService.Command;
using Application.SalonsService.Query;
using AutoMapper;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MappingProfile
{
    public class SalonMppingProfile : Profile
    {
        public SalonMppingProfile()
        {
            //For create salon
            CreateMap<Salon, CreateSalonDto>()
                .ForMember(salon => salon.UserId, dto => dto.MapFrom(p => p.OwnerId)).ReverseMap();

            //For get salon
            CreateMap<Salon, SalonDto>()
                .ForMember(dto => dto.OwnerId, salon => salon.MapFrom(p => p.OwnerId))
                .ForMember(dto => dto.OwnerFullName, salon => salon.MapFrom(p => p.Owner.FullName))
                .ReverseMap();

        }
    }
}
