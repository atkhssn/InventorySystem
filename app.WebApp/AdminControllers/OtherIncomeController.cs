using app.Services.Voucher_Service;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    public class OtherIncomeController : Controller
    {
        private readonly IVoucherService voucherService;
        public OtherIncomeController(IVoucherService voucherService)
        {
            this.voucherService = voucherService;
        }


        public async Task<ActionResult> Index(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await voucherService.GetOtherExpensesPagedListAsync(page, pagesize, stringsearch, 13);
            return View(results);
        }
        [HttpGet]
        public async Task<ActionResult> GetPaged(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await voucherService.GetOtherExpensesPagedListAsync(page, pagesize, stringsearch, 13);
            return PartialView("PartialOtherIncomeVoucher", results);
        }


        [HttpGet]
        public async Task<IActionResult> AddRecord()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            VoucherViewModel viewModel = new VoucherViewModel();
            viewModel.VoucherDate = BaTime;
            viewModel.VoucherTypeId = 13;
            viewModel.VoucherTypeCode = "OINC";
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(VoucherViewModel viewModel)
        {
            var result = await voucherService.OtherIncomeVoucher(viewModel);
            return RedirectToAction("IncomeDetalis", new { id = result });
        }

        [HttpGet]
        public async Task<IActionResult> IncomeDetalis(long id)
        {
            var result = await voucherService.OtherExpensesVoucher(id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> IncomeDetalisReport(long id)
        {
            var result = await voucherService.OtherExpensesVoucher(id);
            return View(result);
        }
    }
}
