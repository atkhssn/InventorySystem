using app.EntityModel.DatabaseView;
using app.Services.DropDownServices;
using app.Services.PurchaseOrder_Services;
using app.Services.Report_service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportService reportService;
        private readonly IDropDownService dropDownService;
        public ReportController(IReportService reportService, IDropDownService dropDownService)
        {
            this.reportService = reportService;
            this.dropDownService = dropDownService;
        }
        public async Task<IActionResult> SupplierGeneralledger()
        {
            return View();
        }
        public async Task<IActionResult> SupplierGeneralledgerReport(long id)
        {
            var result = await reportService.Generalledger(id);
            return View(result);
        }

        public async Task<IActionResult> CustomerGeneralledger()
        {
            return View();
        }
        public async Task<IActionResult> CustomerGeneralledgerReport(long id)
        {
            var result = await reportService.Generalledger(id);
            return View(result);
        }
        [HttpGet]
        public async Task<JsonResult> productbyCompany(long companyid)
        {
            var company = await dropDownService.companyUserproductlist(companyid);
            return Json(company);
        }
        public async Task<IActionResult> SalesReport()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            ReportsViewModel model = new ReportsViewModel();
            model.StartDate = BaTime;
            model.EndDate = BaTime;
            model.VoucherCode = "SV";
            return View(model);
        }

        public async Task<IActionResult> SalesReportPrintView(ReportsViewModel model)
        {
            var res = await reportService.SalesReport(model);
            return View(res);
        }

        public async Task<IActionResult> PurchesReport()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            ReportsViewModel model = new ReportsViewModel();
            model.StartDate = BaTime;
            model.EndDate = BaTime;
            model.VoucherCode = "SV";
            return View(model);
        }

        public async Task<IActionResult> PurchesReportPrintView(ReportsViewModel model)
        {
            var res = await reportService.PurchesReport(model);
            return View(res);
        }

        public async Task<IActionResult> ProfitLoss()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            ReportsViewModel model = new ReportsViewModel();
            model.StartDate = BaTime;
            model.EndDate = BaTime;
            return View(model);
        }

        public async Task<IActionResult> ProfitLossPrintView(ReportsViewModel model)
        {
            var res = await reportService.ProfitLoss(model);
            ProfitandLossStatement profitandLossStatement = new ProfitandLossStatement();
            profitandLossStatement = res.FirstOrDefault(d=>d.Serialno==1);
            return View(profitandLossStatement);
        }


    }
}
