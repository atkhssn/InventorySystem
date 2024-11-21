using app.Services.ProductCategory_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Company_Service
{
    public class CompanyViewModel:BaseViewModel
    {
        public string Name { get; set; }
        public IEnumerable<CompanyViewModel> CompanyList { get; set; }
    }
}
