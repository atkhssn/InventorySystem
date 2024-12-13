using app.Services.Vouchern;
using System.ComponentModel.DataAnnotations;

namespace app.Services.Accounting
{
    public sealed class TransactionsViewModel
    {
        [Display(Name = "Transaction Id")]
        public string Id { get; set; }

        [Display(Name = "Voucher Id")]
        public long VouchersId { get; set; }

        [Display(Name = "Voucher No")]
        public string VoucherNo { get; set; }

        public VouchersViewModel VouchersViewModel { get; set; }

        [Display(Name = "Account Code")]
        public string AccountCode { get; set; }

        public ChartOfAccountsViewModel chartOfAccountsViewModel { get; set; }

        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Debit Amount")]
        public decimal DebitAmount { get; set; }

        [Display(Name = "Credit Amount")]
        public decimal CreditAmount { get; set; }

        [Display(Name = "Balance")]
        public decimal Balance { get; set; }

        [Display(Name = "Success")]
        public bool IsSuccess { get; set; }

        [Display(Name = "Requested By")]
        public string RequestedBy { get; set; }

        [Display(Name = "Requested On")]
        public DateTime RequestedOn { get; set; }

        public List<ChartOfAccountsViewModel> ChartOfAccountsViewModels { get; set; }
        public ResponseViewModel ResponseViewModel { get; set; }
    }
}
