namespace WebBellwether.Models.ViewModels.Version
{
    public class VersionViewModel
    {
        public int Id { get; set; }
        public double VersionNumber { get; set; }
        public int NumberOf { get; set; }
        public int LanguageId { get; set; }
        public string VersionTarget { get; set; }
    }
}