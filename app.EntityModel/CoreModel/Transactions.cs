using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.EntityModel.CoreModel
{
    [Table("Transactions", Schema = "New")]
    public sealed class Transactions
    {
        [Key]
        public string Id { get; set; }
        public long VouchersId { get; set; }
        public Vouchers Vouchers { get; set; }
        [ForeignKey(nameof(ChartOfAccounts))]
        public string AccountCode { get; set; }
        public ChartOfAccounts ChartOfAccounts { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal Balance { get; set; }
        public bool IsSuccess { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }
    }
}
