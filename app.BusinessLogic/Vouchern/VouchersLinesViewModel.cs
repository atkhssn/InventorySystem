using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace app.Services.Vouchern
{
    public sealed class VouchersLinesViewModel : BaseViewModel
    {
        [Display(Name = "Voucher")]
        public long VouchersId { get; set; }

        [Display(Name = "Voucher No")]
        public string VoucherNo { get; set; }

        public VouchersViewModel VouchersViewModel { get; set; }

        [Display(Name = "Accounting Head (GL Code)")]
        public string GlHeadId { get; set; }

        [Display(Name = "Debit Amount")]
        public decimal DebitAmount { get; set; }

        [Display(Name = "Cedit Amount")]
        public decimal CreditAmount { get; set; }

        [Display(Name = "Particular")]
        public string Particular { get; set; }

        [Display(Name = "Attachment")]
        public IFormFile Attachment { get; set; }

        public string AttachmentPath { get; set; }

        public List<VouchersLinesViewModel> VouchersLinesViewModels { get; set; }
        public ResponseViewModel ResponseViewModel { get; set; }
    }
}
