using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class Company:BaseEntity
    {
        public string Name { set; get; }
        public string Address { set; get; }
    }
}
