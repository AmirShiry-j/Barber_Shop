using Application.BarberService.Command;
using Application.BarberService.Query;
using Application.Common;
using Application.SalonsService.Command;
using Domain.Salons;
using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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
        private readonly IGetBarberInfoByUserIdService _getBarberInfoByUserIdService;
        private readonly IGetBaberInformationByBarberIdService _getBaberInformationByBarberIdService;
        private readonly IGetBarbersService _getBarbersService;
        public BarberController(IAddBarberService addBarberService,
            IEditBarberService editBarberService,
            IDeleteBarberService deleteBarberService,
            IGetBarberInfoByUserIdService getBarberInfoByUserIdService,
            IGetBaberInformationByBarberIdService getBaberInformationByBarberIdService,
            IGetBarbersService getBarbersService
            )
        {
            _editBarberService = editBarberService;
            _addBarberService = addBarberService;
            _deleteBarberService = deleteBarberService;
            _getBarberInfoByUserIdService = getBarberInfoByUserIdService;
            _getBaberInformationByBarberIdService = getBaberInformationByBarberIdService;
            _getBarbersService = getBarbersService;
        }

        /// <summary>
        /// بر گردوندن لیست تمام آرایشگر ها (موقت)
        /// </summary>
        /// <returns></returns>
        [ApiVersion("1")]
        [HttpGet("~/api/v{version:apiVersion}/[controller]/[action]")]
        public async Task<IActionResult> GetAll()
        {
            //Barber by service
            var resultService = await _getBarbersService.Execute();

            return Ok(resultService.Data);
        }


        /// <summary>
        /// برگردوندن مشخصات مربوط به یک آرایشگر
        /// </summary>
        /// <param name="BarberId"></param>
        /// <returns></returns>
        [HttpGet("{BarberId}")]
        public async Task<IActionResult> Get(int BarberId)
        {
            //Barber by service
            var resultService = await _getBaberInformationByBarberIdService.Execute(BarberId);

            if (resultService.IsSuccess)
            {
                //HATEAOS
                //Build url of image
                string url = Request.GetDisplayUrl();
                string domainName = url.Substring(0, url.IndexOf("/api"));
                resultService.Data.UrlImage = domainName + "/Images/Profile/" + resultService.Data.ImageName;


                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برگردوندن اطلاعات مربوط به آرایشگری (Auth)
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Get UserId
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Barber by service
            var resultService = await _getBarberInfoByUserIdService.Execute(userId);

            if (resultService.IsSuccess)
            {
                //HATEOAS links
                resultService.Data.Links = new List<Link>
                {
                    new Link
                    {
                        For="Edit",
                        HttpMethod=HttpMethod.Put.ToString(),
                        Url=Url.Action(nameof(Put),"Barber",null,Request.Scheme)
                    },
                    new Link
                    {
                        For="Delete",
                        HttpMethod=HttpMethod.Delete.ToString(),
                        Url=Url.Action(nameof(Delete),"Barber",null,Request.Scheme)
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
        /// ثبت نام کردن به عنوان آرایشگر (Auth)
        /// </summary>
        /// <param name="createBarberDto"></param>
        /// <returns></returns>
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
                PhoneNumber = createBarberDto.PhoneNumber,
            };

            //Create Barber by service
            var resultService = await _addBarberService.Execute(dtoService);

            if (resultService.IsSuccess)
            {
                //HATEOAS links
                string url = Url.Action(nameof(Get), "Barber", null, Request.Scheme);

                return Created(url, "شما به عنوان یک آرایشگر ثبت شدید");
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }


        /// <summary>
        /// ویرایش اطلاعات مربوط به آرایشگری (Auth)
        /// </summary>
        /// <param name="editBarberApiDto"></param>
        /// <returns></returns>
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
                UserId = userId,
                PhoneNumber = editBarberApiDto.PhoneNumber
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

        /// <summary>
        /// تغییر عنوان کاربری از آرایشگر به کاربر عادی (Auth)
        /// </summary>
        /// <returns></returns>
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
