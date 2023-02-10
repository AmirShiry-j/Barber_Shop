using Application.Common;
using Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SalonImageService.Query
{
    public interface IGetSalonImagesBySalonIdService
    {
        Task<ResultDto<List<SalonImageDto>>> Execute(int SalonId, string UserId);
    }
    public class GetSalonImagesBySalonIdService : IGetSalonImagesBySalonIdService
    {
        private readonly IDataBaseContext _dbContext;
        public GetSalonImagesBySalonIdService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultDto<List<SalonImageDto>>> Execute(int SalonId, string UserId)
        {
            //Check is exist salonid
            var salon = _dbContext.Salons.Find(SalonId);
            if (salon == null)
            {
                return new ResultDto<List<SalonImageDto>>
                {
                    Message = "سالنی با این آیدی موجود نیست"
                };
            }

            //Check user is owner Salon
            if (salon.OwnerId != UserId)
            {
                return new ResultDto<List<SalonImageDto>>
                {
                    IsSuccess = false,
                    Message = "شما مالک این سالن آرایشی نیستید"
                };
            }

            var images = _dbContext.SalonImages.Where(p => p.SalonId.Equals(SalonId)).Select(p => new SalonImageDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();

            return new ResultDto<List<SalonImageDto>>
            {
                IsSuccess = true,
                Data = images
            }; ;
        }
    }
    public class SalonImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
