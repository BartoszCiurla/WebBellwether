using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class GameFeatureLanguage
    {
        public int Id { get; set; }
        public string GameFeatureName { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public virtual GameFeature GameFeature { get; set; }
    }
}