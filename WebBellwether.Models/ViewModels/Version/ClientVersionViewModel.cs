using WebBellwether.Models.Models.Translation;

namespace WebBellwether.Models.ViewModels.Version
{
    public class ClientVersionViewModel
    {
        public Language Language { get; set; }
        public double LanguageVersion { get; set; }
        public double IntegrationGameVersion { get; set; }
        public double GameFeatureVersion { get; set; }
        public double JokeCategoryVersion { get; set; }
        public double JokeVersion { get; set; }
    }
}
