using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace app.Infrastructure.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Prefix { get; set; }
        public bool IsBill { get; set; }=false; 
        public decimal SubscriptionFee{get; set;}
        public int UserType { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
    }

}
