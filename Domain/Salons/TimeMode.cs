using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Salons
{
    public class TimeMode : BaseProps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime? TimeLastUpdate { get; set; }
        //Nav
        public int BarberId { get; set; }
        public ICollection<TimeModeItem> TimeModeItems { get; set; }
    }
}
