using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Salons
{
    public class TimeModeItem : BaseProps
    {
        public int Id { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime? TimeLastUpdate { get; set; }
        //Nave
        public int TimeModeId { get; set; }
    }
}
