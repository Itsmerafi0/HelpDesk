using API.Utility;

namespace API.ViewModel.SubCategory
{
    public class SubCategoryVM
    {
        public Guid? Guid { get; set; }
        public string Name { get; set; }
        public Guid CategoryGuid { get; set; }
        public Risk RiskLevel { get; set; }

    }
}
