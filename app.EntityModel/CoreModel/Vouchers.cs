using System.ComponentModel.DataAnnotations.Schema;

namespace app.EntityModel.CoreModel
{
    [Table("Vouchers", Schema = "New")]
    public sealed class Vouchers : BaseEntity
    {
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public long VoucherTypesId { get; set; }
        public VoucherTypes VoucherTypes { get; set; }
        public long CostCentersId { get; set; }
        public CostCenters CostCenters { get; set; }
        public decimal TotalDebitAmount { get; set; }
        public decimal TotalCreditAmount { get; set; }
        public string Narration { get; set; }
        public int Status { get; set; }
    }
}
