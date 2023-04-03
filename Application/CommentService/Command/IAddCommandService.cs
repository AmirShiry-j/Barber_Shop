using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CommentService.Command
{
    public interface IAddCommendService
    {
        Task<ResultDto> Execute(CreateCommentDto Dto);
    }
    public class AddCommendService : IAddCommendService
    {
        private readonly IDataBaseContext _dbContext;
        private readonly IMapper _mapper;
        public AddCommendService(IDataBaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResultDto> Execute(CreateCommentDto Dto)
        {
            //check has barber
            var barber = _dbContext.Barbers.Find(Dto.BarberId);
            if (barber == null)
            {
                return new ResultDto
                {
                    Message = "آرایشگری با این ایدی موجود نیست"
                };
            }

            ////Limiteds
            var hasBefore = _dbContext.Comments.Where(p => p.UserId == Dto.UserId && p.BarberId == Dto.BarberId).Any();
            if (hasBefore)
            {
                return new ResultDto
                {
                    Message = "شما قبلا نظری ثبت کرده اید"
                };
            }

            //The restriction is that the user has already been a customer of this hairdresser
            //... 
            //موقت نیست

            //map to entity
            var newComment = new Comment()
            {
                Text = Dto.Text,
                Confirmation = true,//موقت //temp
                SuggestionMode = (SuggestionMode)Dto.SuggestionMode,
                BarberId = Dto.BarberId,
                UserId = Dto.UserId,
            };

            //Save in db
            _dbContext.Comments.Add(newComment);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
    public class CreateCommentDto
    {
        public string Text { get; set; }
        public string UserId { get; set; }
        public int BarberId { get; set; }
        public SuggestionModeDto SuggestionMode { get; set; }
    }
    public enum SuggestionModeDto
    {
        UnSuggested = 0,//پیشنهاد نمیشود
        Neutral = 1,//نظری ندارد
        Suggested = 2,//پیشنهاد میشود
    }
}
