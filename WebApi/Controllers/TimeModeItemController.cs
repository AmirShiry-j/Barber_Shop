using Application.Common;
using Application.TimeModeItemService.Command;
using Application.TimeModeItemService.Query;
using Application.TimeModeService.Command;
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
    public class TimeModeItemController : ControllerBase
    {
        private readonly IAddTimeModeItemService _addTimeModeItemService;
        private readonly IEditTimeModeItemService _editTimeModeItemService;
        private readonly IDeleteTimeModeItemService _deleteTimeModeItemService;
        private readonly IGetTimeModeItemService _getTimeModeItemService;
        private readonly IGetAllItemsInTimeModeService _getAllItemsInTimeModeService;
        public TimeModeItemController(IGetAllItemsInTimeModeService getAllItemsInTimeModeService,
            IGetTimeModeItemService getTimeModeItemService,
            IAddTimeModeItemService addTimeModeItemService,
            IEditTimeModeItemService editTimeModeItemService,
            IDeleteTimeModeItemService deleteTimeModeItemService)
        {
            _getAllItemsInTimeModeService = getAllItemsInTimeModeService;
            _getTimeModeItemService = getTimeModeItemService;
            _addTimeModeItemService = addTimeModeItemService;
            _editTimeModeItemService = editTimeModeItemService;
            _deleteTimeModeItemService = deleteTimeModeItemService;
        }

        /// <summary>
        /// برگردوندن نوبت های ایجاد شده در یک گروهبندی زمانی (Auth)
        /// </summary>
        /// <param name="TimeModeId"></param>
        /// <returns></returns>
        //[HttpGet("{TimeModeId}")]
        [HttpGet("~/api/v{version:apiVersion}/[controller]/[action]/{TimeModeId}")]
        public async Task<IActionResult> GetAllItems(int TimeModeId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //get TimeModeItems by service
            var resultService = await _getAllItemsInTimeModeService.Execute(userId, TimeModeId);

            if (resultService.IsSuccess)
            {
                //HATEOAS links
                foreach (var timeModeItem in resultService.Data)
                {
                    var link = new Link
                    {
                        For = "Details",
                        HttpMethod = HttpMethod.Get.ToString(),
                        Url = Url.Action(nameof(Get), "TimeModeItem", new { TimeModeItemId = timeModeItem.Id }, Request.Scheme)
                    };

                    timeModeItem.Link = link;
                }

                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برگردوندن یک نوبت از گروهبندی زمانی با آیدی (Auth)
        /// </summary>
        /// <param name="TimeModeItemId"></param>
        /// <returns></returns>
        [HttpGet("{TimeModeItemId}")]
        public async Task<IActionResult> Get(int TimeModeItemId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //get TimeModeItem by service
            var resultService = await _getTimeModeItemService.Execute(userId, TimeModeItemId);


            if (resultService.IsSuccess)
            {
                //HATEOAS links
                resultService.Data.Links = new List<Link>
                {
                    new Link
                    {
                        For="Edit",
                        HttpMethod=HttpMethod.Put.ToString(),
                        Url=Url.Action(nameof(Put),"TimeModeItem",null,Request.Scheme)
                    },
                    new Link
                    {
                        For="Delete",
                        HttpMethod=HttpMethod.Delete.ToString(),
                        Url=Url.Action(nameof(Delete),"TimeModeItem",new { TimeModeItemId=TimeModeItemId},Request.Scheme)
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
        /// برای ایجاد یک نوبت در گروهبندی زمانی (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateTimeModeItemApiDto Dto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map to model
            var inputService = new CreateTimeModeItemDto
            {
                Hour=Dto.Hour,
                Minute=Dto.Minute,
                TimeModeId=Dto.TimeModeId
            };

            //Send to service
            var resultService = await _addTimeModeItemService.Execute(userId, inputService);
            if (resultService.IsSuccess)
            {
                //HATEOAS links
                string url = Url.Action(nameof(Get), "TimeModeItem", new { TimeModeItemId = resultService.Data }, Request.Scheme);

                return Created(url, null);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برای ویرایش یک نوبت از گروهبندی زمانی (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(EditTimeModeItemApiDto Dto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map to model
            var inputService = new EditTimeModeItemDto
            {
                Id = Dto.Id,
                Hour=Dto.Hour,
                Minute=Dto.Minute
            };

            //Send to service
            var resultService = await _editTimeModeItemService.Execute(userId, inputService);
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
        /// برای حذف یک نوبت از گروهبندی زمانی (Auth)
        /// </summary>
        /// <param name="TimeModeItemId"></param>
        /// <returns></returns>
        [HttpDelete("{TimeModeItemId}")]
        public async Task<IActionResult> Delete(int TimeModeItemId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Send id for service
            var resultService = await _deleteTimeModeItemService.Execute(userId, TimeModeItemId);
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
