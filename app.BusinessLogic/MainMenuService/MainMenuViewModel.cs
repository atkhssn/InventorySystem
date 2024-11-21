using app.EntityModel.CoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.MainMenuService
{
    public class MainMenuViewModel
    {
        public long Id { get; set; }    
        public string Name { get; set; }
        public string Icon { get; set; }
        public int OrderNo { get; set; }
        public bool IsActive { get; set; }
        public List<MainMenu> datalist { get; set; }    
    }
}
