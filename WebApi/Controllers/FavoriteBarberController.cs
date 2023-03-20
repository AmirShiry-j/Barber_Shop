using Application.FavoriteBarberService.Command;
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
    public class FavoriteBarberController : ControllerBase
    {
        private readonly IAddFavoriteBarberService _addFavoriteBarberService;
        private readonly IRemoveFavoriteService _removeFavoriteService;
        public FavoriteBarberController(IRemoveFavoriteService removeFavoriteService, IAddFavoriteBarberService addFavoriteBarberService)
        {
            _removeFavoriteService = removeFavoriteService;
            _addFavoriteBarberService = addFavoriteBarberService;
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
                return Ok();
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
