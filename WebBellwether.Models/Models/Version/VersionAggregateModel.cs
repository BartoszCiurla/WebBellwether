namespace WebBellwether.Models.Models.Version
{
    public class VersionAggregateModel
    {
        public VersionDetailModel[] LanguageVersions { get; set; }
        public VersionDetailModel[] IntegrationGameVersions { get; set; }
        public VersionDetailModel[] JokeCategoryVersions { get; set; }
        public VersionDetailModel[] JokeVersions { get; set; }
        public CurrentVersionDetailStateModel CurrentVersionStateModel { get; set; }
    }
}
