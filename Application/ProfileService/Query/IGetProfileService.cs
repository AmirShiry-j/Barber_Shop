using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProfileService.Query
{
    public interface IGetProfileService
    {
        public Task<ResultDto<ProfileDto>> Execute(string UserId);
    }
    public class GetProfileService : IGetProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IDataBaseContext _dbContext;
        public GetProfileService(IDataBaseContext dbContext
            , UserManager<User> userManager
            , IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ResultDto<ProfileDto>> Execute(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());

            var profileDto = _mapper.Map<ProfileDto>(user);

            //User has owner salon?
            var salonOwner = _dbContext.Salons.Where(p => p.OwnerId == UserId).FirstOrDefault();
            if (salonOwner != null)
            {
                profileDto.HasOwner = true;
                profileDto.SalonId = salonOwner.Id;
            }

            return new ResultDto<ProfileDto>
            {
                Data = profileDto,
                IsSuccess = true
            };
        }
    }
    public class ProfileDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageName { get; set; }
        public int CustomerId { get; set; }
        public Gender Gender { get; set; }
        public bool HasOwner { get; set; }
        public int SalonId { get; set; }
        public Link Link { get; set; }
    }
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
