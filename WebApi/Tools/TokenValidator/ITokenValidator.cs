using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.Tools.TokenValidator
{
    public interface ITokenValidator
    {
        Task Execute(TokenValidatedContext context);
    }
    public class TokenValidator : ITokenValidator
    {
        public TokenValidator()
        {
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

                return;
            }
            else
            {
                context.Fail("Token is not jwt token");
                return;
            }
        }
    }
}
