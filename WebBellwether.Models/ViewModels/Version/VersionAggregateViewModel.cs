using System.Collections.Generic;

namespace WebBellwether.Models.ViewModels.Version
{
    public class VersionAggregateViewModel
    {
        public List<VersionDetailViewModel> LanguageVersions { get; set; }
        public List<VersionDetailViewModel> IntegrationGameVersions { get; set; }
        public List<VersionDetailViewModel> GameFeatureVersions { get; set; }
        public List<VersionDetailViewModel> JokeCategoryVersions { get; set; }
        public List<VersionDetailViewModel> JokeVersions { get; set; }
        public CurrentVersionDetailStateViewModel CurrentVersionStateModel { get; set; }
    }
}
