namespace app.Services.AccountingReport
{
    public interface IAccountingReportService
    {
        Task<AccountingReportViewModel> GetTrialBalanceReportAsync();
    }
}
