using app.Services.MenuItemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.UserpermissionsServices
{
   public class UserpermissionViewModel
    {
        public long Id { get; set; }
        public string TrakingId { get; set; }
        public string UserId { get; set; }
        public string MenuName { get; set; }
        public string UserName { get; set; }
        public int OrderNo { get; set; }
        public IEnumerable<MenuItemViewModel> menuitemlist { get; set; }
        public IEnumerable<UserpermissionViewModel> datalist { get; set; }
    }
}
