using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Areas.Admin.ModelsAndDtoes.Roles;
using WebApi.ModelsAndDtoes.Common;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        public RoleController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// برگردوندن لیست نقش ها در سایت
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Get role list
            var roles = _roleManager.Roles.Select(p => new RoleDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                //HATEOAS links
                Links = new List<Link>
                {
                    new Link
                    {
                        For="Details",
                        HttpMethod="Get",
                        Url=Url.Action(nameof(Get),"Role",new {RoleId=p.Id, Area="Admin" },Request.Scheme)
                    },
                    new Link
                    {
                        For="Edit",
                        HttpMethod="Put",
                        Url=Url.Action(nameof(Update),"Role",new {Area="Admin" },Request.Scheme)
                    },
                    new Link
                    {
                        For="Delete",
                        HttpMethod="Delete",
                        Url=Url.Action(nameof(Delete),"Role",new {RoleId=p.Id, Area="Admin" },Request.Scheme)
                    },
                }
            }).ToList();

            return Ok(roles);
        }

        /// <summary>
        /// برگشت یک نقش سایت به وسیله آیدی اون
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpGet("{RoleId}")]
        public async Task<IActionResult> Get(string RoleId)
        {
            //Get one role by id
            var role = await _roleManager.FindByIdAsync(RoleId);

            //Map to model
            RoleDto model = null;
            if (role != null)
            {
                model = new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    //HATEOAS links
                    Links = new List<Link>
                    {
                        new Link
                        {
                            For="Details",
                            HttpMethod="Get",
                            Url=Url.Action(nameof(Get),"Role",new {RoleId=role.Id, Area="Admin" },Request.Scheme)
                        },
                        new Link
                        {
                            For="Edit",
                            HttpMethod="Put",
                            Url=Url.Action(nameof(Update),"Role",new {Area="Admin" },Request.Scheme)
                        },
                        new Link
                        {
                            For="Delete",
                            HttpMethod="Delete",
                            Url=Url.Action(nameof(Delete),"Role",new {RoleId=role.Id, Area="Admin" },Request.Scheme)
                        },
                    }
                };
            }

            return Ok(model);
        }

        /// <summary>
        /// اضافه کردن نقش جدید به سایت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto model)
        {
            //Map to Role
            var newRole = new Role
            {
                Name = model.Name,
                Description = model.Description
            };

            //Add Role
            var resultAddRole = await _roleManager.CreateAsync(newRole);

            if (resultAddRole.Succeeded)
            {
                //HATEOAS link for new item
                var link = Url.Action(nameof(Get), "Role", new { RoleId = newRole.Id, Area = "Admin" }, protocol: Request.Scheme);

                return Created(link, null);
            }
            else
            {
                //Return error
                var error = resultAddRole.Errors.Select(p => p.Description).Aggregate((p1, p2) => p1 + "," + p2);
                return BadRequest(error);
            }
        }

        /// <summary>
        /// ویرایش اطلاعات نقش 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(EditRoleDto model)
        {
            //Find role by id
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ////Create new role for not founding role 
                //Map to Role
                var newRole = new Role
                {
                    Name = model.Name,
                    Description = model.Description
                };

                //Create new role
                var resultCreate = await _roleManager.CreateAsync(newRole);

                //HATEOAS link for new role
                var link = Url.Action(nameof(Get), "Role", new { RoleId = newRole.Id, Area = "Admin" }, protocol: Request.Scheme);

                return Created(link, null);
            }

            //Chech Can not edit Admin role name
            if (role.Name == "Admin" && role.Name != model.Name)
            {
                //Return error its cant edit admin name
                return BadRequest("نام نقش ادمین قابل ادیت نیست");
            }


            //Initail edits
            role.Name = model.Name;
            role.Description = model.Description;

            //Update role
            var resultEdit = await _roleManager.UpdateAsync(role);

            if (resultEdit.Succeeded)
            {
                return Ok();
            }
            else
            {
                //Return error
                var error = resultEdit.Errors.Select(p => p.Description).Aggregate((p1, p2) => p1 + "," + p2);
                return BadRequest(error);
            }
        }

        /// <summary>
        /// حذف یک نقش از سایت
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpDelete("{RoleId}")]
        public async Task<IActionResult> Delete(string RoleId)
        {
            //Find role
            var role = await _roleManager.FindByIdAsync(RoleId);

            //Check exist role
            if (role == null)
            {
                return NotFound();
            }

            //Check it is not Admin role
            if (role.Name == "Admin")
            {
                //Return error its cat not delete admin role
                return BadRequest("نقش ادمین از سایت قابل حذف نیست");
            }

            //Delete Role
            var resultDeleteRole = await _roleManager.DeleteAsync(role);

            if (resultDeleteRole.Succeeded)
            {
                return Ok();
            }
            else
            {
                //Return error
                var error = resultDeleteRole.Errors.Select(p => p.Description).Aggregate((p1, p2) => p1 + "," + p2);
                return BadRequest(error);
            }
        }
    }
}
