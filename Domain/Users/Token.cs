using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class Token
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string TokenHash { get; set; }
        public DateTime TokenExpireTime { get; set; }
        public string RefreshTokenHash { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }

        //Navs
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
