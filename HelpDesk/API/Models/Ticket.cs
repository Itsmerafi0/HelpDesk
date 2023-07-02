using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_tickets")]
    public class Ticket : BaseEntity
    {
        [Column("sub_category")]
        public Guid SubCategoryGuid { get; set; }

        [Column("ticket_id")]
        public string TicketId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("attachment")]
        public byte[]? Attachment { get; set; }

        [Column("employee_guid")]
        public Guid EmployeeGuid { get; set; }

        // Cardinality
        public Employee? Employee { get; set; }

        public Resolution? Resolution { get; set; }

        public SubCategory? SubCategory { get; set; }
    }
}
