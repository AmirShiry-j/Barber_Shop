using Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class Role : IdentityRole, BaseProps
    {
        public string Description { get; set; }
        public DateTime TimeCreate { get; set; }
    }
}
