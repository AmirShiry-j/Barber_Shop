using Domain.Common;
using Domain.Salons;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Comments
{
    public class Comment : BaseProps
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public SuggestionMode SuggestionMode { get; set; }
        public bool Confirmation { get; set; }
        public DateTime TimeCreate { get; set; }
        //Navs
        public User User { get; set; }
        public string UserId { get; set; }
        public Barber Barber { get; set; }
        public int BarberId { get; set; }
    }
    public enum SuggestionMode
    {
        UnSuggested = 0,//پیشنهاد نمیشود
        Neutral = 1,//نظری ندارد
        Suggested = 2,//پیشنهاد میشود
    }
}
