using Application.CommentService.Command;
using Application.Common;
using Application.Interfaces.Contexts;
using Application.SalonsService.Query;
using AutoMapper;
using Domain.Comments;
using Domain.Salons;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CommentService.Query
{
    public interface IGetCommentsService
    {
        Task<ResultDto<ResultSearchCommentDto>> Execute(SearchCommentDto dto);
    }
    public class GetCommentsService : IGetCommentsService
    {
        private readonly IDataBaseContext _dbContext;
        private readonly IMapper _mapper;
        public GetCommentsService(IDataBaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResultDto<ResultSearchCommentDto>> Execute(SearchCommentDto dto)
        {
            var barberIds = new List<int>();

            if (dto.BarberId != null)
            {
                //check Barber id exist
                var salon = _dbContext.Barbers.Find(dto.BarberId);
                if (salon == null)
                {
                    return new ResultDto<ResultSearchCommentDto>
                    {
                        IsSuccess = false,
                        Message = "آرایشگری با این آیدی موجود نیست"
                    };
                }

                barberIds.Add(dto.BarberId.Value);
            }
            else if (dto.SalonId != null)
            {
                //check salon id exist
                var salon = _dbContext.Salons.Find(dto.SalonId);
                if (salon == null)
                {
                    return new ResultDto<ResultSearchCommentDto>
                    {
                        IsSuccess = false,
                        Message = "سالنی با این آیدی موجود نیست"
                    };
                }

                //get barberids
                barberIds = _dbContext.Barbers.Where(p => p.SalonId.Equals(dto.SalonId)).Select(p => p.Id).ToList();

            }
            else
            {
                return new ResultDto<ResultSearchCommentDto>
                {
                    IsSuccess = false,
                    Message = "باید حداقل یکی از مقادیر، آیدی سالن آرایشی یا آیدی آرایشگر ارسال شود"
                };
            }

            //Check has berber
            if (barberIds.Count == 0)
            {
                return new ResultDto<ResultSearchCommentDto>
                {
                    IsSuccess = true,
                    Message = "کامنتی وجود ندارد"
                };
            }

            //Get Comments
            var comments = _dbContext.Comments.Where(p => barberIds.Contains(p.BarberId) && p.Confirmation)
                .Include(p => p.User)
                .Include(p => p.Barber)
                .ThenInclude(p => p.User)
                .OrderByDescending(p => p.TimeCreate)
                //For Pagination
                .Skip((dto.Page.Value - 1) * dto.CountInPage.Value)
                .Take(dto.CountInPage.Value)
                .Select(p => new CommentDto
                {
                    Id = p.Id,
                    Text = p.Text,
                    TimeCreate = p.TimeCreate,
                    SuggestionMode = (SuggestionModeDto)p.SuggestionMode,
                    FullName = p.User.FullName,
                    BarberFullName = p.Barber.User.FullName,
                    BarberId = p.BarberId,
                    ImageProfile = p.User.ImageName
                }).ToList();

            //For Pagination
            int CountAllItems = _dbContext.Comments.Where(p => barberIds.Contains(p.BarberId) && p.Confirmation).Count();

            return new ResultDto<ResultSearchCommentDto>
            {
                IsSuccess = true,
                Data = new ResultSearchCommentDto
                {
                    Page = dto.Page.Value,
                    CountInPage = dto.CountInPage.Value,
                    CountAllItems = CountAllItems,
                    Comments = comments
                }
            };
        }
    }
    public class SearchCommentDto
    {
        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
        public int? SalonId { get; set; }
        public int? BarberId { get; set; }
    }
    public class ResultSearchCommentDto
    {
        public int Page { get; set; }
        public int CountInPage { get; set; }
        public int CountAllItems { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public SuggestionModeDto SuggestionMode { get; set; }
        public DateTime TimeCreate { get; set; }
        public string ImageProfile { get; set; }
        public string UrlImageProfile { get; set; }
        public string FullName { get; set; }

        public int BarberId { get; set; }
        public string BarberFullName { get; set; }
    }
}
