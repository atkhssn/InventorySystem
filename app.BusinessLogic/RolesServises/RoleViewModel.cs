using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.RolesServises
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int CountPermissions { get; set; }
    }
}
