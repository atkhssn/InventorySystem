using app.EntityModel.CoreModel;
using app.Infrastructure.Repository;
using app.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using app.Infrastructure.Auth;
using app.Services.MenuItemService;

namespace app.Services.BillGenerateds
{
    public class BillGeneratedService : IBillGeneratedService
    {
        private readonly IEntityRepository<BillGenerated> _entityRepository;
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public BillGeneratedService(IWorkContext workContext,IEntityRepository<BillGenerated> entityRepository, inventoryDbContext dbContext)
        {
            _entityRepository = entityRepository;
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<bool> AddRecort(BillGeneratedViewModel model)
        {
            var res = await dbContext.Users.Where(d => d.IsActive == true && d.IsBill == true && d.SubscriptionFee > 0).ToListAsync();
          var user= await workContext.GetCurrentUserAsync();
            if (res.Count()>0)
            {
                List<BillGenerated> bills = new List<BillGenerated>();
                foreach (var item in res) {
                    var checkdata= await dbContext.BillGenerated.Where(d=>d.Month==model.BillGeneratedDate.Month&&d.Years== model.BillGeneratedDate.Year && d.UserId==item.Id&&d.IsActive==true).CountAsync();   
                    var billcount = dbContext.BillGenerated.Count();
                    if (checkdata==0) {
                        BillGenerated bill = new BillGenerated();
                        bill.SubscriptionFee = item.SubscriptionFee;
                        bill.Month = model.BillGeneratedDate.Month;
                        bill.Years = model.BillGeneratedDate.Year;
                        bill.BillNo = ("Bill-" + bill.Month + "-" + bill.Years + "-" + (billcount == 0 ? 1 : ((int)billcount +1)).ToString()).ToString();
                        bill.IsPaid = false;
                        bill.CollactionFee = 0;
                        bill.UserId = item.Id;
                        bill.CreatedBy = user.UserName;
                        bill.CreatedOn = DateTime.Now;
                        bill.IsActive = true;
                        bill.TrakingId = user.UserName;
                        dbContext.BillGenerated.Add(bill);
                        var result = await dbContext.SaveChangesAsync();
                   
                    }                           
                }
                return true;
            }
            return false;   
        }

        public async Task<bool> DeleteRecort(long Id)
        {
            var result = await _entityRepository.GetByIdAsync(Id);
            result.IsActive = false;
            await _entityRepository.UpdateAsync(result);
            return true;
        }

        public async Task<BillGeneratedViewModel> GetAllRecort()
        {
            BillGeneratedViewModel model = new BillGeneratedViewModel();
            model.bills = await Task.Run(() => (from t1 in dbContext.BillGenerated
                                                   join t2 in dbContext.Users on t1.UserId equals t2.Id
                                                   where t1.IsActive==true
                                                   select new BillGeneratedViewModel
                                                   {
                                                       Id = t1.Id,
                                                       BillNo = t1.BillNo,
                                                       Month=t1.Month,
                                                       Years=t1.Years,
                                                       SubscriptionFee=t1.SubscriptionFee,  
                                                       CollactionFee=t1.CollactionFee,
                                                       IsPaid=t1.IsPaid,    
                                                       UserName=t2.FullName,
                                                       Email=t2.Email,  
                                                   }).OrderByDescending(x => x.Id).ToListAsync());
            return model;
        }

        public async Task<BillGeneratedViewModel> GetByRecort(long Id)
        {
            var result = await _entityRepository.GetByIdAsync(Id);
            BillGeneratedViewModel model = new BillGeneratedViewModel();
            model.Id=result.Id;
            model.BillNo=result.BillNo;
            model.Month=result.Month;
            model.Years=result.Years;
            model.SubscriptionFee=result.SubscriptionFee;
            model.IsPaid=result.IsPaid;
            model.CreatedOn=result.CreatedOn;   
            model.UpdatedOn=result.UpdatedOn;   
            model.CollactionFee=result.CollactionFee;
            model.FullName= dbContext.Users.FirstOrDefault(f=>f.Id==result.UserId)?.FullName; 
            model.TotalFee = dbContext.BillGenerated.Where(d => d.UserId == result.UserId && d.IsActive == true && d.IsPaid == false).Sum(d => d.SubscriptionFee);
            return model;
        }

        public Task<bool> UpdateRecort(BillGeneratedViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
