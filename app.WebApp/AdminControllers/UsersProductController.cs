using app.Infrastructure.Auth;
using app.Services.DropDownServices;
using app.Services.Product_Services;
using app.Services.UserProduct_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class UsersProductController : Controller
    {
        private readonly IDropDownService dropDownService;
        private readonly  IWorkContext workContext;
        private readonly IUserProductService userProductService;
        public UsersProductController(
            IDropDownService dropDownService,
            IWorkContext workContext,
            IUserProductService userProductService
            )
        {
            this.dropDownService = dropDownService;
            this.workContext = workContext;
            this.userProductService = userProductService;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Recort= await workContext.CurrentUserAsync();     
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> CompanyGet()
        {
            var company = await dropDownService.Companlist();
            return Json(company);
        }
        [HttpGet]
        public async Task<JsonResult> productbyCompany(long companyid)
        {
            var company = await dropDownService.companyproductlist(companyid);
            return Json(company);
        }

        [HttpGet]
        public async Task<JsonResult> AdvanceSurachproductlist(long catagoryId, long subcatagoryid, long companyid)
        {
            var company = await dropDownService.AdvanceSurachproductlist(catagoryId, subcatagoryid,companyid);
            return Json(company);
        }

        [HttpGet]
        public async Task<JsonResult> AssigenProduct(long companyid,string userId,long productId)
        {
            var res= await userProductService.AddRecort(companyid,userId,productId);
            return Json(res);
        }
        [HttpGet]
        public async Task<JsonResult> UpdateProduct(long id,decimal mrp)
        {
            var res = await userProductService.updatet(id,mrp);
            return Json(res);
        }
        [HttpGet]
        public async Task<ActionResult> UserProductList(int page = 1, int pagesize = 10, string sarchString = null)
        {
            if (page < 1)
                page = 1;
            var results = await userProductService.GetPagedListAsync(page, pagesize,sarchString);
            return View(results);
        }
        [HttpGet]
        public async Task<ActionResult> GetPaged(int page = 1, int pagesize = 10, string sarchString = null)
        {
            if (page < 1)
                page = 1;
            var results = await userProductService.GetPagedListAsync(page, pagesize, sarchString);
            return PartialView("_UserProductpartial", results);
        }

    }
}
