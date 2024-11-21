using app.EntityModel.CoreModel;
using app.Infrastructure;
using app.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.MenuItemService
{
    public class MenuItemServices : IMenuItemService
    {
        private readonly IEntityRepository<MenuItem> _entityRepository;
        private readonly inventoryDbContext dbContext;
        public MenuItemServices(IEntityRepository<MenuItem> entityRepository, inventoryDbContext dbContext)
        {
            _entityRepository = entityRepository;
            this.dbContext = dbContext;
        }
        public async Task<bool> AddRecort(MenuItemViewModel model)
        {
            var getitem = _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.Name == model.Name &&f.Action==model.Action&&f.Controller==model.Controller);
            if (getitem == null)
            {
                MenuItem item = new MenuItem();
                item.Name = model.Name;
                item.ShortName = model.ShortName;
                item.Icon = model.Icon;
                item.MenuId = model.MenuId;
                item.Action = model.Action;
                item.Controller = model.Controller;
                item.OrderNo = model.OrderNo;
                var result = await _entityRepository.AddAsync(item);
                if (result.Id > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }

        public async Task<bool> DeleteRecort(long Id)
        {
            var getitem = await _entityRepository.GetByIdAsync(Id);
            if (getitem != null)
            {
                getitem.IsActive = false;
                var result = await _entityRepository.UpdateAsync(getitem);
                if (result)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<MenuItemViewModel> GetAllRecort()
        {
            MenuItemViewModel model = new MenuItemViewModel();
            model.datalist = await Task.Run(() => (from t1 in dbContext.MenuItem
                                                   join t2 in dbContext.MainMenu on t1.MenuId equals t2.Id
                                                   select new MenuItemViewModel
                                                   {
                                                       Id = t1.Id,
                                                       OrderNo = t1.OrderNo,
                                                       Name = t1.Name,
                                                       ShortName = t1.ShortName,
                                                       Icon = t1.Icon,
                                                       Action = t1.Action,
                                                       Controller = t1.Controller,
                                                       MenuId = t1.MenuId,
                                                       MenuName=t2.Name,
                                                       IsActive = t1.IsActive,
                                                   }).OrderByDescending(x => x.OrderNo).AsEnumerable());
            return model;
        }
        public async Task<MenuItemViewModel> GetByRecort(long Id)
        {
            MenuItemViewModel model = new MenuItemViewModel();
            MenuItem item = await _entityRepository.GetByIdAsync(Id);
            model.Name= item.Name;
            model.ShortName= item.ShortName;
            model.OrderNo= item.OrderNo;    
            model.Controller = item.Controller;
            model.Action = item.Action;
            model.Icon = item.Icon;
            model.MenuId = item.MenuId;
            model.IsActive = item.IsActive;
            return model;
        }

        public async Task<bool> UpdateRecort(MenuItemViewModel model)
        {
            var getitem = _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.Name == model.Name && f.Action == model.Action && f.Controller == model.Controller&&f.Id!=model.Id);
            if (getitem == null)
            {
                MenuItem item = await _entityRepository.GetByIdAsync(model.Id);
                item.Name = model.Name;
                item.ShortName = model.ShortName;
                item.Icon = model.Icon;
                item.MenuId = model.MenuId;
                item.Action = model.Action;
                item.Controller = model.Controller;
                item.OrderNo = model.OrderNo;
                item.IsActive = model.IsActive; 
                var result = await _entityRepository.UpdateAsync(item);
                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }
    }
}
