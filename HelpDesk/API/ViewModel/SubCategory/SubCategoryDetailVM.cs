using API.Utility;

namespace API.ViewModel.SubCategory
{
    public class SubCategoryDetailVM
    {
        public Guid? Guid { get; set; }

        public string SubCategoryName { get; set; }

        public string CategoryName { get; set; }

        public Risk RiskLevel { get; set; }
    }
}
