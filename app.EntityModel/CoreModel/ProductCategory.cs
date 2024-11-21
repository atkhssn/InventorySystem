using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public partial class ProductCategory:BaseEntity
    {
        public string Name { get; set; }
        public string ProductType { get; set; }
        public string Remarks { get; set; }
        public int OrderNo { get; set; }
    }
}
