using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class Upazila:BaseEntity
    {
        public string Name { get; set; }
        public long DistrictId { get; set; }
    }
}
