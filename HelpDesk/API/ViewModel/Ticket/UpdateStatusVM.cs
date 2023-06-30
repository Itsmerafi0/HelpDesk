using API.Utility;

namespace API.ViewModel.Ticket
{
    public class UpdateStatusVM
    {
        public Guid Guid { get; set; }
        public StatusLevel Status { get; set; }
    }
}
