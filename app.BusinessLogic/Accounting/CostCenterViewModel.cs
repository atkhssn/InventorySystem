using System.ComponentModel.DataAnnotations;

namespace app.Services.Accounting
{
    public class CostCenterViewModel : BaseViewModel
    {
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Display(Name = "Full Name")]
        public string Name { get; set; }
        public List<CostCenterViewModel> CostCenterViewModels { get; set; }
    }
}
