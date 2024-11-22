using System.ComponentModel.DataAnnotations.Schema;

namespace app.EntityModel.CoreModel
{
    [Table("VouchersLines", Schema = "New")]
    public sealed class VouchersLines : BaseEntity
    {
        public long VouchersId { get; set; }
        public Vouchers Vouchers { get; set; }
        public long GlHeadId { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CeditAmount { get; set; }
        public string Particular { get; set; }
    }
}
