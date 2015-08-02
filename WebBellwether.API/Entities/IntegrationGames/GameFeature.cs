using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class GameFeature
    {
        public GameFeature()
        {
            GameFeatureLanguages = new Collection<GameFeatureLanguage>();
        }
        public int Id { get; set; }
        public virtual ICollection<GameFeatureLanguage> GameFeatureLanguages { get; set; }
    }
}