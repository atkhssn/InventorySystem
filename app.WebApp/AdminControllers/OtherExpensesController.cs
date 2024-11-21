using app.Services.Vendor_Service;
using app.Services.Voucher_Service;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    public class OtherExpensesController : Controller
    {
        private readonly IVoucherService voucherService;
        public OtherExpensesController(IVoucherService voucherService)
        {
            this.voucherService = voucherService;
        }


        public async Task<ActionResult> Index(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await voucherService.GetOtherExpensesPagedListAsync(page, pagesize, stringsearch, 12);
            return View(results);
        }
        [HttpGet]
        public async Task<ActionResult> GetPaged(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await voucherService.GetOtherExpensesPagedListAsync(page, pagesize, stringsearch, 12);
            return PartialView("PartialotherExpensesVoucher", results);
        }


        [HttpGet]
        public async Task<IActionResult> AddRecord()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            VoucherViewModel viewModel = new VoucherViewModel();
            viewModel.VoucherDate = BaTime;         
            viewModel.VoucherTypeId = 12;
            viewModel.VoucherTypeCode ="OEXP";
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(VoucherViewModel viewModel)
        {
            var result= await voucherService.OtherExpenses(viewModel);
            return RedirectToAction("ExpensesDetalis", new { id = result });
        }

        [HttpGet]
        public async Task<IActionResult> ExpensesDetalis(long id)
        {
            var result = await voucherService.OtherExpensesVoucher(id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> ExpensesDetalisReport(long id)
        {
            var result = await voucherService.OtherExpensesVoucher(id);
            return View(result);
        }
    }
}
