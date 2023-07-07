namespace API.ViewModel.Response
{
    public class ClaimVM
    {
        public string NameIdentifer { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
