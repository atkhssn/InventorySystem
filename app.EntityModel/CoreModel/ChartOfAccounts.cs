using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.EntityModel.CoreModel
{
    [Table("ChartOfAccounts", Schema = "New")]
    public sealed class ChartOfAccounts
    {
        [Key]
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string ParentAccountCode { get; set; }
        public int Level { get; set; }
        [ForeignKey(nameof(CoATypes))]
        public int CoATypeId { get; set; }
        public CoATypes CoATypes { get; set; }
        public bool IsRoot { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
