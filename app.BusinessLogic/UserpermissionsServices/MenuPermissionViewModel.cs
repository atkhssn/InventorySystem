using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.UserpermissionsServices
{
    public class MenuPermissionViewModel
    {
        public List<MainMenuVM> MainMenuVM { get; set; }
    }
    public class MainMenuVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Activeid { get; set; }
        public List<MenuItemVM> menuItemVMs { get; set; }
    }
    public class MenuItemVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }
        public int OrderNo { get; set; }
    }


}
