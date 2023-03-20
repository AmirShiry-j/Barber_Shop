using Application.FavoriteSalonService.Command;
using Application.FavoriteSalonService.Query;
using Domain.Salons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.ModelsAndDtoes.Common;
using WebApi.ModelsAndDtoes.Salon;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FavoriteSalonController : ControllerBase
    {
        private readonly IAddFavoriteSalonService _addFavoriteSalonService;
        private readonly IRemoveFavoriteSalonService _removeFavoriteSalonService;
        private readonly IGetFavoriteSalonService _getFavoriteSalonService;
        public FavoriteSalonController(IRemoveFavoriteSalonService removeFavoriteSalonService,
            IAddFavoriteSalonService addFavoriteSalonService,
            IGetFavoriteSalonService getFavoriteSalonService)
        {
            _removeFavoriteSalonService = removeFavoriteSalonService;
            _addFavoriteSalonService = addFavoriteSalonService;
            _getFavoriteSalonService = getFavoriteSalonService;
        }

        /// <summary>
        /// بر گردوندن آرایشگاه های مورد علاقه کاربر (Auth)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get data from service
            var resultService = await _getFavoriteSalonService.Execute(userId);
            if (resultService.IsSuccess)
            {
                string url = Request.GetDisplayUrl();
                string domainName = url.Substring(0, url.IndexOf("/api"));

                foreach (var salonDto in resultService.Data)
                {
                    //Hatheoas for image profile
                    if (string.IsNullOrWhiteSpace(salonDto.NameSalonImage) == false)
                    {
                        salonDto.UrlSalonImage = domainName + "/Images/SalonImage/" + salonDto.NameSalonImage;
                    }

                    //Hatheoas for profile Barber
                    salonDto.UrlSalon = Url.Action(nameof(Get), "Salon", new { SalonId = salonDto.SalonId }, Request.Scheme);

                    //Hateaos for Remove favorite
                    var linkRemove = new Application.Common.Link
                    {
                        For = "Remove",
                        HttpMethod = HttpMethod.Delete.ToString(),
                        Url = Url.Action(nameof(Delete), "FavoriteSalon", new { SalonId = salonDto.SalonId }, Request.Scheme)
                    };
                    salonDto.Link = linkRemove;

                }

                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برای اضافه کردن آرایشگاه به علاقه مندی های کاربر (Auth)
        /// </summary>
        /// <param name="SalonId"></param>
        /// <returns></returns>
        [HttpPost("{SalonId}")]
        public async Task<IActionResult> Post(int SalonId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //call service
            var resultService = await _addFavoriteSalonService.Execute(userId, SalonId);
            if (resultService.IsSuccess)
            {
                //Hateaos
                var linkRemove = new Link
                {
                    For = "Remove",
                    HttpMethod = HttpMethod.Delete.ToString(),
                    Url = Url.Action(nameof(Delete), "FavoriteSalon", new { SalonId = SalonId }, Request.Scheme)
                };

                return Ok(linkRemove);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برای حذف یک آرایشگاه از علاقه مندی های کاربر (Auth)
        /// </summary>
        /// <param name="SalonId"></param>
        /// <returns></returns>
        [HttpDelete("{SalonId}")]
        public async Task<IActionResult> Delete(int SalonId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //call service
            var resultService = await _removeFavoriteSalonService.Execute(userId, SalonId);
            if (resultService.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }
    }
}
