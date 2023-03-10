using Application.ProfileService.Command;
using Application.ProfileService.Query;
using Application.TokenService;
using Application.UserService;
using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static System.Net.WebRequestMethods;

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
        /// بر گردوندن تصویر پروفایل (Auth)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get()
        {
            //Base Path Image
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Profile");

            //Find user
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            //Check user has profile image
            if (string.IsNullOrEmpty(user.ImageName))
                return NoContent();

            //file image
            string pathFile = Path.Combine(basePath, user.ImageName);

            //Build url of image
            string url = Request.GetDisplayUrl();
            string domainName = url.Substring(0, url.IndexOf("/api"));
            string imageUrl = domainName + "/Images/Profile/" + user.ImageName;

            //Does not find image file
            if (System.IO.File.Exists(pathFile) == false)
            {
                //So make sure delete in db for user
                await DeleteImageFile(user);
            }


            //Return image file
            //var imageFileStream = System.IO.File.OpenRead(pathFile);

            //var extension = Path.GetExtension(pathFile);
            //if (extension == ".jpg")
            //    return File(imageFileStream, "image/jpeg");
            //else
            //    return File(imageFileStream, "image/png");

            return Ok(new { ImageUrl = imageUrl, ImageName = user.ImageName });
        }


        /// <summary>
        /// آپدیت کردن عکس پروفایل (Auth)
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(IFormFile file)
        {
            //Check size image
            var megabyte = file.Length / (1024 * 1024);
            if (megabyte > 10)
                return BadRequest("حجم تصویر بیشتر از 10 مگابایت نمیتواند باشد");

            //Check extension  jpg or png
            var extension = Path.GetExtension(file.FileName);
            if ((extension == ".jpg" || extension == ".png") == false)
                return BadRequest("فرمت تصویر پروفایل میتواند jpg یا png باشد");

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
        /// حدف کردن عکس پروفایل (Auth)
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
            if (!string.IsNullOrEmpty(user.ImageName))
            {
                var resultDelete = await DeleteImageFile(user);

                if (resultDelete == false)
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
