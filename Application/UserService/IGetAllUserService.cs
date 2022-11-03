using Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService
{
    public interface IGetAllUserService
    {
        public Task<List<UserDto>> Execute();
    }
    public class GetAllUserService : IGetAllUserService
    {
        private readonly IDataBaseContext _dataBase;
        public GetAllUserService(IDataBaseContext dataBase)
        {
            _dataBase = dataBase;
        }
        public async Task<List<UserDto>> Execute()
        {
            var users = _dataBase.Users.Select(p => new UserDto
            {
                Id = p.Id,
                FullName = p.FullName,
                Email = p.Email,
            }).ToList();

            return users;
        }
    }
    public class UserDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
