using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
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
            var salon = _dbContext.Salons.Find(dto.Id);

            //Check in exist
            if (salon != null)
            {

                //Check user is owner
                if (dto.UserId == salon.OwnerId)
                {
                    //Update salon
                    salon.Name = dto.Name;
                    salon.Description = dto.Description;
                    salon.PhoneNumber = dto.PhoneNumber;
                    salon.Telphone = dto.Telphone;
                    //salon.Address = dto.Address;

                    //Save in db
                    _dbContext.SaveChanges();

                    return new ResultDto
                    {
                        IsSuccess = true
                    };
                }
                else
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "این  یوزر مالک آرایشگاه نیست"
                    };
                }

            }
            else//Is not exist
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "Salon is not Exist"
                };
            }
        }
    }
    public class EditSalonDto
    {
        public Guid UserId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telphone { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
    }
}
