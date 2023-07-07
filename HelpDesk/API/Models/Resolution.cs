using API.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_resolutions")]
    public class Resolution : BaseEntity
    {
        [Column("status")]
        public StatusLevel Status { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("finished_date")]
        public DateTime? FinishedDate { get; set; }

        [Column("resolved_by")]
        public Guid? ResolvedBy { get; set; }

        //Cardinality
        [ForeignKey("Guid")]
        public Ticket? Complains { get; set; }

        [ForeignKey("ResolvedBy")]
        public Employee? Employee { get; set; }
    }


}
