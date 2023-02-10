using Application.SalonImageService.Command;
using Application.SalonImageService.Query;
using Application.SalonsService.Query;
using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class SalonImageController : ControllerBase
    {
        private readonly IGetSalonImagesBySalonIdService _getSalonImagesBySalonIdService;
        private readonly IAddSalonImageService _addSalonImageService;
        private readonly IDeleteSalonImageByNameService _deleteSalonImageByNameService;
        public SalonImageController(IDeleteSalonImageByNameService deleteSalonImageByNameService,
            IAddSalonImageService addSalonImageService,
            IGetSalonImagesBySalonIdService getSalonImagesBySalonIdService
            )
        {
            _addSalonImageService = addSalonImageService;
            _getSalonImagesBySalonIdService = getSalonImagesBySalonIdService;
            _deleteSalonImageByNameService = deleteSalonImageByNameService;
        }

        [HttpGet("{SalonId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get(int SalonId)
        {
            //Find user
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get Images by service
            var resultService = await _getSalonImagesBySalonIdService.Execute(SalonId, userId);
            if (resultService.IsSuccess)
            {
                if (resultService.Data.Any() == false)
                {
                    return NoContent();
                }

                //HATEAOS
                //Build url of image
                string url = Request.GetDisplayUrl();
                string domainName = url.Substring(0, url.IndexOf("/api"));
                foreach (var imageOb in resultService.Data)
                {
                    string imageUrl = domainName + "/Images/SalonImage/" + imageOb.Name;
                    imageOb.Url = imageUrl;
                }

                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        [HttpPost("{SalonId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post(IFormFile file, int SalonId)
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
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/SalonImage");

            //Find user
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

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


            //Save imageName by service
            var inputService = new CreateSalonImageDto
            {
                Name = imageName,
                SalonId = SalonId,
                UserId = userId
            };
            var resultService = await _addSalonImageService.Execute(inputService);

            if (resultService.IsSuccess)
            {
                //Build url of image
                string url = Request.GetDisplayUrl();
                string domainName = url.Substring(0, url.IndexOf("/api"));
                string imageUrl = domainName + "/Images/SalonImage/" + resultService.Data;

                return Created(imageUrl, null);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        [HttpDelete("{Name}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(string Name)
        {
            //Find user
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            var resultService = await _deleteSalonImageByNameService.Execute(Name, userId);
            if (resultService.IsSuccess)
            {
                //Base Path Image
                string basePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/SalonImage");

                //Delete old image file
                string pathOldFile = Path.Combine(basePath, Name);
                if (System.IO.File.Exists(pathOldFile))
                {
                    System.IO.File.Delete(pathOldFile);
                }

                return Ok();
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }
    }
}
