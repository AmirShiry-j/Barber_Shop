using Application.Common;
using Application.Interfaces.Contexts;
using Application.SalonsService.Command;
using AutoMapper;
using Domain.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomerService.Command
{
    public interface IAddCustomerService
    {
        Task<ResultDto<int>> Execute(string UserId);
    }
    public class AddCustomerService: IAddCustomerService
    {
        private readonly IDataBaseContext _dbContext;
        public AddCustomerService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultDto<int>> Execute(string UserId)
        {
            //Map 
            var newCustomer = new Customer
            {
                UserId = UserId
            };

            //Add and save in db
            _dbContext.Customers.Add(newCustomer);
            _dbContext.SaveChanges();

            //Set Customerid in user table
            var user=_dbContext.Users.Find(UserId.ToString());
            user.CustomerId = newCustomer.Id;
            _dbContext.SaveChanges();


            //Return CustomerId
            return new ResultDto<int>
            {
                Data = newCustomer.Id
            };
        }
    }
}
