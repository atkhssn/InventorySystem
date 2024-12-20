using System.ComponentModel.DataAnnotations.Schema;

namespace app.EntityModel.CoreModel
{
    [Table("Transactions", Schema = "New")]
    public sealed class Transactions
    {
        public string Id { get; set; }
        [ForeignKey(nameof(Vouchers))]
        public long VouchersId { get; set; }
        public Vouchers Vouchers { get; set; }
        [ForeignKey(nameof(ChartOfAccounts))]
        public string AccountCode { get; set; }
        public ChartOfAccounts ChartOfAccounts { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string TransactionBy { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
