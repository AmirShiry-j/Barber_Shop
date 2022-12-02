using Application.Common;
using Application.Interfaces.Contexts;
using Application.ProfileService.Query;
using AutoMapper;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProfileService.Command
{
    public interface IEditProfileService
    {
        Task<ResultDto> Execute(string UserId, EditProfileAppDto profDto);
    }
    public class EditProfileService: IEditProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IDataBaseContext _dbContext;
        public EditProfileService(IDataBaseContext dbContext
            , UserManager<User> userManager
            , IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ResultDto> Execute(string UserId, EditProfileAppDto profDto)
        {
            //Get User i Db
            var user = await _userManager.FindByIdAsync(UserId);

            //Map
            user.FullName = profDto.FullName;
            user.Gender = (Domain.Users.Gender)profDto.Gender;
            user.PhoneNumber = profDto.PhoneNumber;

            var resultUpdate = await _userManager.UpdateAsync(user);
            if (resultUpdate.Succeeded)
                return new ResultDto
                {
                    IsSuccess = true
                };

            else
                return new ResultDto
                {
                    Message = "عملیات ویرایش اطلاعات کاربر با مشکل مواجه شد"
                };
        }
    }
    public class EditProfileAppDto
    {
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
    }
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
