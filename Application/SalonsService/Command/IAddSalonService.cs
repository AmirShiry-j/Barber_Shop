using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Salons;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SalonsService.Command
{
    public interface IAddSalonService
    {
        public Task<ResultDto<Salon>> Execute(CreateSalonDto dto);
    }
    public class AddSalonService : IAddSalonService
    {
        ILogger<AddSalonService> _logger;
        private readonly IMapper _mapper;
        private readonly IDataBaseContext _dbContext;
        public AddSalonService(IDataBaseContext dbContext, ILogger<AddSalonService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ResultDto<Salon>> Execute(CreateSalonDto dto)
        {
            //Map
            var newSalon = _mapper.Map<Salon>(dto);

            //Add to db
            _dbContext.Salons.Add(newSalon);

            //Save in db
            _dbContext.SaveChanges();

            return new ResultDto<Salon>
            {
                IsSuccess = true,
                Data = newSalon
            };
        }
    }
    public class CreateSalonDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telphone { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
    }
}
