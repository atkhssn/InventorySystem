using app.EntityModel.CoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.MainMenuService
{
    public interface IMainMenuService
    {
        Task<bool> AddRecort(MainMenuViewModel model);
        Task<List<MainMenu>> GetAllRecort();
        Task<bool> UpdateRecort(MainMenuViewModel model);
        Task<bool> DeleteRecort(long Id);
        Task<MainMenuViewModel> GetByRecort(long Id);
    }
}
