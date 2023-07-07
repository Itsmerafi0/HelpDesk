using API.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_subcategories")]
    public class SubCategory : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("category_guid")]
        public Guid CategoryGuid { get; set; }

        [Column("risk_level")]
        public Risk RiskLevel { get; set; }

        // Cardinality
        public ICollection<Ticket>? Complains { get; set; }

        public Category? Category { get; set; }
    }
}
