using app.Services.Accounting;
using app.Services.Vouchern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> DetailVoucher(long id)
        {
            VouchersViewModel vouchersViewModel = new VouchersViewModel();
            vouchersViewModel = await _voucherServices.VoucherAsync(id);
            return await Task.Run(() => View(vouchersViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> AddVoucher(long id = 0)
        {
            VouchersViewModel viewModel = new VouchersViewModel();
            if (id.Equals(0))
            {
                viewModel.VoucherTypesViewModel = await _voucherServices.VoucherTypesAsync();
                viewModel.CostCenterViewModel = await _accoutingServices.CostCentersAsync();
            }
            else
            {
                viewModel = await _voucherServices.VoucherAsync(id);
            }
            return await Task.Run(() => View(viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> AddVoucher(VouchersViewModel vouchersViewModel)
        {
            if (vouchersViewModel.Id.Equals(0))
            {
                VouchersViewModel request = await _voucherServices.AddVoucherAsync(vouchersViewModel);
                return await Task.Run(() => RedirectToAction("AddVoucher", new { Id = request.Id }));
            }
            else
            {
                VouchersViewModel request = await _voucherServices.AddVoucherLineAsync(vouchersViewModel);
                return await Task.Run(() => RedirectToAction("AddVoucher", new { Id = request.Id }));
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitVoucher(VouchersViewModel vouchersViewModel)
        {
            var request = await _voucherServices.MakeSubmitVoucherAync(vouchersViewModel.Id);
            return await Task.Run(() => RedirectToAction("Vouchers"));
        }

        [HttpGet]
        public async Task<IActionResult> SearchVouchers(int? voucherType, int? costCenter, string fromDate, string toDate)
        {
            SearchVoucherViewModel viewModel = new SearchVoucherViewModel
            {

                FromDate = fromDate is null ? null : Convert.ToDateTime(fromDate),
                ToDate = toDate is null ? null : Convert.ToDateTime(toDate),
                VoucherTypesId = voucherType,
                CostCentersId = costCenter
            };

            if (voucherType.HasValue || costCenter.HasValue || viewModel.FromDate.HasValue || viewModel.ToDate.HasValue)
            {
                viewModel = await _voucherServices.SearchVoucherAsync(viewModel);
            }

            viewModel.VoucherTypesViewModel = await _voucherServices.VoucherTypesAsync();
            viewModel.CostCenterViewModel = await _accoutingServices.CostCentersAsync();

            return View(viewModel);
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
            var request = await _voucherServices.MakeApproveVoucherAsync(voucherNo);
            return await Task.Run(() => RedirectToAction("Vouchers"));
        }
        #endregion

    }
}
