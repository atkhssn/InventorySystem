using System.ComponentModel.DataAnnotations;

namespace app.Services.Vouchern
{
    public sealed class VoucherTypesViewModel : BaseViewModel
    {
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public List<VoucherTypesViewModel> VoucherTypesViewModels { get; set; }
        public ResponseViewModel ResponseViewModel { get; set; }
    }
}
