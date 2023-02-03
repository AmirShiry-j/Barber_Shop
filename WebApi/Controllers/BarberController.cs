using Application.BarberService.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


    }
}
