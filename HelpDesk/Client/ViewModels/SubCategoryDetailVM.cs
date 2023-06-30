using Client.Utility;

namespace Client.ViewModels
{
    public class SubCategoryDetailVM
    {
        public Guid? Guid { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public Risk RiskLevel { get; set; }
    }
}
