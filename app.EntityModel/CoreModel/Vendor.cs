using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class Vendor:BaseEntity
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string OptionalMobile { get; set; }
        public string Email { get; set; }
        public string NID { get; set; }
        public string Address { get; set; }
        public long ZoneId { get; set; }    
        public long SubZoneId { get; set; }    
        public int VendorType { get; set; }    
        public string CustomerType { get; set; }    
        public string PaymentType { get; set; }    
        public int VendorStatus { get; set; }    
        public long VendorReferenceId { get; set; }    
        public long HeadGLId { get; set; }   
        public string ACName {get; set; }   
        public string ACNo { get; set; }   
        public string AcCode { get; set; }   
        public string BankName { get; set; }   
        public string BranchName { get; set; }   
        public string ContactName { get; set; }   
        public long DivisionId { get; set; }   
        public long DistrictId { get; set; }   
        public long UpazilaId { get; set; }   
        public decimal SecurityAmount { get; set; }
        public decimal CreditLimit { get; set; }
        public bool IsForeign { get; set;}


    }
}
