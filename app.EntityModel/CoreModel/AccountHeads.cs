using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class AccountHeads:BaseEntity
    {
        public string AccName { set; get; }
        public string AccCode {set; get; } 
        public string Remarks { set; get; } 
        public int  AccType { set; get; } 
    }
}
