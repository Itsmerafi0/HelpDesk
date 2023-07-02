using Client.Utitlity;

namespace Client.Models
{
    public class Resolution
    {
        public StatusLevel Status { get; set; }
        public string Notes { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
