using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BarberService.Query
{
    public interface IGetBarbersService
    {
        Task<ResultDto<List<BarberMainInfoDto>>> Execute();

    }
    public class GetBarbersService : IGetBarbersService
    {
        private readonly IDataBaseContext _dbContext;
        private readonly IMapper _mapper;
        public GetBarbersService(IDataBaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResultDto<List<BarberMainInfoDto>>> Execute()
        {
            var barbers = _dbContext.Barbers.Include(p => p.User)
                .OrderByDescending(p => p.Id)
                .Select(p => new BarberMainInfoDto
                {
                    BarberId = p.Id,
                    Description = p.Description,
                    FullName = p.User.FullName,
                }).ToList();

            return new ResultDto<List<BarberMainInfoDto>>
            {
                IsSuccess = true,
                Data = barbers
            };
        }
    }
    public class BarberMainInfoDto
    {
        public int BarberId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
    }
}
