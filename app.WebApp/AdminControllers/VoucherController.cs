using app.Services.Vouchern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class VoucherController : Controller
    {
        private readonly IVoucherServices _voucherServices;
        public VoucherController(IVoucherServices voucherServices)
        {
            _voucherServices = voucherServices;
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
        public async Task<IActionResult> VoucherTypeDetail(long id)
        {
            var response = await _voucherServices.VoucherTypeAsync(id);
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> AddVoucherType()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> AddVoucherType(VoucherTypesViewModel voucherTypesViewModel)
        {
            var request = await _voucherServices.AddVoucherTypeAsync(voucherTypesViewModel);
            if (request.ResponseCode == 200)
            {
                TempData["Message"] = request.ResponseMessage;
                return await Task.Run(() => RedirectToAction("VoucherTypes"));
            }
            voucherTypesViewModel.ResponseViewModel = request;
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
            var request = await _voucherServices.UpdateVoucherTypeAync(voucherTypesViewModel);
            if (request.ResponseCode == 200)
            {
                TempData["Message"] = request.ResponseMessage;
                return await Task.Run(() => RedirectToAction("VoucherTypes"));
            }
            voucherTypesViewModel.ResponseViewModel = request;
            return await Task.Run(() => View(voucherTypesViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCostCenter(long id)
        {
            var request = await _voucherServices.DeleteVoucherTypeAync(id);
            if (request.ResponseCode == 200)
                TempData["Message"] = request.ResponseMessage;
            return await Task.Run(() => RedirectToAction("VoucherTypes"));
        }

        #endregion

        #region Voucher

        [HttpGet]
        public IActionResult Vouchers()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DetailVoucher(long id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddVoucher()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeleteVoucher(long id)
        {
            return View();
        }

        #endregion

        #region Voucher Approve

        [HttpGet]
        public IActionResult PendingVouchers()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchVouchers()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ApprovalVoucherDetail(long id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult ApprovedVoucher(long id)
        {
            return Json(new { });
        }

        #endregion

    }
}
