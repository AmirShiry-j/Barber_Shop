using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SalonsService.Command
{
    public interface IEditSalonService
    {
        public Task<ResultDto> Execute(EditSalonDto dto);
    }
    public class EditSalonService : IEditSalonService
    {
        ILogger<AddSalonService> _logger;
        private readonly IDataBaseContext _dbContext;
        public EditSalonService(IDataBaseContext dbContext, ILogger<AddSalonService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<ResultDto> Execute(EditSalonDto dto)
        {
            //Get in db
            var salon = _dbContext.Salons.Where(p => p.Id.Equals(dto.Id)).Include(p => p.Address).FirstOrDefault();

            //Check if has CityId
            var hasCity = _dbContext.Cities.Any(p => p.Id.Equals(dto.CityId));
            if (hasCity == false)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شهری با آیدی ارسال شده وجود ندارد"
                };
            }

            //Check in exist
            if (salon == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "Salon is not Exist"
                };
            }

            //Check user is owner
            if (dto.UserId != salon.OwnerId)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این  یوزر مالک آرایشگاه نیست"
                };

            }

            //Update salon
            salon.Name = dto.Name;
            salon.Description = dto.Description;
            salon.PhoneNumber = dto.PhoneNumber;
            salon.Telphone = dto.Telphone;
            salon.Address.FullAddress = dto.FullAddress;
            salon.Address.CityId = dto.CityId;

            //Save in db
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };

        }
    }
    public class EditSalonDto
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string FullAddress { get; set; }
        public string Telphone { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
    }
}
