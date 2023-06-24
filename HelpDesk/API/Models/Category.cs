using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Category : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        //Cardinality
        public ICollection<SubCategory>? SubCategories { get; set; }
    }
}
