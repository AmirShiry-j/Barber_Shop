using Application.Interfaces.Contexts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FavoriteBarberService.Command
{
    public interface IAddFavoriteBarberService
    {

    }
    public class AddFavoriteBarberService
    {
        private readonly IDataBaseContext _dbContext;
        public AddFavoriteBarberService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
