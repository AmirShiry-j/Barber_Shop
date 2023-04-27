using Application.Common;
using Application.ServiceBarberService.Command;
using Application.ServiceBarberService.Query;
using Application.TimeModeService.Command;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.ModelsAndDtoes.Barber;
using WebApi.ModelsAndDtoes.Service;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IAddServiceBarberService _addServiceBarberService;
        private readonly IEditServiceBarberService _editServiceBarberService;
        private readonly IDeleteServiceBarberService _deleteServiceBarberService;
        private readonly IGetServiceBarberByIdService _getServiceBarberByIdService;
        private readonly IGetAllServiceBarberForBarber_Service _getAllServiceBarberForBarber_Service;

        public ServiceController(IAddServiceBarberService addServiceBarberService, IEditServiceBarberService editServiceBarberService, IDeleteServiceBarberService deleteServiceBarberService, IGetServiceBarberByIdService getServiceBarberByIdService, IGetAllServiceBarberForBarber_Service getAllServiceBarberForBarber_Service)
        {
            _addServiceBarberService = addServiceBarberService;
            _editServiceBarberService = editServiceBarberService;
            _deleteServiceBarberService = deleteServiceBarberService;
            _getServiceBarberByIdService = getServiceBarberByIdService;
            _getAllServiceBarberForBarber_Service = getAllServiceBarberForBarber_Service;
        }

        /// <summary>
        /// برگردوندن همه خدماتی که توسط آرایشگر ارائه می شود (Auth)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //get Service by service
            var resultService = await _getAllServiceBarberForBarber_Service.Execute(userId);

            if (resultService.IsSuccess)
            {
                //HATEOAS links
                foreach (var service in resultService.Data)
                {
                    var link = new Link
                    {
                        For = "Details",
                        HttpMethod = HttpMethod.Get.ToString(),
                        Url = Url.Action(nameof(Get), "Service", new { ServiceId = service.Id }, Request.Scheme)
                    };

                    service.Link = link;
                }

                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برگردوندن یکی از خدمات آرایشگر با آیدی (Auth)
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        [HttpGet("{ServiceId}")]
        public async Task<IActionResult> Get(int ServiceId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //get Service by service
            var resultService = await _getServiceBarberByIdService.Execute(userId, ServiceId);


            if (resultService.IsSuccess)
            {
                //HATEOAS links
                resultService.Data.Links = new List<Link>
                {
                    new Link
                    {
                        For="Edit",
                        HttpMethod=HttpMethod.Put.ToString(),
                        Url=Url.Action(nameof(Put),"Service",null,Request.Scheme)
                    },
                    new Link
                    {
                        For="Delete",
                        HttpMethod=HttpMethod.Delete.ToString(),
                        Url=Url.Action(nameof(Delete),"Service",new { ServiceId=ServiceId},Request.Scheme)
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
        /// برای ایجاد یک خدمت جدید (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateServiceApiDto Dto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map to model
            var inputService = new CreateServiceDto
            {
                Name = Dto.Name,
                Description= Dto.Description,
                Price= Dto.Price
            };

            //Send to service
            var resultService = await _addServiceBarberService.Execute(userId, inputService);
            if (resultService.IsSuccess)
            {
                //HATEOAS links
                string url = Url.Action(nameof(Get), "Service", new { ServiceId = resultService.Data }, Request.Scheme);

                return Created(url, null);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برای ویرایش یک خدمت (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(EditServiceApiDto Dto)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map to model
            var inputService = new EditServiceDto
            {
                Id = Dto.Id,
                Name = Dto.Name,
                Description=Dto.Description,
                Price=Dto.Price
            };

            //Send to service
            var resultService = await _editServiceBarberService.Execute(userId, inputService);
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
        /// برای حذف یک خدمت (Auth)
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        [HttpDelete("{ServiceId}")]
        public async Task<IActionResult> Delete(int ServiceId)
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Send id for service
            var resultService = await _deleteServiceBarberService.Execute(userId, ServiceId);
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
