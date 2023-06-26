using Client.Utility;

namespace Client.Models
{
    public class Complain
    {
        public Guid? Guid { get; set; }
        public Guid SubCategoryGuid { get; set; }
        public string Description { get; set; }
        public byte[] Attachment { get; set; }
        public Guid EmployeeGuid { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
