using API.Utility;

namespace API.ViewModel.Ticket
{
    public class GetTicketForDevVM
    {
        public Guid? Guid { get; set; }
        public string TicketId { get; set; }
        public string Requester { get; set; }
        public string Email { get; set; }
        public string SubCategoryName { get; set; }
        public byte[] Attachment { get; set; }
        public Risk RiskLevel { get; set; }
        public StatusLevel StatusLevel { get; set; }
        public string Description { get; set; }
        public string ResolutionNote { get; set; }
        public string ResolvedBy { get; set; }
        public DateTime? FinishedDate { get; set; }
    }
}
