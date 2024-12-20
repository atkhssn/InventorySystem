namespace app.Services.AccountingReport
{
    public interface IAccountingReportService
    {
        Task<AccountingReportViewModel> GetTrialBalanceReportAsync(AccountingReportViewModel model);
        Task<AccountingReportViewModel> GetReceivableReportAsync(AccountingReportViewModel model);
    }
}
