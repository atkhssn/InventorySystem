using app.Services.Accounting;
using app.Services.AccountingReport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class AccountingReportController : Controller
    {
        private readonly IAccountingService _accountingService;
        private readonly IAccountingReportService _accountingReportService;

        public AccountingReportController(IAccountingService accountingService, IAccountingReportService accountingReportService)
        {
            _accountingService = accountingService;
            _accountingReportService = accountingReportService;
        }

        [HttpGet]
        public async Task<IActionResult> TrailBalanceReport(long? costCenter, string fromDate, string toDate)
        {
            var response = new AccountingReportViewModel
            {
                CostCentersId = costCenter,
                FromDate = fromDate is null ? null : Convert.ToDateTime(fromDate),
                ToDate = toDate is null ? null : Convert.ToDateTime(toDate)
            };

            if (response.CostCentersId.HasValue || response.FromDate.HasValue || response.ToDate.HasValue)
            {
                response = await _accountingReportService.GetTrialBalanceReportAsync(response);
            }
            response.CostCentersViewModel = await _accountingService.CostCentersAsync();
            return await Task.Run(() => View(response));
        }

        [HttpPost]
        public async Task<IActionResult> SearchTrialBalance(AccountingReportViewModel accountingReportViewModel)
        {
            return await Task.Run(() => RedirectToAction("TrailBalanceReport", new
            {
                costCenter = accountingReportViewModel.CostCentersId is null ? 0 : accountingReportViewModel.CostCentersId,
                fromDate = accountingReportViewModel.FromDate is null ? null : accountingReportViewModel.FromDate,
                toDate = accountingReportViewModel.ToDate is null ? null : accountingReportViewModel.ToDate
            }));
        }

        [HttpGet]
        public async Task<IActionResult> BalanceSheetReport()
        {
            return await Task.Run(() => View());
        }

        [HttpGet]
        public async Task<IActionResult> CashBookReport(long? costCenter, string accountCode, string fromDate, string toDate)
        {
            var response = new AccountingReportViewModel
            {
                CostCentersId = costCenter,
                SearchAccountCode = accountCode,
                FromDate = fromDate is null ? null : Convert.ToDateTime(fromDate),
                ToDate = toDate is null ? null : Convert.ToDateTime(toDate)
            };

            if (response.CostCentersId.HasValue || response.SearchAccountCode is not null || response.FromDate.HasValue || response.ToDate.HasValue)
            {
                response = await _accountingReportService.GetCashBookReportAsync(response);
            }
            response.CostCentersViewModel = await _accountingService.CostCentersAsync();
            response.ChartOfAccountsViewModel = await _accountingService.CashBookAccountHeadsAsync();
            return await Task.Run(() => View(response));
        }

        [HttpPost]
        public async Task<IActionResult> SearchCashBook(AccountingReportViewModel accountingReportViewModel)
        {
            return await Task.Run(() => RedirectToAction("CashBookReport", new
            {
                costCenter = accountingReportViewModel.CostCentersId is null ? 0 : accountingReportViewModel.CostCentersId,
                accountCode = accountingReportViewModel.SearchAccountCode is null ? "0" : accountingReportViewModel.SearchAccountCode,
                fromDate = accountingReportViewModel.FromDate is null ? null : accountingReportViewModel.FromDate,
                toDate = accountingReportViewModel.ToDate is null ? null : accountingReportViewModel.ToDate
            }));
        }

        [HttpGet]
        public async Task<IActionResult> BankBookReport(long? costCenter, string accountCode, string fromDate, string toDate)
        {
            var response = new AccountingReportViewModel
            {
                CostCentersId = costCenter,
                SearchAccountCode = accountCode,
                FromDate = fromDate is null ? null : Convert.ToDateTime(fromDate),
                ToDate = toDate is null ? null : Convert.ToDateTime(toDate)
            };

            if (response.CostCentersId.HasValue || response.SearchAccountCode is not null || response.FromDate.HasValue || response.ToDate.HasValue)
            {
                response = await _accountingReportService.GetBankBookReportAsync(response);
            }
            response.CostCentersViewModel = await _accountingService.CostCentersAsync();
            response.ChartOfAccountsViewModel = await _accountingService.BankBookAccountHeadsAsync();
            return await Task.Run(() => View(response));
        }

        [HttpPost]
        public async Task<IActionResult> SearchBankBook(AccountingReportViewModel accountingReportViewModel)
        {
            return await Task.Run(() => RedirectToAction("BankBookReport", new
            {
                costCenter = accountingReportViewModel.CostCentersId is null ? 0 : accountingReportViewModel.CostCentersId,
                accountCode = accountingReportViewModel.SearchAccountCode is null ? "0" : accountingReportViewModel.SearchAccountCode,
                fromDate = accountingReportViewModel.FromDate is null ? null : accountingReportViewModel.FromDate,
                toDate = accountingReportViewModel.ToDate is null ? null : accountingReportViewModel.ToDate
            }));
        }

        [HttpGet]
        public async Task<IActionResult> ReceivableReport(long? costCenter, string accountCode, string fromDate, string toDate)
        {
            var response = new AccountingReportViewModel
            {
                CostCentersId = costCenter,
                SearchAccountCode = accountCode,
                FromDate = fromDate is null ? null : Convert.ToDateTime(fromDate),
                ToDate = toDate is null ? null : Convert.ToDateTime(toDate)
            };

            if (response.CostCentersId.HasValue || response.SearchAccountCode is not null || response.FromDate.HasValue || response.ToDate.HasValue)
            {
                response = await _accountingReportService.GetReceivableReportAsync(response);
            }
            response.CostCentersViewModel = await _accountingService.CostCentersAsync();
            response.ChartOfAccountsViewModel = await _accountingService.ReceivableAccountHeadsAsync();
            return await Task.Run(() => View(response));
        }

        [HttpPost]
        public async Task<IActionResult> SearchReceivable(AccountingReportViewModel accountingReportViewModel)
        {
            return await Task.Run(() => RedirectToAction("ReceivableReport", new
            {
                costCenter = accountingReportViewModel.CostCentersId is null ? 0 : accountingReportViewModel.CostCentersId,
                accountCode = accountingReportViewModel.SearchAccountCode is null ? "0" : accountingReportViewModel.SearchAccountCode,
                fromDate = accountingReportViewModel.FromDate is null ? null : accountingReportViewModel.FromDate,
                toDate = accountingReportViewModel.ToDate is null ? null : accountingReportViewModel.ToDate
            }));
        }

        [HttpGet]
        public async Task<IActionResult> PayableReport(long? costCenter, string accountCode, string fromDate, string toDate)
        {
            var response = new AccountingReportViewModel
            {
                CostCentersId = costCenter,
                SearchAccountCode = accountCode,
                FromDate = fromDate is null ? null : Convert.ToDateTime(fromDate),
                ToDate = toDate is null ? null : Convert.ToDateTime(toDate)
            };

            if (response.CostCentersId.HasValue || response.SearchAccountCode is not null || response.FromDate.HasValue || response.ToDate.HasValue)
            {
                response = await _accountingReportService.GetPayableReportAsync(response);
            }
            response.CostCentersViewModel = await _accountingService.CostCentersAsync();
            response.ChartOfAccountsViewModel = await _accountingService.PayableAccountHeadsAsync();
            return await Task.Run(() => View(response));
        }

        [HttpPost]
        public async Task<IActionResult> SearchPayable(AccountingReportViewModel accountingReportViewModel)
        {
            return await Task.Run(() => RedirectToAction("PayableReport", new
            {
                costCenter = accountingReportViewModel.CostCentersId is null ? 0 : accountingReportViewModel.CostCentersId,
                accountCode = accountingReportViewModel.SearchAccountCode is null ? "0" : accountingReportViewModel.SearchAccountCode,
                fromDate = accountingReportViewModel.FromDate is null ? null : accountingReportViewModel.FromDate,
                toDate = accountingReportViewModel.ToDate is null ? null : accountingReportViewModel.ToDate
            }));
        }
    }
}
