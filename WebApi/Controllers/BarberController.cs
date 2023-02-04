using Application.BarberService.Command;
using Application.SalonsService.Command;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.ModelsAndDtoes.Barber;
using WebApi.ModelsAndDtoes.Salon;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class BarberController : ControllerBase
    {
        private readonly IAddBarberService _addBarberService;
        private readonly IEditBarberService _editBarberService;
        private readonly IDeleteBarberService _deleteBarberService;
        public BarberController(IAddBarberService addBarberService,
            IEditBarberService editBarberService,
            IDeleteBarberService deleteBarberService
            )
        {
            _editBarberService = editBarberService;
            _addBarberService = addBarberService;
            _deleteBarberService = deleteBarberService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateBarberApiDto createBarberDto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map
            var dtoService = new CreateBarberDto
            {
                Description = createBarberDto.Description,
                UserId = userId,
                SalonId = createBarberDto.SalonId,
            };

            //Create Barber by service
            var resultService = await _addBarberService.Execute(dtoService);

            if (resultService.IsSuccess)
            {
                //HATEOAS links
                //string url = Url.Action(nameof(Get), "Barber", new { SalonId = resultService.Data.Id }, Request.Scheme);

                return Created("", "شما به عنوان یک آرایشگر ثبت شدید");
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> Put(EditBarberApiDto editBarberApiDto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map
            var dtoService = new EditBarberDto
            {
                Description = editBarberApiDto.Description,
                SalonId = editBarberApiDto.SalonId,
                UserId = userId
            };

            //Edit Barber by service
            var resultService = await _editBarberService.Execute(dtoService);

            if (resultService.IsSuccess)
            {
                return Ok("اطلاعات مربوط به آرایشگری شما با موفقیت ویرایش شد");
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Delete Barber by service
            var resultService = await _deleteBarberService.Execute(userId);

            if (resultService.IsSuccess)
            {
                return Ok("عنوان کاربری شما از آرایشگر به کاربر عادی تغییر کرد");
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }
    }
}
