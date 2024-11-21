using app.EntityModel.CoreModel;
using app.Infrastructure.Repository;
using app.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using app.Services.MenuItemService;
using app.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using app.Service.Medias;

namespace app.Services.UserServices
{
    public class UsersServices : IUserServices
    {
        private readonly inventoryDbContext dbContext;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private readonly IWorkContext workContext;
        private readonly IMediaService mediaService;
        public UsersServices(inventoryDbContext dbContext, RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager, IWorkContext workContext, IMediaService mediaService)
        {
            this.dbContext = dbContext;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.workContext = workContext; 
            this.mediaService = mediaService; 
        }

        public async Task<int> AddUser(UserViewModel model)
        {
            var usercheck = dbContext.Users.FirstOrDefault(f => f.Email == model.Email);
            var loguser = await workContext.CurrentUserAsync();
            if (usercheck != null) { return 1; }
            ApplicationUser users = new ApplicationUser();
            if (model.ImageFile != null) { model.ImageUrl = mediaService.UploadFile(model.ImageFile, "kgfile/customarlogo"); }
               
            users.FullName = model.FullName;
            users.UserName = model.Email;
            users.PhoneNumber = model.Mobile;
            users.Email = model.Email;
            users.Address = model.Addreass;
            users.Prefix = model.Password;
            users.ImageUrl = model.ImageUrl;
            users.IsBill = model.IsBill;
            users.SubscriptionFee = model.SubscriptionFee;
            if (model.RoleName == "Admin")
            { model.UserType = 1; }
            if (model.RoleName == "Customer")
            { model.UserType = 2; }
            users.UserType=model.UserType;
            users.CreatedOn = DateTime.Now;
            users.CreatedBy = loguser.FullName;
            users.IsActive = true;
            var result = await userManager.CreateAsync(users, model.Password);
            if (result.Succeeded)
            {
                var role = roleManager.FindByNameAsync(model.RoleName).Result;
                if (role != null)
                {
                    await userManager.AddToRoleAsync(users, role.Name);
                }
            }
            return 2;
        }

        public async Task<UserViewModel> GetAllRecort()
        {
            UserViewModel model = new UserViewModel();
            model.datalist = await Task.Run(() => (from t1 in dbContext.Users
                                                   select new UserViewModel
                                                   {
                                                       UserId = t1.Id,
                                                       FullName = t1.FullName,
                                                       UserName = t1.UserName,
                                                       IsActive = t1.IsActive,
                                                       Email = t1.Email,
                                                       Mobile = t1.PhoneNumber,
                                                       IsBill = t1.IsBill,
                                                       SubscriptionFee = t1.SubscriptionFee,
                                                   }).OrderByDescending(x => x.UserName).AsEnumerable());
            return model;
        }

        public async Task<ApplicationUser> GetByUser(string username)
        {
            var result = await dbContext.Users.FirstOrDefaultAsync(d => d.Email == username&&d.IsActive==true);
            return result;
        }

        public async Task<UserViewModel> GetByUserId(string username)
        {
            var model = await dbContext.Users.FirstOrDefaultAsync(d => d.Id == username);
            UserViewModel users = new UserViewModel();
            users.FullName = model.FullName;
            users.UserName = model.Email;
            users.Mobile = model.PhoneNumber;
            users.Email = model.Email;
            users.UserType = model.UserType;
            users.UserId = model.Id;
            users.Addreass = model.Address;
            users.ImageUrl = model.ImageUrl;
            users.IsBill = model.IsBill;
            users.SubscriptionFee = model.SubscriptionFee;
            users.IsActive= model.IsActive; 
            var RoleId = dbContext.UserRoles.FirstOrDefault(g => g.UserId == model.Id).RoleId;
            users.RoleName = dbContext.Roles.FirstOrDefault(g => g.Id == RoleId).Name;
            return users;
        }

        public async Task<bool> SoftDelete(string username)
        {
            var model = await dbContext.Users.FirstOrDefaultAsync(d => d.Id == username);
            var user = await workContext.CurrentUserAsync();

            model.IsActive = false;
            model.UpdatedOn = DateTime.Now;
            model.UpdatedBy = user.FullName;
            var result = await userManager.UpdateAsync(model);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<int> UpdateUser(UserViewModel model)
        {
            var usercheck = dbContext.Users.FirstOrDefault(f => f.Email == model.Email && f.Id != model.UserId);
            if (usercheck != null) { return 1; }
            ApplicationUser users = dbContext.Users.FirstOrDefault(f => f.Id == model.UserId);
            var curentname = dbContext.UserRoles.FirstOrDefault(g => g.UserId == model.UserId);
            var oldrole = dbContext.Roles.FirstOrDefault(g => g.Id == curentname.RoleId);
            var loguser = await workContext.CurrentUserAsync();
            var role = roleManager.FindByIdAsync(model.RoleId).Result;
            if (model.ImageFile != null) {
                model.ImageUrl = mediaService.UpdateFile(model.ImageUrl, model.ImageFile, "kgfile/customarlogo");
            }
            users.ImageUrl = model.ImageUrl;
            users.FullName = model.FullName;
            users.UserName = model.Email;
            users.PhoneNumber = model.Mobile;
            users.Email = model.Email;
            users.Address = model.Addreass;
            users.IsActive = model.IsActive;
            users.IsBill = model.IsBill;
            users.SubscriptionFee = model.SubscriptionFee;
            if (model.RoleName == "Admin") { users.UserType = 1; }
            if (model.RoleName == "Customer") { users.UserType = 2; }
            users.UpdatedOn = DateTime.Now;
            users.UpdatedBy = loguser.FullName;
            var result = await userManager.UpdateAsync(users);
            if (result.Succeeded)
            {
                if (role != null)
                {
                    await userManager.RemoveFromRoleAsync(users, oldrole.Name);
                    await userManager.AddToRoleAsync(users, role.Name);
                }
                return 2;
            }
            return 0;
        }
    }
}
