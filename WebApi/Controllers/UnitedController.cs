using Application.AddressesService.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class UnitedController : ControllerBase
    {
        private readonly IGetUnitedsService _getUnitedsService;
        public UnitedController(IGetUnitedsService getUnitedsService)
        {
            _getUnitedsService = getUnitedsService;
        }
        /// <summary>
        /// برگردوندن لیست استان ها برای کمبوباکس
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Take uniteds from service
            var result = await _getUnitedsService.Execute();

            return Ok(result.Data);
        }
    }
}
