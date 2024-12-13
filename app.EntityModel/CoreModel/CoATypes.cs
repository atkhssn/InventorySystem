using System.ComponentModel.DataAnnotations.Schema;

namespace app.EntityModel.CoreModel
{
    [Table("CoATypes", Schema = "New")]
    public sealed class CoATypes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
