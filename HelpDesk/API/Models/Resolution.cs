using API.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Resolution : BaseEntity
    {
        [Column("status")]
        public StatusLevel Status { get; set; }

        [Column("notes")]
        public string Notes { get; set; }

        [Column("finished_date")]
        public DateTime FinishedDate { get; set; }

        [Column("complain_guid")]
        public Guid ComplainGuid { get; set; }

        //Cardinality

        public Complain? Complain { get; set; }
    }


}
