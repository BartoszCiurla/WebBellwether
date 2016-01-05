using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBellwether.Repositories.Entities.IntegrationGame
{
    [Table("GameFeature")]
    public class GameFeatureDao
    {
        public GameFeatureDao()
        {
            GameFeatureLanguages = new Collection<GameFeatureLanguageDao>();
            GameFeatureDetails = new Collection<GameFeatureDetailDao>();
        }
        public int Id { get; set; }
        public virtual ICollection<GameFeatureLanguageDao> GameFeatureLanguages { get; set; }
        public virtual ICollection<GameFeatureDetailDao> GameFeatureDetails { get; set; } 
    }
}