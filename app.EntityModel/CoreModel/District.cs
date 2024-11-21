using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class District:BaseEntity
    {
        public string Name { set; get; }
        public long DivisionId { set; get; }
    }
}
