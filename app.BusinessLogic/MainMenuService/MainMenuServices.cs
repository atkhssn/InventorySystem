using app.EntityModel.CoreModel;
using app.Infrastructure;
using app.Infrastructure.Repository;
namespace app.Services.MainMenuService
{
    public class MainMenuServices : IMainMenuService
    {
        private readonly IEntityRepository<MainMenu> _entityRepository;
        public MainMenuServices( IEntityRepository<MainMenu> entityRepository)
        {
            _entityRepository = entityRepository;
        }
        public async Task<bool> AddRecort(MainMenuViewModel model)
        {
            var getitem =  _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.Name == model.Name.Trim());
            if (getitem == null)
            {
                MainMenu menu=new MainMenu();
                menu.Name = model.Name;
                menu.Icon=model.Icon;
                menu.OrderNo = model.OrderNo;
                var result=await _entityRepository.AddAsync(menu);
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
                return  true;
                }
                return false;
            }
            return false;
        }

        public async Task<List<MainMenu>> GetAllRecort()
        {
            List<MainMenu> getitem = _entityRepository.AllIQueryableAsync().OrderBy(d=>d.OrderNo).ToList();
            return getitem;
        }

        public async Task<MainMenuViewModel> GetByRecort(long Id)
        {
            MainMenu menu= await _entityRepository.GetByIdAsync(Id);
            MainMenuViewModel model= new MainMenuViewModel();
            model.Id = menu.Id;
            model.Name = menu.Name; 
            model.OrderNo = menu.OrderNo; 
            model.Icon = menu.Icon; 
            model.IsActive = menu.IsActive;
            return model;
        }

        public async Task<bool> UpdateRecort(MainMenuViewModel model)
        {
            var checkmenu = _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.Name == model.Name&&f.Id!=model.Id);
            if (checkmenu == null)
            {
                var menu= await _entityRepository.GetByIdAsync(model.Id);
                menu.Name = model.Name;
                menu.Icon = model.Icon;
                menu.OrderNo = model.OrderNo;
                menu.IsActive = model.IsActive;
                var result = await _entityRepository.UpdateAsync(menu);
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
