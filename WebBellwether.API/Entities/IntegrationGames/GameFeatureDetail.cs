using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class GameFeatureDetail
    {
        public GameFeatureDetail()
        {
            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>();
        }
        public int Id { get; set; }
        public virtual ICollection<GameFeatureDetailLanguage> GameFeatureDetailLanguages { get; set; }
        public virtual GameFeature GameFeature { get; set; }
    }
}