using API.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Resolution : BaseEntity
    {
        [Column("status")]
        public StatusLevel Status { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("finished_date")]
        public DateTime? FinishedDate { get; set; }

        //Cardinality

        public Ticket? Complain { get; set; }
    }


}
