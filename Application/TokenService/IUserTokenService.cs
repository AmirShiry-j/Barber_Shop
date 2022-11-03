using Application.Interfaces.Contexts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TokenService
{
    public interface IUserTokenService
    {
        public Token GetToken(string TokenHash);
        public void DeleteToken(Token token);
        public void DeleteToken(string HashToken);
        public Task SaveToken(UserTokenDto userToken);
        public Token FindTokenByRefreshToken(string RefreshTokenHash);
    }
    public class UserTokenService : IUserTokenService
    {
        private readonly IDataBaseContext _dbContext;
        private readonly ILogger<UserTokenService> _logger;
        public UserTokenService(IDataBaseContext dbContext, ILogger<UserTokenService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public void DeleteToken(Token token)
        {
            _dbContext.Tokens.Remove(token);
            _dbContext.SaveChanges();
        }

        public Token FindTokenByRefreshToken(string RefreshTokenHash)
        {
            return _dbContext.Tokens.Where(p => p.RefreshTokenHash == RefreshTokenHash)
                                    .Include(include => include.User).FirstOrDefault();
        }
        public void DeleteToken(string HashToken)
        {
            //Find token
            var token = _dbContext.Tokens.Where(p => p.TokenHash == HashToken).FirstOrDefault();

            //Delete it and save
            if (token != null)
            {
                _dbContext.Tokens.Remove(token);
                _dbContext.SaveChanges();
            }
        }

        public Token GetToken(string TokenHash)
        {
            return _dbContext.Tokens.Where(p => p.TokenHash == TokenHash).FirstOrDefault();
        }

        public async Task SaveToken(UserTokenDto userToken)
        {
            try
            {
                //Check count tokens of user
                int maxCountToken = 5;
                int countTokensUser = _dbContext.Tokens.Where(p => p.UserId == userToken.UserId).Count();
                if (countTokensUser >= maxCountToken)
                {
                    //Get count overflow
                    int countOverflow = (countTokensUser - maxCountToken) + 1;

                    //Get token overflow
                    var tokens = _dbContext.Tokens.Where(p => p.UserId == userToken.UserId)
                                                  .OrderBy(p => p.CreateTime)
                                                  .Take(countOverflow)
                                                  .ToList();

                    //Delete them
                    _dbContext.Tokens.RemoveRange(tokens);
                }

                //Map dto to Token 
                var newToken = new Token
                {
                    TokenExpireTime = userToken.ExpireTime,
                    TokenHash = userToken.TokenHash,
                    UserId = userToken.UserId,
                    RefreshTokenHash = userToken.RefreshTokenHash,
                    RefreshTokenExpireTime = userToken.RefreshExpireTime,
                    CreateTime = DateTime.Now
                };

                //Add to db 
                _dbContext.Tokens.Add(newToken);

                //Save all in db
                _dbContext.SaveChanges();
            }
            catch (Exception error)
            {
                _logger.LogError(error.ToString());
            }

            await Task.CompletedTask;
        }
    }

    public class UserTokenDto
    {
        public string UserId { get; set; }
        public DateTime ExpireTime { get; set; }
        public string TokenHash { get; set; }
        public DateTime RefreshExpireTime { get; set; }
        public string RefreshTokenHash { get; set; }
    }
}
