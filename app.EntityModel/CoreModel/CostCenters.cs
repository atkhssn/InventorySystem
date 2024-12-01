using System.ComponentModel.DataAnnotations.Schema;

namespace app.EntityModel.CoreModel
{
    [Table("CostCenters", Schema = "New")]
    public sealed class CostCenters : BaseEntity
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
    }
}
