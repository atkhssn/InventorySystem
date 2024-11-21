using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class MainMenu:BaseEntity
    {
        public string Name { get; set; }
        public int OrderNo { get; set; } 
        public string Icon { get; set; }    
    }
}
