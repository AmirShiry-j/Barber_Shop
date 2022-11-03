using Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService
{
    public interface IUserAuthorizeService
    {
        public void Logout(string UserId);
    }
    public class UserAuthorizeService : IUserAuthorizeService
    {
        private readonly IDataBaseContext _dbContext;
        public UserAuthorizeService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Logout(string UserId)
        {
            //Find user
            var user = _dbContext.Users.Find(UserId);
            if (user != null)
            {
                //Delete his or her tokens
                var tokens = _dbContext.Tokens.Where(p => p.UserId == UserId).ToList();
                if (tokens != null)
                {
                    _dbContext.Tokens.RemoveRange(tokens);
                    _dbContext.SaveChanges();
                }
            }
        }


    }
}
