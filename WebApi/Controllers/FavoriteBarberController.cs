using Application.FavoriteBarberService.Command;
using Application.FavoriteBarberService.Query;
using Application.SalonsService.Command;
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
    public class FavoriteBarberController : ControllerBase
    {
        private readonly IAddFavoriteBarberService _addFavoriteBarberService;
        private readonly IRemoveFavoriteBarberService _removeFavoriteService;
        private readonly IGetFavoriteBarberService _getFavoriteBarberService;
        public FavoriteBarberController(IRemoveFavoriteBarberService removeFavoriteService,
            IAddFavoriteBarberService addFavoriteBarberService,
            IGetFavoriteBarberService getFavoriteBarberService
            )
        {
            _removeFavoriteService = removeFavoriteService;
            _addFavoriteBarberService = addFavoriteBarberService;
            _getFavoriteBarberService = getFavoriteBarberService;
        }

        /// <summary>
        /// بر گردوندن آرایشگر های مورد علاقه کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get data from service
            var resultService = await _getFavoriteBarberService.Execute(userId);
            if (resultService.IsSuccess)
            {
                string url = Request.GetDisplayUrl();
                string domainName = url.Substring(0, url.IndexOf("/api"));

                foreach (var barberDto in resultService.Data)
                {
                    //Hatheoas for image profile
                    if (string.IsNullOrWhiteSpace(barberDto.NameProfileImage) == false)
                    {
                        barberDto.UrlProfileImage = domainName + "/Images/Profile/" + barberDto.NameProfileImage;
                    }

                    //Hatheoas for profile Barber
                    barberDto.UrlProfileBarber = Url.Action(nameof(Get), "Barber", new { BarberId = barberDto.BarberId }, Request.Scheme);
                }

                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }
        /// <summary>
        /// برای اضافه کردن آرایشگر به علاقه مندی های کاربر
        /// </summary>
        /// <param name="BarberId"></param>
        /// <returns></returns>
        [HttpPost("{BarberId}")]
        public async Task<IActionResult> Post(int BarberId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //call service
            var resultService = await _addFavoriteBarberService.Execute(userId, BarberId);
            if (resultService.IsSuccess)
            {
                //Hateaos
                var linkRemove = new Link
                {
                    For = "Remove",
                    HttpMethod = HttpMethod.Delete.ToString(),
                    Url = Url.Action(nameof(Delete), "FavoriteBarber", new { BarberId = BarberId }, Request.Scheme)
                };

                return Ok(linkRemove);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برای حذف یک آرایشگر از علاقه مندی های کاربر
        /// </summary>
        /// <param name="BarberId"></param>
        /// <returns></returns>
        [HttpDelete("{BarberId}")]
        public async Task<IActionResult> Delete(int BarberId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //call service
            var resultService = await _removeFavoriteService.Execute(userId, BarberId);
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
