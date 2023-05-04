using Application.Common;
using Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TurnService.Query
{
    public class GetTurnOneDay
    {
        private readonly IDataBaseContext _dbContext;
        public GetTurnOneDay(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultDto<List<TempDto>>> Execute(int BarberId, DateTime dateTime)
        {

            var barber = _dbContext.Barbers.Find(BarberId);
            if(barber == null)
            {
                return new ResultDto<List<TempDto>>
                {
                    Message = "آرایشگری با آیدی ارسالی موجود نیست"
                };
            }

            var dayOfWeek = dateTime.DayOfWeek;

            //var r=_dbContext.

            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:

                    break;
            }
            return new ResultDto<List<TempDto>>
            {

            };
        }
    }
    public class TempDto
    {

    }
}
