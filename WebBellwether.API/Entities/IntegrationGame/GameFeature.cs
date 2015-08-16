using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebBellwether.API.Entities.IntegrationGame
{
    public class GameFeature
    {
        public GameFeature()
        {
            GameFeatureLanguages = new Collection<GameFeatureLanguage>();
            GameFeatureDetails = new Collection<GameFeatureDetail>();
        }
        public int Id { get; set; }
        public virtual ICollection<GameFeatureLanguage> GameFeatureLanguages { get; set; }
        public virtual ICollection<GameFeatureDetail> GameFeatureDetails { get; set; } 
    }
}