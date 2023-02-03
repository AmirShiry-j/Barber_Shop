using Application.SalonsService.Command;
using Application.SalonsService.Query;
using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.ModelsAndDtoes.Salon;
using Application.Common;
using System.Data;
using Domain.Salons;

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
        private readonly IGetSalonByIdService _getSalonByIdService;
        private readonly IGetAllSalonsService _getAllSalonsService;
        public SalonController(IAddSalonService addSalonService,
            IEditSalonService editSalonService,
            IDeleteSalonService deleteSalonService,
            IGetSalonByIdService getSalonByIdService,
            IGetAllSalonsService getAllSalonsService
            )
        {
            _editSalonService = editSalonService;
            _addSalonService = addSalonService;
            _deleteSalonService = deleteSalonService;
            _getSalonByIdService = getSalonByIdService;
            _getAllSalonsService = getAllSalonsService;
        }

        /// <summary>
        /// برگردوندن لیست همه ی سالن های آرایشی (موقت)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Get data from service
            var resultService = await _getAllSalonsService.Execute();

            foreach (var salon in resultService.Data)
            {
                salon.Link = new Link
                {
                    For = "Details",
                    HttpMethod = HttpMethod.Get.ToString(),
                    Url = Url.Action(nameof(Get), "Salon", new { SalonId = salon.Id }, Request.Scheme)
                };
            }

            return Ok(resultService.Data);
        }

        /// <summary>
        /// برگردوندن اطلاعات سالن آرایشی با آیدی
        /// </summary>
        /// <param name="SalonId"></param>
        /// <returns></returns>
        [HttpGet("{SalonId}")]
        public async Task<IActionResult> Get(int SalonId)
        {
            //Get data from service
            var resultService = await _getSalonByIdService.Execute(SalonId);

            if (resultService.IsSuccess)
            {
                //HATEOAS links
                resultService.Data.Links = new List<Link>
                {
                    new Link
                    {
                        For="Edit",
                        HttpMethod=HttpMethod.Put.ToString(),
                        Url=Url.Action(nameof(Put),"Salon",null,Request.Scheme)
                    },
                    new Link
                    {
                        For="Delete",
                        HttpMethod=HttpMethod.Delete.ToString(),
                        Url=Url.Action(nameof(Delete),"Salon",new {SalonId=SalonId },Request.Scheme)
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
        /// برای ایجاد سالن آرایشی (Auth)
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
                UserId = userId,
                ForGender = (Application.SalonsService.Command.ForGender)salonDto.ForGender
            };

            //Create salon by service
            var resultService = await _addSalonService.Execute(dtoService);

            if (resultService.IsSuccess)
            {
                //HATEOAS links
                string url = Url.Action(nameof(Get), "Salon", new { SalonId = resultService.Data.Id }, Request.Scheme);

                return Created(url, null);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برای ویرایش کردن سالن آرایشی (Auth)
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
                UserId = userId,
                ForGender = (Application.SalonsService.Command.ForGender)salonDto.ForGender
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
        /// برای حذف کردن یک سالن آرایشی (Auth)
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
