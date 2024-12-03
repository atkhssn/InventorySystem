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
        public async Task<IActionResult> ChartOfAccounts()
        {
            return await Task.Run(() => View());
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
            var response = await _accountingService.CostCentersAsync();
            ViewBag.Message = TempData["Message"]?.ToString();
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> CostCenterDetail(long id)
        {
            var response = await _accountingService.CostCenterAync(id);
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> AddCostCenter()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> AddCostCenter(CostCentersViewModel costCenterViewModel)
        {
            var response = await _accountingService.AddCostCenterAsync(costCenterViewModel);
            if (response.ResponseCode == 200)
            {
                TempData["Message"] = response.ResponseMessage;
                return await Task.Run(() => RedirectToAction("CostCenters"));
            }
            costCenterViewModel.ResponseViewModel = response;
            return await Task.Run(() => View(costCenterViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCostCenter(long id)
        {
            var response = await _accountingService.CostCenterAync(id);
            return await Task.Run(() => View(response));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCostCenter(CostCentersViewModel costCenterViewModel)
        {
            var response = await _accountingService.UpdateCostCenterAync(costCenterViewModel);
            if (response.ResponseCode == 200)
            {
                TempData["Message"] = response.ResponseMessage;
                return await Task.Run(() => RedirectToAction("CostCenters"));
            }
            costCenterViewModel.ResponseViewModel = response;
            return await Task.Run(() => View(costCenterViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCostCenter(long id)
        {
            var response = await _accountingService.DeleteCostCenterAync(id);
            if (response.ResponseCode == 200)
                TempData["Message"] = response.ResponseMessage;
            return await Task.Run(() => RedirectToAction("CostCenters"));
        }

        #endregion

    }
}
