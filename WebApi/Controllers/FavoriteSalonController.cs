using Application.FavoriteSalonService.Command;
using Application.SalonsService.Command;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public FavoriteSalonController(IRemoveFavoriteSalonService removeFavoriteSalonService, IAddFavoriteSalonService addFavoriteSalonService)
        {
            _removeFavoriteSalonService = removeFavoriteSalonService;
            _addFavoriteSalonService = addFavoriteSalonService;
        }

        /// <summary>
        /// برای اضافه کردن آرایشگاه به علاقه مندی های کاربر
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
                return Ok();
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برای حذف یک آرایشگاه از علاقه مندی های کاربر
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
