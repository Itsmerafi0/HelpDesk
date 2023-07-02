namespace API.ViewModel.Ticket
{
    public class TicketResoVM
    {
        //data complain
        public Guid SubCategoryGuid { get; set; }
        public string Description { get; set; }
        public byte[]? Attachment { get; set; }
        public Guid EmployeeGuid { get; set; }
    }
}
