using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class VoucherController : Controller
    {
        #region Voucher type

        [HttpGet]
        public IActionResult VoucherTypes()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VoucherTypeDetail(long id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddVoucherType()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateVoucherType()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeleteVoucherType(long id)
        {
            return View();
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
