using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebBellwether.API.Entities.IntegrationGame
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