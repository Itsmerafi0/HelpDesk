using Client.Utility;

namespace Client.Models
{
    public class Complain
    {
        public Guid? Guid { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public StatusLevel StatusLevel { get; set; }
        public Risk RiskLevel { get; set; }
        public string Description { get; set; }
        public string? Attachment { get; set; }
        public DateTime FinishDate { get; set; }
        public Guid EmployeeGuid { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
