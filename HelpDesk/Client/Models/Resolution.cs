using Client.Utility;

namespace Client.Models
{
    public class Resolution
    {
        public StatusLevel Status { get; set; }
        public string Notes { get; set; }
        public DateTime FInishDate { get; set; }
        public Guid ComplainGuid { get; set; }
    }
}
