using app.Services;
using app.Services.Accounting;
using app.Utility;
using app.Utility.DtoModel;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class AccountingController : Controller
    {
        private readonly IAccountingService _accountingService;
        private readonly string[] _allowedFileExtensions = { ".csv" };
        private readonly string _cashAccountCode = "1011011011";
        private readonly string _bankAccountCode = "1011011012";

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

        #endregion

        #region Cost Center

        [HttpGet]
        public async Task<IActionResult> CostCenters()
        {
            var response = await _accountingService.CostCentersAsync();
            ViewBag.Response = TempData["Response"]?.ToString();
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
            if (response.ResponseCode.Equals(200))
            {
                TempData["Response"] = JsonConvert.SerializeObject(response);
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
            if (response.ResponseCode.Equals(200))
            {
                TempData["Response"] = JsonConvert.SerializeObject(response);
                return await Task.Run(() => RedirectToAction("CostCenters"));
            }
            costCenterViewModel.ResponseViewModel = response;
            return await Task.Run(() => View(costCenterViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCostCenter(long id)
        {
            var response = await _accountingService.DeleteCostCenterAync(id);
            TempData["Response"] = JsonConvert.SerializeObject(response);
            return await Task.Run(() => RedirectToAction("CostCenters"));
        }

        #endregion

        //Send json for voucher account code
        [HttpGet]
        public async Task<IActionResult> GetChartOfAccountGlLayer(string term, long typeId = 0)
        {
            string parentCode = "0";

            if (typeId.Equals((long)NewVoucherTypes.BankPayment) || typeId.Equals((long)NewVoucherTypes.BankRecieve)) parentCode = _bankAccountCode;

            if (typeId.Equals((long)NewVoucherTypes.CashPayment) || typeId.Equals((long)NewVoucherTypes.CashRecieve)) parentCode = _cashAccountCode;

            var response = await _accountingService.GetGLAccountHeadAsync(parentCode);

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

        //CoA Bulk Upload
        [HttpPost]
        public async Task<IActionResult> CoABulkUpload(ChartOfAccountsViewModel chartOfAccountsViewModel)
        {
            IFormFile file = chartOfAccountsViewModel.Attachment;
            var response = new ResponseViewModel();

            if (file is not null)
            {
                string fileExtention = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_allowedFileExtensions.Contains(fileExtention))
                {
                    response.ResponseCode = 400;
                    response.ResponseMessage = $"Invalid file extention {fileExtention}.";
                    TempData["Response"] = JsonConvert.SerializeObject(response);
                    return await Task.Run(() => RedirectToAction("ChartOfAccounts"));
                }

                if (file.Length >= 10 * 1024 * 1024)
                {
                    response.ResponseCode = 400;
                    response.ResponseMessage = $"Maximum allowed file size 10MB.";
                    TempData["Response"] = JsonConvert.SerializeObject(response);
                    return await Task.Run(() => RedirectToAction("ChartOfAccounts"));
                }

                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    using (var csv = new CsvReader(stream, CultureInfo.InvariantCulture))
                    {
                        try
                        {
                            var chartOfAccountList = new List<ChartOfAccoutDtoModel>();

                            csv.Read();
                            csv.ReadHeader();
                            chartOfAccountList = csv.GetRecords<ChartOfAccoutDtoModel>().ToList();

                            if (chartOfAccountList is not null)
                            {
                                response = await _accountingService.BulkUploadAccountHead(chartOfAccountList);

                                TempData["Response"] = JsonConvert.SerializeObject(response);
                                return await Task.Run(() => RedirectToAction("ChartOfAccounts"));
                            }
                            else
                            {
                                response.ResponseCode = 400;
                                response.ResponseMessage = "Invalid or corrupted file.";
                                TempData["Response"] = JsonConvert.SerializeObject(response);
                                return await Task.Run(() => RedirectToAction("ChartOfAccounts"));
                            }
                        }
                        catch (Exception ex)
                        {
                            response.ResponseCode = 500;
                            response.ResponseMessage = ex.Message.ToString();
                            TempData["Response"] = JsonConvert.SerializeObject(response);
                            return await Task.Run(() => RedirectToAction("ChartOfAccounts"));
                        }
                    }
                }
            }
            else
            {
                response.ResponseCode = 400;
                response.ResponseMessage = $"Invalid CSV file.";
                TempData["Response"] = JsonConvert.SerializeObject(response);
                return await Task.Run(() => RedirectToAction("ChartOfAccounts"));
            }
        }

    }
}
