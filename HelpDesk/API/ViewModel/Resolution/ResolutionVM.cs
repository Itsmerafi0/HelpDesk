using API.Utility;

namespace API.ViewModel.Resolution
{
    public class ResolutionVM
    {
        public Guid Guid { get; set; }
        public StatusLevel Status { get; set; }
        public string Notes { get; set; }
        public DateTime FInishDate { get; set; }
        public Guid ComplainGuid { get; set; }
    }
}
