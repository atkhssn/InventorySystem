using System.ComponentModel.DataAnnotations.Schema;

namespace app.EntityModel.CoreModel
{
    [Table("CostCenter", Schema = "new")]
    public sealed class CostCenter : BaseEntity
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
    }
}
