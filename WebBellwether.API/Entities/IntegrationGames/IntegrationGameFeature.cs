using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class IntegrationGameFeature
    {
        public int Id { get; set; }
        public virtual IntegrationGameDetail IntegrationGameDetail { get; set; }
        public virtual GameFeatureLanguage GameFeatureLanguage { get; set; }
        public virtual GameFeatureDetailLanguage GameFeatureDetailLanguage { get; set; }
    }
}