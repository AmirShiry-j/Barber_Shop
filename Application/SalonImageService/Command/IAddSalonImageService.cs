using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Salons;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SalonImageService.Command
{
    public interface IAddSalonImageService
    {
        Task<ResultDto<int>> Execute(CreateSalonImageDto Dto);
    }
    public class AddSalonImageService : IAddSalonImageService
    {
        private readonly IDataBaseContext _dbContext;
        public AddSalonImageService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultDto<int>> Execute(CreateSalonImageDto Dto)
        {
            //Check is exist salonid
            var salon = _dbContext.Salons.Find(Dto.SalonId);
            if (salon == null)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "سالنی با این آیدی موجود نیست"
                };
            }

            //Check user is owner Salon
            if (salon.OwnerId != Dto.UserId)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "شما مالک این سالن آرایشی نیستید"
                };
            }

            //Check Limited 5 Image for every Salon
            var countImage = _dbContext.SalonImages.Where(p => p.SalonId.Equals(Dto.SalonId)).Count();
            if (countImage >= 5)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "بیشتر از 5 تصویر برای هر سالن نمیتوان ذخیره کرده"
                };
            }

            //Map
            var newSalonImage = new SalonImage
            {
                Name = Dto.Name,
                SalonId = Dto.SalonId
            };

            //Save in db
            _dbContext.SalonImages.Add(newSalonImage);
            _dbContext.SaveChanges();

            return new ResultDto<int>
            {
                IsSuccess = true,
                Data = newSalonImage.Id
            };
        }
    }
    public class CreateSalonImageDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public int SalonId { get; set; }
    }
}
