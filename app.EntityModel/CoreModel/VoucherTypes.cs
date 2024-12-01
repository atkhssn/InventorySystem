using System.ComponentModel.DataAnnotations.Schema;

namespace app.EntityModel.CoreModel
{
    [Table("Vouchertypes", Schema = "New")]
    public sealed class VoucherTypes : BaseEntity
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
    }
}
