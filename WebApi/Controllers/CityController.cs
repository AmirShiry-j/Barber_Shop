using Application.AddressesService.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IGetCitiesService _getCitiesService;
        public CityController(IGetCitiesService getCitiesService)
        {
            _getCitiesService = getCitiesService;
        }
        /// <summary>
        /// برگردوندن لیست شهر های هر استان برای کمبوباکس
        /// </summary>
        /// <param name="UnitedId"></param>
        /// <returns></returns>
        [HttpGet("{UnitedId}")]
        public async Task<IActionResult> Get(int UnitedId)
        {
            //Take Cities from service
            var result =await _getCitiesService.Execute(UnitedId);

            if (result.IsSuccess)
                return Ok(result.Data);
            else
                return BadRequest(result.Message);
        }
    }
}
