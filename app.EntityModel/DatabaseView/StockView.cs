using app.EntityModel.CoreModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.DatabaseView
{

    //[Table("StockView")]
    public class StockView
    {
        [Key]
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public string TrakingId { get; set; }
        public decimal InQty { get; set; }
        public decimal OutQty { get; set; }
        public decimal SalesReturnQty { get; set; }
        public decimal AVGPrice { get; set; }
    }
}



//USE[InvetoryDB]
//GO

///****** Object:  View [dbo].[StockView]    Script Date: 9/21/2023 8:54:25 PM ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO


//CREATE VIEW [dbo].[StockView] AS
//select
//s.ProductId,
//sum (s.InQty) as InQty,
//sum(s.OutQty) as OutQty,
//s.TrakingId,
//pc.Name as ProductCategoryName,
//psc.Name as ProductSubCategoryName,
//p.ProductName,
//up.AVGPrice,
//c.Name as CompanyName,
//c.Id as CompanyId
//from[dbo].[StockInfo] as s
//join[dbo].[UserProduct] as up on s.ProductId = up.ProductId
//join[dbo].[Product] as p on up.ProductId = p.Id
//join[dbo].[ProductCategory] as pc on p.ProductCategoryId = pc.Id
//join[dbo].[ProductSubCategory] as psc on p.ProductSubCategoryId = psc.Id
//join[dbo].[Company] as c on up.CompanyId = c.Id
//where s.IsActive = 1
//group by s.ProductId, s.TrakingId, p.ProductName, pc.Name, psc.Name, up.AVGPrice, c.Name, c.Id

//GO


