using Application.Common;
using Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TurnService.Command
{
    public interface IGiveBackTurnService
    {
        Task<ResultDto> Execute(int TurnId, string UserId);

    }

    public class GiveBackTurnService : IGiveBackTurnService
    {
        private readonly IDataBaseContext _dbContext;
        public GiveBackTurnService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto> Execute(int TurnId, string UserId)
        {
            //Find customer
            var customer = _dbContext.Customers.Where(p => p.UserId.Equals(UserId)).FirstOrDefault();

            //Check has customer
            if (customer == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر دارای کد مشتری نیست"
                };
            }

            //Find Turn
            var turn = _dbContext.Turns.Find(TurnId);
            if (turn == null)
            {
                return new ResultDto
                {
                    Message = "نوبتی با آیدی ارسالی موجود نیست"
                };
            }

            //Check Turn is for this user
            if (!turn.CustomerId.Equals(customer.Id))
            {
                return new ResultDto
                {
                    Message = "نوبت متعلق به این یوزر نیست"
                };
            }

            //Delete In Db
            _dbContext.Turns.Remove(turn);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };

        }
    }
}
