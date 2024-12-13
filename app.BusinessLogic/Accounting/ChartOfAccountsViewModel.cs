using System.ComponentModel.DataAnnotations;

namespace app.Services.Accounting
{
    public sealed class ChartOfAccountsViewModel
    {
        [Display(Name = "Account Code")]
        public string AccountCode { get; set; }

        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Display(Name = "Parent Account Code")]
        public string ParentAccountCode { get; set; }

        [Display(Name = "Level")]
        public int Level { get; set; }

        [Display(Name = "CoA Type Id")]
        public int CoATypeId { get; set; }

        public CoATypesViewModel CoATypesViewModel { get; set; }

        [Display(Name = "Root")]
        public bool IsRoot { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Create By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Create On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Update By")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Update On")]
        public DateTime? UpdatedOn { get; set; }

        public List<ChartOfAccountsViewModel> ChartOfAccountsViewModels { get; set; }
        public ResponseViewModel ResponseViewModel { get; set; }
    }
}
