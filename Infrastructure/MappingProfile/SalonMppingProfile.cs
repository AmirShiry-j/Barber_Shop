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
                .ForMember(salon => salon.UserId, dto => dto.MapFrom(p => p.OwnerId))
                .ForMember(salon=>salon.FullAddress,dto=>dto.MapFrom(p=>p.Address.FullAddress))
                .ForMember(salon=>salon.CityId,dto=>dto.MapFrom(p=>p.Address.CityId))
                .ForMember(salon=>salon.ForGender,dto=>dto.MapFrom(p=>p.ForGender))
                .ReverseMap();

            //For get salon
            CreateMap<Salon, SalonDetailDto>()
                .ForMember(dto => dto.OwnerId, salon => salon.MapFrom(p => p.OwnerId))
                .ForMember(dto => dto.OwnerFullName, salon => salon.MapFrom(p => p.Owner.FullName))
                .ForMember(dto => dto.United, salon => salon.MapFrom(p => p.Address.City.United.Name))
                .ForMember(dto => dto.UnitedId, salon => salon.MapFrom(p => p.Address.City.United.Id))
                .ForMember(dto => dto.City, salon => salon.MapFrom(p => p.Address.City.Name))
                .ForMember(dto => dto.CityId, salon => salon.MapFrom(p => p.Address.City.Id))
                .ForMember(dto => dto.FullAddress, salon => salon.MapFrom(p => p.Address.FullAddress))
                .ForMember(dto => dto.ForGender, salon => salon.MapFrom(p => p.ForGender))
                .ForMember(dto => dto.Barbers, salon => salon.MapFrom(p => p.Barbers.Select(p => new BarberDto
                {
                    BarberId = p.Id,
                    FullName = p.User.FullName,
                    ProfileImageName = p.User.ImageName
                }).ToList()))
                .ForMember(dto => dto.Images, salon => salon.MapFrom(p => p.SalonImages.Select(p => new ImageDto
                {
                    Name = p.Name
                }).ToList()))
                .ReverseMap();

        }
    }
}
