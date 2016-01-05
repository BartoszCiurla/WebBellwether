using System.Collections.Generic;

namespace WebBellwether.Models.Models.Version
{
    public class VersionAggregateModel
    {
        public IEnumerable<VersionDetailModel> LanguageVersions { get; set; }
        public IEnumerable<VersionDetailModel> IntegrationGameVersions { get; set; }
        public IEnumerable<VersionDetailModel> JokeCategoryVersions { get; set; }
        public IEnumerable<VersionDetailModel> JokeVersions { get; set; }
        public CurrentVersionDetailStateModel CurrentVersionStateModel { get; set; }
    }
}
