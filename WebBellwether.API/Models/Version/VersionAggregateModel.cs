using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace WebBellwether.API.Models.Version
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
