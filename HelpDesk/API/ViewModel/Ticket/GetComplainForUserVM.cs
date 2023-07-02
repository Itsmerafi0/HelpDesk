using API.Utility;

namespace API.ViewModel.Ticket
{
    public class GetComplainForUserVM
    {
        public Guid? Guid { get; set; }
        public string TicketId { get; set; }
        public string Requester { get; set; }
        public string Description { get; set; }
        public byte[] Attachment { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public StatusLevel StatusLevel { get; set; }
    }
}
