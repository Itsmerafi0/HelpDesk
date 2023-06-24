using API.Utility;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.ViewModel.Complain
{
    public class ComplainVM
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public StatusLevel StatusLevel { get; set; }
        public Risk RiskLevel { get; set; }
        public string Description { get; set; }
        public string? Attachment { get; set; }
        public DateTime FinishDate { get; set; }
        public Guid EmployeeGuid { get; set; }
    }
}
