using Client.Utitlity;

namespace Client.Models
{
    public class SubCategory
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public Guid CategoryGuid { get; set; }
        public Risk RiskLevel { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
