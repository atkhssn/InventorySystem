namespace app.Services.AccountingReport
{
    public interface IAccountingReportService
    {
        Task<AccountingReportViewModel> GetTrialBalanceReportAsync(AccountingReportViewModel model);
        Task<AccountingReportViewModel> GetReceivableReportAsync(AccountingReportViewModel model);
        Task<AccountingReportViewModel> GetPayableReportAsync(AccountingReportViewModel model);
        Task<AccountingReportViewModel> GetCashBookReportAsync(AccountingReportViewModel model);
        Task<AccountingReportViewModel> GetBankBookReportAsync(AccountingReportViewModel model);
        Task<AccountingReportViewModel> GetGeneralLedgerReportAsync(AccountingReportViewModel model);
        Task<AccountingReportViewModel> GetProfitLossReportAsync(AccountingReportViewModel model);
    }
}
