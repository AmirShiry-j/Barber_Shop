using Application.TokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.Tools.Hasher;

namespace WebApi.Tools.TokenValidator
{
    public interface ITokenValidator
    {
        Task Execute(TokenValidatedContext context);
    }
    public class TokenValidator : ITokenValidator
    {
        private readonly IUserTokenService _userTokenService;
        public TokenValidator(IUserTokenService userTokenService)
        {
            _userTokenService = userTokenService;
        }
        public async Task Execute(TokenValidatedContext context)
        {
            //Check exist claims
            var claimsidentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsidentity?.Claims == null || !claimsidentity.Claims.Any())
            {
                context.Fail("Claims are not exist...");
                return;
            }

            //Get userid from token
            var userid = claimsidentity.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userid))
            {
                context.Fail("UserId is not found...");
                return;
            }

            ////Check token is exist in db
            //Check token is JwtSecurityToken
            if (context.SecurityToken is JwtSecurityToken)
            {
                //Convert to JwtSecurityToken
                var jwtToken = context.SecurityToken as JwtSecurityToken;

                //Find Token
                var tokenEntity = _userTokenService.GetToken(new SecurityHasher().GetSha256Hash(jwtToken.RawData));

                //Check exist token in db
                if (tokenEntity == null)
                {
                    context.Fail("Token is not exist in DataBase");
                    return;
                }

                //Check expire time of token
                if (tokenEntity.TokenExpireTime < DateTime.Now)
                {
                    //Delete token in db
                    //_userTokenService.DeleteToken(tokenEntity);

                    context.Fail("Token is Expired");
                    return;
                }

            }
            else
            {
                context.Fail("Token is not jwt token");
                return;
            }
        }
    }
}
