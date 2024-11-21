using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Utility.Miscellaneous
{
    public class PagedModel<T>
    {
        public IEnumerable<T> Models { get; set; }
        public PagedList PagedList { get; set; }
        public int TotalEntity { get; set; }
        public int FirstSerialNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int UserType { get; set; }
    }
}
