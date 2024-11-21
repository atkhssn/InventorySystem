using app.Services.Accounting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class AccountingController : Controller
    {
        private readonly IAccountingService _accountingService;
        public AccountingController(IAccountingService accountingService)
        {
            _accountingService = accountingService;
        }

        #region Chart of Account

        [HttpGet]
        public IActionResult ChartOfAccounts()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchChartOfAccounts(string costConeter)
        {
            return Json(new { });
        }

        [HttpPost]

        #endregion

        #region Cost Center

        [HttpGet]
        public async Task<IActionResult> CostCenters()
        {
            var response = await _accountingService.GetAllRecordAsync();
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> CostCenterDetail(long id)
        {
            var response = await _accountingService.GetRecordDetailAync(id);
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> AddCostCenter()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> AddCostCenter(CostCenterViewModel costCenterViewModel)
        {
            var response = await _accountingService.AddRecordAsync(costCenterViewModel);
            return RedirectToAction("CostCenters");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCostCenter(long id)
        {
            var response = await _accountingService.GetRecordDetailAync(id);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCostCenter(CostCenterViewModel costCenterViewModel)
        {
            var response = await _accountingService.UpdateRecordAync(costCenterViewModel);
            return RedirectToAction("CostCenters");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCostCenter(long id)
        {
            var response = await _accountingService.DeleteRecordAync(id);
            return RedirectToAction("CostCenters");
        }

        #endregion

    }
}
