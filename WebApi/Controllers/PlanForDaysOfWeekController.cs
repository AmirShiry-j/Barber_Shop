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
        private readonly ISetTimeModeToDaysOfWeekService _setTimeModeToDaysOfWeekService;
        private readonly IUnSetTimeModeToDaysOfWeekService _unSetTimeModeToDaysOfWeekService;
        public PlanForDaysOfWeekController(ISetTimeModeToDaysOfWeekService setTimeModeToDaysOfWeekService,
            IUnSetTimeModeToDaysOfWeekService unSetTimeModeToDaysOfWeekService)
        {
            _setTimeModeToDaysOfWeekService = setTimeModeToDaysOfWeekService;
            _unSetTimeModeToDaysOfWeekService = unSetTimeModeToDaysOfWeekService;
        }

        /// <summary>
        /// تنظیم برنامه زمانی برای روزهای هفته (Auth)
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
            var resultService = await _setTimeModeToDaysOfWeekService.Execute(inputService);
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
        /// برای از تنظیم خارج کردن یک برنامه زمانی برای روزهایی از هفته (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(UnSetPlanApiDto Dto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //map to model
            var inputService = new UnSetPlanDto
            {
                UserId = userId,
                DaysOfWeek = Dto.DaysOfWeek,
                Excepts = Dto.Excepts,
                TimeModeId = Dto.TimeModeId
            };

            //Send model to service
            var resultService = await _unSetTimeModeToDaysOfWeekService.Execute(inputService);
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
