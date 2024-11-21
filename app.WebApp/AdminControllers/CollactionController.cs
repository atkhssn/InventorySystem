using app.Services.Voucher_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class CollactionController : Controller
    {
        private readonly IVoucherService voucherService;
        public CollactionController(IVoucherService voucherService)
        {
            this.voucherService = voucherService;
        }

        public async Task<ActionResult> Index(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await voucherService.GetPagedListAsync(page, pagesize, stringsearch);
            return View(results);
        }
        [HttpGet]
        public async Task<ActionResult> GetPaged(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await voucherService.GetPagedListAsync(page, pagesize, stringsearch);
            return PartialView("collactionvoucher", results);
        }

        public async Task<IActionResult> CustomerCollaction()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            VoucherViewModel viewModel = new VoucherViewModel();
            viewModel.VoucherDate = BaTime;
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CustomerCollaction(VoucherViewModel model)
        {
            var res = await voucherService.AddPurchaseVoucher(model);
            return RedirectToAction("Customer_Payment_Details", new { id = res });
        }
        public async Task<IActionResult> Customer_Payment_Details(long id)
        {
            var result = await voucherService.DetailsVoucher(id);
            return View(result);
        }
        public async Task<IActionResult> Customer_Payment_Details_Report(long id)
        {
            var result = await voucherService.DetailsVoucher(id);
            return View(result);
        }


    }
}
