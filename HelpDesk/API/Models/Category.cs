using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_categories")]
    public class Category : BaseEntity
    {
        [Column("name")]
        public string CategoryName { get; set; }

        //Cardinality
        public ICollection<SubCategory>? SubCategories { get; set; }
    }
}
