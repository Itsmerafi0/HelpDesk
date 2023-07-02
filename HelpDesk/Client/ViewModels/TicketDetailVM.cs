using Client.Utitlity;

namespace Client.ViewModels
{
    public class TicketDetailVM
    {
        public Guid? Guid { get; set; }
        public string TicketId { get; set;}
        public string Requester { get; set; }
        public string Email { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public Risk RiskLevel { get; set; }
        public StatusLevel StatusLevel { get; set; }
        public string Description { get; set; }
        public string ResolutionNote { get; set; }
    }
}
