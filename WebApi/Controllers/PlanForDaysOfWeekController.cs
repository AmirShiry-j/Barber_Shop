using Application.WeekDayPlanService.Command;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PlanForDaysOfWeekController : ControllerBase
    {
        private readonly ISetTimeModeToDaysOfWeekService _timeModeToDaysOfWeekService;
        public PlanForDaysOfWeekController(ISetTimeModeToDaysOfWeekService timeModeToDaysOfWeekService)
        {
            _timeModeToDaysOfWeekService = timeModeToDaysOfWeekService;
        }

        /// <summary>
        /// تنظیم برنامه زمانی برای روزهای هفته
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(SetPlanApiDto Dto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //map to model
            var inputService = new SetPlanDto
            {
                UserId = userId,
                DaysOfWeek = Dto.DaysOfWeek,
                Excepts = Dto.Excepts,
                TimeModeId = Dto.TimeModeId
            };

            //Send model to service
            var resultService = await _timeModeToDaysOfWeekService.Execute(inputService);
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
