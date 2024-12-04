using app.Services;
using app.Services.Accounting;
using app.Services.Vouchern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class VoucherController : Controller
    {
        private readonly IVoucherServices _voucherServices;
        private readonly IAccountingService _accoutingServices;
        public VoucherController(IVoucherServices voucherServices, IAccountingService accountingService)
        {
            _voucherServices = voucherServices;
            _accoutingServices = accountingService;
        }

        #region Voucher type

        [HttpGet]
        public async Task<IActionResult> VoucherTypes()
        {
            var response = await _voucherServices.VoucherTypesAsync();
            ViewBag.Message = TempData["Message"]?.ToString();
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> AddVoucherType()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> AddVoucherType(VoucherTypesViewModel voucherTypesViewModel)
        {
            var response = await _voucherServices.AddVoucherTypeAsync(voucherTypesViewModel);
            if (response.ResponseCode == 200)
            {
                TempData["Message"] = response.ResponseMessage;
                return await Task.Run(() => RedirectToAction("VoucherTypes"));
            }
            voucherTypesViewModel.ResponseViewModel = response;
            return await Task.Run(() => View(voucherTypesViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateVoucherType(long id)
        {
            var response = await _voucherServices.VoucherTypeAsync(id);
            return await Task.Run(() => View(response));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVoucherType(VoucherTypesViewModel voucherTypesViewModel)
        {
            var response = await _voucherServices.UpdateVoucherTypeAync(voucherTypesViewModel);
            if (response.ResponseCode == 200)
            {
                TempData["Message"] = response.ResponseMessage;
                return await Task.Run(() => RedirectToAction("VoucherTypes"));
            }
            voucherTypesViewModel.ResponseViewModel = response;
            return await Task.Run(() => View(voucherTypesViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCostCenter(long id)
        {
            var response = await _voucherServices.DeleteVoucherTypeAync(id);
            if (response.ResponseCode == 200)
                TempData["Message"] = response.ResponseMessage;
            return await Task.Run(() => RedirectToAction("VoucherTypes"));
        }

        #endregion

        #region Voucher

        [HttpGet]
        public async Task<IActionResult> Vouchers()
        {
            var response = await _voucherServices.VouchersAsync();
            ViewBag.Response = TempData["Response"]?.ToString();
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> DetailVoucher(long id)
        {
            VouchersViewModel response = new VouchersViewModel();
            response = await _voucherServices.VoucherAsync(id);
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> AddVoucher(long id = 0)
        {
            ViewBag.Response = TempData["Response"]?.ToString();
            VouchersViewModel response = new VouchersViewModel();
            if (id.Equals(0))
            {
                response.VoucherTypesViewModel = await _voucherServices.VoucherTypesAsync();
                response.CostCenterViewModel = await _accoutingServices.CostCentersAsync();
            }
            else
            {
                response = await _voucherServices.VoucherAsync(id);
                if (response is null)
                {
                    VouchersViewModel nullResponse = new VouchersViewModel
                    {
                        VoucherTypesViewModel = await _voucherServices.VoucherTypesAsync(),
                        CostCenterViewModel = await _accoutingServices.CostCentersAsync(),
                        ResponseViewModel = new ResponseViewModel
                        {
                            ResponseCode = 404,
                            ResponseMessage = "Nothing found or Invalid Voucher Id."
                        }
                    };
                    TempData["Response"] = JsonConvert.SerializeObject(nullResponse.ResponseViewModel);
                    return await Task.Run(() => RedirectToAction("AddVoucher", new { id = 0 }));
                }
            }
            return await Task.Run(() => View(response));
        }

        [HttpPost]
        public async Task<IActionResult> AddVoucher(VouchersViewModel vouchersViewModel)
        {
            if (vouchersViewModel.Id.Equals(0))
            {
                VouchersViewModel response = await _voucherServices.AddVoucherAsync(vouchersViewModel);
                TempData["Response"] = JsonConvert.SerializeObject(response.ResponseViewModel);
                return await Task.Run(() => RedirectToAction("AddVoucher", new { Id = response.Id }));
            }
            else
            {
                VouchersViewModel response = await _voucherServices.AddVoucherLineAsync(vouchersViewModel);
                TempData["Response"] = JsonConvert.SerializeObject(response.ResponseViewModel);
                return await Task.Run(() => RedirectToAction("AddVoucher", new { Id = response.Id }));
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitVoucher(VouchersViewModel vouchersViewModel)
        {
            var response = await _voucherServices.MakeSubmitVoucherAync(vouchersViewModel.Id);
            TempData["Response"] = JsonConvert.SerializeObject(response);
            return await Task.Run(() => RedirectToAction("Vouchers"));
        }

        [HttpGet]
        public async Task<IActionResult> SearchVouchers(int? voucherType, int? costCenter, string fromDate, string toDate)
        {
            SearchVoucherViewModel response = new SearchVoucherViewModel
            {

                FromDate = fromDate is null ? null : Convert.ToDateTime(fromDate),
                ToDate = toDate is null ? null : Convert.ToDateTime(toDate),
                VoucherTypesId = voucherType,
                CostCentersId = costCenter
            };

            if (response.VoucherTypesId.HasValue || response.CostCentersId.HasValue || response.FromDate.HasValue || response.ToDate.HasValue)
            {
                response = await _voucherServices.SearchVoucherAsync(response);
            }

            response.VoucherTypesViewModel = await _voucherServices.VoucherTypesAsync();
            response.CostCenterViewModel = await _accoutingServices.CostCentersAsync();

            return await Task.Run(() => View(response));
        }


        [HttpPost]
        public async Task<IActionResult> SearchVouchers(SearchVoucherViewModel searchVoucherViewModel)
        {
            return await Task.Run(() => RedirectToAction("SearchVouchers", new
            {
                voucherType = searchVoucherViewModel.VoucherTypesId is null ? null : searchVoucherViewModel.VoucherTypesId,
                costCenter = searchVoucherViewModel.CostCentersId is null ? null : searchVoucherViewModel.CostCentersId,
                fromDate = searchVoucherViewModel.FromDate is null ? null : searchVoucherViewModel.FromDate,
                toDate = searchVoucherViewModel.ToDate is null ? null : searchVoucherViewModel.ToDate
            }));
        }

        #endregion

        #region Voucher Approve

        [HttpGet]
        public async Task<IActionResult> PendingVouchers()
        {
            var response = await _voucherServices.UnapprovedVoucherList();
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> ApprovedVoucher(string voucherNo)
        {
            var response = await _voucherServices.MakeApproveVoucherAsync(voucherNo);
            TempData["Response"] = JsonConvert.SerializeObject(response);
            return await Task.Run(() => RedirectToAction("Vouchers"));
        }
        #endregion

    }
}
