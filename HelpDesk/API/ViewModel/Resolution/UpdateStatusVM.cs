using API.Utility;

namespace API.ViewModel.Resolution
{
    public class UpdateStatusVM
    {
        public Guid ResolutionGuid { get; set; }
        public StatusLevel NewStatus { get; set; }
    }
}
