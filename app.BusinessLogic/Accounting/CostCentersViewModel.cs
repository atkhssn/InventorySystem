using System.ComponentModel.DataAnnotations;

namespace app.Services.Accounting
{
    public sealed class CostCentersViewModel : BaseViewModel
    {
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Display(Name = "Full Name")]
        public string Name { get; set; }
        public List<CostCentersViewModel> CostCenterViewModels { get; set; }
        public ResponseViewModel ResponseViewModel { get; set; }
    }
}
