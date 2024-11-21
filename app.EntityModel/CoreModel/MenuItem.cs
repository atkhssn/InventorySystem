using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class MenuItem:BaseEntity
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int OrderNo { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }
        public long MenuId { get; set; }
    }
}
