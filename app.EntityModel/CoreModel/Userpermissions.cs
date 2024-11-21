using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class Userpermissions:BaseEntity
    {
        public string UserId { get; set; }
        public long MenuItem { get; set; }
        public int OrderNo { get; set; }
    }
}
