using app.Services.Accounting;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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

        [Display(Name = "Cost Center")]
        public long? CostCentersId { get; set; }
        public CostCentersViewModel CostCentersViewModel { get; set; }

        [Display(Name = "From Date")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To Date")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Account Name")]
        [AllowNull]
        public string SearchAccountCode { get; set; }

        public bool IsOpening { get; set; } = false;

        public ChartOfAccountsViewModel ChartOfAccountsViewModel { get; set; }
        public List<AccountingReportViewModel> AccountingReportViewModels { get; set; }
        public List<AccountingReportViewModel> AccountingReportOpeningViewModels { get; set; }
        public List<AccountingReportViewModel> Asset { get; set; }
        public List<AccountingReportViewModel> Liability { get; set; }
        public List<AccountingReportViewModel> Equity { get; set; }
        public List<AccountingReportViewModel> Revenue { get; set; }
        public List<AccountingReportViewModel> Expense { get; set; }
    }
}
