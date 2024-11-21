using app.EntityModel.CoreModel;
using app.Services.ProductCategory_Services;
using app.Services.ProductSubCategory_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class ProductSubCategoryController : Controller
    {
        private readonly IProductSubCategoryService productSubCategoryService;
        private readonly IProductCategoryServices productCategoryServices;
        public ProductSubCategoryController(IProductSubCategoryService productSubCategoryService,
             IProductCategoryServices productCategoryServices)
        {
            this.productSubCategoryService = productSubCategoryService;
            this.productCategoryServices = productCategoryServices;
        }
        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int pagesize = 10, int ProductCategoryId = 0,string ProductType = null, string sarchString = null)
        {
            if (page < 1)
                page = 1;
            var results = await productSubCategoryService.GetPagedListAsync(page, pagesize, ProductCategoryId, ProductType, sarchString);
            ViewBag.Record = await productCategoryServices.GetAllRecord();
            return View(results);
        }
        [HttpGet]
        public async Task<ActionResult> GetPaged(int page = 1, int pagesize = 10, int ProductCategoryId = 0,string ProductType = null, string sarchString = null)
        {
            if (page < 1)
                page = 1;
            var results = await productSubCategoryService.GetPagedListAsync(page, pagesize, ProductCategoryId, ProductType, sarchString);
            return PartialView("_ProductSubCategorypartial", results);
        }

        [HttpGet]
        public async Task<IActionResult> AddRecort()
        {
            ProductSubCategoryViewModel viewModel = new ProductSubCategoryViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecort(ProductSubCategoryViewModel viewModel)
        {
            var result = await productSubCategoryService.AddRecord(viewModel);
            if (result == 2)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Same Name already exists!");
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await productSubCategoryService.DeleteRecord(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRecort(long id)
        {
            ProductSubCategoryViewModel viewModel = new ProductSubCategoryViewModel();
            viewModel = await productSubCategoryService.GetRecord(id);
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRecort(ProductSubCategoryViewModel viewModel)
        {
            var result = await productSubCategoryService.UpdateRecord(viewModel);
            if (result == 2)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Same Name already exists!");
            return View(viewModel);
        }
        [HttpGet]
        public async Task<JsonResult> GetByProductCategoryList(long id)
        {
            var res = await productSubCategoryService.GetProductTypeWiseList(id);
            return Json(res);
        }

    }
}
