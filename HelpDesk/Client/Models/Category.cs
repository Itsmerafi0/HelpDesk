namespace Client.Models
{
    public class Category
    {
        public Guid? Guid { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
