using app.EntityModel.CoreModel;
using app.Infrastructure.Repository;
using app.Infrastructure;
using app.Services.UserpermissionsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using app.Services.MenuItemService;
using app.Infrastructure.Auth;
using app.Services.UserServices;
using System.Text.RegularExpressions;

namespace app.Services.UserpermissionsService
{
    public class UserpermissionServices : IUserpermissionServices
    {
        private readonly IEntityRepository<Userpermissions> _entityRepository;
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;  
        public UserpermissionServices(IEntityRepository<Userpermissions> entityRepository, inventoryDbContext dbContext, IWorkContext workContext)
        {
            _entityRepository = entityRepository;
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<bool> AddRecort(long Id, string UserId)
        {
            Userpermissions userpermissions = new Userpermissions();
            var result = await dbContext.Userpermissions.FirstOrDefaultAsync(d => d.MenuItem == Id && d.UserId == UserId);
            if (result == null)
            {
                userpermissions.UserId = UserId;
                userpermissions.MenuItem = Id;
                var res = await _entityRepository.AddAsync(userpermissions);
            }
            else
            {
                if (result.IsActive == true)
                {
                    result.IsActive = false;
                }
                else
                {
                    result.IsActive = true;
                }
                var res = await _entityRepository.UpdateAsync(result);
            }
            return true;
        }

        public async Task<MenuPermissionViewModel> GetAllMenuItemRecort(string username)
        {
            MenuPermissionViewModel viewModel = new MenuPermissionViewModel();
            List<MainMenuVM> models = new List<MainMenuVM>();
            //var user = await workContext.CurrentUserAsync();
            var result = await dbContext.MainMenu.Where(s => s.IsActive == true).OrderBy(f=>f.OrderNo).AsNoTracking().ToListAsync();
            foreach (var item in result)
            {
                string str = item.Name;
                str = Regex.Replace(str, @"\s", "-");
                MainMenuVM vM = new MainMenuVM();
                vM.Name = item.Name;
                vM.Icon = item.Icon;
                vM.Activeid = str;
                vM.menuItemVMs=await  mappermission(username,item.Id);
                models.Add(vM);
            }
            viewModel.MainMenuVM = models;
            return viewModel;
        }

        private async Task<List<MenuItemVM>> mappermission(string id,long menuid)
        {
             List < MenuItemVM > menus=new List<MenuItemVM>();
            menus= await Task.Run(() => (from t1 in dbContext.MenuItem
                                         join t2 in dbContext.Userpermissions on t1.Id equals t2.MenuItem
                                         where t2.IsActive==true && t1.MenuId==menuid &&t1.IsActive==true && t2.UserId==id
                                                   select new MenuItemVM
                                                   {
                                                       Id = t1.Id,
                                                       Name = t1.Name,
                                                       Action = t1.Action,
                                                       Controller = t1.Controller, 
                                                       OrderNo = t1.OrderNo,
                                                       Icon = t1.Icon,  
                                                   }).OrderBy(x => x.OrderNo).AsNoTracking().ToListAsync());
           return menus;    
        }

        public async Task<UserpermissionViewModel> GetAllRecort(string Id)
        {
            UserpermissionViewModel viewModel = new UserpermissionViewModel();
            List<UserpermissionViewModel> models = new List<UserpermissionViewModel>();
            var result = await dbContext.MainMenu.Where(s => s.IsActive == true).AsNoTracking().ToListAsync();
            foreach (var item in result)
            {
                UserpermissionViewModel model = new UserpermissionViewModel();
                model.MenuName = item.Name;
                models.Add(model);
                var itemlist = dbContext.MenuItem.Where(d => d.MenuId == item.Id &&d.IsActive==true).AsNoTracking().ToList();
                List<MenuItemViewModel> menuitemlist = new List<MenuItemViewModel>();
                foreach (var menu in itemlist)
                {
                    var res = await dbContext.Userpermissions.FirstOrDefaultAsync(d => d.MenuItem == menu.Id && d.UserId == Id);
                    MenuItemViewModel vm = new MenuItemViewModel();
                    vm.Id = menu.Id;
                    vm.Name = menu.Name;
                    if (res != null && res.IsActive == true)
                    { vm.IsPermission = true; }

                    else { vm.IsPermission = false; }
                    menuitemlist.Add(vm);
                }
                model.menuitemlist = menuitemlist;
            }
            viewModel.datalist = models;
            return viewModel;
        }
    }
}
