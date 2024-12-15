using app.Services.Accounting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            var response = await _accountingService.ChartOfAccoutingsAsync();
            var hierarchicalData = BuildHierarchy(response.ChartOfAccountsViewModels, "0");
            ViewBag.JsonData = System.Text.Json.JsonSerializer.Serialize(hierarchicalData);
            ViewBag.Response = TempData["Response"]?.ToString();
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountHead(ChartOfAccountsViewModel chartOfAccountsViewModel)
        {
            var response = await _accountingService.AddAccountHeadAsync(chartOfAccountsViewModel);
            TempData["Response"] = JsonConvert.SerializeObject(response);
            return await Task.Run(() => RedirectToAction("ChartOfAccounts"));
        }

        [HttpPost]
        public async Task<IActionResult> EditAccountHead(ChartOfAccountsViewModel chartOfAccountsViewModel)
        {
            var response = await _accountingService.UpdateAccountHeadAsync(chartOfAccountsViewModel);
            TempData["Response"] = JsonConvert.SerializeObject(response);
            return await Task.Run(() => RedirectToAction("ChartOfAccounts"));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccountHead(ChartOfAccountsViewModel chartOfAccountsViewModel)
        {
            var response = await _accountingService.DeleteAccountHeadAsync(chartOfAccountsViewModel.AccountCode);
            TempData["Response"] = JsonConvert.SerializeObject(response);
            return await Task.Run(() => RedirectToAction("ChartOfAccounts"));
        }

        [HttpGet]
        public async Task<IActionResult> TrailBalance()
        {
            return await Task.Run(() => View());
        }

        [HttpGet]
        public async Task<IActionResult> BalanceSheet()
        {
            return await Task.Run(() => View());
        }

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

        //Send json for voucher account code
        [HttpGet]
        public async Task<IActionResult> GetChartOfAccountGlLayer(string term)
        {
            var response = await _accountingService.GetGLAcoountHeadAsync();

            var suggestions = response
                .Where(item => item.text.Contains(term, StringComparison.OrdinalIgnoreCase) || item.id.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(item => new { id = item.id, label = item.text })
                .Take(10)
                .ToList();
            return Json(suggestions);
        }

        //Hierarchy for js tree
        private List<ChartOfAccountHierarchy> BuildHierarchy(List<ChartOfAccountsViewModel> nodes, string parentCode)
        {
            return nodes
                .Where(x => x.ParentAccountCode == parentCode)
                .Select(x => new ChartOfAccountHierarchy
                {
                    id = x.AccountCode,
                    text = $"[{x.AccountCode}] - {x.AccountName}",
                    children = BuildHierarchy(nodes, x.AccountCode)
                }).ToList();
        }

    }
}
