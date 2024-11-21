using app.Services.DropDownServices;
using app.Services.PurchaseFinalized_Services;
using app.Services.PurchaseOrder_Services;
using app.Services.SalaesReturn_service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class SalesReturnsController : Controller
    {
        private readonly ISalesReturns salesReturns;
        private readonly IDropDownService dropDownService;
        private readonly IPurchaseFinalizedServices purchaseFinalizedServices;
        public SalesReturnsController(ISalesReturns salesReturns,
            IDropDownService dropDownService,
            IPurchaseFinalizedServices purchaseFinalizedServices)
        {
            this.salesReturns = salesReturns;
            this.dropDownService = dropDownService;
            this.purchaseFinalizedServices = purchaseFinalizedServices;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await salesReturns.GetPagedListAsync(page, pagesize, stringsearch);
            return View(results);
        }
        [HttpGet]
        public async Task<ActionResult> GetPaged(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await salesReturns.GetPagedListAsync(page, pagesize, stringsearch);
            return PartialView("_salesReturns", results);
        }


        [HttpGet]
        public async Task<IActionResult> AddRecord()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            SalesReturnViewModel viewModel = new SalesReturnViewModel();
            viewModel.SalesReturnDate = BaTime;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(SalesReturnViewModel viewModel)
        {
            var result = await salesReturns.AddSalesReturn(viewModel);
            if (result > 0)
            {
                return RedirectToAction("Detalis", new { id = result });
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalis(long id)
        {
            var result = await salesReturns.GetSalesReturn(id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> SalesReturnDetailReport(long id)
        {
            var result = await salesReturns.GetSalesReturn(id);
            return View(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetAutoCompleteSupplierGet()
        {
            var products = await dropDownService.vendorlist(1);
            return Json(products);
        }
        [HttpGet]
        public async Task<JsonResult> GetAutoCompleteCustomerGet()
        {
            var products = await dropDownService.vendorlist(2);
            return Json(products);
        }
        [HttpGet]
        public async Task<JsonResult> GetAutoCompleteProductGet()
        {
            var products = await dropDownService.productlist();
            return Json(products);
        }
        [HttpGet]
        public async Task<JsonResult> GetCustomer(long id)
        {
            var products = await dropDownService.sigleCustomerinfo(id);
            return Json(products);
        }
        [HttpGet]
        public async Task<JsonResult> ProductGet(long id)
        {
            var products = await dropDownService.sigleproduct(id);
            return Json(products);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeStatus(long id)
        {
            var result = await purchaseFinalizedServices.GetSalesReturnFinalizedAsync(id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> UpdateRecord(long id)
        {
            var result = await salesReturns.GetSalesReturn(id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> DeletepurchaseOrderDetalis(long id)
        {
            var result = await salesReturns.DeleteSalesReturnDetalies(id);
            return RedirectToAction("UpdateRecord", new { id = result });
        }
        [HttpPost]
        public async Task<IActionResult> AddSalesReturnDetalies(SalesReturnDetailsViewmodel model)
        {
            var result = await salesReturns.AddSalesReturnDetalies(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRecord(SalesReturnViewModel viewModel)
        {
            var result = await salesReturns.UpdateSalesReturn(viewModel);
            if (result > 0)
            {
                return RedirectToAction("Detalis", new { id = result });
            }

            return View(viewModel);
        }
    }
}
