using Application.Common;
using Application.TimeModeService.Command;
using Application.TimeModeService.Query;
using Domain.Salons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.ModelsAndDtoes.Barber;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TimeModeController : ControllerBase
    {
        private readonly IAddTimeModeService _addTimeModeService;
        private readonly IEditTimeModeService _editTimeModeService;
        private readonly IDeleteTimeModeService _deleteTimeModeService;
        private readonly IGetAllTimeModeForBarberService _getAllTimeModeForBarberService;
        private readonly IGetTimeModeByIdService _getTimeModeByIdService;
        public TimeModeController(IAddTimeModeService addTimeModeService,
            IEditTimeModeService editTimeModeService,
            IDeleteTimeModeService deleteTimeModeService,
            IGetAllTimeModeForBarberService getAllTimeModeForBarberService,
            IGetTimeModeByIdService getTimeModeByIdService)
        {
            _addTimeModeService = addTimeModeService;
            _editTimeModeService = editTimeModeService;
            _deleteTimeModeService = deleteTimeModeService;
            _getAllTimeModeForBarberService = getAllTimeModeForBarberService;
            _getTimeModeByIdService = getTimeModeByIdService;
        }

        /// <summary>
        /// بر گردوندن گروهبندی های زمانی ایجاد شده توسط آرایشگر (Auth)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //get TimeMode by service
            var resultService = await _getAllTimeModeForBarberService.Execute(userId);

            if (resultService.IsSuccess)
            {
                //HATEOAS links
                foreach (var timeMode in resultService.Data)
                {
                    var link = new Link
                    {
                        For = "Details",
                        HttpMethod = HttpMethod.Get.ToString(),
                        Url = Url.Action(nameof(Get), "TimeMode", new { TimeModeId = timeMode.Id }, Request.Scheme)
                    };

                    timeMode.Link = link;
                }

                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برگردوندن یک گروهبندی زمانی با آیدی (Auth)
        /// </summary>
        /// <param name="TimeModeId"></param>
        /// <returns></returns>
        [HttpGet("{TimeModeId}")]
        public async Task<IActionResult> Get(int TimeModeId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //get TimeMode by service
            var resultService = await _getTimeModeByIdService.Execute(userId, TimeModeId);


            if (resultService.IsSuccess)
            {
                //HATEOAS links
                resultService.Data.Links = new List<Link>
                {
                    new Link
                    {
                        For="Edit",
                        HttpMethod=HttpMethod.Put.ToString(),
                        Url=Url.Action(nameof(Put),"TimeMode",null,Request.Scheme)
                    },
                    new Link
                    {
                        For="Delete",
                        HttpMethod=HttpMethod.Delete.ToString(),
                        Url=Url.Action(nameof(Delete),"TimeMode",new { TimeModeId=TimeModeId},Request.Scheme)
                    },
                };

                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }

        }

        /// <summary>
        /// برای ایجاد یک گروهبندی زمانی (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateTimeModeApiDto Dto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map to model
            var inputService = new CreateTimeModeDto
            {
                Name = Dto.Name
            };

            //Send to service
            var resultService = await _addTimeModeService.Execute(userId, inputService);
            if (resultService.IsSuccess)
            {
                //HATEOAS links
                string url = Url.Action(nameof(Get), "TimeMode", new { TimeModeId = resultService.Data }, Request.Scheme);

                return Created(url, null);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برای ویرایش یک گروهبندی زمانی (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(EditTimeModeApiDto Dto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map to model
            var inputService = new EditTimeModeDto
            {
                Id = Dto.Id,
                Name = Dto.Name
            };

            //Send to service
            var resultService = await _editTimeModeService.Execute(userId, inputService);
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
        /// برای حذف یک گروهبندی زمانی (Auth)
        /// </summary>
        /// <param name="TimeModeId"></param>
        /// <returns></returns>
        [HttpDelete("{TimeModeId}")]
        public async Task<IActionResult> Delete(int TimeModeId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Send id for service
            var resultService = await _deleteTimeModeService.Execute(userId, TimeModeId);
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
