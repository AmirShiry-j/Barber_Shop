using Application.ProfileService.Command;
using Application.ProfileService.Query;
using Application.TokenService;
using Application.UserService;
using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class ProfileImageController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserAuthorizeService _userAuthorizeService;
        private readonly IUserTokenService _userTokenService;
        private readonly IGetProfileService _getProfileService;
        private readonly IEditProfileService _editProfileService;
        public ProfileImageController(UserManager<User> userManager,
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
        /// آپدیت کردن عکس پروفایل
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(IFormFile file)
        {
            //Base Path Image
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Profile");

            //Find user
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            //Delete image profile user if has
            if (!string.IsNullOrEmpty(user.ImageName))
            {
                var resultDelete = await DeleteImageFile(user);

                if (resultDelete == false)
                    return Problem();
            }

            ////Save Image in files and db
            //in files
            string imageName = "";
            string fullPath = "";
            do
            {
                imageName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                fullPath = Path.Combine(basePath, imageName);

            } while (System.IO.File.Exists(fullPath));
            using (Stream streamFile = new FileStream(fullPath, FileMode.CreateNew))
            {
                file.CopyTo(streamFile);
            }

            //in db
            user.ImageName = imageName;
            var resultUpdate2 = await _userManager.UpdateAsync(user);
            if (resultUpdate2.Succeeded == false)
            {
                return Problem();
            }

            //Return success
            return Ok();
        }

        /// <summary>
        /// حدف کردن عکس پروفایل
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete()
        {
            //Find user
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            //Delete image if has
            if(!string.IsNullOrEmpty(user.ImageName))
            {
                var resultDelete = await DeleteImageFile(user);

                if (resultDelete==false)
                    return Problem();
            }
                
            //Return success
            return Ok();
        }

        [NonAction]
        public async Task<bool> DeleteImageFile(User user)
        {
            //Base Path Image
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Profile");

            //Delete image profile user if has
            if (!string.IsNullOrEmpty(user.ImageName))
            {
                //Delete old image file
                string pathOldFile = Path.Combine(basePath, user.ImageName);
                if (System.IO.File.Exists(pathOldFile))
                {
                    System.IO.File.Delete(pathOldFile);
                    user.ImageName = null;

                    //Update User (delete profile img)
                    var resultUpdate1 = await _userManager.UpdateAsync(user);
                    if (resultUpdate1.Succeeded == false)
                    {
                        return false;
                    }
                }
                else
                {
                    user.ImageName = null;

                    //Update User (delete profile img)
                    var resultUpdate1 = await _userManager.UpdateAsync(user);
                    if (resultUpdate1.Succeeded == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
