using Application.Common;
using Application.CustomerService.Command;
using Application.ProfileService.Command;
using Application.ProfileService.Query;
using Application.TokenService;
using Application.UserService;
using Domain.Users;
using Infrastructure.EmailService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApi.ModelsAndDtoes.Profile;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserAuthorizeService _userAuthorizeService;
        private readonly IUserTokenService _userTokenService;
        private readonly IGetProfileService _getProfileService;
        private readonly IEditProfileService _editProfileService;
        public ProfileController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<AccountController> logger,
            IUserTokenService userTokenService,
            IGetProfileService getProfileService,
            IEditProfileService editProfileService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _userTokenService = userTokenService;
            _getProfileService = getProfileService;
            _editProfileService = editProfileService;
        }

        /// <summary>
        /// بر گردوندن اطلاعات پروفایل (Auth)
        /// </summary>
        /// <returns></returns>
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
            var resultService = await _getProfileService.Execute(userId);
            if (resultService.IsSuccess == false)
                return Problem();

            //HATEOAS links
            resultService.Data.Link = new Application.Common.Link
            {
                For = "Edit",
                HttpMethod = HttpMethod.Put.ToString(),
                Url = Url.Action(nameof(Put), "Profile", null, Request.Scheme)
            };

            return Ok(resultService.Data);
        }

        /// <summary>
        /// ویرایش اطلاعات پروفایل (Auth)
        /// </summary>
        /// <param name="editPofileDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(EditProfileDto editPofileDto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map to app dto 
            var profAppDto = new EditProfileAppDto
            {
                FullName = editPofileDto.FullName,
                Gender = (Application.ProfileService.Command.Gender)editPofileDto.Gender,
                PhoneNumber = string.IsNullOrWhiteSpace(editPofileDto.PhoneNumber) ? null : editPofileDto.PhoneNumber
            };

            //Update prof
            var resultEditService = await _editProfileService.Execute(userId, profAppDto);
            if (resultEditService.IsSuccess == false)
            {
                return Problem(resultEditService.Message);
            }

            //HATEOAS links
            var link = new Link
            {
                For = "Details",
                HttpMethod = HttpMethod.Get.ToString(),
                Url = Url.Action(nameof(Get), "Profile", null, Request.Scheme)
            };

            return Ok(link);
        }
    }
}
