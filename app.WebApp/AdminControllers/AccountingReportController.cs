using app.Services.AccountingReport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class AccountingReportController : Controller
    {
        private readonly IAccountingReportService _accountingReportService;
        public AccountingReportController(IAccountingReportService accountingReportService)
        {
            _accountingReportService = accountingReportService;
        }

        [HttpGet]
        public async Task<IActionResult> TrailBalanceReport()
        {
            var response = await _accountingReportService.GetTrialBalanceReportAsync();
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> BalanceSheetReport()
        {
            return await Task.Run(() => View());
        }

        [HttpGet]
        public async Task<IActionResult> CashBookReport()
        {
            return await Task.Run(() => View());
        }

        [HttpGet]
        public async Task<IActionResult> BankBookReport()
        {
            return await Task.Run(() => View());
        }

        [HttpGet]
        public async Task<IActionResult> ReceivableReport()
        {
            return await Task.Run(() => View());
        }

        [HttpGet]
        public async Task<IActionResult> PayableReport()
        {
            return await Task.Run(() => View());
        }
    }
}
