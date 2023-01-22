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
    public class SalonController : ControllerBase
    {
        private readonly IAddSalonService _addSalonService;
        private readonly IEditSalonService _editSalonService;
        private readonly IDeleteSalonService _deleteSalonService;
        public SalonController(IAddSalonService addSalonService,
            IEditSalonService editSalonService,
            IDeleteSalonService deleteSalonService)
        {
            _editSalonService = editSalonService;
            _addSalonService = addSalonService;
            _deleteSalonService = deleteSalonService;
        }

        /// <summary>
        /// برای ایجاد سالن آرایشی
        /// </summary>
        /// <param name="salonDto"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateSalonApiDto salonDto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map
            var dtoService = new CreateSalonDto
            {
                Name = salonDto.Name,
                CityId = salonDto.CityId,
                Description = salonDto.Description,
                FullAddress = salonDto.FullAddress,
                PhoneNumber = salonDto.PhoneNumber,
                Telphone = salonDto.Telphone,
                UserId = userId
            };

            //Create salon by service
            var resultService = await _addSalonService.Execute(dtoService);

            if (resultService.IsSuccess)
            {
                return Created("temp", null);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برای ویرایش کردن سالن آرایشی
        /// </summary>
        /// <param name="salonDto"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> Put(EditSalonApiDto salonDto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            var modelService = new EditSalonDto
            {
                Id = salonDto.Id,
                CityId = salonDto.CityId,
                Description = salonDto.Description,
                FullAddress = salonDto.FullAddress,
                Name = salonDto.Name,
                PhoneNumber = salonDto.PhoneNumber,
                Telphone = salonDto.Telphone,
                UserId = userId
            };

            //Edit salon by service
            var resultService = await _editSalonService.Execute(modelService);

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
        /// برای حذف کردن یک سالن آرایشی
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{SalonId}")]
        public async Task<IActionResult> Delete(int SalonId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Delete by service
            var resultService = await _deleteSalonService.Execute(userId, SalonId);

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
