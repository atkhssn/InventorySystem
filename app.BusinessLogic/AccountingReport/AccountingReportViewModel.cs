using System.ComponentModel.DataAnnotations;

namespace app.Services.AccountingReport
{
    public sealed class AccountingReportViewModel
    {
        [Display(Name = "Transaction")]
        public string TransactionId { get; set; }

        [Display(Name = "Voucher")]
        public string VoucherNo { get; set; }

        [Display(Name = "Account Code")]
        public string AccountCode { get; set; }

        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Display(Name = "Debit Amount")]
        public decimal DebitAmount { get; set; }

        [Display(Name = "Credit Amount")]
        public decimal CreditAmount { get; set; }

        [Display(Name = "Total Debit Amount")]
        public decimal TotalDebitAmount { get; set; }

        [Display(Name = "Total Credit Amount")]
        public decimal TotalCreditAmount { get; set; }

        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        public List<AccountingReportViewModel> accountingReportViewModels { get; set; }
    }
}
