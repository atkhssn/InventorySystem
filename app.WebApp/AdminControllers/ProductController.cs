using app.EntityModel.CoreModel;
using app.Services.MenuItemService;
using app.Services.Product_Services;
using app.Services.ProductCategory_Services;
using app.Services.ProductSubCategory_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductSubCategoryService productSubCategoryService;
        private readonly IProductCategoryServices productCategoryServices;
        private readonly IProductServices productServices;
        public ProductController(IProductSubCategoryService productSubCategoryService,
             IProductCategoryServices productCategoryServices,
             IProductServices productServices)
        {
            this.productSubCategoryService = productSubCategoryService;
            this.productCategoryServices = productCategoryServices;
            this.productServices = productServices;
        }
        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int pagesize = 10, int ProductCategoryId = 0, int ProductSubCategoryId = 0, string ProductType = null, string sarchString = null)
        {
            if (page < 1)
                page = 1;
            var results = await productServices.GetPagedListAsync(page, pagesize, ProductCategoryId, ProductSubCategoryId, ProductType, sarchString);
            return View(results);
        }
        [HttpGet]
        public async Task<ActionResult> GetPaged(int page = 1, int pagesize = 10, int ProductCategoryId = 0, int ProductSubCategoryId = 0, string ProductType = null, string sarchString = null)
        {
            if (page < 1)
                page = 1;
            var results = await productServices.GetPagedListAsync(page, pagesize, ProductCategoryId, ProductSubCategoryId, ProductType, sarchString);
            return PartialView("_Productpartial", results);
        }

        [HttpGet]
        public async Task<IActionResult> AddRecord()
        {
            ProductViewModel viewModel = new ProductViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(ProductViewModel viewModel)
        {
            var result = await productServices.AddRecord(viewModel);
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
            var res = await productServices.DeleteRecord(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRecort(long id)
        {
            ProductViewModel model = new ProductViewModel();
            model = await productServices.GetRecord(id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRecort(ProductViewModel viewModel)
        {
            var result = await productServices.UpdateRecord(viewModel);
            if (result == 2)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Same Name already exists!");
            return View(viewModel);
        }



    }
}
