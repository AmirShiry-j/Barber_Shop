using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Roles
{
    public class CreateRoleDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required]
        [MaxLength(70)]
        public string Description { get; set; }
    }
}
