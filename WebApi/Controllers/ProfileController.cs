using Application.CustomerService.Command;
using Application.ProfileService.Query;
using Application.TokenService;
using Application.UserService;
using Domain.Users;
using Infrastructure.EmailService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/[Action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserAuthorizeService _userAuthorizeService;
        private readonly IUserTokenService _userTokenService;
        private readonly IGetProfileService _getProfileService;
        public ProfileController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<AccountController> logger,
            IUserTokenService userTokenService,
            IGetProfileService getProfileService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _userTokenService = userTokenService;
            _getProfileService = getProfileService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get()
        {
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;
            if (userId == null)
            {
                return BadRequest();
            }

            //Get Profil infoes
            var resultService = await _getProfileService.Execute(Guid.Parse(userId));
            if (resultService.IsSuccess == false)
                return Problem(); 

            return Ok(resultService.Data); 
        }
    }
}
