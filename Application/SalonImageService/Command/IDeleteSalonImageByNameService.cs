using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SalonImageService.Command
{
    public interface IDeleteSalonImageByNameService
    {
        Task<ResultDto> Execute(string Name, string UserId);
    }
    public class DeleteSalonImageByNameService : IDeleteSalonImageByNameService
    {
        private readonly IDataBaseContext _dbContext;
        public DeleteSalonImageByNameService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultDto> Execute(string Name, string UserId)
        {
            //Find Image
            var salonImage = _dbContext.SalonImages.Where(p => p.Name.Equals(Name)).FirstOrDefault();

            //Check is exist image
            if (salonImage == null)
            {
                return new ResultDto
                {
                    Message = "تصویری با این نام موجود نیست"
                };
            }

            var salon = _dbContext.Salons.Find(salonImage.SalonId);

            //Check user is owner Salon
            if (salon.OwnerId != UserId)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شما مالک این سالن آرایشی نیستید"
                };
            }

            //Check is exist
            if (salonImage == null)
            {
                return new ResultDto
                {
                    Message = "تصویری با این نام موجود نیست"
                };
            }

            //Delete in db
            _dbContext.SalonImages.Remove(salonImage);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
