using app.Services.DropDownServices;
using app.Services.PurchaseFinalized_Services;
using app.Services.PurchaseOrder_Services;
using app.Services.Sales_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly ISalesService salesService;
        private readonly IDropDownService dropDownService;
        private readonly IPurchaseFinalizedServices purchaseFinalizedServices;
        public SalesController(ISalesService salesService,
            IDropDownService dropDownService,
            IPurchaseFinalizedServices purchaseFinalizedServices)
        {
            this.salesService = salesService;
            this.dropDownService = dropDownService;
            this.purchaseFinalizedServices = purchaseFinalizedServices;
        }
        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await salesService.GetPagedListAsync(page, pagesize, stringsearch);
            return View(results);
        }
        [HttpGet]
        public async Task<ActionResult> GetPaged(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await salesService.GetPagedListAsync(page, pagesize, stringsearch);
            return PartialView("partialSales", results);
        }


        [HttpGet]
        public async Task<IActionResult> AddRecord()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            SalesViewModel viewModel = new SalesViewModel();
            viewModel.SalesDate = BaTime;
            viewModel.DeliveryDate = BaTime;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(SalesViewModel viewModel)
        {
            if (viewModel.MappVm==null)
            {
                ModelState.AddModelError(string.Empty, "Product Item Zero!");
                return View(viewModel);
            }
            var result = await salesService.AddSalesOrder(viewModel);
            if (result > 0)
            {
                return RedirectToAction("Detalis", new { id = result });
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalis(long id)
        {
            var result = await salesService.GetSaleOrder(id);
            return View(result);
        } 

        [HttpGet]
        public async Task<IActionResult> SalesDetailReport(long id)
        {
            var result = await salesService.GetSaleOrder(id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> SaleOrderDetailReport(long id)
        {
            var result = await salesService.GetSaleOrder(id);
            return View(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetAutoCompleteSupplierGet()
        {
            var products = await dropDownService.vendorlist(1);
            return Json(products);
        }

        [HttpGet]
        public async Task<JsonResult> GetAutoCompleteProductGet()
        {
            var products = await dropDownService.productlist();
            return Json(products);
        }
        [HttpGet]
        public async Task<JsonResult> ProductGet(long id)
        {
            var products = await dropDownService.sigleproductWithstock(id);
            return Json(products);
        } 
        [HttpGet]
        public async Task<JsonResult> customerGet(string mobile)
        {
            var customer = await dropDownService.sigleCustomer(mobile);
            return Json(customer);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRecord(long id)
        {
            var result = await salesService.GetSaleOrder(id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRecord(SalesViewModel viewModel)
        {
            var result = await salesService.UpdateSaleOrder(viewModel);
            if (result > 0)
            {
                return RedirectToAction("Detalis", new { id = result });
            }

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteSalesDetalis(long id)
        {
            var result = await salesService.DeleteSaleOrderDetalies(id);
            return RedirectToAction("UpdateRecord", new { id = result });
        }
        [HttpPost]
        public async Task<IActionResult> AddSalesDetalis(SalesOrderDetailsViewModel model)
        {
            var result = await salesService.AddSaleDetalies(model);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeStatus(long id)
        {
            var result = await purchaseFinalizedServices.GetSalesFinalizedAsync(id);
            return RedirectToAction("Index");
        }

    }
}
